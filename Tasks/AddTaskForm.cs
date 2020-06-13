using EnvDTE;
using Syncfusion.Windows.Forms.Tools;
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
    public partial class AddTaskForm : Form
    {
        internal List<String> names;
        int currentUserId;
        Form2 f;
        public AddTaskForm(int currentUserId,Form2 f)
        {
            InitializeComponent();
            bunifuFormDock1.SubscribeControlToDragEvents(panel1);
            this.currentUserId = currentUserId;
            loadComboBox();
            this.f = f;
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

                for (int i = 0;i<= dt.Rows.Count - 1; i++){
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

        private void btn_Add_Task_Click(object sender, EventArgs e)
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
                    String Target ;
                    int TargetId;
                    String TargetRelation;
                    if (i==0)
                    {
                        TargetId = currentUserId;
                        TargetRelation = "User";
                    }
                    else
                    {
                        Target = dt.Rows[i-1].ItemArray[0].ToString();
                        TargetId = int.Parse(Target.Substring(0, Target.IndexOf("-")));
                        TargetRelation = Target.Substring(Target.IndexOf("-")+1);
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
                    cmd.CommandText = "INSERT INTO Task VALUES(@UserId,@TaskName,@StartDate,@EndDate,@Desc,@Type,@TargetId,@Relation,@StartTime,@EndTime,@ToleratedTime,'N/A')";
                    cmd.Parameters.AddWithValue("@UserId",currentUserId);
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
                    Bunifu.Snackbar.Show(this, "Task added", 3000, Snackbar.Views.SnackbarDesigner.MessageTypes.Success);
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
            if(String.IsNullOrWhiteSpace(txt_TaskName.Text) || String.IsNullOrWhiteSpace(txt_TaskDesc.Text))
            {
                Bunifu.Snackbar.Show(this, "Please fill out all fields", 3000, Snackbar.Views.SnackbarDesigner.MessageTypes.Error);
                return false;
            }
            if (DateTime.Compare(dp_Start.Value.Date, dp_end.Value.Date) > 0)
            {
                Bunifu.Snackbar.Show(this, "Start date must be earlier than end date", 3000, Snackbar.Views.SnackbarDesigner.MessageTypes.Error);
                return false;            }
            else if(DateTime.Compare(dp_Start.Value.Date, dp_end.Value.Date) == 0){
                if(TimeSpan.Compare(dt_start.Value.TimeOfDay,dt_end.Value.TimeOfDay) > 0)
                {
                    Bunifu.Snackbar.Show(this, "Start time must be earlier than end time", 3000, Snackbar.Views.SnackbarDesigner.MessageTypes.Error);
                    return false;
                }
            }
            return true; 

        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

  

        private void AddTaskForm_Load(object sender, EventArgs e)
        {
            Font font = new Font("Segoe UI", 14, FontStyle.Bold);
            bunifuLabel6.Font = font;
            font = new Font("Segoe UI", 12);
            bunifuLabel7.Font = font;
            font = new Font("Segoe UI Semibold", 14);
            bunifuLabel1.Font = font;
            bunifuLabel2.Font = font;
            bunifuLabel3.Font = font;
            bunifuLabel4.Font = font;
            bunifuLabel5.Font = font;
            bunifuLabel8.Font = font;
            bunifuLabel9.Font = font;
            bunifuLabel10.Font = font;
            bunifuLabel11.Font = font;

        }
    }
}
