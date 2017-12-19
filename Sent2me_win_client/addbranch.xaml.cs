using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Sent2me_win_client {
    /// <summary>
    /// Interaction logic for addbranch.xaml
    /// </summary>
    public partial class addbranch : Window {
        public addbranch() {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e) {

            Sent2me_win_client.sent2meDataSet sent2meDataSet = ((Sent2me_win_client.sent2meDataSet)(this.FindResource("sent2meDataSet")));
            // Load data into the table Employee. You can modify this code as needed.
            Sent2me_win_client.sent2meDataSetTableAdapters.EmployeeTableAdapter sent2meDataSetEmployeeTableAdapter = new Sent2me_win_client.sent2meDataSetTableAdapters.EmployeeTableAdapter();
            sent2meDataSetEmployeeTableAdapter.Fill(sent2meDataSet.Employee);
            System.Windows.Data.CollectionViewSource employeeViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("employeeViewSource")));
            employeeViewSource.View.MoveCurrentToFirst();
        }

        private void Exit_btn_Click(object sender, RoutedEventArgs e) {
            this.Close();
        }

        private void Insert_btn_Click(object sender, RoutedEventArgs e) {
            if(MessageBox.Show("คุณแน่ใจใช้ไหมที่จะเพิ่มสาขานี้","ยืนยันการเพิ่มสาขา",MessageBoxButton.YesNo,MessageBoxImage.Question) == MessageBoxResult.Yes) {
                try {
                    String strQry = @"select * from Branch Order by branch_id desc";
                    sqlConnection sql = new sqlConnection();
                    sql.sqlCon.Open();
                    sql.cmd = new SqlCommand(strQry, sql.sqlCon);
                    sql.cmd.ExecuteNonQuery();
                    sql.da = new SqlDataAdapter(sql.cmd);
                    DataSet ds = new DataSet();
                    sql.da.Fill(ds, "branch_last");
                    String branch_id_prev = ds.Tables["branch_last"].Rows[0]["branch_id"].ToString();
                    String new_id = Create_id(branch_id_prev);
                    sql.sqlCon.Close();

                    strQry = @"INSERT INTO Branch (branch_id,branch_name,branch_add,emp_id,latlong) VALUES (@id,@name,@add,@emp_id,@latlong)";
                    sql.sqlCon.Open();
                    sql.cmd = new SqlCommand(strQry, sql.sqlCon);
                    sql.cmd.Parameters.AddWithValue("@id", new_id);
                    sql.cmd.Parameters.AddWithValue("@name", branch_name_txt.Text);
                    sql.cmd.Parameters.AddWithValue("@add", branch_add_txt.Text);
                    sql.cmd.Parameters.AddWithValue("@emp_id", emp_nameComboBox.SelectedValuePath);
                    sql.cmd.Parameters.AddWithValue("@latlong", branch_latlong_txt.Text);
                    sql.cmd.ExecuteNonQuery();
                    sql.sqlCon.Close();
                   
                    MessageBox.Show("เพิ่มสาขาสำเร็จ");
                    this.Close();

                } catch (SqlException sqlerr){
                    MessageBox.Show(sqlerr.ToString());
                }
            }
        }
        public String Create_id(String prev_id) {
            String new_id_string = "";
            var result = Regex.Match(prev_id, @"\d+").Value;
            int new_id = Convert.ToInt32(result) + 1;
            if (new_id >= 10 && new_id < 100) {
                new_id_string = "B0" + new_id.ToString();
            } else if (new_id >= 100) {
                new_id_string = "B" + new_id.ToString();
            } else {
                new_id_string = "B00" + new_id.ToString();
            }
            return new_id_string;
        }
    }
}
