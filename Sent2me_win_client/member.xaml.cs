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
    /// Interaction logic for member.xaml
    /// </summary>
    public partial class member : Window {
        public member() {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e) {

            sent2meDataSet sent2meDataSet = ((sent2meDataSet)(this.FindResource("sent2meDataSet")));
            // Load data into the table Title. You can modify this code as needed.
            sent2meDataSetTableAdapters.TitleTableAdapter sent2meDataSetTitleTableAdapter = new sent2meDataSetTableAdapters.TitleTableAdapter();
            sent2meDataSetTitleTableAdapter.Fill(sent2meDataSet.Title);
            CollectionViewSource titleViewSource = ((CollectionViewSource)(this.FindResource("titleViewSource")));
            titleViewSource.View.MoveCurrentToFirst();
            // Load data into the table Branch. You can modify this code as needed.
            sent2meDataSetTableAdapters.BranchTableAdapter sent2meDataSetBranchTableAdapter = new sent2meDataSetTableAdapters.BranchTableAdapter();
            sent2meDataSetBranchTableAdapter.Fill(sent2meDataSet.Branch);
            CollectionViewSource branchViewSource = ((CollectionViewSource)(this.FindResource("branchViewSource")));
            branchViewSource.View.MoveCurrentToFirst();
        }

        private void emp_add_btn_Click(object sender, RoutedEventArgs e) {

            if ( MessageBox.Show("คุณต้องการที่จะเพิ่มพนักงานคนนี้จริงหรือไม่","ยืนยันทำการเพิ่ม",MessageBoxButton.YesNo,MessageBoxImage.Information) == MessageBoxResult.Yes) {
                try {
                String new_id_String;
                String strQry = "SELECT * FROM Employee ORDER BY emp_id desc";
                sqlConnection sql = new sqlConnection();
                sql.sqlCon.Open();
                sql.cmd = new SqlCommand(strQry,sql.sqlCon);
                sql.cmd.ExecuteNonQuery();
                sql.da = new SqlDataAdapter(sql.cmd);
                DataSet ds = new DataSet();
                sql.da.Fill(ds, "employee");
                String emp_id_prev = ds.Tables["employee"].Rows[0]["emp_id"].ToString();
                sql.sqlCon.Close();
                new_id_String = Create_id(emp_id_prev);

                strQry = @"INSERT INTO Employee (emp_id,emp_name,emp_add,title_id,branch_id) 
                VALUES(@id,@name,@add,@title_id,@branch_id) ";
                sql.sqlCon.Open();
                sql.cmd = new SqlCommand(strQry, sql.sqlCon);
                sql.cmd.Parameters.AddWithValue("@id", new_id_String);
                sql.cmd.Parameters.AddWithValue("@name", emp_name_txt.Text);
                sql.cmd.Parameters.AddWithValue("@add", emp_add_txt.Text);
                sql.cmd.Parameters.AddWithValue("@title_id", title_nameComboBox.SelectedValuePath);
                sql.cmd.Parameters.AddWithValue("@branch_id", branch_nameComboBox.SelectedValuePath);
                sql.cmd.ExecuteNonQuery();
                sql.sqlCon.Close();

                strQry = @"INSERT INTO Web_login(username,password,emp_id) values (@username,@password,@id)";
                sql.sqlCon.Open();
                sql.cmd = new SqlCommand(strQry, sql.sqlCon);
                    sql.cmd.Parameters.AddWithValue("@username", usernane_txt.Text);
                    sql.cmd.Parameters.AddWithValue("@password", password_txt.Text);
                    sql.cmd.Parameters.AddWithValue("@id", new_id_String);
                    sql.cmd.ExecuteNonQuery();
                    sql.sqlCon.Close();
                    this.Close();
    
            } catch (SqlException sqlErr) {
                MessageBox.Show("คุณใส่ข้อมูลไม่ครบ หรือติดต่อฐานข้อมูลไม่ได้");
                    Console.WriteLine(sqlErr.Message);
            } catch (Exception err) {
                MessageBox.Show(err.ToString());
            }
        }
   
        }

        public String Create_id (String prev_id) {
            String new_id_string = "";
            var result = Regex.Match(prev_id, @"\d+").Value;
            int new_id = Convert.ToInt32(result) + 1;
            if (new_id >= 10 && new_id < 100) {
                new_id_string = "EMP0" + new_id.ToString();
            } else if (new_id >= 100) {
                new_id_string = "EMP" + new_id.ToString();
            } else {
                new_id_string = "EMP00" + new_id.ToString();
            }
            return new_id_string;
        }

        private void cancel_btn_Click(object sender, RoutedEventArgs e) {
            this.Close();
        }
    }
}
