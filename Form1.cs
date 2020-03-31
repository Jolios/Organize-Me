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

namespace Organize_Me
{
    public partial class Form1 : Form
    {
        internal static int compteur = 1;
        private Form2 f2 = new Form2();
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
            bunifuLabel6.Font = segoe;
            bunfifuLabel50.Font = segoe;
            bunifuLabel9.Font = segoe;
            bunifuLabel10.Font = segoe;
            bunifuLabel18.Font = segoe;
            bunifuLabel19.Font = segoe;
            bunifuLabel25.Font = segoe;
            bunifuLabel33.Font = segoe;
            bunifuLabel34.Font = segoe;
            bunifuLabel35.Font = segoe;
            bunifuLabel36.Font = segoe;
            bunifuLabel38.Font = segoe;
        }




        private void bunifuButton3_Click(object sender, EventArgs e)
        {
            bunifuPages1.SetPage(1);
        }

       
        private void bunifuButton4_Click(object sender, EventArgs e)
        {
            bunifuPages1.SetPage(2);
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

        

        private void bunifuButton7_Click(object sender, EventArgs e)
        {
            bunifuPages1.SetPage(4);

        }

        private void bunifuButton8_Click(object sender, EventArgs e)
        {
            try
            {
                int nombreEnfants = int.Parse(txt_ChN.Text);
                if (compteur < nombreEnfants)
                {
                    compteur++;
                    txt_ChildLName.Text = String.Empty;
                    txt_ChildFName.Text = String.Empty;
                    txt_SchoolName.Text = String.Empty;
                    txt_ChildGender.Text = String.Empty;
                    rd_ChildGender1.Checked = false;
                    txt_ChildGender2.Checked = false;
                    if (compteur == 2) label6.Text = "2nd Child";
                    else if (compteur == 3) label6.Text = "3rd Child";
                    else label6.Text = compteur + "th Child";
                }
                else if (compteur == nombreEnfants)
                {
                    btn_Next3.Text = "SIGN UP";
                    compteur++;
                }
                else
                {

                    f2.Show();
                }
            }
            catch(FormatException ex)
            {
                MessageBox.Show("Must be a number");
            }

            
        }

        private void bunifuButton11_Click(object sender, EventArgs e)
        {
            bunifuPages1.SetPage(1);
        }

        private void bunifuButton9_Click(object sender, EventArgs e)
        {
            bunifuPages1.SetPage(2);
        }

        private void bunifuButton10_Click(object sender, EventArgs e)
        {
            bunifuPages1.SetPage(3);
        }

        

        private void bunifuButton13_Click(object sender, EventArgs e)
        {
            bunifuPages1.SetPage(1);
        }

      

        private void bunifuButton17_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        
        

        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            bunifuPages1.SetPage(0);
        }

        private void bunifuTextBox3_TextChanged(object sender, EventArgs e)
        {
            passwordChar(txt_SignInPassword);

        }



        private void bunifuTextBox4_TextChanged(object sender, EventArgs e)
        {
            passwordChar(txt_SIgnUpPassword);
        }

        private void bunifuTextBox1_TextChanged(object sender, EventArgs e)
        {
            passwordChar(txt_ConfirmPassword);
        }

        private void bunifuButton14_Click(object sender, EventArgs e)
        {
            f2.Show();
        }

        private void bunifuButton15_Click(object sender, EventArgs e)
        {
            f2.Show();
        }

        private void bunifuButton16_Click(object sender, EventArgs e)
        {
            f2.Show();
        }

        
        private void bunifuButton18_Click(object sender, EventArgs e)
        {
            GoogleAuth g = new GoogleAuth();
            g.Login();
        }

        
    }
}

