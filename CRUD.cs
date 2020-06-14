using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Data;
using System.IO;

namespace Organize_Me
{
    class CRUD
    {
        private SqlConnection con;
        private SqlCommand cmd;
        private string ConnectionString;
        public int currentId { get; set; }
        public CRUD()
        {
            this.currentId = 0;
            this.ConnectionString = @"Data Source=.\;Initial Catalog=OrganizeMeDB;Integrated Security=True;Pooling=False";
        }
        public bool SignInVerification(Form1 f)
        {
            String email = f.txt_SignInEmail.Text;
            String password = f.txt_SignInPassword.Text;
            if (String.IsNullOrWhiteSpace(email))
            {
                Bunifu.Snackbar.Show(f, "Please enter an e-mail", 3000, Snackbar.Views.SnackbarDesigner.MessageTypes.Error);
            }
            if (String.IsNullOrWhiteSpace(password))
            {
                Bunifu.Snackbar.Show(f, "Please enter a password", 3000, Snackbar.Views.SnackbarDesigner.MessageTypes.Error);
            }
            try
            {
                con = new SqlConnection();
                con.ConnectionString = this.ConnectionString;
                cmd = new SqlCommand();
                con.Open();
                cmd.CommandText = "SELECT Id,Mail,Password FROM [User] WHERE Mail = @email AND Password = @password";
                cmd.Connection = con;
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@password", password);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    this.currentId = reader.GetInt32(0);
                    Bunifu.Snackbar.Show(f, "Login successful", 3000, Snackbar.Views.SnackbarDesigner.MessageTypes.Success);

                    if (con.State == ConnectionState.Open)
                    {
                        con.Dispose();
                    }
                    return true;
                }
                else
                {
                    Bunifu.Snackbar.Show(f, "Invalid credentials", 3000, Snackbar.Views.SnackbarDesigner.MessageTypes.Error);
                    if (con.State == ConnectionState.Open)
                    {
                        con.Dispose();
                    }
                    return false;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

        }
        public bool ContinueBtnVerification(Form1 f)
        {
            bool verif = true;
            String email=f.txt_SignUpEmail.Text;
            String password=f.txt_SignUpPassword.Text;
            String confirm_password = f.txt_ConfirmPassword.Text;
            Regex reg = new Regex(@"^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,6}$", RegexOptions.IgnoreCase);  

            if (String.IsNullOrWhiteSpace(email))
            {
                Bunifu.Snackbar.Show(f, "Please enter an e-mail", 3000, Snackbar.Views.SnackbarDesigner.MessageTypes.Error);
                verif = false;
            }else if (!reg.IsMatch(email))
            {
                Bunifu.Snackbar.Show(f, "Please enter a valid e-mail", 3000, Snackbar.Views.SnackbarDesigner.MessageTypes.Error);
                verif = false;
            }
            if (String.IsNullOrWhiteSpace(password) || String.IsNullOrWhiteSpace(confirm_password))
            {
                Bunifu.Snackbar.Show(f, "Please enter a password", 3000, Snackbar.Views.SnackbarDesigner.MessageTypes.Error);
                verif = false;
            }
            if (String.Compare(password, confirm_password) != 0)
            {
                Bunifu.Snackbar.Show(f, "Passwords must match", 3000, Snackbar.Views.SnackbarDesigner.MessageTypes.Error);
                verif = false;
            }
            if (verif)
            {
                try
                {
                    con = new SqlConnection();
                    con.ConnectionString = this.ConnectionString;
                    con.Open();
                    cmd = new SqlCommand();
                    cmd.CommandText = "SELECT Id FROM [User] WHERE Mail=@email";
                    cmd.Connection = con;
                    cmd.Parameters.AddWithValue("@email", email);
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        Bunifu.Snackbar.Show(f, "Email already exists", 3000, Snackbar.Views.SnackbarDesigner.MessageTypes.Error);
                        verif = false;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    verif = false;
                }
                finally
                {
                    if (con.State == ConnectionState.Open)
                    {
                        con.Close();
                    }
                }
            }
            return verif;




        }
        public bool InfoVerification(Form1 f)
        {
            bool verif = true;
            String F_Name = f.txt_UserFName.Text;
            String L_Name = f.txt_UserLName.Text;
            DateTime Birth_Date = f.DP_bDate.Value;
            String Address = f.txt_Adress.Text;
            String Job = f.txt_UserJob.Text;
            if (String.IsNullOrWhiteSpace(F_Name) || String.IsNullOrWhiteSpace(L_Name) || String.IsNullOrWhiteSpace(Address) || String.IsNullOrWhiteSpace(Job))
            {
                verif = false;
                Bunifu.Snackbar.Show(f, "Please fill out all fields", 3000, Snackbar.Views.SnackbarDesigner.MessageTypes.Error);
            }
            if((DateTime.Now.Year - Birth_Date.Year) < 18)
            {
                verif = false;
                Bunifu.Snackbar.Show(f, "Please enter a valid birth date", 3000, Snackbar.Views.SnackbarDesigner.MessageTypes.Error);          
            }
            return verif;

        }
        public void InsertUser(Form1 f)
        {
            try
            {
                con = new SqlConnection();
                con.ConnectionString = this.ConnectionString;
                con.Open();
                this.cmd = new SqlCommand();
                cmd.CommandText = "INSERT INTO [User] VALUES(@password,@f_name,@l_name,@gender,@job,@photo,@email,@address,@marital_status,@childNum,@BDate)";
                cmd.Connection = con;
                string m_status = f.cb_MaritalStatus.SelectedItem.ToString();
                string gender = f.rd_UserGender1.Checked ? "Female" : "Male";
                DateTime BirthDate = f.DP_bDate.Value.Date;
                int ChildNumber = f.rd_hasChild_yes.Checked ? int.Parse(f.txt_ChN.Text) : 0;
                MemoryStream ms = new MemoryStream();
                f.userImage.Save(ms, f.userImage.RawFormat);
                byte[] buffer= ms.ToArray();
                ms.Close();   
                cmd.Parameters.AddWithValue("@password", f.txt_SignUpPassword.Text);
                cmd.Parameters.AddWithValue("@f_name",f.txt_UserFName.Text);
                cmd.Parameters.AddWithValue("@l_name", f.txt_UserLName.Text);
                cmd.Parameters.AddWithValue("@gender", gender);
                cmd.Parameters.AddWithValue("@job", f.txt_UserJob.Text);
                cmd.Parameters.AddWithValue("@photo",buffer);
                cmd.Parameters.AddWithValue("@email", f.txt_SignUpEmail.Text);
                cmd.Parameters.AddWithValue("@address", f.txt_Adress.Text);
                cmd.Parameters.AddWithValue("@marital_status", m_status);
                cmd.Parameters.AddWithValue("@childNum", ChildNumber);
                cmd.Parameters.AddWithValue("@BDate", BirthDate);
                cmd.ExecuteNonQuery();
                Bunifu.Snackbar.Show(f, "User added", 3000, Snackbar.Views.SnackbarDesigner.MessageTypes.Success);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }
        public int getCurrentUserId(String email)
        {
            int CurrentId = 0;
            con = new SqlConnection();
            con.ConnectionString = this.ConnectionString;
            con.Open();
            this.cmd = new SqlCommand();
            cmd.CommandText = "SELECT Id FROM [User] WHERE Mail=@email";
            cmd.Connection = con;
            cmd.Parameters.AddWithValue("@email", email);
            SqlDataReader reader = cmd.ExecuteReader();
            if(reader.Read()) CurrentId = reader.GetInt32(0);
            reader.Close();
            cmd.Dispose();
            con.Close();
            return CurrentId; 
        }
        public bool SpouseVerification(Form1 f)
        {
            bool verif = true;
            if (String.IsNullOrWhiteSpace(f.txt_SpouseFName.Text)||String.IsNullOrWhiteSpace(f.txt_SpouseLName.Text)||String.IsNullOrWhiteSpace(f.txt_SpouseJob.Text))
                
            {
                Bunifu.Snackbar.Show(f, "Please fill out all fields", 3000, Snackbar.Views.SnackbarDesigner.MessageTypes.Error);
                verif = false;
            }
            if ((DateTime.Now.Year - f.DP_SpouseBdate.Value.Year) < 18)
            {
                verif = false;
                Bunifu.Snackbar.Show(f, "Please enter a valid birth date", 3000, Snackbar.Views.SnackbarDesigner.MessageTypes.Error);
            }
            return verif;


        }
        public void InsertSpouse(Form1 f)
        {

            try
            {
                con = new SqlConnection();
                con.ConnectionString = this.ConnectionString;
                con.Open();
                this.cmd = new SqlCommand();
                cmd.CommandText = "INSERT INTO Spouse VALUES(@UserId,@FName,@LName,@BDay,@Job,@Health,@Gender)";
                string gender = f.rd_SpouseGender1.Checked? "Female" : "Male";
                cmd.Connection = con;
                cmd.Parameters.AddWithValue("@UserId", f.CurrentUserId);
                cmd.Parameters.AddWithValue("@FName", f.txt_SpouseFName.Text);
                cmd.Parameters.AddWithValue("@LName", f.txt_SpouseLName.Text);
                cmd.Parameters.AddWithValue("@BDay", f.DP_SpouseBdate.Value.Date);
                cmd.Parameters.AddWithValue("@Job", f.txt_SpouseJob.Text);
                cmd.Parameters.AddWithValue("@Health", f.cb_SpouseHealth.SelectedItem.ToString());
                cmd.Parameters.AddWithValue("@Gender", gender);
                cmd.ExecuteNonQuery();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (con.State == ConnectionState.Open) con.Close();
            }
        }
        public bool ChildVerification(Form1 f)
        {
            bool verif = true;
            if(String.IsNullOrWhiteSpace(f.txt_ChildFName.Text)|| String.IsNullOrWhiteSpace(f.txt_ChildLName.Text) || String.IsNullOrWhiteSpace(f.txt_EduLvl.Text)
                || String.IsNullOrWhiteSpace(f.txt_SchoolName.Text) || String.IsNullOrWhiteSpace(f.txt_Home_School_Dist.Text))
            {
                Bunifu.Snackbar.Show(f, "Please fill out all fields", 3000, Snackbar.Views.SnackbarDesigner.MessageTypes.Error);
                verif = false;
            }
            try { 


                int userId = getCurrentUserId(f.txt_SignUpEmail.Text);
                con = new SqlConnection();
                con.ConnectionString = this.ConnectionString;
                cmd = new SqlCommand();
                con.Open();
                cmd.CommandText = "SELECT First_Name,Last_Name FROM [Child] WHERE First_Name = @FirstName AND Last_Name = @LastName AND IdUser=@UserId";
                cmd.Connection = con;
                cmd.Parameters.AddWithValue("@FirstName", f.txt_ChildFName.Text);
                cmd.Parameters.AddWithValue("@LastName", f.txt_ChildLName.Text);
                cmd.Parameters.AddWithValue("@UserId",userId);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows) {
                    Bunifu.Snackbar.Show(f, "Two children can't have the same name", 3000, Snackbar.Views.SnackbarDesigner.MessageTypes.Error);
                    verif = false;
                }
                
                reader.Close();
                cmd.Dispose();
                con.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            try
            {
                int n = int.Parse(f.txt_Home_School_Dist.Text);
            }
            catch (FormatException)
            {
                Bunifu.Snackbar.Show(f, "Invalid format", 3000, Snackbar.Views.SnackbarDesigner.MessageTypes.Error);
                verif = false;
            }
            return verif;
        }
        public void InsertChildren(Form1 f)
        {
            try
            {
                con = new SqlConnection();
                con.ConnectionString = this.ConnectionString;
                con.Open();
                foreach (Childd c in f.Children)
                {
                    cmd = new SqlCommand();
                    cmd.CommandText = "INSERT INTO Child VALUES(@UserId,@FName,@School_Name,@Education_Level,@Gender,@LName,@BDate,@Home_School_Distance)";

                    cmd.Connection = con;
                    cmd.Parameters.AddWithValue("@UserId", f.CurrentUserId);
                    cmd.Parameters.AddWithValue("@FName", c.FirstName);
                    cmd.Parameters.AddWithValue("@LName", c.LastName);
                    cmd.Parameters.AddWithValue("@BDate", c.BirthDate);
                    cmd.Parameters.AddWithValue("@Education_Level", c.EduLvl);
                    cmd.Parameters.AddWithValue("@School_Name", c.SchoolName);                    
                    cmd.Parameters.AddWithValue("@Home_School_Distance", c.Home_School_Distance);
                    cmd.Parameters.AddWithValue("@Gender", c.Gendder);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (con.State == ConnectionState.Open) con.Close();
            }
        }
        public bool ParentsVerification(Form1 f)
        {
            if(String.IsNullOrWhiteSpace(f.txt_FatherFName.Text)||String.IsNullOrWhiteSpace(f.txt_FatherLName.Text)||
                String.IsNullOrWhiteSpace(f.txt_MotherFName.Text) || String.IsNullOrWhiteSpace(f.txt_MotherLName.Text))
            {
                Bunifu.Snackbar.Show(f, "Please fill out all fields", 3000, Snackbar.Views.SnackbarDesigner.MessageTypes.Error);
                return false;           
            }
            else if ((DateTime.Now.Year - f.DP_MotherBDate.Value.Year) < 18 || (DateTime.Now.Year - f.DP_FatherBDate.Value.Year) < 18)
            {
                Bunifu.Snackbar.Show(f, "Please enter a valid birth date", 3000, Snackbar.Views.SnackbarDesigner.MessageTypes.Error);
                return false;
            }
            return true;
        }
        public void InsertParents(Form1 f)
        {
            try
            {
                con = new SqlConnection();
                con.ConnectionString = this.ConnectionString;
                con.Open();
                cmd = new SqlCommand();
                cmd.CommandText = "INSERT INTO Parent VALUES(@UserId,@Father_FName,@Mother_FName,@Mother_LName,@Housing,@Father_LName,@MotherBDate,@FatherBDate)";
                String housing;
                if (f.rd_FDom1.Checked && f.rd_MDom1.Checked) housing = "Both";
                else if (f.rd_FDom1.Checked && f.rd_Mdom2.Checked) housing = "Father only";
                else if (f.rd_FDom2.Checked && f.rd_MDom1.Checked) housing = "Mother only";
                else housing = "None";

                cmd.Connection = con;
                cmd.Parameters.AddWithValue("@UserId", f.CurrentUserId);
                cmd.Parameters.AddWithValue("@Father_FName", f.txt_FatherFName.Text);
                cmd.Parameters.AddWithValue("@Mother_FName", f.txt_MotherFName.Text);
                cmd.Parameters.AddWithValue("@Mother_LName", f.txt_MotherLName.Text);
                cmd.Parameters.AddWithValue("@Housing", housing);
                cmd.Parameters.AddWithValue("@Father_LName", f.txt_FatherLName.Text);
                cmd.Parameters.AddWithValue("@MotherBDate", f.DP_MotherBDate.Value.Date);
                cmd.Parameters.AddWithValue("@FatherBDate", f.DP_FatherBDate.Value.Date);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }
    }
    
}
