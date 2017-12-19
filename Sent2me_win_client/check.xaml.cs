using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Data;

namespace Sent2me_win_client {
    /// <summary>
    /// Interaction logic for check.xaml
    /// </summary>
    public partial class check : Window {
        public check() {
            InitializeComponent();
        }

        private void Search_btn_Click(object sender, RoutedEventArgs e) {
            sqlConnection sql = new sqlConnection();
            String strQry = @"SELECT branch_id as 'สาขาที่พัสดุอยู่' , Delivery_desciption.des_text as 'สถานะ' , time as 'เวลา'
                            FROM Delivery_detail INNER JOIN Delivery_desciption ON Delivery_desciption.dev_des_id = Delivery_detail.dev_des_id  
                            WHERE dev_id = @id";
            sql.sqlCon.Open();
            sql.cmd = new SqlCommand(strQry, sql.sqlCon);
            sql.cmd.Parameters.AddWithValue("@id", Search_box.Text);
            sql.cmd.ExecuteNonQuery();
            sql.da = new SqlDataAdapter(sql.cmd);
            DataSet ds = new DataSet();
            sql.da.Fill(ds, "package");
            package_grid.ItemsSource = ds.Tables["package"].DefaultView;
            sql.sqlCon.Close();

        }
    }
}
