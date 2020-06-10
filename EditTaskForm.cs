using EnvDTE;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Organize_Me
{
    public partial class EditTaskForm : Form
    {
        private int currentUserId;
        private int TaskId;
        internal List<String> names;
        Form2 f;
        public EditTaskForm(int currentUserId,int TaskId,Form2 f)
        {
            InitializeComponent();
            bunifuFormDock1.SubscribeControlToDragEvents(panel1);
            this.currentUserId = currentUserId;
            this.TaskId = TaskId;
            this.f = f;
            loadTaskData();
        }
        private DataTable loadComboBox()
        {
            String connectionstring = @"Data Source=DESKTOP-HSUI4QK;Initial Catalog=OrganizeMeDB;Integrated Security=True;Pooling=False";
            SqlConnection con;
            SqlCommand cmd;
            SqlDataAdapter adapter = new SqlDataAdapter();
            DataSet ds = new DataSet();
            DataTable dt;
            String firstsql = "SELECT CONVERT(varchar(10),Id)+'-Spouse' as SpId,First_Name+' '+Last_Name as Full_Name FROM Spouse WHERE IdUser=@UserId";
            String secondsql = "SELECT CONVERT(varchar(10),Id)+'-Child' as SpId,First_Name+' '+Last_Name as Full_Name FROM Child WHERE IdUser=@UserId";
            String thirdsql = "SELECT CONVERT(varchar(10),Id)+'-Father' as SpId,Father_FName+' '+Father_LName as Full_Name FROM Parent WHERE IdUser=@UserId";
            String fourthsql = "SELECT CONVERT(varchar(10),Id)+'-Mother' as SpId,Mother_FName+' '+Mother_LName as Full_Name FROM Parent WHERE IdUser=@UserId";

            con = new SqlConnection(connectionstring);
            this.names = new List<string>();
            try
            {
                con.Open();
                cmd = new SqlCommand(firstsql, con);
                cmd.Parameters.AddWithValue("@UserId", currentUserId);
                adapter.SelectCommand = cmd;
                adapter.Fill(ds, "Table(0)");
                adapter.SelectCommand.CommandText = secondsql;
                adapter.Fill(ds, "Table(1)");
                adapter.SelectCommand.CommandText = thirdsql;
                adapter.Fill(ds, "Table(2)");
                adapter.SelectCommand.CommandText = fourthsql;
                adapter.Fill(ds, "Table(3)");

                ds.Tables[0].Merge(ds.Tables[1]);
                ds.Tables[0].Merge(ds.Tables[2]);
                ds.Tables[0].Merge(ds.Tables[3]);
                String FullName;
                String Relation;
                dt = ds.Tables[0];
                names.Add("Myself");

                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    FullName = dt.Rows[i].ItemArray[1].ToString();
                    Relation = dt.Rows[i].ItemArray[0].ToString();
                    if (names.Contains(FullName)) names.Add(FullName + Relation.Substring(Relation.IndexOf('-')));
                    else names.Add(FullName);

                }

                cb_targets.DataSource = names;
                cb_TaskType.DataSource = new String[5] { "Sport", "Work", "Health", "Education", "Other" };
                adapter.Dispose();
                cmd.Dispose();
                con.Close();
                return dt;


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return new DataTable();

        }
        private void loadTaskData()
        {
            DataTable dt = loadComboBox();
            try
            {
                SqlConnection con = new SqlConnection(@"Data Source = DESKTOP-HSUI4QK; Initial Catalog = OrganizeMeDB; Integrated Security = True; Pooling = False");
                SqlCommand cmd = new SqlCommand();
                con.Open();
                cmd.Connection = con;
                cmd.CommandText = "SELECT * FROM Task WHERE Id=@Id";
                cmd.Parameters.AddWithValue("@Id", TaskId);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    String relationToTarget = reader.GetString(8);
                    if (relationToTarget.Equals("User")) cb_targets.SelectedIndex = 0;
                    else
                    {
                        string target = Convert.ToString(reader.GetInt32(7)) + "-" + relationToTarget;
                        
                        for (int i = 0; i <= dt.Rows.Count - 1; i++)
                        {
                            if (dt.Rows[i].ItemArray[1].ToString().Equals(target))
                            {
                                cb_targets.SelectedIndex = i;
                                break;
                            }
                        }
                    }
                    cb_TaskType.SelectedItem = reader.GetString(6);
                    txt_TaskName.Text = reader.GetString(2);
                    txt_TaskDesc.Text = reader.GetString(5);
                    dp_Start.Value = reader.GetDateTime(3);
                    dp_end.Value = reader.GetDateTime(4);
                    dt_start.Value = DateTime.ParseExact(reader.GetString(9), "HH:mm", CultureInfo.InvariantCulture,
                                              DateTimeStyles.None);
                    dt_end.Value = DateTime.ParseExact(reader.GetString(10), "HH:mm", CultureInfo.InvariantCulture,
                                              DateTimeStyles.None);
                    float toleratedTime = (float)reader.GetDouble(11);
                    if (toleratedTime < 1)
                    {
                        toleratedTime *= 60;
                        TimeUnit.SelectedItem = "Minutes";
                    }
                    else
                    {
                        TimeUnit.SelectedItem = "Hours";
                    }
                    txt_ToleratedTime.Text = Convert.ToString(toleratedTime);
                }
            }
            catch(Exception ex) { MessageBox.Show(ex.Message); }
        }
        private void btn_EditTask_Click(object sender, EventArgs e)
        {
            if (verification())
            {
                try
                {
                    String connectionstring = @"Data Source=DESKTOP-HSUI4QK;Initial Catalog=OrganizeMeDB;Integrated Security=True;Pooling=False";
                    SqlConnection con;
                    SqlCommand cmd;
                    int i = names.IndexOf((String)cb_targets.SelectedValue);
                    DataTable dt = loadComboBox();
                    String Target;
                    int TargetId;
                    String TargetRelation;
                    if (i == 0)
                    {
                        TargetId = currentUserId;
                        TargetRelation = "User";
                    }
                    else
                    {
                        Target = dt.Rows[i - 1].ItemArray[0].ToString();
                        TargetId = int.Parse(Target.Substring(0, Target.IndexOf("-")));
                        TargetRelation = Target.Substring(Target.IndexOf("-") + 1);
                    }
                    con = new SqlConnection(connectionstring);
                    con.Open();
                    cmd = new SqlCommand();
                    cmd.Connection = con;
                    cmd.CommandText = "SELECT * FROM Target WHERE Id=@TargetId AND Relation = @Relation";
                    cmd.Parameters.AddWithValue("@TargetId", TargetId);
                    cmd.Parameters.AddWithValue("@Relation", TargetRelation);
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (!reader.HasRows)
                    {
                        reader.Close();

                        cmd.CommandText = "INSERT INTO Target VALUES(@TargetId,@Relation)";
                        cmd.ExecuteNonQuery();
                    }
                    reader.Close();
                    float toleratedTime = float.Parse(txt_ToleratedTime.Text);
                    cmd.CommandText = "UPDATE Task SET Name=@TaskName,Start_Date=@StartDate,End_Date=@EndDate,Description=@Desc,Type=@Type,IdTarget=@TargetId,relationTarget=@Relation,StartTime=@StartTime,EndTime=@EndTime,ToleratedTime=@ToleratedTime WHERE Id=@Id";
                    cmd.Parameters.AddWithValue("@Id",TaskId);
                    cmd.Parameters.AddWithValue("@TaskName", txt_TaskName.Text);
                    cmd.Parameters.AddWithValue("@StartDate", dp_Start.Value.Date);
                    cmd.Parameters.AddWithValue("@EndDate", dp_end.Value.Date);
                    cmd.Parameters.AddWithValue("@Desc", txt_TaskDesc.Text);
                    cmd.Parameters.AddWithValue("@Type", cb_TaskType.SelectedValue);
                    cmd.Parameters.AddWithValue("@StartTime", dt_start.Value.TimeOfDay.ToString(@"hh\:mm"));
                    cmd.Parameters.AddWithValue("@EndTime", dt_end.Value.TimeOfDay.ToString(@"hh\:mm"));
                    if (TimeUnit.SelectedItem.ToString().Equals("Minutes")) toleratedTime /= 60;
                    cmd.Parameters.AddWithValue("@ToleratedTime", toleratedTime);

                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    con.Close();
                    Bunifu.Snackbar.Show(this, "Task edited", 3000, Snackbar.Views.SnackbarDesigner.MessageTypes.Success);
                    f.bunifuDataGridView1.Rows.Clear();
                    f.btn_Tasks_Click(new object(), new EventArgs());
                    f.bunifuDataGridView1.Update();
                    f.bunifuDataGridView1.Refresh();
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }


            }
        }
        private bool verification()
        {
            bool verif = true;
            if (String.IsNullOrWhiteSpace(txt_TaskName.Text) || String.IsNullOrWhiteSpace(txt_TaskDesc.Text))
            {
                MessageBox.Show("Please fill out all fields");
                verif = false;
            }
            if (DateTime.Compare(dp_Start.Value.Date, dp_end.Value.Date) > 0)
            {
                MessageBox.Show("Start date must be earlier than end date");
                verif = false;
            }
            else if (DateTime.Compare(dp_Start.Value.Date, dp_end.Value.Date) == 0)
            {
                if (TimeSpan.Compare(dt_start.Value.TimeOfDay, dt_end.Value.TimeOfDay) > 0)
                {
                    MessageBox.Show("Start time must be earlier than end time");
                    verif = false;
                }
            }
            return verif;

        }

        private void btn_Close_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
