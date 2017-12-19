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

namespace Sent2me_win_client
{
    /// <summary>
    /// Interaction logic for package_price_editor.xaml
    /// </summary>
    public partial class package_price_editor : Window
    {
        String branch_id;
        public package_price_editor(String branch_id)
        {
            InitializeComponent();
            this.branch_id = branch_id;
            Show_branch.Content = "การปรับอัตราค่าขนส่งสาขา " + branch_id;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e) {

            sent2meDataSet sent2meDataSet = ((Sent2me_win_client.sent2meDataSet)(this.FindResource("sent2meDataSet")));
            // Load data into the table Package_type. You can modify this code as needed.
            sent2meDataSetTableAdapters.Package_typeTableAdapter sent2meDataSetPackage_typeTableAdapter = new Sent2me_win_client.sent2meDataSetTableAdapters.Package_typeTableAdapter();
            sent2meDataSetPackage_typeTableAdapter.Fill(sent2meDataSet.Package_type);
            CollectionViewSource package_typeViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("package_typeViewSource")));
            package_typeViewSource.View.MoveCurrentToFirst();
            reloadtable();
            
        }

        private void add_package_price_Click(object sender, RoutedEventArgs e) {
            if (MessageBox.Show("ยืนยันการเพิ่มข้อมูล", "ยืนยัน", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes) {
                try {
                String strQry = @"INSERT INTO Package_price (package_type_id
                                ,weight_rate
                                ,cost
                                ,branch_id) VALUES (@id,@weight,@cost,@branch_id)";

                sqlConnection sql = new sqlConnection();
                sql.sqlCon.Open();
                sql.cmd = new SqlCommand(strQry,sql.sqlCon);
                sql.cmd.Parameters.AddWithValue("@id", package_type_nameComboBox.SelectedValuePath);
                sql.cmd.Parameters.AddWithValue("@weight",Weight_rate_txt.Text);
                sql.cmd.Parameters.AddWithValue("@cost", cost_txt.Text);
                sql.cmd.Parameters.AddWithValue("@branch_id", branch_id);
                sql.cmd.ExecuteNonQuery();
                sql.sqlCon.Close();
                    reloadtable();
                } catch (SqlException sqlerr) {
                    MessageBox.Show(sqlerr.Message);
                 }

            }

            
        }

        private void delete_package_price_Click(object sender, RoutedEventArgs e) {
            if (MessageBox.Show("ยืนยันการลบ", "ยืนยัน", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes) {
                try {
                    DataRowView drv = (DataRowView)package_price_datagrid.SelectedItem;
                    String id_String = (drv["ลำดับ"]).ToString();
                    String strQry = @"DELETE FROM package_price WHERE id = @id";
                    sqlConnection sql = new sqlConnection();
                    sql.sqlCon.Open();
                    sql.cmd = new SqlCommand(strQry, sql.sqlCon);
                    sql.cmd.Parameters.AddWithValue("@id", id_String);
                    sql.cmd.ExecuteNonQuery();
                    sql.sqlCon.Close();
                    MessageBox.Show("ลบสำเร็จ");
                    reloadtable();
                } catch (SqlException sqlerr) {
                    MessageBox.Show(sqlerr.Message);
                }
            }
            
        }

        private void reloadtable() {
            try {
                String strQry = "SELECT Package_type_id as 'ชนิดพัสดุ' , weight_rate as 'น้ำหนักไม่เกิน' , cost as 'ราคา' ,id as 'ลำดับ'  FROM Package_price WHERE branch_id = @id";
                sqlConnection sql = new sqlConnection();
                sql.sqlCon.Open();
                sql.cmd = new SqlCommand(strQry, sql.sqlCon);
                sql.cmd.Parameters.AddWithValue("@id", branch_id);
                sql.cmd.ExecuteNonQuery();
                sql.sqlCon.Close();
                sql.da = new SqlDataAdapter(sql.cmd);
                DataSet ds = new DataSet();
                sql.da.Fill(ds, "Package_price_current");
                package_price_datagrid.ItemsSource = ds.Tables["Package_price_current"].DefaultView;
            } catch (SqlException sqlerr) {
                MessageBox.Show(sqlerr.Message);
            }
        }

        private void close_btn_Click(object sender, RoutedEventArgs e) {
            Close();
        }
    }
}
