using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Bunifu.UI.WinForms.BunifuTextbox;
using Bunifu.UI.WinForms.BunifuButton;
namespace Organize_Me
{
    public partial class Form1 : Form
    {
        internal static int count = 1;
        private Form2 f2 = new Form2();
        internal static int NuChildren = 0; 
        private Font segoe = new Font("Segoe UI", 10);

        private void passwordChar(BunifuTextBox txt)
        {
            txt.UseSystemPasswordChar = String.IsNullOrEmpty(txt.Text) ? false : true;
        }

        public Form1()
        {
            InitializeComponent();
            bunifuFormDock1.SubscribeControlToDragEvents(bunifuGradientPanel1);
            bunifuFormDock1.SubscribeControlToDragEvents(tabPage1);
            bunifuFormDock1.SubscribeControlToDragEvents(tabPage2);
            rd_MDom1.Checked = true;
            rd_Married1.Checked = true;
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            Label_Info_M1.Font = segoe;
            Label_Info_M2.Font = segoe;
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


       private void bunifuButton_Click(object sender, EventArgs e)
        {
            BunifuButton b = (BunifuButton) sender;
            switch (b.Name)
            {
                case "btn_GradientSignIN" : bunifuPages1.SetPage(0);
                                            break;
                case "btn_GradientSignUp" : bunifuPages1.SetPage(1);
                                            break;
                case "btn_SignIn"         : f2.Show();
                                            break;
                case "btn_Continue"       : bunifuPages1.SetPage(2);
                                            break;
                case "btn_Back1"          : bunifuPages1.SetPage(1);
                                            break;
                case "btn_Back2"          : bunifuPages1.SetPage(2);
                                            break;
                case "btn_Back3"          : bunifuPages1.SetPage(3);
                                            break;
                case "btn_Back4"          : if (NuChildren == 0) bunifuPages1.SetPage(3);
                                            else bunifuPages1.SetPage(4);
                                            break;
                case "btn_Next2"          : bunifuPages1.SetPage(4);
                                            break;
                case "btn_Next3"          : bunifuPages1.SetPage(5);
                                            break;
                case "btn_SignUp2"        : f2.Show();
                                            break;

                default:
                    break;
            }
        }
       

        

        private void btn_Next2_Click(object sender,EventArgs e)
        {
            if (String.IsNullOrEmpty(txt_ChN.Text))
            {
                MessageBox.Show("Please enter a number");
            }
            else
            {
                try
                {
                    NuChildren = int.Parse(txt_ChN.Text);
                    if (NuChildren == 0) bunifuPages1.SetPage(5);
                    else bunifuPages1.SetPage(4);
                }
                catch (FormatException ex)
                {
                    MessageBox.Show("Must be a number");
                }
            }
        }

        private void bunifuButton6_Click(object sender, EventArgs e)
        {
            OpenFileDialog opnfd = new OpenFileDialog();
            opnfd.Filter = "Image Files (*.jpg;*.jpeg;.*.gif;)|*.jpg;*.jpeg;.*.gif";
            opnfd.InitialDirectory = "D:\\";
            if (opnfd.ShowDialog() == DialogResult.OK)
            {
                MessageBox.Show(opnfd.FileName);
            }
        }

        

        private void bunifuButton5_Click(object sender, EventArgs e)
        {
            if (rd_Married1.Checked) bunifuPages1.SetPage(3);
            else bunifuPages1.SetPage(5);
        }

        

        private void bunifuButton8_Click(object sender, EventArgs e)
        {
            
                if (count < NuChildren)
                {
                    count++;
                    txt_ChildLName.Text = String.Empty;
                    txt_ChildFName.Text = String.Empty;
                    txt_SchoolName.Text = String.Empty;
                    txt_ChildGender.Text = String.Empty;
                    rd_ChildGender1.Checked = false;
                    txt_ChildGender2.Checked = false;
                    if (count == 2) label6.Text = "2nd Child";
                    else if (count == 3) label6.Text = "3rd Child";
                    else label6.Text = count + "th Child";
                }
                else
                {
                    bunifuPages1.SetPage(5);
                }
            
            
            
        }
        
        private void bunifuButton17_Click(object sender, EventArgs e)
        {
            this.Close();
            
        }

        private void bunifuButton18_Click(object sender, EventArgs e)
        {
            GoogleAuth g = new GoogleAuth();
            g.Login();
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
    }
}

