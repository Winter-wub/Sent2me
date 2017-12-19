using System;
using System.Data.SqlClient;
using System.Data;
using System.Windows;

namespace Sent2me_win_client {
    class dashboardLoader {
        sqlConnection sql = new sqlConnection();
        String title;
        public DataSet ds = new DataSet();
        public dashboardLoader(String emp_id) {
            String strQry = "SELECT * FROM Employee WHERE emp_id =@empid";
            
            try {
                sql.sqlCon.Open();
                sql.cmd = new SqlCommand(strQry, sql.sqlCon);
                sql.cmd.Parameters.AddWithValue("@empid", emp_id);
                sql.cmd.ExecuteNonQuery();
                sql.da = new SqlDataAdapter(sql.cmd);
                sql.da.Fill(ds, "employee");
                title = ds.Tables["employee"].Rows[0]["title_id"].ToString();
                //MessageBox.Show(title);
                sql.sqlCon.Close();
                
            } catch (Exception err){
                Console.WriteLine(err.ToString());
                MessageBox.Show(err.Message);
                new exitConfrim();
            }
            dashboard db = new dashboard(ds);
            switch (title) {

                case "HBOSS":
                db.create_package_tab.Visibility = Visibility.Hidden;
                db.delivery_btn.Visibility = Visibility.Hidden;
                db.Update_cus_btn.Visibility = Visibility.Hidden;
                db.add_cus_btn.Visibility = Visibility.Hidden;
                db.delivery_btn.Visibility = Visibility.Hidden;
                db.cost_managerment.Visibility = Visibility.Hidden;
                db.Show();
                break;

                case "BOSS":
                db.create_package_tab.Visibility = Visibility.Hidden;
                db.Update_cus_btn.Visibility = Visibility.Hidden;
                db.add_cus_btn.Visibility = Visibility.Hidden;
                db.Show();
                break;

                case "T":
                db.package_btn.Visibility = Visibility.Hidden;
                db.Emplyee_show.Visibility = Visibility.Hidden;
                db.Branch_managerment_btn.Visibility = Visibility.Hidden;
                db.emp_manage_btn.Visibility = Visibility.Hidden;
                db.Show();
                break;

                case "P":
                db.package_btn.Visibility = Visibility.Hidden;
                db.Emplyee_show.Visibility = Visibility.Hidden;
                db.Branch_managerment_btn.Visibility = Visibility.Hidden;
                db.emp_manage_btn.Visibility = Visibility.Hidden;
                db.Show();
                break;

                case "R":
                db.delivery_btn.Visibility = Visibility.Hidden;
                db.Emplyee_show.Visibility = Visibility.Hidden;
                db.Branch_managerment_btn.Visibility = Visibility.Hidden;
                db.Show();
                break;

                case "S":
                db.Show();
                break;
                default: new exitConfrim();
                break;


            }

        }
    }
}
