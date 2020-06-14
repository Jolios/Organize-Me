using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Organize_Me
{
    public partial class AddChildForm : Form
    {
        private Form2 f;
        private int currentUserId;
        public AddChildForm(int currentUserId,Form2 f)
        {
            InitializeComponent();
            bunifuFormDock1.SubscribeControlToDragEvents(panel1);
            this.currentUserId = currentUserId;
            this.f = f;
        }

        private bool verification()
        {
            bool verif = true;
            if (String.IsNullOrWhiteSpace(txt_ChildFName.Text) || String.IsNullOrWhiteSpace(txt_ChildLName.Text) || String.IsNullOrWhiteSpace(txt_EduLvl.Text)
                || String.IsNullOrWhiteSpace(txt_School_Name.Text) || String.IsNullOrWhiteSpace(txt_Home_School_Dist.Text))
            {
                Bunifu.Snackbar.Show(this, "Please fill out all fields", 3000, Snackbar.Views.SnackbarDesigner.MessageTypes.Error);
                verif = false;
            }
            try
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = @"Data Source = DESKTOP-HSUI4QK; Initial Catalog = OrganizeMeDB; Integrated Security = True; Pooling = False"; 
                SqlCommand cmd = new SqlCommand();
                con.Open();
                cmd.CommandText = "SELECT First_Name,Last_Name FROM [Child] WHERE First_Name = @FirstName AND Last_Name = @LastName AND IdUser=@UserId";
                cmd.Connection = con;
                cmd.Parameters.AddWithValue("@FirstName", this.txt_ChildFName.Text);
                cmd.Parameters.AddWithValue("@LastName", this.txt_ChildLName.Text);
                cmd.Parameters.AddWithValue("@UserId", this.currentUserId);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    Bunifu.Snackbar.Show(this, "Two children can't have the same name", 3000, Snackbar.Views.SnackbarDesigner.MessageTypes.Error);
                    verif=false;

                }
                reader.Close();
                cmd.Dispose();
                con.Close();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            try
            {
                int n = int.Parse(txt_Home_School_Dist.Text);
            }
            catch (FormatException)
            {
                Bunifu.Snackbar.Show(this, "Invalid format", 3000, Snackbar.Views.SnackbarDesigner.MessageTypes.Error);
                verif = false;
            }
            return verif;
        }

        private void btn_AddChild_Click(object sender, EventArgs e)
        {
            if (verification())
            {
                try
                {
                    SqlConnection con = new SqlConnection(@"Data Source = DESKTOP-HSUI4QK; Initial Catalog = OrganizeMeDB; Integrated Security = True; Pooling = False");
                    SqlCommand cmd = new SqlCommand();
                    con.Open();
                    cmd.Connection = con;
                    cmd.CommandText = "INSERT INTO Child VALUES(@UserId,@FirstName,@SchoolName,@EduLvl,@Gender,@LastName,@BirthDate,@HomeSchoolDist)";
                    cmd.Parameters.AddWithValue("@UserId", currentUserId);
                    cmd.Parameters.AddWithValue("@FirstName", txt_ChildFName.Text);
                    cmd.Parameters.AddWithValue("@LastName", txt_ChildLName.Text);
                    cmd.Parameters.AddWithValue("@SchoolName", txt_School_Name.Text);
                    cmd.Parameters.AddWithValue("@EduLvl", txt_EduLvl.Text);
                    string gender = rd_ChildGender1.Checked ? "Female" : "Male";
                    cmd.Parameters.AddWithValue("@Gender", gender);
                    cmd.Parameters.AddWithValue("@BirthDate", DP_childBdate.Value.Date);
                    int distance = int.Parse(txt_Home_School_Dist.Text);
                    if (distanceUnit.SelectedItem.ToString().Equals("km")) distance *= 1000;
                    cmd.Parameters.AddWithValue("@HomeSchoolDist", distance);
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = "UPDATE [User] SET Children_Num=Children_Num+1 WHERE Id=@UserID";
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    con.Close();
                    Bunifu.Snackbar.Show(this, "Child added", 3000, Snackbar.Views.SnackbarDesigner.MessageTypes.Success);
                    f.label_Chilrden_Click(new object(), new EventArgs());
                    f.bunifuPages2.Update();
                    f.bunifuPages2.Refresh();
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void AddChildForm_Load(object sender, EventArgs e)
        {
            Font font = new Font("Segoe UI", 14, FontStyle.Bold);
            bunifuLabel6.Font = font;
            font = new Font("Segoe UI", 10);
            Label_Child_G1.Font = font;
            Label_Child_G2.Font = font;
            font = new Font("Segoe UI", 12);
            bunifuLabel7.Font = font;
            font = new Font("Segoe UI Semibold", 14);
            bunifuLabel9.Font = font;
            bunifuLabel19.Font = font;
            bunifuLabel21.Font = font;
            bunifuLabel22.Font = font;
            bunifuLabel23.Font = font;
            bunifuLabel24.Font = font;
            bunifuLabel27.Font = font;

        }
    }
}
