using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Organize_Me
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            bunifuFormDock1.SubscribeControlToDragEvents(bunifuGradientPanel1);
            bunifuFormDock1.SubscribeControlToDragEvents(tabPage1);
            bunifuFormDock1.SubscribeControlToDragEvents(tabPage2);
            bunifuRadioButton12.Checked = true;
            bunifuRadioButton3.Checked = true;
        }

        internal static int compteur = 1;

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
            Console.WriteLine("test");
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
            if (bunifuRadioButton3.Checked) bunifuPages1.SetPage(3);
            else bunifuPages1.SetPage(5);
        }

        

        private void bunifuButton7_Click(object sender, EventArgs e)
        {
            bunifuPages1.SetPage(4);

        }

        private void bunifuButton8_Click(object sender, EventArgs e)
        {
            int nombreEnfants = comboBox1.SelectedIndex+1;


            if (compteur < nombreEnfants)
            {
                compteur++;
                bunifuTextBox14.Text = String.Empty;
                bunifuTextBox15.Text = String.Empty;
                bunifuTextBox16.Text = String.Empty;
                bunifuTextBox17.Text = String.Empty;
                bunifuRadioButton7.Checked = false;
                bunifuRadioButton8.Checked = false;
                if (compteur == 2) label6.Text = "2nd Child";
                else if (compteur == 3) label6.Text = "3rd Child";
                else label6.Text = compteur + "th Child";
            }
            else if(compteur==nombreEnfants)
            {
                bunifuButton8.Text = "SIGN UP";
                compteur++;
            }
            else
            {
                Form2 f2 = new Form2();
                f2.Show();
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

        private void bunifuImageButton1_Click(object sender, EventArgs e)
        {
            bunifuPages1.SetPage(6);
        }

        private void bunifuImageButton2_Click(object sender, EventArgs e)
        {
            bunifuPages1.SetPage(7);
        }

        private void bunifuImageButton3_Click(object sender, EventArgs e)
        {
            bunifuPages1.SetPage(8);
        }

        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            bunifuPages1.SetPage(0);
        }
    }
}
