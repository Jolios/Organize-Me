using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using Syncfusion.WinForms.Input.Events;

namespace Organize_Me
{
    public partial class TaskCard : UserControl
    {
        int TaskId;
        Form2 f;
        SelectionChangedEventArgs args;
        public TaskCard(int TaskId,Form2 f,SelectionChangedEventArgs args)
        {
            InitializeComponent();
            this.TaskId = TaskId;
            this.f = f;
            this.args = args;
            bunifuLabel1.Font = new Font("Microsoft Sans Serif", 8);
        }

        private void btn_TaskCompleted_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(@"Data Source = DESKTOP-HSUI4QK; Initial Catalog = OrganizeMeDB; Integrated Security = True; Pooling = False");
            SqlCommand cmd = new SqlCommand();
            con.Open();
            cmd.Connection = con;
            cmd.CommandText = "UPDATE Task SET TaskCompleted ='yes' WHERE Id=@Id";
            cmd.Parameters.AddWithValue("@Id", TaskId);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            con.Close();
            string pText = label1.Text;
            if (pText.Contains("-")) pText = pText.Substring(0, pText.IndexOf("-") );
            label1.Text = pText + "-Completed";
            //f.SfCalendar1_SelectionChanged(f.sfCalendar1, args);
            
        }

        private void btn_TaskFailed_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(@"Data Source = DESKTOP-HSUI4QK; Initial Catalog = OrganizeMeDB; Integrated Security = True; Pooling = False");
            SqlCommand cmd = new SqlCommand();
            con.Open();
            cmd.Connection = con;
            cmd.CommandText = "UPDATE Task SET TaskCompleted ='no' WHERE Id=@Id";
            cmd.Parameters.AddWithValue("@Id", TaskId);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            con.Close();
            string pText = label1.Text;
            if (pText.Contains("-")) pText = pText.Substring(0, pText.IndexOf("-"));
            label1.Text = pText + "-Cancelled";
            //f.SfCalendar1_SelectionChanged(f.sfCalendar1, args);
        }
    }
}
