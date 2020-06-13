extern alias DataViz;
using Bunifu;
using Bunifu.UI.WinForms;
using Bunifu.UI.WinForms.BunifuButton;
using DataViz::Bunifu.DataViz.WinForms;
using EnvDTE;
using Organize_Me.Properties;
using Syncfusion.Windows.Forms.Tools;
using Syncfusion.WinForms.Input;
using Syncfusion.WinForms.Input.Enums;
using Syncfusion.WinForms.Input.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Speech;
using System.Speech.Recognition;

namespace Organize_Me
{
    public partial class Form2 : Form
    {
        [DllImport("user32.dll")]
        private static extern long LockWindowUpdate(IntPtr Handle);
        public bool isListening = true;
        private int CurrentUserId;
        private Image UserImage=Resources.kindpng_4952535;
        private String ConnectionString = @"Data Source = .\; Initial Catalog = OrganizeMeDB; Integrated Security = True; Pooling = False";
        public Form2(int CurrentUserId)
        {
            InitializeComponent();
            bunifuFormDock1.SubscribeControlToDragEvents(SideGradient);
            bunifuFormDock1.SubscribeControlToDragEvents(Header);
            bunifuFormDock1.SubscribeControlToDragEvents(panel1);
            bunifuFormDock1.SubscribeControlToDragEvents(panel2);
            this.CurrentUserId = CurrentUserId;


        }

