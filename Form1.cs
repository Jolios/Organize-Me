using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Bunifu.UI.WinForms;
using Bunifu.UI.WinForms.BunifuTextbox;
using Bunifu.UI.WinForms.BunifuButton;
using System.IO;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace Organize_Me
{
    public partial class Form1 : Form
    {
        internal static int count = 1;
        private CRUD crud = new CRUD();
        public byte[] buffer;
        public int CurrentUserId = 0;
        internal static int NuChildren = 0;
        private bool isDivorcedAndHasChildren = false;
        private bool isDivorced = false;
        internal List<Childd> Children;
        private Font segoe = new Font("Segoe UI", 10);
        public String FirstName = String.Empty;
        public String LastName = String.Empty;

        

        public Form1()
        {
            InitializeComponent();
            bunifuFormDock1.SubscribeControlToDragEvents(bunifuGradientPanel1);
            bunifuFormDock1.SubscribeControlToDragEvents(tabPage1);
            bunifuFormDock1.SubscribeControlToDragEvents(tabPage2);
            bunifuFormDock1.SubscribeControlToDragEvents(tabPage3);
            bunifuFormDock1.SubscribeControlToDragEvents(tabPage4);
            bunifuFormDock1.SubscribeControlToDragEvents(tabPage5);
            CRUD crud = new CRUD();
            Children = new List<Childd>();
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            bunifuPages1.SetPage(1);
            Label_info_G1.Font = segoe;
            Label_Info_G2.Font = segoe;
            Label_Spouse_G1.Font = segoe;
            Label_Spouse_G2.Font = segoe;
            Label_Child_G2.Font = segoe;
            Label_Child_G1.Font = segoe;
            Label_Parent_D1.Font = segoe;
            Label_Parent_D2.Font = segoe;
            Label_Parent_D3.Font = segoe;
            Label_Parent_D4.Font = segoe;
        }
        private void passwordChar(BunifuTextBox txt)
        {
            txt.UseSystemPasswordChar = String.IsNullOrEmpty(txt.Text) ? false : true;
        }

        private void txt_SignInPassword_TextChanged(object sender, EventArgs e)
        {
            passwordChar(txt_SignInPassword);
        }

        private void txt_SignUpPassword_TextChanged(object sender, EventArgs e)
        {
            passwordChar(txt_SignUpPassword);

        }

        private void txt_ConfirmPassword_TextChanged(object sender, EventArgs e)
        {
            passwordChar(txt_ConfirmPassword);

        }
        private void bunifuButton_Click(object sender, EventArgs e)
        {
            BunifuButton b = (BunifuButton) sender;
            switch (b.Name)
            {
                case "btn_GradientSignIn" : bunifuPages1.SetPage(0);
                        
                                            break;
                case "btn_GradientSignUp" : bunifuPages1.SetPage(1);
                                            break;
                case "btn_SignIn"         : if (crud.SignInVerification(this))
                                            {
                                                CurrentUserId = crud.getCurrentUserId(txt_SignInEmail.Text);
                                                Form2 f2 = new Form2(CurrentUserId);
                                                f2.Show();
                                                this.Hide();
                                            }
                                            break;
                case "btn_Continue"       : if (crud.ContinueBtnVerification(this))
                                            {
                                                bunifuPages1.SetPage(2);
                                                txt_UserFName.Text = FirstName;
                                                txt_UserLName.Text = LastName;
                                            }
                                                break;
                case "btn_Back1"          : bunifuPages1.SetPage(1);
                                            break;
                case "btn_Back2"          : bunifuPages1.SetPage(2);
                                            break;
                case "btn_Back3"          : if (isDivorced) bunifuPages1.SetPage(2);
                                            else bunifuPages1.SetPage(3);
                                            break;
                case "btn_Back4"          : if (txt_ChN.Visible || isDivorcedAndHasChildren) bunifuPages1.SetPage(4);
                                            else bunifuPages1.SetPage(3);
                                            break;
                case "btn_SignUp2"        : if (crud.ParentsVerification(this))
                                            {
                                                crud.InsertUser(this);
                                                CurrentUserId = crud.getCurrentUserId(txt_SignUpEmail.Text);
                                                if (!String.IsNullOrWhiteSpace(txt_SpouseFName.Text)) crud.InsertSpouse(this);
                                                if (Children.Count>0) crud.InsertChildren(this);
                                                crud.InsertParents(this);
                                                bunifuPages1.SetPage(0);
                                            }
                                            break;

                default                   : break;
            }
        }
        private void btnUpload_Click(object sender, EventArgs e)
        {
            OpenFileDialog opnfd = new OpenFileDialog();
            opnfd.Filter = "Image Files (*.jpg;*.jpeg;.*.gif;*.png;)|*.jpg;*.jpeg;.*.gif;*.png";
            opnfd.InitialDirectory = "D:\\";
            if (opnfd.ShowDialog() == DialogResult.OK)
            {
               MessageBox.Show(opnfd.FileName);
            }
            this.buffer = File.ReadAllBytes(opnfd.FileName);
            
        }
        private void btn_Next1_Click(object sender, EventArgs e)
        {
            if (crud.InfoVerification(this))
            {
                if (cb_MaritalStatus.SelectedItem.ToString().Equals("Married"))
                {
                    isDivorced = false;
                    isDivorcedAndHasChildren = false;
                    bunifuPages1.SetPage(3);
                }
                else if (cb_MaritalStatus.SelectedItem.ToString().Equals("Divorced"))
                {
                    isDivorced = true;
                    DialogResult dr = MessageBox.Show("Do you have children?",
                      "Children Test", MessageBoxButtons.YesNo);
                    switch (dr)
                    {
                        case DialogResult.Yes:
                            isDivorcedAndHasChildren = true;
                            bunifuPages1.SetPage(4);
                            break;
                        case DialogResult.No:
                            isDivorcedAndHasChildren = false;
                            bunifuPages1.SetPage(5);
                            break;
                    }
                }
                else
                {
                    isDivorced = false;
                    isDivorcedAndHasChildren = false;
                    bunifuPages1.SetPage(5);
                }
            }
        }
        private void hasChildren(object sender, EventArgs e)
        {
            BunifuRadioButton r = (BunifuRadioButton)sender;
            bool expression = (r.Name == "rd_hasChild_yes");
            label_howMany.Visible = expression;
            txt_ChN.Visible = expression;
        }
        private void btn_Next2_Click(object sender,EventArgs e)
        {
            
            try
            {
                if (crud.SpouseVerification(this))
                {
                    if (txt_ChN.Visible)
                    {
                        if (String.IsNullOrEmpty(txt_ChN.Text))
                        {
                            MessageBox.Show("Please enter a number");
                        }
                        NuChildren = int.Parse(txt_ChN.Text);
                        if (NuChildren <= 0) MessageBox.Show("Number must be greater than 0");
                        bunifuPages1.SetPage(4);
                    }
                    else bunifuPages1.SetPage(5);
                }
                    
            }
            catch (FormatException ex)
            {
                MessageBox.Show("Must be a number");
            }
        }
        private void btn_Next3_Click(object sender, EventArgs e)
        {
            if (crud.ChildVerification(this))
            {

                if (count < NuChildren)
                {
                    String gender = rd_ChildGender1.Checked ? "Female" : "Male";
                    int distance = int.Parse(txt_Home_School_Dist.Text);
                    if (distanceUnit.SelectedItem.ToString().Equals("km")) distance *= 1000;
                    Childd c = new Childd(txt_ChildFName.Text, txt_SchoolName.Text, txt_EduLvl.Text, gender, txt_ChildLName.Text, DP_ChildBDate.Value.Date, distance);
                    Children.Add(c);
                    count++;
                    txt_ChildLName.Text = String.Empty;
                    txt_ChildFName.Text = String.Empty;
                    txt_EduLvl.Text = String.Empty;
                    txt_Home_School_Dist.Text = String.Empty;
                    rd_ChildGender1.Checked = false;
                    rd_ChildGender2.Checked = false;
                    if (count == 2) label6.Text = "2nd Child";
                    else if (count == 3) label6.Text = "3rd Child";
                    else label6.Text = count + "th Child";
                }
                else
                {
                    bunifuPages1.SetPage(5);
                }
        }
    }
        
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
            
        }

        private void btn_Google_SignUp_Click(object sender, EventArgs e)
        {
            GoogleAuth g = new GoogleAuth(this);
            g.login();
        }

        private void btn_twitter_signUp_Click(object sender, EventArgs e)
        {
            TwitterAuth t = new TwitterAuth(this);
            t.login();
        }
    }
}

