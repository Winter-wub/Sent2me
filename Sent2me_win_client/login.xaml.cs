using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;


namespace Sent2me_win_client {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        sqlConnection sql = new sqlConnection();
        public MainWindow() {
            InitializeComponent();
           // Closing += new System.ComponentModel.CancelEventHandler(Window_Closing);
        }

        private void Login_btn_Click(object sender, RoutedEventArgs e) {
            
            String strQry = "SELECT * FROM login_view WHERE username =@username AND password =@password";
            try {
                sql.sqlCon.Open();
                sql.cmd = new SqlCommand(strQry, sql.sqlCon);
                sql.cmd.Parameters.AddWithValue("@username", username_txt.Text);
                sql.cmd.Parameters.AddWithValue("@password", password_txt.Password);
                sql.cmd.ExecuteNonQuery();
                sql.da = new SqlDataAdapter(sql.cmd);
                DataSet ds = new DataSet();
                sql.da.Fill(ds, "Web_login");
                if (ds.Tables["Web_login"].Rows.Count != 0) {
                    String emp_id = ds.Tables["Web_login"].Rows[0]["emp_id"].ToString();
                    new dashboardLoader(emp_id);
                    Hide();
                } else {
                    MessageBox.Show("Username หรือ Password ไม่ถูกต้อง", "ผิดพลาด", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            } catch (Exception err) {
                MessageBox.Show("การเชื่อมต่อฐานข้อมูลผิดพลาด กรุณาลองใหม่อีกครั้ง", "ผิดพลาด",MessageBoxButton.OK,MessageBoxImage.Error);
                Console.WriteLine(err.Message);
               // new exitConfrim();
            }
            
            sql.sqlCon.Close();
        }

        private void Exit_btn_Click(object sender, RoutedEventArgs e) {
            new exitConfrim();
        }
        void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
           
        }

        private void Check_btn_Click(object sender, RoutedEventArgs e) {
            check check = new check();
            check.Show();
            this.Close();
        }
    }
}