        private void Form2_Load(object sender, EventArgs args)
        {
            bunifuPages1.SetPage(0);
            edit_fonts();
            render_spline();
            load_circle_bars();
            this.sfCalendar1.SelectionChanged += SfCalendar1_SelectionChanged;
            sfCalendar1.SelectedDate = DateTime.Now.Date;
            this.sfCalendar1.DrawCell += SfCalendar1_DrawCell;
            this.flowLayoutPanel1.AutoScroll = true;
            this.sfScrollFrame1.Control = flowLayoutPanel1;
            try
            {
                SqlConnection con = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand();
                con.Open();
                cmd.CommandText = "SELECT Photo,First_Name+' '+Last_Name as Full_Name FROM [User] WHERE Id=@UserId";
                cmd.Parameters.AddWithValue("@UserId", CurrentUserId);
                cmd.Connection = con;
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    byte[] buffer = (byte[])reader["Photo"];
                    MemoryStream ms = new MemoryStream(buffer);
                    Image img = Image.FromStream(ms);
                    UserImage = img;
                    User_Photo.Image = img;
                    txt_UserName.Text = reader.GetString(1);
                }
                reader.Close();
                cmd.Dispose();
                con.Close();
                txt_UserName.Location = new Point(btn_Profile.Location.X+((btn_Profile.Width-txt_UserName.Width)/2), btn_Profile.Location.Y - 30);
                User_Photo.BorderRadius = 40;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        public void SfCalendar1_SelectionChanged(SfCalendar sender, SelectionChangedEventArgs args)
        {
            
            LockWindowUpdate(this.panel4.Handle);

            this.flowLayoutPanel1.Controls.Clear();
            if (this.sfCalendar1.SelectedDate == null)
            {
                return;
            }
            try
            {
                SqlConnection con = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand();
                con.Open();
                cmd.CommandText = "SELECT * FROM Task WHERE IdUser=@UserId ";
                cmd.Parameters.AddWithValue("@UserId", CurrentUserId);
                cmd.Connection = con;
                TaskCard tc;
                DateTime startDate;
                DateTime endDate;
                String startTime;
                String endTime;
                String RelationToTarget;
                int TargetId;
                DateTime selectedDate = args.NewValue.Value.Date;
                SqlDataReader reader = cmd.ExecuteReader();
                SqlConnection connection;
                SqlCommand command;
                SqlDataReader rd;

                while (reader.Read())
                {
                    tc = new TaskCard(reader.GetInt32(0),this,args);
                    startDate = reader.GetDateTime(3).Date;
                    endDate = reader.GetDateTime(4).Date;
                    startTime = reader.GetString(9);
                    endTime = reader.GetString(10);
                    RelationToTarget = reader.GetString(8);
                    TargetId = reader.GetInt32(7);
                    
                    command = new SqlCommand();
                    connection = new SqlConnection(ConnectionString);
                    connection.Open();
                    command.Connection = connection;
                    if (RelationToTarget.Equals("Father")) command.CommandText = "SELECT Father_FName+' '+Father_LName as Full_Name From Parent WHERE Id=@TargetId";
                    else if (RelationToTarget.Equals("Mother")) command.CommandText = "SELECT Mother_FName+' '+Mother_LName as Full_Name FROM Parent WHERE Id=@TargetId";
                    else
                    {
                        command.CommandText = string.Format("SELECT First_Name +' '+ Last_Name as Full_Name,Gender FROM [{0}] WHERE Id=@TargetId", RelationToTarget);
                    }
                    command.Parameters.AddWithValue("@TargetId", TargetId);
                    rd = command.ExecuteReader();
                    rd.Read();
                    switch (RelationToTarget)
                    {
                        case "Father":
                            tc.bunifuPictureBox1.Image = Resources.Father;
                            break;
                        case "Mother":
                            tc.bunifuPictureBox1.Image = Resources.Mother;
                            break;
                        case "Child":
                            tc.bunifuPictureBox1.Image = Resources.Children;
                            break;
                        case "Spouse":
                            if (rd.GetString(1).Equals("Male")) tc.bunifuPictureBox1.Image = Resources.Spouse_Male;
                            else tc.bunifuPictureBox1.Image = Resources.Spouse_Female;
                            break;
                        case "User":
                            tc.bunifuPictureBox1.Image = UserImage;
                            tc.bunifuPictureBox1.IsCircle = true;
                            break;
                        default: break;
                    }
                    if (startDate == selectedDate)
                    {

                        if (startDate == endDate) tc.bunifuLabel1.Text = startTime + "-" + endTime;
                        else tc.bunifuLabel1.Text = "Today-" + endDate.ToString();
                        tc.label1.Text = reader.GetString(2);
                        tc.label2.Text = rd.GetString(0);
                        this.flowLayoutPanel1.Controls.Add(tc);

                    }
                    else if (endDate == selectedDate)
                    {
                        tc.bunifuLabel1.Text = startDate.ToString() + "-Today";
                        tc.label1.Text = reader.GetString(2);
                        tc.label2.Text = rd.GetString(0);
                        this.flowLayoutPanel1.Controls.Add(tc);
                    }
                    rd.Close();
                    command.Dispose();
                    connection.Close();

                }
                reader.Close();
                cmd.Dispose();
                con.Close();

                LockWindowUpdate(IntPtr.Zero);
                this.flowLayoutPanel1.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SfCalendar1_DrawCell(SfCalendar sender, DrawCellEventArgs args)

        {
            try
            {
                SqlConnection con = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand();
                con.Open();
                cmd.Connection = con;
                cmd.CommandText = "SELECT Start_Date FROM Task WHERE IdUser = @UserId";
                cmd.Parameters.AddWithValue("@UserId", CurrentUserId);
                SqlDataReader reader = cmd.ExecuteReader();
                DateTime startDate;
                while (reader.Read())
                {
                    startDate = reader.GetDateTime(0);
                    if (args.ViewType == CalendarViewType.Month && args.Value.Value.Date == startDate.Date)
                    {
                        args.Handled = true;
                        TextRenderer.DrawText(args.Graphics, args.Value.Value.ToString("%d"), new Font("Segoe UI",
                                                this.sfCalendar1.Style.Cell.CellFont.Size), args.CellBounds, Color.Black);
                        args.Graphics.FillRectangle(new SolidBrush(Color.ForestGreen), new Rectangle((args.CellBounds.X +
                                                (args.CellBounds.Width - args.CellBounds.Width / 2)) - 6,
                                                (args.CellBounds.Y + (args.CellBounds.Height - 20)) + 6, 10, 10));
                    }
                }
                cmd.Dispose();
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, this.panel5.ClientRectangle, Color.FromArgb(204, 204, 204), ButtonBorderStyle.Solid);
        }

        private void bunifuSeparator1_Load(object sender, EventArgs e)
        {
            bunifuSeparator2.Visible = true;
            bunifuSeparator3.Visible = false;
            bunifuSeparator4.Visible = false;
            bunifuPages2.SetPage(0);
            try
            {
                SqlConnection con = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand();
                con.Open();
                cmd.Connection = con;

                cmd.CommandText = "SELECT * FROM Parent WHERE Iduser=@UserId";
                cmd.Parameters.AddWithValue("@UserId", CurrentUserId);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    txt_FatherFName.Text = reader.GetString(2);
                    txt_FatherLName.Text = reader.GetString(6);
                    txt_MotherFName.Text = reader.GetString(3);
                    txt_MotherLName.Text = reader.GetString(4);
                    DP_MotherBDate.Value = reader.GetDateTime(7).Date;
                    DP_FatherBDate.Value = reader.GetDateTime(8).Date;
                    if (reader.GetString(5).Equals("Both"))
                    {
                        rd_Housing1.Checked = true;
                        rd_HousingM1.Checked = true;
                    }
                    else if (reader.GetString(5).Equals("Father only"))
                    {
                        rd_Housing1.Checked = true;
                        rd_HousingM2.Checked = true;
                    }
                    else if (reader.GetString(5).Equals("Mother only"))
                    {
                        rd_Housing2.Checked = true;
                        rd_HousingM1.Checked = true;
                    }
                    else
                    {
                        rd_Housing2.Checked = true;
                        rd_HousingM2.Checked = true;
                    }
                }
                else
                {
                    btn_ParentsEdit.Text = "Add";
                    btn_DelParents.Enabled = false;
                }
                reader.Close();
                cmd.Dispose();
                con.Close();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }

        }

        private void label_Spouse_Click(object sender, EventArgs e)
        {
            bunifuSeparator2.Visible = false;
            bunifuSeparator3.Visible = true;
            bunifuSeparator4.Visible = false;
            bunifuPages2.SetPage(1);
            try
            {
                SqlConnection con = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand();
                con.Open();
                cmd.CommandText = "SELECT * FROM Spouse WHERE IdUser=@UserId";
                cmd.Parameters.AddWithValue("@UserId", CurrentUserId);
                cmd.Connection = con;
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    txt_SpouseFName.Text = reader.GetString(2);
                    txt_SpouseLName.Text = reader.GetString(3);
                    DP_SpouseBDate.Value = reader.GetDateTime(4);
                    txt_SpouseJob.Text = reader.GetString(5);
                    cb_SpouseHealth.SelectedItem = reader.GetString(6);
                    rd_SpouseGender1.Checked = reader.GetString(7).Equals("Female") ? true : false;
                    rd_SpouseGender2.Checked = !rd_SpouseGender1.Checked;
                }
                else
                {
                    btn_SpouseEdit.Text = "Add";
                    btn_DelSpouse.Enabled = false;
                    btn_DelSpouse.DisabledFillColor = Color.DimGray;
                    btn_DelSpouse.DisabledBorderColor = Color.DimGray;
                    btn_DelSpouse.DisabledForecolor = Color.White;
                }
                reader.Close();
                cmd.Dispose();
                con.Close();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        public void label_Chilrden_Click(object sender, EventArgs e)
        {
            bunifuSeparator2.Visible = false;
            bunifuSeparator3.Visible = false;
            bunifuSeparator4.Visible = true;
            bunifuPages2.SetPage(2);
            try
            {
                SqlConnection con = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand();
                con.Open();
                cmd.Connection = con;
                List<String> names = new List<string>();
                DataTable dt = new DataTable();
                cmd.CommandText = "SELECT Id,First_Name+' '+Last_Name as Full_Name FROM Child WHERE IdUser=@UserId";
                cmd.Parameters.AddWithValue("@UserId", CurrentUserId);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    btn_DeleteChild.Enabled = true;
                    btn_ChildrenEdit.Enabled = true;
                    dt.Load(reader);
                    for (int i = 0; i < dt.Rows.Count ; i++) names.Add(dt.Rows[i].ItemArray[1].ToString());
                    ChildrenNames.DataSource = names;
                    
                }
                else
                {
                    btn_DeleteChild.Enabled = false;
                    btn_ChildrenEdit.Enabled = false;
                    ChildrenNames.DataSource=null;
                    txt_ChildFName.Text = string.Empty;
                    txt_ChildLName.Text = string.Empty;
                    txt_SchoolName.Text = string.Empty;
                    txt_EduLvl.Text = string.Empty;
                    txt_Home_School_Dist.Text = string.Empty;
                    DP_ChildBDate.Value = DateTime.Now;
                    distanceUnit.SelectedItem = "";


                }
                dt.Dispose();
                reader.Close();
                cmd.Dispose();
                con.Close();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void ChildrenNames_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ChildrenNames.SelectedValue is null) return;
            string selectedvalue = ChildrenNames.SelectedValue.ToString();
            String First_Name = selectedvalue.Substring(0, selectedvalue.IndexOf(" "));
            String Last_Name = selectedvalue.Substring(selectedvalue.IndexOf(" ") + 1);
            try
            {
                SqlConnection con = new SqlConnection(ConnectionString);
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "SELECT * FROM Child WHERE IdUser=@UserId AND First_Name=@FirstName AND Last_Name=@LastName";
                cmd.Parameters.AddWithValue("@FirstName", First_Name);
                cmd.Parameters.AddWithValue("@LastName", Last_Name);
                cmd.Parameters.AddWithValue("@UserId", CurrentUserId);


                cmd.Connection = con;
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    txt_ChildFName.Text = reader.GetString(2);
                    txt_ChildLName.Text = reader.GetString(6);
                    DP_ChildBDate.Value = reader.GetDateTime(7);
                    rd_ChildGender1.Checked = reader.GetString(5).Equals("Female") ? true : false;
                    rd_ChildGender2.Checked = !rd_ChildGender1.Checked;

                    txt_EduLvl.Text = reader.GetString(4);
                    txt_SchoolName.Text = reader.GetString(3);
                    int distance = reader.GetInt32(8);
                    if (distance >= 1000)
                    {
                        distance /= 1000;
                        txt_Home_School_Dist.Text = distance.ToString();
                        distanceUnit.SelectedItem = "km";
                    }
                    else
                    {
                        txt_Home_School_Dist.Text = distance.ToString();
                        distanceUnit.SelectedItem = "m";
                    }
                }
                reader.Close();
                cmd.Dispose();
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void label_Parents_Click(object sender, EventArgs e)
        {
            bunifuPages2.SetPage(0);
            bunifuSeparator2.Visible = true;
            bunifuSeparator3.Visible = false;
            bunifuSeparator4.Visible = false;
        }

        public void btn_Tasks_Click(object sender, EventArgs e)
        {
            bunifuPages1.SetPage(2);
            bunifuDataGridView1.Rows.Clear();
            try
            {
                SqlConnection con = new SqlConnection(@"Data Source = .\; Initial Catalog = OrganizeMeDB;MultipleActiveResultSets=true; Integrated Security = True; Pooling = False");
                SqlCommand cmd = new SqlCommand();
                con.Open();
                cmd.Connection = con;
                cmd.CommandText = "SELECT * FROM Task WHERE IdUser=@UserId";
                cmd.Parameters.AddWithValue("@UserId", CurrentUserId);
                SqlDataReader reader = cmd.ExecuteReader();
                int i = 0;
                ArrayList list = new ArrayList();
                while (reader.Read())
                {

                    list.Add(reader.GetInt32(0).ToString());
                    list.Add(reader.GetString(2));
                    list.Add(reader.GetString(6));
                    list.Add(reader.GetDateTime(3).ToString("dd-MM-yyyy"));
                    list.Add(reader.GetString(5));
                    list.Add(reader.GetInt32(7).ToString());
                    list.Add(reader.GetString(8));
                    if (reader.GetString(8).Equals("User")) list.Add("Myself");
                    else
                    { 
                        SqlCommand command = new SqlCommand();
                        command.Connection = con;
                        if (reader.GetString(8).Equals("Father")) command.CommandText = "SELECT Father_FName + ' '+Father_LName+' (Father)' as Full_Name FROM Parent WHERE Id=@TargetId";
                        else if (reader.GetString(8).Equals("Mother")) command.CommandText = "SELECT Mother_FName + ' '+Mother_LName+' (Mother)' as Full_Name FROM Parent WHERE Id=@TargetId";
                        else if (reader.GetString(8).Equals("Spouse")) command.CommandText = "SELECT First_Name + ' '+Last_Name+' (Spouse)' as Full_Name FROM Spouse WHERE Id=@TargetId";
                        else if (reader.GetString(8).Equals("Child")) command.CommandText = "SELECT First_Name + ' '+Last_Name+' (Child)' as Full_Name FROM Child WHERE Id=@TargetId";
                        command.Parameters.AddWithValue("@TargetId", reader.GetInt32(7));
                        SqlDataReader rd = command.ExecuteReader();
                        rd.Read();
                        list.Add(rd.GetString(0));
                        rd.Close();
                        command.Dispose();
                    }
                    list.Add(reader.GetInt32(1).ToString());
                    list.Add(reader.GetDateTime(4).ToString("dd-MM-yyyy"));
                    list.Add(reader.GetString(9));
                    list.Add(reader.GetString(10));
                    list.Add(reader.GetDateTime(3).ToString("dd-MM-yyyy") + " " + reader.GetString(9));
                    list.Add(reader.GetDateTime(4).ToString("dd-MM-yyyy") + " " + reader.GetString(10));
                    float toleratedTime = (float)reader.GetDouble(11);
                    if (toleratedTime < 1)
                    {
                        toleratedTime *= 60;
                        list.Add(toleratedTime.ToString() + " minutes");
                    }
                    else list.Add(toleratedTime.ToString() + " hours");
                    bunifuDataGridView1.Rows.Add(list.ToArray());
                    list.Clear();
                    i++;
                }
                reader.Close();
                cmd.Dispose();
                con.Close();
                bunifuDataGridView1.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void btn_Profile_Click(object sender, EventArgs e)
        {
            bunifuPages1.SetPage(4);
            try
            {
                SqlConnection con = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand();
                con.Open();
                cmd.Connection = con;
                cmd.CommandText = "SELECT * FROM [User] WHERE Id=@UserId";
                cmd.Parameters.AddWithValue("@UserId", CurrentUserId);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    txt_Email.Text = reader.GetString(7);
                    txt_Password.Text = reader.GetString(1);
                    txt_FirstName.Text = reader.GetString(2);
                    txt_LastName.Text = reader.GetString(3);
                    BirthDate.Value = reader.GetDateTime(11).Date;
                    txt_Job.Text = reader.GetString(5);
                    txt_address.Text = reader.GetString(8);
                    cb_MaritalStatus.SelectedItem = reader.GetString(9);
                    byte[] buffer = (byte[])reader["Photo"];
                    MemoryStream ms = new MemoryStream(buffer);
                    Image img = Image.FromStream(ms);
                    pb_user.Image = img;
                    pb_user.IsCircle = true;
                }
                reader.Close();
                cmd.Dispose();
                con.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void btn_Family_Click(object sender, EventArgs e)
        {
            bunifuPages1.SetPage(3);
        }

        public void btn_Dashboard_Click(object sender, EventArgs e)
        {
            bunifuPages1.SetPage(0);
            render_spline();
            load_circle_bars();
        }

        public void btn_Calendar_Click(object sender, EventArgs e)
        {
            bunifuPages1.SetPage(1);
            SfCalendar1_SelectionChanged(sfCalendar1, new SelectionChangedEventArgs(null, DateTime.Now, false));
        }

        public void bunifuImageButton1_Click(object sender, EventArgs e)
        {
            AddTaskForm form = new AddTaskForm(CurrentUserId,this);
            form.Show();

        }

        private void btn_ParentsEdit_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(txt_FatherFName.Text) || String.IsNullOrWhiteSpace(txt_FatherLName.Text) ||
                String.IsNullOrWhiteSpace(txt_MotherFName.Text) || String.IsNullOrWhiteSpace(txt_MotherLName.Text))
            {
                Bunifu.Snackbar.Show(this, "Please fill out all fields", 3000, Snackbar.Views.SnackbarDesigner.MessageTypes.Error);
                return;
            }
            else if ((DateTime.Now.Year - DP_MotherBDate.Value.Year) < 18 || (DateTime.Now.Year - DP_FatherBDate.Value.Year) < 18)
            {
                Bunifu.Snackbar.Show(this, "Please enter a valid birth date", 3000, Snackbar.Views.SnackbarDesigner.MessageTypes.Error);
                return;
            }
            try
            {
                SqlConnection con = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand();
                con.Open();
                cmd.Connection = con;
                if (btn_ParentsEdit.Text.Equals("Edit")) cmd.CommandText = "UPDATE Parent SET Father_FName=@FatherFirstName," +
                                                                                          "Father_LName=@FatherLastName," +
                                                                                          "Mother_FName=@MotherFirstName," +
                                                                                          "Mother_LName=@MotherLastName," +
                                                                                          "Housing=@Housing," +
                                                                                          "MotherBDate=@MotherBirthDate," +
                                                                                          "FatherBDate=@FatherBirthDate WHERE IdUser=@UserId";
                else cmd.CommandText = "INSERT INTO Parent VALUES(@UserId,@FatherFirstName,@MotherFirstName,@MotherLastName,@Housing,@FatherLastName,@MotherBirthDate,@FatherBirthDate)";
                String housing;
                if (rd_Housing1.Checked && rd_HousingM1.Checked) housing = "Both";
                else if (rd_Housing1.Checked && rd_HousingM2.Checked) housing = "Father only";
                else if (rd_Housing2.Checked && rd_HousingM1.Checked) housing = "Mother only";
                else housing = "None";
                cmd.Parameters.AddWithValue("@UserId", CurrentUserId);
                cmd.Parameters.AddWithValue("@FatherFirstName", txt_FatherFName.Text);
                cmd.Parameters.AddWithValue("@MotherFirstName", txt_MotherFName.Text);
                cmd.Parameters.AddWithValue("@MotherLastName", txt_MotherLName.Text);
                cmd.Parameters.AddWithValue("@Housing", housing);
                cmd.Parameters.AddWithValue("@FatherLastName", txt_FatherLName.Text);
                cmd.Parameters.AddWithValue("@MotherBirthDate", DP_MotherBDate.Value.Date);
                cmd.Parameters.AddWithValue("@FatherBirthDate", DP_FatherBDate.Value.Date);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                con.Close();
                btn_ParentsEdit.Text = "Edit";
                btn_DelParents.Enabled = true;
                Bunifu.Snackbar.Show(this, "Spouse deleted", 3000, Snackbar.Views.SnackbarDesigner.MessageTypes.Success);
                bunifuPages2.Update();
                bunifuPages2.Refresh();
                
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void btn_SpouseEdit_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(txt_SpouseFName.Text) || String.IsNullOrWhiteSpace(txt_SpouseLName.Text) || String.IsNullOrWhiteSpace(txt_SpouseJob.Text))

            {
                Bunifu.Snackbar.Show(this, "Please fill out all fields", 3000, Snackbar.Views.SnackbarDesigner.MessageTypes.Error);
                return;            }
            if ((DateTime.Now.Year - this.DP_SpouseBDate.Value.Year) < 18)
            {
                Bunifu.Snackbar.Show(this, "Please enter a valid birth date", 3000, Snackbar.Views.SnackbarDesigner.MessageTypes.Error);
                return;
            }
            try
            {
                SqlConnection con = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand();
                con.Open();
                cmd.Connection = con;
                if (btn_SpouseEdit.Text.Equals("Edit")) cmd.CommandText = "UPDATE Spouse SET First_Name=@FirstName," +
                                                                                          "Last_Name=@LastName," +
                                                                                          "BDate=@BirthDate," +
                                                                                          "Job=@Job," +
                                                                                          "Health=@Health," +
                                                                                          "Gender=@Gender WHERE IdUser=@UserId";
                else cmd.CommandText = "INSERT INTO Spouse VALUES(@UserId,@FirstName,@LastName,@BirthDate,@Job,@Health,@Gender)";
                string gender = rd_SpouseGender1.Checked ? "Female" : "Male";
                cmd.Parameters.AddWithValue("@UserId", CurrentUserId);
                cmd.Parameters.AddWithValue("@FirstName", txt_SpouseFName.Text);
                cmd.Parameters.AddWithValue("@LastName", txt_SpouseLName.Text);
                cmd.Parameters.AddWithValue("@BirthDate", DP_SpouseBDate.Value.Date);
                cmd.Parameters.AddWithValue("@Job", txt_SpouseJob.Text);
                cmd.Parameters.AddWithValue("@Health", cb_SpouseHealth.SelectedItem.ToString());
                cmd.Parameters.AddWithValue("@Gender", gender);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                con.Close();
                btn_SpouseEdit.Text = "Edit";
                btn_DelSpouse.Enabled = true;
                Bunifu.Snackbar.Show(this, "Success", 3000, Snackbar.Views.SnackbarDesigner.MessageTypes.Success);
                bunifuPages2.Update();
                bunifuPages2.Refresh();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }

        }

        private void btn_ChildrenEdit_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(txt_ChildFName.Text) || String.IsNullOrWhiteSpace(txt_ChildLName.Text) || String.IsNullOrWhiteSpace(txt_EduLvl.Text)
                || String.IsNullOrWhiteSpace(txt_SchoolName.Text) || String.IsNullOrWhiteSpace(txt_Home_School_Dist.Text))
            {
                Bunifu.Snackbar.Show(this, "Please fill out all fields", 3000, Snackbar.Views.SnackbarDesigner.MessageTypes.Error);
                return;
            }
            string selectedvalue = ChildrenNames.SelectedValue.ToString();
            String First_Name = selectedvalue.Substring(0, selectedvalue.IndexOf(" "));
            String Last_Name = selectedvalue.Substring(selectedvalue.IndexOf(" ") + 1);
            //if the user wants to edit child's name 
            if (!First_Name.Equals(txt_ChildFName.Text) || !Last_Name.Equals(txt_ChildLName.Text)){
                try
                {
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = this.ConnectionString;
                    SqlCommand cmd = new SqlCommand();
                    con.Open();
                    cmd.CommandText = "SELECT First_Name,Last_Name FROM [Child] WHERE First_Name = @FirstName AND Last_Name = @LastName AND IdUser=@UserId";
                    cmd.Connection = con;
                    cmd.Parameters.AddWithValue("@FirstName", this.txt_ChildFName.Text);
                    cmd.Parameters.AddWithValue("@LastName", this.txt_ChildLName.Text);
                    cmd.Parameters.AddWithValue("@UserId", this.CurrentUserId);
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        Bunifu.Snackbar.Show(this, "Two children can't have the same name", 3000, Snackbar.Views.SnackbarDesigner.MessageTypes.Error);
                        return;

                    }
                    reader.Close();
                    cmd.Dispose();
                    con.Close();
                } catch (Exception ex) { MessageBox.Show(ex.Message); }
            }
            try
            {
                int n = int.Parse(txt_Home_School_Dist.Text);
            }
            catch (FormatException)
            {
                Bunifu.Snackbar.Show(this, "Invalid format", 3000, Snackbar.Views.SnackbarDesigner.MessageTypes.Error);
                return;
            }
            
            try
            {
                SqlConnection con = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand();
                con.Open();
                cmd.Connection = con;
                cmd.CommandText = "UPDATE Child SET First_Name = @FirstName,Last_Name = @LastName,School_Name=@SchoolName,Education_Level=@EduLvl,Gender=@Gender," +
                        "Birth_Date=@BirthDate,Home_School_Dist=@HomeSchoolDist WHERE First_Name=@PreviousFirstName AND Last_Name=@PreviousLastName AND IdUser=@UserId";
                cmd.Parameters.AddWithValue("@PreviousFirstName", First_Name);
                cmd.Parameters.AddWithValue("@PreviousLastName", Last_Name);

                cmd.Parameters.AddWithValue("@UserId", CurrentUserId);
                cmd.Parameters.AddWithValue("@FirstName", txt_ChildFName.Text);
                cmd.Parameters.AddWithValue("@LastName", txt_ChildLName.Text);
                cmd.Parameters.AddWithValue("@SchoolName", txt_SchoolName.Text);
                cmd.Parameters.AddWithValue("@EduLvl", txt_EduLvl.Text);
                string gender = rd_ChildGender1.Checked ? "Female" : "Male";
                cmd.Parameters.AddWithValue("@Gender",gender);
                cmd.Parameters.AddWithValue("@BirthDate", DP_ChildBDate.Value.Date);
                int distance = int.Parse(txt_Home_School_Dist.Text);
                if (distanceUnit.SelectedItem.ToString().Equals("km")) distance *= 1000;
                cmd.Parameters.AddWithValue("@HomeSchoolDist", distance);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                con.Close();
                Bunifu.Snackbar.Show(this, "Edit successful", 3000, Snackbar.Views.SnackbarDesigner.MessageTypes.Success);
                label_Chilrden_Click(new object(), new EventArgs());
                bunifuPages2.Update();
                bunifuPages2.Refresh();
            }catch(Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void btn_EditTask_Click(object sender, EventArgs e)
        {
            int i = bunifuDataGridView1.SelectedRows[0].Index;
            int id = Convert.ToInt32(bunifuDataGridView1.Rows[i].Cells[0].Value);
            EditTaskForm f = new EditTaskForm(CurrentUserId, id, this);
            f.Show();
        }

        private void btn_DeleteTask_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(bunifuDataGridView1.SelectedRows[0].Cells[0].Value);
            try
            {
                SqlConnection con = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand();
                con.Open();
                cmd.Connection = con;
                cmd.CommandText = "DELETE FROM Task WHERE Id=@Id";
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                con.Close();
                bunifuDataGridView1.Rows.Clear();
                Bunifu.Snackbar.Show(this, "Task deleted", 3000, Snackbar.Views.SnackbarDesigner.MessageTypes.Success);
                btn_Tasks_Click(new object(), new EventArgs());
                bunifuDataGridView1.Update();
                bunifuDataGridView1.Refresh();
            }catch(Exception ex) { MessageBox.Show(ex.Message);}
        }

        private void btn_EditPhoto_Click(object sender, EventArgs e)
        {
            OpenFileDialog opnfd = new OpenFileDialog();
            opnfd.Filter = "Image Files (*.jpg;*.jpeg;.*.gif;*.png;)|*.jpg;*.jpeg;.*.gif;*.png";
            opnfd.InitialDirectory = "D:\\";
            if (opnfd.ShowDialog() == DialogResult.OK)
            {
                UserImage = new Bitmap(opnfd.FileName);
                pb_user.Image = UserImage;
                pb_user.Update();
                pb_user.Refresh();
                User_Photo.Image = UserImage;
                pb_user.Update();
                pb_user.Refresh();
            }
            byte[] buffer = File.ReadAllBytes(opnfd.FileName);
            try
            {
                SqlConnection con = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand();
                con.Open();
                cmd.Connection = con;
                cmd.CommandText = "UPDATE [User] SET Photo=@Photo WHERE Id=@Id";
                cmd.Parameters.AddWithValue("@Photo", buffer);
                cmd.Parameters.AddWithValue("@Id", CurrentUserId);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                con.Close();
            }catch(Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void btn_EditUser_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(txt_FirstName.Text) || String.IsNullOrWhiteSpace(txt_LastName.Text) || String.IsNullOrWhiteSpace(txt_address.Text) || String.IsNullOrWhiteSpace(txt_Job.Text) || String.IsNullOrWhiteSpace(txt_Email.Text) || String.IsNullOrWhiteSpace(txt_Password.Text))
            {
                Bunifu.Snackbar.Show(this, "Please fill out all fields", 3000, Snackbar.Views.SnackbarDesigner.MessageTypes.Error);
                return;
            }
            if ((DateTime.Now.Year - BirthDate.Value.Year) < 18)
            {
                  Bunifu.Snackbar.Show(this, "Please enter a valid birth date", 3000, Snackbar.Views.SnackbarDesigner.MessageTypes.Error);
                  return;         
            }
            try
            {
                SqlConnection con = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand();
                con.Open();
                cmd.Connection = con;
                cmd.CommandText = "UPDATE [User] SET First_Name=@FirstName, Last_Name=@LastName,BDate=@BirthDate,Job=@Job,Marital_Status=@MaritalStatus,Address=@Address,Mail=@Email,Password=@Password WHERE Id=@Id";
                cmd.Parameters.AddWithValue("@FirstName", txt_FirstName.Text);
                cmd.Parameters.AddWithValue("@LastName", txt_LastName.Text);
                cmd.Parameters.AddWithValue("@BirthDate", BirthDate.Value);
                cmd.Parameters.AddWithValue("@Job", txt_Job.Text);
                cmd.Parameters.AddWithValue("@MaritalStatus", cb_MaritalStatus.SelectedItem.ToString());
                cmd.Parameters.AddWithValue("@Address", txt_address.Text);
                cmd.Parameters.AddWithValue("@Email", txt_Email.Text);
                cmd.Parameters.AddWithValue("@Password", txt_Password.Text);
                cmd.Parameters.AddWithValue("@Id", CurrentUserId);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                con.Close();
                Bunifu.Snackbar.Show(this, "Edit successful", 3000, Snackbar.Views.SnackbarDesigner.MessageTypes.Success);
                Form2_Load(new object(), new EventArgs());
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void btn_DelParents_Click(object sender, EventArgs e)
        {
            try
            {
                int id;
                SqlConnection con = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand();
                con.Open();
                cmd.Connection = con;
                cmd.CommandText = "SELECT Id FROM Parent WHERE IdUser=@UserId ";
                cmd.Parameters.AddWithValue("@UserId", CurrentUserId);
                SqlDataReader reader = cmd.ExecuteReader();
                reader.Read();
                id = reader.GetInt32(0);
                reader.Close();
                cmd.CommandText = "DELETE FROM Task WHERE IdUser=@UserId AND IdTarget=@TargetId AND (RelationTarget='Father' OR RelationTarget='Mother')";
                cmd.Parameters.AddWithValue("@TargetId", id);
                cmd.ExecuteNonQuery();
                cmd.CommandText = "DELETE FROM Target WHERE Id=@TargetId AND (Relation='Father' OR Relation='Mother')";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "DELETE FROM Parent WHERE IdUser=@UserId";
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                con.Close();
                txt_FatherFName.Text = String.Empty;
                txt_FatherLName.Text = String.Empty;
                DP_FatherBDate.Value = DateTime.Now;
                DP_MotherBDate.Value = DateTime.Now;
                txt_MotherFName.Text = String.Empty;
                txt_MotherLName.Text = String.Empty;
                Bunifu.Snackbar.Show(this, "Parents deleted", 3000, Snackbar.Views.SnackbarDesigner.MessageTypes.Success);
                bunifuSeparator1_Load(new object(), new EventArgs());
                bunifuPages2.Update();
                bunifuPages2.Refresh();

            }catch(Exception ex) { MessageBox.Show(ex.Message); }
        }
        private void btn_DelSpouse_Click(object sender, EventArgs e)
        {
            try
            {
                int id;
                SqlConnection con = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand();
                con.Open();
                cmd.Connection = con;
                cmd.CommandText = "SELECT Id FROM Spouse WHERE Child WHERE IdUser=@UserId ";
                cmd.Parameters.AddWithValue("@UserId", CurrentUserId);
                SqlDataReader reader = cmd.ExecuteReader();
                reader.Read();
                id = reader.GetInt32(0);
                reader.Close();
                cmd.CommandText = "DELETE FROM Task WHERE IdUser=@UserId AND IdTarget=@TargetId AND relationTarget='Spouse'";
                cmd.Parameters.AddWithValue("@TargetId", id);
                cmd.ExecuteNonQuery();
                cmd.CommandText = "DELETE FROM Target WHERE Id=@TargetId AND Relation='Spouse'";
                cmd.ExecuteNonQuery();
                
                cmd.CommandText = "DELETE FROM Spouse WHERE IdUser=@UserId";
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                con.Close();
                txt_SpouseFName.Text = String.Empty;
                txt_SpouseLName.Text = String.Empty;
                DP_SpouseBDate.Value = DateTime.Now;
                txt_SpouseJob.Text = String.Empty;
                btn_SpouseEdit.Text = "Add";
                btn_DelSpouse.Enabled = false;
                Bunifu.Snackbar.Show(this, "Spouse deleted", 3000, Snackbar.Views.SnackbarDesigner.MessageTypes.Success);
                bunifuPages2.Update();
                bunifuPages2.Refresh();

            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        private void btn_AddChild_Click(object sender, EventArgs e)
        {
            AddChildForm f = new AddChildForm(CurrentUserId, this);
            f.Show();
        }
        private void btn_DeleteChild_Click(object sender, EventArgs e)
        {
            try
            {
                int id;
                SqlConnection con = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand();
                con.Open();
                cmd.Connection = con;
                cmd.CommandText = "SELECT Id FROM Child  WHERE IdUser=@UserId AND First_Name=@FirstName AND Last_Name=@LastName";
                cmd.Parameters.AddWithValue("@UserId", CurrentUserId);
                cmd.Parameters.AddWithValue("@FirstName", txt_ChildFName.Text);
                cmd.Parameters.AddWithValue("@LastName", txt_ChildLName.Text);
                SqlDataReader reader = cmd.ExecuteReader();
                reader.Read();
                id = reader.GetInt32(0);
                reader.Close();
                cmd.CommandText = "DELETE FROM Task WHERE IdUser=@UserId AND IdTarget=@TargetId AND relationTarget='Child'";
                cmd.Parameters.AddWithValue("@TargetId", id);
                cmd.ExecuteNonQuery();
                cmd.CommandText = "DELETE FROM Target WHERE Id=@TargetId AND Relation='Child'";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "DELETE FROM Child WHERE IdUser=@UserId AND First_Name=@FirstName AND Last_Name=@LastName";
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                con.Close();
                Bunifu.Snackbar.Show(this, "Child deleted", 3000, Snackbar.Views.SnackbarDesigner.MessageTypes.Success);
                label_Chilrden_Click(new object(), new EventArgs());
                bunifuPages2.Update();
                bunifuPages2.Refresh();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }

        }
        private void render_spline()
        {
            Canvas canvas = new Canvas();
            DataPoint datapoint = new DataPoint(BunifuDataViz._type.Bunifu_column);
            String[] months = new String[12] { "JAN","FEB","MAR","APR","MAY","JUN","JUL","AUG","SEP","OCT","NOV","DEC"};
            int i = 1;
            try
            {
                SqlConnection con = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand();
                con.Open();
                cmd.Connection = con;
                foreach (String month in months)
                {
                    cmd.CommandText = String.Format("SELECT COUNT(Id) FROM Task WHERE IdUser={0} AND MONTH(Start_Date)={1}", CurrentUserId, i);

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read()) datapoint.addLabely(month, reader.GetInt32(0));
                    else datapoint.addLabely(month, 0);
                    reader.Close();
                    i++;
                }

                // Add data sets to canvas   
                canvas.addData(datapoint);
                //render canvas   
                bunifuDataViz1.colorSet.Add(Color.DarkGray);
                bunifuDataViz1.AxisXGridColor = Color.White;
                bunifuDataViz1.Render(canvas);
                cmd.Dispose();
                con.Close();
            }catch(Exception ex) { MessageBox.Show(ex.Message); }
        }
        private void load_circle_bars()
        {
            int total;
            int performed;
            try
            {
                SqlConnection con = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand();
                con.Open();
                cmd.Connection = con;
                //user
                cmd.CommandText = "SELECT COUNT(Id) FROM Task WHERE IdUser=@UserId AND relationTarget='User'";
                cmd.Parameters.AddWithValue("@UserId", CurrentUserId);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read()) total = reader.GetInt32(0);
                else total = 0;
                reader.Close();
                cmd.CommandText = "SELECT COUNT(Id) FROM Task WHERE IdUser=@UserId AND relationTarget='User' AND TaskCompleted='yes'";
                reader = cmd.ExecuteReader();
                if (reader.Read()) performed = reader.GetInt32(0);
                else performed = 0;
                label_userTotal.Text = total.ToString();
                label_userDone.Text = performed.ToString();
                try
                {
                    cp_user.Value = (performed *100) / total;
                    cp_user.Text = cp_user.Value.ToString();
                    cp_user.SuperScriptMargin = new Padding(5, 26, 0, 0);
                }
                catch (DivideByZeroException ex)
                {
                    cp_user.Value = 0;
                    cp_user.Text = "0";
                    cp_user.SuperScriptMargin = new Padding(5, 26, 0, 0);
                }
                reader.Close();
                //spouse
                cmd.CommandText = "SELECT COUNT(Id) FROM Task WHERE IdUser=@UserId AND relationTarget='Spouse'";
                reader = cmd.ExecuteReader();
                if (reader.Read()) total = reader.GetInt32(0);
                else total = 0;
                reader.Close();
                cmd.CommandText = "SELECT COUNT(Id) FROM Task WHERE IdUser=@UserId AND relationTarget='Spouse' AND TaskCompleted='yes'";
                reader = cmd.ExecuteReader();
                if (reader.Read()) performed = reader.GetInt32(0);
                else performed = 0;
                label_spouseTotal.Text = total.ToString();
                label_spouseDone.Text = performed.ToString();
                try
                {
                    cp_spouse.Value = (performed * 100) / total;
                    cp_spouse.Text = cp_spouse.Value.ToString();
                    cp_spouse.SuperScriptMargin = new Padding(5, 26, 0, 0);
                }
                catch (DivideByZeroException ex)
                {
                    cp_spouse.Value = 0;
                    cp_spouse.Text = "0";
                    cp_spouse.SuperScriptMargin = new Padding(5, 26, 0, 0);
                }
                reader.Close();
                //children
                cmd.CommandText = "SELECT COUNT(Id) FROM Task WHERE IdUser=@UserId AND relationTarget='Child'";
                reader = cmd.ExecuteReader();
                if (reader.Read()) total = reader.GetInt32(0);
                else total = 0;
                reader.Close();
                cmd.CommandText = "SELECT COUNT(Id) FROM Task WHERE IdUser=@UserId AND relationTarget='Child' AND TaskCompleted='yes'";
                reader = cmd.ExecuteReader();
                if (reader.Read()) performed = reader.GetInt32(0);
                else performed = 0;
                label_childrenTotal.Text = total.ToString();
                label_childrenDone.Text = performed.ToString();
                try
                {
                    cp_children.Value = (performed * 100) / total;
                    cp_children.Text = cp_children.Value.ToString();
                    cp_children.SuperScriptMargin = new Padding(5, 26, 0, 0);
                }
                catch (DivideByZeroException ex)
                {
                    cp_children.Value = 0;
                    cp_children.Text = "0";
                    cp_children.SuperScriptMargin = new Padding(5, 26, 0, 0);
                }
                reader.Close();
                //parents
                cmd.CommandText = "SELECT COUNT(Id) FROM Task WHERE IdUser=@UserId AND (relationTarget='Father' OR relationTarget='Mother')";
                reader = cmd.ExecuteReader();
                if (reader.Read()) total = reader.GetInt32(0);
                else total = 0;
                reader.Close();
                cmd.CommandText = "SELECT COUNT(Id) FROM Task WHERE IdUser=@UserId AND (relationTarget='Father' OR relationTarget='Mother') AND TaskCompleted='yes'";
                reader = cmd.ExecuteReader();
                if (reader.Read()) performed = reader.GetInt32(0);
                else performed = 0;
                label_parentsTotal.Text = total.ToString();
                label_parentsDone.Text = performed.ToString();
                try
                {
                    cp_parents.Value = (performed * 100) / total;
                    cp_parents.Text = cp_spouse.Value.ToString();
                    cp_parents.SuperScriptMargin = new Padding(5, 26, 0, 0);
                }
                catch (DivideByZeroException ex)
                {
                    cp_parents.Value = 0;
                    cp_parents.Text = "0";
                    cp_parents.SuperScriptMargin = new Padding(5, 26, 0, 0);
                }
                reader.Close();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }

        }
        private void bunifuImageButton1_Click_1(object sender, EventArgs e)
        {
            SpeechRecognitionEngine RecognitionEngine = new SpeechRecognitionEngine();
            if (isListening == false)
            {
                Choices commands = new Choices(new string[] { "dashboard", "calendar", "tasks", "family", "profile" });
                GrammarBuilder grammarBuilder = new GrammarBuilder(commands);
                grammarBuilder.Culture = new System.Globalization.CultureInfo("en-US");
                Grammar grammar = new Grammar(grammarBuilder);
                RecognitionEngine.LoadGrammarAsync(grammar);
                RecognitionEngine.SetInputToDefaultAudioDevice();
                RecognitionEngine.RecognizeAsync(RecognizeMode.Multiple);
                RecognitionEngine.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(RecognitionEngine_SpeechRecognized);
            }
            else RecognitionEngine.RecognizeAsyncStop();
            isListening = !isListening;


        }
        private void RecognitionEngine_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            switch (e.Result.Text)
            {
                case "dashboard":
                    btn_Dashboard_Click(new object(), new EventArgs());
                    break;
                case "calendar":
                    btn_Calendar_Click(new object(), new EventArgs());
                    break;
                case "tasks":
                    btn_Tasks_Click(new object(), new EventArgs());
                    break;
                case "family":
                    btn_Family_Click(new object(), new EventArgs());
                    break;
                case "profile":
                    btn_Profile_Click(new object(), new EventArgs());
                    break;
                default:
                    MessageBox.Show(e.Result.Text);
                    break;
            }
        }
        private void edit_fonts()
        {
            Font font = new Font("Segoe UI", 10);
            txt_UserName.Font = font;
            bunifuLabel1.Font = font;
            bunifuLabel33.Font = font;
            bunifuLabel43.Font = font;
            bunifuLabel48.Font = font;
            Label_Parent_D1.Font = font;
            Label_Parent_D2.Font = font;
            bunifuLabel20.Font = font;
            bunifuLabel18.Font = font;
            bunifuLabel3.Font  = font;
            bunifuLabel4.Font  = font;
            bunifuLabel16.Font = font;
            bunifuLabel35.Font = font;
            font = new Font("Segoe UI", 8);
            bunifuLabel8.Font = font;
            bunifuLabel5.Font = font;
            bunifuLabel27.Font = font;
            bunifuLabel28.Font = font;
            bunifuLabel41.Font = font;
            bunifuLabel42.Font = font;
            bunifuLabel46.Font = font;
            bunifuLabel47.Font = font;
            font = new Font("Segoe UI",8,FontStyle.Bold);
            label_userDone.Font = font;
            label_userTotal.Font = font;
            label_spouseDone.Font = font;
            label_spouseTotal.Font = font;
            label_parentsDone.Font = font;
            label_parentsTotal.Font = font;
            label_childrenDone.Font = font;
            label_childrenTotal.Font = font;











        }
    }
}

