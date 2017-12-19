using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Animation;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Reporting.WinForms;
using System.Threading;
using System.Windows.Threading;

namespace Sent2me_win_client {
    /// <summary>
    /// Interaction logic for dashboard.xaml
    /// </summary>
    public partial class dashboard : Window {
        private const int GWL_STYLE = -16;
        private const int WS_SYSMENU = 0x80000;
        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        String empid;
        String branch;
        private DispatcherTimer dispatcherTimer;
        DataSet ds = new DataSet();





        public dashboard(DataSet ds) {
            InitializeComponent();
            Loaded += ToolWindow_Loaded;
            //Closing += new System.ComponentModel.CancelEventHandler(Window_Closing);
            
            display_empid.Content = "EMP ID: " + ds.Tables["employee"].Rows[0]["emp_id"].ToString();
            display_title.Content = "รหัสตำแหน่ง: " + ds.Tables["employee"].Rows[0]["title_id"].ToString();
            display_branch.Content = "สาขาที่ประจำ :" + ds.Tables["employee"].Rows[0]["branch_id"].ToString();
            empid = ds.Tables["employee"].Rows[0]["emp_id"].ToString();
            branch = ds.Tables["employee"].Rows[0]["branch_id"].ToString();

            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();


        }


        private void dispatcherTimer_Tick(object sender, EventArgs e) {
            //Things which happen after 1 timer interval

            time.Content = DateTime.Now;
            //Disable the timer
            CommandManager.InvalidateRequerySuggested();
        }
        void ToolWindow_Loaded(object sender, RoutedEventArgs e) {
            // Code to remove close box from window
            var hwnd = new System.Windows.Interop.WindowInteropHelper(this).Handle;
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);
        }

        private void btnLeftMenuHide_Click(object sender, RoutedEventArgs e) {
            ShowHideMenu("sbHideLeftMenu", btnLeftMenuHide, btnLeftMenuShow, pnlLeftMenu);
        }

        private void btnLeftMenuShow_Click(object sender, RoutedEventArgs e) {
            ShowHideMenu("sbShowLeftMenu", btnLeftMenuHide, btnLeftMenuShow, pnlLeftMenu);
        }
        private void ShowHideMenu(string Storyboard, Button btnHide, Button btnShow, StackPanel pnl) {
            Storyboard sb = Resources[Storyboard] as Storyboard;
            sb.Begin(pnl);

            if (Storyboard.Contains("Show")) {
                btnHide.Visibility = Visibility.Visible;
                btnShow.Visibility = Visibility.Hidden;
            } else if (Storyboard.Contains("Hide")) {
                btnHide.Visibility = Visibility.Hidden;
                btnShow.Visibility = Visibility.Visible;
            }
        }

        private void package_btn_Click(object sender, RoutedEventArgs e) {
            ShowHideMenu("sbShowRightMenu", Hide_pack, package_btn, Package_menu);
            //ShowHideMenu("sbHideLeftMenu", btnLeftMenuHide, btnLeftMenuShow, pnlLeftMenu);

            try {
                String strQry = "SELECT * FROM package_view";
                sqlConnection sql = new sqlConnection();
                sql.sqlCon.Open();
                sql.cmd = new SqlCommand(strQry, sql.sqlCon);
                sql.cmd.ExecuteNonQuery();
                sql.da = new SqlDataAdapter(sql.cmd);
                sql.sqlCon.Close();
                DataSet ds = new DataSet();
                sql.da.Fill(ds, "Package");
                Package_dataGrid.ItemsSource = ds.Tables["Package"].DefaultView;

            } catch (Exception err) {
                Console.WriteLine(err.Message);
            }

            try {
                String strQry = "SELECT * FROM package";
                sqlConnection sql = new sqlConnection();
                sql.sqlCon.Open();
                sql.cmd = new SqlCommand(strQry, sql.sqlCon);
                sql.cmd.ExecuteNonQuery();
                sql.da = new SqlDataAdapter(sql.cmd);
                sql.sqlCon.Close();
                DataSet ds = new DataSet();
                sql.da.Fill(ds, "Package");
                Gird_package_0.ItemsSource = ds.Tables["Package"].DefaultView;

            } catch (Exception err) {
                Console.WriteLine(err.Message);
            }


        }

        private void Hide_pack_Click(object sender, RoutedEventArgs e) {
            ShowHideMenu("sbHideRightMenu", Hide_pack, package_btn, Package_menu);

        }

        private void delivery_btn_Click(object sender, RoutedEventArgs e) {
            ShowHideMenu("sbShowDeli", hide_Deli, delivery_btn, deli_menu);
            //ShowHideMenu("sbHideLeftMenu", btnLeftMenuHide, btnLeftMenuShow, pnlLeftMenu);
            try {
                String strQry = "SELECT dev_id as 'รหัสติดตามพัสดุ' ,Package_id as 'รหัสพัสดุ' , status as 'สถานะ' ,dev_datetime as 'เวลา' FROM Delivery ";
                sqlConnection sql = new sqlConnection();
                sql.sqlCon.Open();
                sql.cmd = new SqlCommand(strQry, sql.sqlCon);
                sql.cmd.ExecuteNonQuery();
                sql.da = new SqlDataAdapter(sql.cmd);
                sql.sqlCon.Close();
                DataSet ds = new DataSet();
                sql.da.Fill(ds, "Delivery");
                Delivery_pack_thisbranch_datagrid.ItemsSource = ds.Tables["Delivery"].DefaultView;

            } catch (SqlException sqlErr) {
                MessageBox.Show(sqlErr.Message);
            } catch (Exception err) {
                Console.WriteLine(err.ToString());
            }

            try {
                String strQry = "SELECT * FROM Delivery WHERE Branch_id = @branch";
                sqlConnection sql = new sqlConnection();
                sql.sqlCon.Open();
                sql.cmd = new SqlCommand(strQry, sql.sqlCon);
                sql.cmd.Parameters.AddWithValue("@branch", branch);
                sql.cmd.ExecuteNonQuery();
                sql.da = new SqlDataAdapter(sql.cmd);
                sql.sqlCon.Close();
                DataSet ds = new DataSet();
                sql.da.Fill(ds, "Delivery");
                Deli_package_datagrid.ItemsSource = ds.Tables["Delivery"].DefaultView;

            } catch (SqlException sqlErr) {
                MessageBox.Show(sqlErr.Message);
            } catch (Exception err) {
                Console.WriteLine(err.ToString());
            }

        }
        private void hide_Deli_Click(object sender, RoutedEventArgs e) {

            ShowHideMenu("sbHideDeli", hide_Deli, delivery_btn, deli_menu);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e) {
            ShowHideMenu("sbShowLeftMenu", btnLeftMenuHide, btnLeftMenuShow, pnlLeftMenu);

            sent2meDataSet sent2meDataSet = ((sent2meDataSet)(this.FindResource("sent2meDataSet")));
            // Load data into the table Package_type. You can modify this code as needed.
            sent2meDataSetTableAdapters.Package_typeTableAdapter sent2meDataSetPackage_typeTableAdapter = new sent2meDataSetTableAdapters.Package_typeTableAdapter();
            sent2meDataSetPackage_typeTableAdapter.Fill(sent2meDataSet.Package_type);
            CollectionViewSource package_typeViewSource = ((CollectionViewSource)(this.FindResource("package_typeViewSource")));
            package_typeViewSource.View.MoveCurrentToFirst();
            // Load data into the table Delivery_desciption. You can modify this code as needed.
            sent2meDataSetTableAdapters.Delivery_desciptionTableAdapter sent2meDataSetDelivery_desciptionTableAdapter = new sent2meDataSetTableAdapters.Delivery_desciptionTableAdapter();
            sent2meDataSetDelivery_desciptionTableAdapter.Fill(sent2meDataSet.Delivery_desciption);
            CollectionViewSource delivery_desciptionViewSource = ((CollectionViewSource)(this.FindResource("delivery_desciptionViewSource")));
            delivery_desciptionViewSource.View.MoveCurrentToFirst();
            // Load data into the table Branch1. You can modify this code as needed.
            sent2meDataSetTableAdapters.BranchTableAdapter sent2meDataSetBranch1TableAdapter = new sent2meDataSetTableAdapters.BranchTableAdapter();
            sent2meDataSetBranch1TableAdapter.Fill(sent2meDataSet.Branch);
            CollectionViewSource branch1ViewSource = ((CollectionViewSource)(this.FindResource("branchViewSource")));
            branch1ViewSource.View.MoveCurrentToFirst();


            // Load data into the table Branch. You can modify this code as needed.
            Sent2me_win_client.sent2meDataSetTableAdapters.BranchTableAdapter sent2meDataSetBranchTableAdapter = new Sent2me_win_client.sent2meDataSetTableAdapters.BranchTableAdapter();
            sent2meDataSetBranchTableAdapter.Fill(sent2meDataSet.Branch);
            System.Windows.Data.CollectionViewSource branchViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("branchViewSource")));
            branchViewSource.View.MoveCurrentToFirst();
            // Load data into the table Title. You can modify this code as needed.
            Sent2me_win_client.sent2meDataSetTableAdapters.TitleTableAdapter sent2meDataSetTitleTableAdapter = new Sent2me_win_client.sent2meDataSetTableAdapters.TitleTableAdapter();
            sent2meDataSetTitleTableAdapter.Fill(sent2meDataSet.Title);
            System.Windows.Data.CollectionViewSource titleViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("titleViewSource")));
            titleViewSource.View.MoveCurrentToFirst();
            // Load data into the table Employee. You can modify this code as needed.
            Sent2me_win_client.sent2meDataSetTableAdapters.EmployeeTableAdapter sent2meDataSetEmployeeTableAdapter = new Sent2me_win_client.sent2meDataSetTableAdapters.EmployeeTableAdapter();
            sent2meDataSetEmployeeTableAdapter.Fill(sent2meDataSet.Employee);
            System.Windows.Data.CollectionViewSource employeeViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("employeeViewSource")));
            employeeViewSource.View.MoveCurrentToFirst();
        }

        private void product_Detail(object sender, MouseButtonEventArgs e) {
            dev_stat.Text = "";
            dev_id.Text = "";
            // MessageBox.Show(result);

            try {
                sqlConnection sql = new sqlConnection();
                DataRowView drv = (DataRowView)Package_dataGrid.SelectedItem;
                String result = (drv["รหัสพัสดุ"]).ToString();
                int value = Convert.ToInt32(result);
                String strQry = @"SELECT Delivery_detail.branch_id,Branch.branch_name,time,Delivery_detail.emp_id,Delivery_desciption.des_text 
                                FROM Delivery INNER JOIN Delivery_detail ON Delivery.dev_id = Delivery_detail.dev_id 
                                INNER JOIN Delivery_desciption ON Delivery_detail.dev_des_id = Delivery_desciption.dev_des_id 
                                INNER JOIN Package ON Package.package_id = Delivery_detail.package_id 
                                INNER JOIN Package_detail ON Package.package_id = Package_detail.package_id 
                                INNER JOIN Package_type ON Package_detail.package_type_id = Package_type.package_type_id 
                                INNER JOIN Branch ON Branch.branch_id = Delivery_detail.branch_id 
                                WHERE Package.Package_id = @id
                                ORDER BY Delivery_detail.time DESC";
                sql.sqlCon.Open();
                sql.cmd = new SqlCommand(strQry, sql.sqlCon);
                sql.cmd.Parameters.AddWithValue("@id", value);
                sql.cmd.ExecuteNonQuery();
                sql.da = new SqlDataAdapter(sql.cmd);
                sql.sqlCon.Close();
                DataSet ds1 = new DataSet();
                sql.da.Fill(ds1, "package_delivery");
                delivery_packageDatagrid.ItemsSource = ds1.Tables["package_delivery"].DefaultView;
                /*DETAIL++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++*/
                strQry = @"SELECT Delivery.dev_id,Branch.branch_name,Delivery_detail.branch_id,time,Delivery_detail.emp_id,Delivery_desciption.des_text 
                                FROM Delivery INNER JOIN Delivery_detail ON Delivery.dev_id = Delivery_detail.dev_id 
                                INNER JOIN Delivery_desciption ON Delivery_detail.dev_des_id = Delivery_desciption.dev_des_id 
                                INNER JOIN Package ON Package.package_id = Delivery_detail.package_id 
                                INNER JOIN Package_detail ON Package.package_id = Package_detail.package_id 
                                INNER JOIN Package_type ON Package_detail.package_type_id = Package_type.package_type_id 
                                INNER JOIN Branch ON Branch.branch_id = Delivery_detail.branch_id 
                                WHERE Package.Package_id = @id
                                ORDER BY Delivery_detail.time DESC";
                sql.sqlCon.Open();
                sql.cmd = new SqlCommand(strQry, sql.sqlCon);
                sql.cmd.Parameters.AddWithValue("@id", value);
                sql.cmd.ExecuteNonQuery();
                sql.da = new SqlDataAdapter(sql.cmd);
                sql.sqlCon.Close();
                DataSet ds2 = new DataSet();
                sql.da.Fill(ds2, "package_delivery");
                deli_label.Content = "รายละเอียดการขนส่ง : พัสดุหมายเลข " + result;
                dev_stat.Text = ds2.Tables["package_delivery"].Rows[0][5].ToString();
                dev_id.Text = ds2.Tables["package_delivery"].Rows[0][0].ToString();

            } catch (SqlException sqlErr) {
                MessageBox.Show(sqlErr.Message);
            } catch (Exception err) {
                Console.WriteLine(err.ToString());
            }

        }

        private void Search_package_btn_Click(object sender, RoutedEventArgs e) {
            String searchString = search_txt.Text;
            try {
                String strQry = @"SELECT * FROM package_view WHERE ชื่อผู้ส่ง LIKE '%" + searchString + "' " +
                    "OR ชื่อผู้รับ LIKE '%" + searchString + "' " +
                    "OR ที่อยู่ผู้ส่ง LIKE '%" + searchString + "' " +
                    "OR ที่อยู่ผู้รับ LIKE '%" + searchString + "'";
                sqlConnection sql = new sqlConnection();
                sql.sqlCon.Open();
                sql.cmd = new SqlCommand(strQry, sql.sqlCon);
                sql.cmd.ExecuteNonQuery();
                sql.da = new SqlDataAdapter(sql.cmd);
                sql.sqlCon.Close();
                DataSet ds = new DataSet();
                sql.da.Fill(ds, "Package");
                Package_dataGrid.ItemsSource = ds.Tables["Package"].DefaultView;

            } catch (SqlException sqlErr) {
                MessageBox.Show(sqlErr.Message);
            } catch (Exception err) {
                Console.WriteLine(err.ToString());
            }
        }




        private void GetCurrentTime_Click(object sender, RoutedEventArgs e) {
            Regis_timePicker.Value = DateTime.Now;
        }
        private void Package_detail_preview(object sender, MouseButtonEventArgs e) {
            try {
                DataRowView drv = (DataRowView)Gird_package_0.SelectedItem;
                String result = (drv["package_id"]).ToString();
                int value = Convert.ToInt32(result);
                sqlConnection sql = new sqlConnection();
                String strQry = @"SELECT * FROM Package_detail WHERE package_id = @id";
                sql.sqlCon.Open();
                sql.cmd = new SqlCommand(strQry, sql.sqlCon);
                sql.cmd.Parameters.AddWithValue("@id", value);
                sql.cmd.ExecuteNonQuery();
                sql.da = new SqlDataAdapter(sql.cmd);
                sql.sqlCon.Close();
                DataSet ds = new DataSet();
                sql.da.Fill(ds, "package");
                //package_detail_preview.ItemsSource = ds.Tables["package_detail_preview"].DefaultView;
                Package_id_txt.Text = ds.Tables["package"].Rows[0]["package_id"].ToString();
                package_name_txt.Text = ds.Tables["package"].Rows[0]["package_name"].ToString();
                package_sender_txt.Text = ds.Tables["package"].Rows[0]["sender_name"].ToString();
                sender_add_txt.Text = ds.Tables["package"].Rows[0]["sender_add"].ToString();
                rec_name_txt.Text = ds.Tables["package"].Rows[0]["rec_name"].ToString();
                rec_add_txt.Text = ds.Tables["package"].Rows[0]["rec_add"].ToString();
                weight_txt.Text = ds.Tables["package"].Rows[0]["weight"].ToString();
                if (ds.Tables["package"].Rows[0]["package_type_id"].ToString() == "A") {
                    package_type_nameComboBox.SelectedIndex = 0;
                } else if (ds.Tables["package"].Rows[0]["package_type_id"].ToString() == "E") {
                    package_type_nameComboBox.SelectedIndex = 1;
                } else if (ds.Tables["package"].Rows[0]["package_type_id"].ToString() == "R") {
                    package_type_nameComboBox.SelectedIndex = 2;
                }


            } catch (SqlException sqlErr) {
                MessageBox.Show(sqlErr.Message);
            } catch (Exception err) {
                Console.WriteLine(err.ToString());
            }
        }
        private void Insert_btn_Click_1(object sender, RoutedEventArgs e) {
            String strQry = @"INSERT INTO Package (regis_date,emp_id,branch_id) VALUES(@date,@emp_id,@branch)";
            if (Regis_timePicker.Value == null) {
                MessageBox.Show("ไม่ได้ใส่วันที่");
                return;
            }
            try {
                sqlConnection sql = new sqlConnection();
                sql.sqlCon.Open();
                sql.cmd = new SqlCommand(strQry, sql.sqlCon);
                sql.cmd.Parameters.AddWithValue("@date", Regis_timePicker.Value);
                sql.cmd.Parameters.AddWithValue("@emp_id", empid);
                sql.cmd.Parameters.AddWithValue("@branch", branch);
                sql.cmd.ExecuteNonQuery();
                sql.sqlCon.Close();
            } catch (SqlException sqlErr) {
                MessageBox.Show(sqlErr.ToString());
            } catch (Exception err) {
                Console.WriteLine(err.ToString());
            }
            try {
                sqlConnection sql = new sqlConnection();
                strQry = "SELECT * FROM Package_detail ORDER BY package_id DESC;";
                sql.sqlCon.Open();
                sql.cmd = new SqlCommand(strQry, sql.sqlCon);
                sql.da = new SqlDataAdapter(sql.cmd);
                sql.sqlCon.Close();
                DataSet ds = new DataSet();
                sql.da.Fill(ds, "package");
                Package_id_txt.Text = ds.Tables["package"].Rows[0]["package_id"].ToString();
                package_name_txt.Text = ds.Tables["package"].Rows[0]["package_name"].ToString();
                package_sender_txt.Text = ds.Tables["package"].Rows[0]["sender_name"].ToString();
                sender_add_txt.Text = ds.Tables["package"].Rows[0]["sender_add"].ToString();
                rec_name_txt.Text = ds.Tables["package"].Rows[0]["rec_name"].ToString();
                rec_add_txt.Text = ds.Tables["package"].Rows[0]["rec_add"].ToString();
                weight_txt.Text = ds.Tables["package"].Rows[0]["weight"].ToString();
                if (ds.Tables["package"].Rows[0]["package_type_id"].ToString() == "A") {
                    package_type_nameComboBox.SelectedIndex = 0;
                } else if (ds.Tables["package"].Rows[0]["package_type_id"].ToString() == "E") {
                    package_type_nameComboBox.SelectedIndex = 1;
                } else if (ds.Tables["package"].Rows[0]["package_type_id"].ToString() == "R") {
                    package_type_nameComboBox.SelectedIndex = 2;
                }

            } catch (SqlException sqlErr) {
                MessageBox.Show(sqlErr.ToString());
            } catch (Exception err) {
                Console.WriteLine(err.ToString());
            }

            try {
                strQry = "SELECT * FROM package";
                sqlConnection sql = new sqlConnection();
                sql.sqlCon.Open();
                sql.cmd = new SqlCommand(strQry, sql.sqlCon);
                sql.cmd.ExecuteNonQuery();
                sql.da = new SqlDataAdapter(sql.cmd);
                sql.sqlCon.Close();
                DataSet ds = new DataSet();
                sql.da.Fill(ds, "Package");
                Gird_package_0.ItemsSource = ds.Tables["Package"].DefaultView;


            } catch (SqlException sqlErr) {
                MessageBox.Show(sqlErr.ToString());
            } catch (Exception err) {
                Console.WriteLine(err.ToString());
            }
            Package_dataGrid.SelectedItem = Package_dataGrid.Items[Package_dataGrid.Items.Count - 1];
            Package_dataGrid.ScrollIntoView(Package_dataGrid.Items[Package_dataGrid.Items.Count - 1]);
            DataGridRow dgrow = (DataGridRow)Package_dataGrid.ItemContainerGenerator.ContainerFromItem(Package_dataGrid.Items[Package_dataGrid.Items.Count - 1]);
            dgrow.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));


        }

        private void Delete_btn_Click(object sender, RoutedEventArgs e) {
            String strQry = "DELETE FROM package_detail WHERE package_id = @id";
            try {
                if (MessageBox.Show("คุณแน่ใจใช้ไหมที่จะลบเรดคอร์ดนี้ ?", "REMOVE ALERT", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes) {
                    DataRowView drv = (DataRowView)Gird_package_0.SelectedItem;
                    String result = (drv["รหัสพัสดุ"]).ToString();
                    int value = Convert.ToInt32(result);
                    sqlConnection sql = new sqlConnection();
                    sql.sqlCon.Open();
                    sql.cmd = new SqlCommand(strQry, sql.sqlCon);
                    sql.cmd.Parameters.AddWithValue("@id", value);
                    sql.cmd.ExecuteNonQuery();
                    sql.sqlCon.Close();
                } else {
                    return;
                }


            } catch (SqlException sqlErr) {
                MessageBox.Show(sqlErr.ToString());
            } catch (Exception err) {
                Console.WriteLine(err.ToString());
            }

            try {
                strQry = "SELECT * FROM package";
                sqlConnection sql = new sqlConnection();
                sql.sqlCon.Open();
                sql.cmd = new SqlCommand(strQry, sql.sqlCon);
                sql.cmd.ExecuteNonQuery();
                sql.da = new SqlDataAdapter(sql.cmd);
                sql.sqlCon.Close();
                DataSet ds = new DataSet();
                sql.da.Fill(ds, "Package");
                Gird_package_0.ItemsSource = ds.Tables["Package"].DefaultView;

            } catch (SqlException sqlErr) {
                MessageBox.Show(sqlErr.ToString());
            } catch (Exception err) {
                Console.WriteLine(err.ToString());
            }

        }

        private void insert_detail_btn_Click(object sender, RoutedEventArgs e) {

            if (MessageBox.Show("คุณต้องการที่จะแก้ไข หรือเพิ่มข้อมูลใช้ไหม", "ยืนยันการแก้ไข", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes) {
                try {

                    int value = Convert.ToInt32(Package_id_txt.Text);
                    sqlConnection sql = new sqlConnection();
                    String strQry = @"UPDATE Package_detail SET package_name = @package_name,sender_name = @sender_name ,sender_add = @sender_add
                                  ,rec_name = @rec_name,rec_add = @rec_add ,package_type_id = @type_id ,weight = @weight WHERE package_id = @id";
                    sql.sqlCon.Open();
                    sql.cmd = new SqlCommand(strQry, sql.sqlCon);
                    sql.cmd.Parameters.AddWithValue("@id", value);
                    sql.cmd.Parameters.AddWithValue("@package_name", package_name_txt.Text);
                    sql.cmd.Parameters.AddWithValue("@sender_name", package_sender_txt.Text);
                    sql.cmd.Parameters.AddWithValue("@sender_add", sender_add_txt.Text);
                    sql.cmd.Parameters.AddWithValue("@rec_name", rec_name_txt.Text);
                    sql.cmd.Parameters.AddWithValue("@rec_add", rec_add_txt.Text);
                    sql.cmd.Parameters.AddWithValue("@weight", weight_txt.Text);

                    if (package_type_nameComboBox.SelectedIndex == 0) {
                        sql.cmd.Parameters.AddWithValue("@type_id", "A");
                    } else if (package_type_nameComboBox.SelectedIndex == 1) {
                        sql.cmd.Parameters.AddWithValue("@type_id", "E");
                    } else if (package_type_nameComboBox.SelectedIndex == 2) {
                        sql.cmd.Parameters.AddWithValue("@type_id", "R");
                    }
                    sql.cmd.ExecuteNonQuery();
                    sql.sqlCon.Close();
                } catch (SqlException sqlErr) {
                    MessageBox.Show(sqlErr.ToString());
                } catch (Exception err) {
                    Console.WriteLine(err.ToString());
                }
                try {
                    String strQry = "SELECT * FROM package";
                    sqlConnection sql = new sqlConnection();
                    sql.sqlCon.Open();
                    sql.cmd = new SqlCommand(strQry, sql.sqlCon);
                    sql.cmd.ExecuteNonQuery();
                    sql.da = new SqlDataAdapter(sql.cmd);
                    sql.sqlCon.Close();
                    DataSet ds = new DataSet();
                    sql.da.Fill(ds, "Package");
                    Gird_package_0.ItemsSource = ds.Tables["Package"].DefaultView;

                } catch (SqlException sqlErr) {
                    MessageBox.Show(sqlErr.ToString());
                } catch (Exception err) {
                    Console.WriteLine(err.ToString());
                }

                try {
                    String strQry = "SELECT id FROM package_price WHERE branch_id = @bid AND weight_rate > @weight ";
                    sqlConnection sql = new sqlConnection();
                    sql.sqlCon.Open();
                    sql.cmd = new SqlCommand(strQry, sql.sqlCon);
                    sql.cmd.Parameters.AddWithValue("@bid", branch);
                    sql.cmd.Parameters.AddWithValue("@weight", weight_txt.Text);
                    sql.cmd.ExecuteNonQuery();
                    sql.da = new SqlDataAdapter(sql.cmd);
                    DataSet ds = new DataSet();
                    sql.da.Fill(ds, "cost_id");
                    String id = ds.Tables["cost_id"].Rows[0]["id"].ToString();
                    sql.sqlCon.Close();


                    strQry = "INSERT INTO Bill(package_id , cus_id , dev_id , id) VALUES (@pid,@cus_id,@dev_id,@id)";
                    sql.sqlCon.Open();
                    sql.cmd = new SqlCommand(strQry, sql.sqlCon);
                    sql.cmd.Parameters.AddWithValue("@pid", Convert.ToInt32(Package_id_txt.Text));
                    sql.cmd.Parameters.AddWithValue("@dev_id", "DEV" + Package_id_txt.Text);
                    sql.cmd.Parameters.AddWithValue("@id", id);
                    if (customer_id_txt.Text == "") {
                        sql.cmd.Parameters.AddWithValue("@cus_id", "6001");
                    }else {
                        sql.cmd.Parameters.AddWithValue("@cus_id", Convert.ToInt32(customer_id_txt.Text));
                    }
                    sql.cmd.ExecuteNonQuery();
                    sql.sqlCon.Close();
                    MessageBox.Show("การแก้ไขหรือเพิ่มข้อมูลสำเร็จ");
                } catch (SqlException sqlErr) {
                    MessageBox.Show("ฐานข้อมูลผิดพลาด");
                } catch (Exception err) {
                    Console.WriteLine(err.ToString());
                }
            }

        }

        private void Button_Click_1(object sender, RoutedEventArgs e) {
            String searchString = search_txt.Text;
            try {
                String strQry = @"SELECT * FROM package_view WHERE ชื่อผู้ส่ง LIKE '%" + searchString + "' " +
                    "OR ชื่อผู้รับ LIKE '%" + searchString + "' " +
                    "OR ที่อยู่ผู้ส่ง LIKE '%" + searchString + "' " +
                    "OR ที่อยู่ผู้รับ LIKE '%" + searchString + "'";
                sqlConnection sql = new sqlConnection();
                sql.sqlCon.Open();
                sql.cmd = new SqlCommand(strQry, sql.sqlCon);
                sql.cmd.ExecuteNonQuery();
                sql.da = new SqlDataAdapter(sql.cmd);
                sql.sqlCon.Close();
                DataSet ds = new DataSet();
                sql.da.Fill(ds, "Package");
                Package_dataGrid.ItemsSource = ds.Tables["Package"].DefaultView;
            } catch (SqlException sqlErr) {
                MessageBox.Show(sqlErr.Message);
            } catch (Exception err) {
                Console.WriteLine(err.ToString());
            }
        }
        private void Onclick_deli_package_cell(object sender, MouseButtonEventArgs e) {
            try {
                DataRowView drv = (DataRowView)Deli_package_datagrid.SelectedItem;
                String result = (drv["package_id"]).ToString();
                int value = Convert.ToInt32(result);
                String strQry = @"SELECT * FROM Delivery_detail WHERE package_id = @id";
                sqlConnection sql = new sqlConnection();
                sql.sqlCon.Open();
                sql.cmd = new SqlCommand(strQry, sql.sqlCon);
                sql.cmd.Parameters.AddWithValue("@id", value);
                sql.cmd.ExecuteNonQuery();
                sql.da = new SqlDataAdapter(sql.cmd);
                sql.sqlCon.Close();
                DataSet ds = new DataSet();
                sql.da.Fill(ds, "delivery_detail");
                Deli_detail_datagrid.ItemsSource = ds.Tables["delivery_detail"].DefaultView;
            } catch (SqlException sqlErr) {
                MessageBox.Show(sqlErr.Message);
            } catch (Exception err) {
                Console.WriteLine(err.ToString());
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e) {
            try {
                String strQry = "SELECT * FROM Delivery ";
                sqlConnection sql = new sqlConnection();
                sql.sqlCon.Open();
                sql.cmd = new SqlCommand(strQry, sql.sqlCon);
                sql.cmd.ExecuteNonQuery();
                sql.da = new SqlDataAdapter(sql.cmd);
                sql.sqlCon.Close();
                DataSet ds = new DataSet();
                sql.da.Fill(ds, "Delivery");
                Delivery_pack_thisbranch_datagrid.ItemsSource = ds.Tables["Delivery"].DefaultView;

            } catch (SqlException sqlErr) {
                MessageBox.Show(sqlErr.Message);
            } catch (Exception err) {
                Console.WriteLine(err.ToString());
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e) {
            String searchStr = search_deli_txt.Text;
            try {
                String strQry = "SELECT * FROM Delivery WHERE Package_id LIKE '" + searchStr + "%' OR dev_id LIKE '%" + searchStr + "%' ";
                sqlConnection sql = new sqlConnection();
                sql.sqlCon.Open();
                sql.cmd = new SqlCommand(strQry, sql.sqlCon);
                sql.cmd.ExecuteNonQuery();
                sql.da = new SqlDataAdapter(sql.cmd);
                sql.sqlCon.Close();
                DataSet ds = new DataSet();
                sql.da.Fill(ds, "Delivery");
                Delivery_pack_thisbranch_datagrid.ItemsSource = ds.Tables["Delivery"].DefaultView;
                search_deli_txt.Text = "";

            } catch (SqlException sqlErr) {
                MessageBox.Show(sqlErr.Message);
            } catch (Exception err) {
                Console.WriteLine(err.ToString());
            }

        }

        private void Exit_btn_Click(object sender, RoutedEventArgs e) {
            new exitConfrim();
        }

        private void Insert_delivery_btn_Click(object sender, RoutedEventArgs e) {

            try {
                int dev_stat_id = 0;
                String branch_select = branch;
                switch (des_textComboBox.SelectedIndex) {
                    case 0:
                    dev_stat_id = 1;
                    break;
                    case 1:
                    dev_stat_id = 2;
                    break;
                    case 2:
                    dev_stat_id = 3;
                    //MessageBox.Show(branch1ComboBox.SelectedValuePath.ToString());
                    branch_select = branch1ComboBox.SelectedValuePath.ToString();
                    break;
                    case 3:
                    dev_stat_id = 4;
                    break;
                    case 4:
                    dev_stat_id = 5;
                    break;
                    case 5:
                    dev_stat_id = 6;
                    break;
                }
                DataRowView drv = (DataRowView)Deli_package_datagrid.SelectedItem;
                String package_id_String = (drv["package_id"]).ToString();
                String dev_id = (drv["dev_id"]).ToString();
                int value = Convert.ToInt32(package_id_String);
                String strQry = @"INSERT INTO Delivery_detail (dev_id,package_id,branch_id,status,dev_des_id,emp_id,time) VALUES (@dev_id,@package_id,@branch_id,@status,@dev_des_id,@emp_id,@time)";
                sqlConnection sql = new sqlConnection();
                sql.sqlCon.Open();
                sql.cmd = new SqlCommand(strQry, sql.sqlCon);
                sql.cmd.Parameters.AddWithValue("@dev_id", dev_id);
                sql.cmd.Parameters.AddWithValue("@package_id", value);
                sql.cmd.Parameters.AddWithValue("@branch_id", branch_select);
                sql.cmd.Parameters.AddWithValue("@status", 0);
                sql.cmd.Parameters.AddWithValue("@dev_des_id", dev_stat_id);
                sql.cmd.Parameters.AddWithValue("@emp_id", empid);
                sql.cmd.Parameters.AddWithValue("@time", DatePicker.Value);
                sql.cmd.ExecuteNonQuery();
                sql.sqlCon.Close();
                MessageBox.Show("ทำรายการข้อมูลสำเร็จ", "สำเร็จ", MessageBoxButton.OK, MessageBoxImage.Information);

            } catch (SqlException sqlErr) {
                MessageBox.Show(sqlErr.Message);
                Console.WriteLine(sqlErr.ToString());
            } catch (Exception err) {
                Console.WriteLine(err.ToString());
            }

            try {
                String strQry = "SELECT * FROM Delivery WHERE Branch_id = @branch";
                sqlConnection sql = new sqlConnection();
                sql.sqlCon.Open();
                sql.cmd = new SqlCommand(strQry, sql.sqlCon);
                sql.cmd.Parameters.AddWithValue("@branch", branch);
                sql.cmd.ExecuteNonQuery();
                sql.da = new SqlDataAdapter(sql.cmd);
                sql.sqlCon.Close();
                DataSet ds = new DataSet();
                sql.da.Fill(ds, "Delivery");
                Deli_package_datagrid.ItemsSource = ds.Tables["Delivery"].DefaultView;

            } catch (SqlException sqlErr) {
                MessageBox.Show(sqlErr.Message);
            } catch (Exception err) {
                Console.WriteLine(err.ToString());
            }
        }

        private void des_textComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            switch (des_textComboBox.SelectedIndex) {
                case 2:
                Transport_Grid.Visibility = Visibility.Visible;
                break;
                default:
                Transport_Grid.Visibility = Visibility.Hidden;
                break;


            }
        }

        private void Current_time_Click(object sender, RoutedEventArgs e) {
            DatePicker.Value = DateTime.Now;
        }

        private void Cancel_DeliStat_btn_Click(object sender, RoutedEventArgs e) {

            MessageBox.Show(Deli_detail_datagrid.Items.Count.ToString());
            if (Deli_detail_datagrid.Items.Count <= 2) {
                MessageBox.Show("คุณไม่สามารถลบรายการการขนส่งนี้ได้ เพราะพัสดุนี้ลงทะเบียนไว้แล้ว หากต้องการลบต้องลบพัสดุออกจากระบบก่อน", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }


            try {
                DataRowView drv = (DataRowView)Deli_package_datagrid.SelectedItem;
                String package_id_String = (drv["package_id"]).ToString();
                String strQry = @"WITH item AS (SELECT TOP 1 * FROM Delivery_detail WHERE package_id = @id ORDER BY TIME desc) 
                                    DELETE FROM item";
                sqlConnection sql = new sqlConnection();
                sql.sqlCon.Open();
                sql.cmd = new SqlCommand(strQry, sql.sqlCon);
                sql.cmd.Parameters.AddWithValue("@id", Convert.ToInt32(package_id_String));
                sql.cmd.ExecuteNonQuery();
                sql.sqlCon.Close();

            } catch (SqlException sqlErr) {
                MessageBox.Show(sqlErr.Message);
            } catch (Exception err) {
                Console.WriteLine(err.ToString());
            }

            try {
                DataRowView drv = (DataRowView)Deli_package_datagrid.SelectedItem;
                String result = (drv["package_id"]).ToString();
                int value = Convert.ToInt32(result);
                String strQry = @"SELECT * FROM Delivery_detail WHERE package_id = @id";
                sqlConnection sql = new sqlConnection();
                sql.sqlCon.Open();
                sql.cmd = new SqlCommand(strQry, sql.sqlCon);
                sql.cmd.Parameters.AddWithValue("@id", value);
                sql.cmd.ExecuteNonQuery();
                sql.da = new SqlDataAdapter(sql.cmd);
                sql.sqlCon.Close();
                DataSet ds = new DataSet();
                sql.da.Fill(ds, "delivery_detail");
                Deli_detail_datagrid.ItemsSource = ds.Tables["delivery_detail"].DefaultView;
            } catch (SqlException sqlErr) {
                MessageBox.Show(sqlErr.Message);
            } catch (Exception err) {
                Console.WriteLine(err.ToString());
            }


        }

        private void Emplyee_show_Click(object sender, RoutedEventArgs e) {
            ShowHideMenu("sbShowEmplyee", Close_emplyee, Emplyee_show, Employee_Manager);

            try {
                String strQry = "SELECT Employee.emp_id as 'รหัสพนักงาน',Employee.emp_name as 'ชื่อพนักงาน',Employee.emp_add as 'ที่อยู่',Employee.title_id as 'รหัสตำแหน่ง',Title.title_name as 'ชื่อตำแหน่ง',Employee.branch_id as 'รหัสสาขา',Branch.branch_name as 'ชื่อสาขา' FROM Employee INNER JOIN title ON title.title_id = Employee.title_id INNER JOIN Branch ON Branch.branch_id = Employee.branch_id";

                sqlConnection sql = new sqlConnection();
                sql.sqlCon.Open();
                sql.cmd = new SqlCommand(strQry, sql.sqlCon);
                sql.cmd.ExecuteNonQuery();
                sql.da = new SqlDataAdapter(sql.cmd);
                sql.sqlCon.Close();
                DataSet ds = new DataSet();
                sql.da.Fill(ds, "employee");
                Employee_list_grid.ItemsSource = ds.Tables["employee"].DefaultView;


            } catch (SqlException sqlErr) {
                MessageBox.Show(sqlErr.Message);
            } catch (Exception err) {
                Console.WriteLine(err.ToString());
            }
        }

        private void selected_cell_employee_list_grid(object sender, MouseButtonEventArgs e) {

            try {
                DataRowView drv = (DataRowView)Employee_list_grid.SelectedItem;
                String emp_id_String = (drv["รหัสพนักงาน"]).ToString();

                String strQry = @"SELECT Employee.emp_id,Employee.emp_name,Employee.emp_add,Title.title_name,Branch.branch_name 
                FROM employee INNER JOIN title ON title.title_id = Employee.title_id INNER JOIN Branch ON Branch.branch_id = Employee.branch_id 
                WHERE Employee.emp_id = @id";

                sqlConnection sql = new sqlConnection();
                sql.sqlCon.Open();
                sql.cmd = new SqlCommand(strQry, sql.sqlCon);
                sql.cmd.Parameters.AddWithValue("@id", emp_id_String);
                sql.cmd.ExecuteNonQuery();
                sql.sqlCon.Close();
                sql.da = new SqlDataAdapter(sql.cmd);
                DataSet ds = new DataSet();
                sql.da.Fill(ds, "employee_selected");

                emp_id_txt.Text = ds.Tables["employee_selected"].Rows[0]["emp_id"].ToString();
                emp_name_txt.Text = ds.Tables["employee_selected"].Rows[0]["emp_name"].ToString();
                emp_add_txt.Text = ds.Tables["employee_selected"].Rows[0]["emp_add"].ToString();
                title_txt.Text = ds.Tables["employee_selected"].Rows[0]["title_name"].ToString();
                branch_id_txt.Text = ds.Tables["employee_selected"].Rows[0]["branch_name"].ToString();

            } catch (SqlException sqlErr) {
                MessageBox.Show(sqlErr.Message);
            } catch (Exception err) {
                Console.WriteLine(err.ToString());
            }

            try {
                DataRowView drv = (DataRowView)Employee_list_grid.SelectedItem;
                String emp_id_String = (drv["รหัสพนักงาน"]).ToString();
                String strQry = "SELECT * FROM web_login WHERE emp_id = @id";
                sqlConnection sql = new sqlConnection();
                sql.sqlCon.Open();
                sql.cmd = new SqlCommand(strQry, sql.sqlCon);
                sql.cmd.Parameters.AddWithValue("@id", emp_id_String);
                sql.cmd.ExecuteNonQuery();
                sql.sqlCon.Close();
                sql.da = new SqlDataAdapter(sql.cmd);
                DataSet ds = new DataSet();
                sql.da.Fill(ds, "web_login");
                Username_txt.Text = ds.Tables["web_login"].Rows[0]["username"].ToString();
                Password_txt.Text = ds.Tables["web_login"].Rows[0]["password"].ToString();



            } catch (SqlException sqlErr) {
                MessageBox.Show(sqlErr.Message);
            } catch (Exception err) {
                Console.WriteLine(err.ToString());
            }

        }

        private void Close_emplyee_Click(object sender, RoutedEventArgs e) {
            ShowHideMenu("sbHideEmplyee", Close_emplyee, Emplyee_show, Employee_Manager);
        }

        private void update_emp_btn_Click(object sender, RoutedEventArgs e) {
            if (MessageBox.Show("ยืนยันการแก้ไขข้อมูล", "ตุณแน่ใจที่จะแก้ไข ?", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes) {
                try {
                    String strQry = "UPDATE Employee SET emp_name = @name ,emp_add = @add , title_id = @title,branch_id = @branch  WHERE emp_id = @id";
                    sqlConnection sql = new sqlConnection();
                    sql.sqlCon.Open();
                    sql.cmd = new SqlCommand(strQry, sql.sqlCon);
                    sql.cmd.Parameters.AddWithValue("@id", emp_id_txt.Text);
                    sql.cmd.Parameters.AddWithValue("@name", emp_name_txt.Text);
                    sql.cmd.Parameters.AddWithValue("@add", emp_add_txt.Text);
                    sql.cmd.Parameters.AddWithValue("@title", title_idComboBox.SelectedValuePath);
                    sql.cmd.Parameters.AddWithValue("@branch", branch_nameComboBox.SelectedValuePath);
                    sql.cmd.ExecuteNonQuery();
                    sql.sqlCon.Close();

                    strQry = "UPDATE web_login SET username = @username , password = @password WHERE emp_id = @id";
                    sql.sqlCon.Open();
                    sql.cmd.Parameters.AddWithValue("@id", emp_id_txt.Text);
                    sql.cmd.Parameters.AddWithValue("@username", Username_txt.Text);
                    sql.cmd.Parameters.AddWithValue("@password", Password_txt.Text);
                    sql.cmd.ExecuteNonQuery();
                    sql.sqlCon.Close();


                } catch (SqlException sqlErr) {
                    MessageBox.Show(sqlErr.Message);
                } catch (Exception err) {
                    Console.WriteLine(err.ToString());
                }
                try {
                    DataRowView drv = (DataRowView)Employee_list_grid.SelectedItem;
                    String emp_id_String = (drv["emp_id"]).ToString();
                    String strQry = "SELECT * FROM web_login WHERE emp_id = @id";
                    sqlConnection sql = new sqlConnection();
                    sql.sqlCon.Open();
                    sql.cmd = new SqlCommand(strQry, sql.sqlCon);
                    sql.cmd.Parameters.AddWithValue("@id", emp_id_String);
                    sql.cmd.ExecuteNonQuery();
                    sql.sqlCon.Close();
                    sql.da = new SqlDataAdapter(sql.cmd);
                    DataSet ds = new DataSet();
                    sql.da.Fill(ds, "web_login");
                    Username_txt.Text = ds.Tables["web_login"].Rows[0]["username"].ToString();
                    Password_txt.Text = ds.Tables["web_login"].Rows[0]["password"].ToString();



                } catch (SqlException sqlErr) {
                    MessageBox.Show(sqlErr.Message);
                } catch (Exception err) {
                    Console.WriteLine(err.ToString());
                }

                try {
                    DataRowView drv = (DataRowView)Employee_list_grid.SelectedItem;
                    String emp_id_String = (drv["emp_id"]).ToString();

                    String strQry = @"SELECT Employee.emp_id,Employee.emp_name,Employee.emp_add,Title.title_name,Branch.branch_name 
                FROM employee INNER JOIN title ON title.title_id = Employee.title_id INNER JOIN Branch ON Branch.branch_id = Employee.branch_id 
                WHERE Employee.emp_id = @id";

                    sqlConnection sql = new sqlConnection();
                    sql.sqlCon.Open();
                    sql.cmd = new SqlCommand(strQry, sql.sqlCon);
                    sql.cmd.Parameters.AddWithValue("@id", emp_id_String);
                    sql.cmd.ExecuteNonQuery();
                    sql.sqlCon.Close();
                    sql.da = new SqlDataAdapter(sql.cmd);
                    DataSet ds = new DataSet();
                    sql.da.Fill(ds, "employee_selected");

                    emp_id_txt.Text = ds.Tables["employee_selected"].Rows[0]["emp_id"].ToString();
                    emp_name_txt.Text = ds.Tables["employee_selected"].Rows[0]["emp_name"].ToString();
                    emp_add_txt.Text = ds.Tables["employee_selected"].Rows[0]["emp_add"].ToString();
                    title_txt.Text = ds.Tables["employee_selected"].Rows[0]["title_name"].ToString();
                    branch_id_txt.Text = ds.Tables["employee_selected"].Rows[0]["branch_name"].ToString();

                } catch (SqlException sqlErr) {
                    MessageBox.Show(sqlErr.Message);
                } catch (Exception err) {
                    Console.WriteLine(err.ToString());
                }

                try {
                    String strQry = "SELECT Employee.emp_id,Employee.emp_name,Employee.emp_add,Employee.title_id,Title.title_name,Employee.branch_id,Branch.branch_name FROM Employee INNER JOIN title ON title.title_id = Employee.title_id INNER JOIN Branch ON Branch.branch_id = Employee.branch_id";

                    sqlConnection sql = new sqlConnection();
                    sql.sqlCon.Open();
                    sql.cmd = new SqlCommand(strQry, sql.sqlCon);
                    sql.cmd.ExecuteNonQuery();
                    sql.da = new SqlDataAdapter(sql.cmd);
                    sql.sqlCon.Close();
                    DataSet ds = new DataSet();
                    sql.da.Fill(ds, "employee");
                    Employee_list_grid.ItemsSource = ds.Tables["employee"].DefaultView;


                } catch (SqlException sqlErr) {
                    MessageBox.Show(sqlErr.Message);
                } catch (Exception err) {
                    Console.WriteLine(err.ToString());
                }
            }
        }

        private void Delete_emp_btn_Click(object sender, RoutedEventArgs e) {

            try {

                DataRowView drv = (DataRowView)Employee_list_grid.SelectedItem;
                String emp_id_String = (drv["emp_id"]).ToString();
                if (emp_id_String == this.empid) {
                    MessageBox.Show("ไม่สามารถลบตัวเองออกจากระบบได้");
                    return;
                }
                if (MessageBox.Show("คุณต้องการที่จะพนักงานคนนี้จริงหรอ ? " + emp_id_String, "ยืนยันการลบ", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes) {
                    String strQry = @"  DELETE FROM Web_login WHERE emp_id = @id
                                        DELETE FROM Employee WHERE emp_id = @id";
                    sqlConnection sql = new sqlConnection();
                    sql.sqlCon.Open();
                    sql.cmd = new SqlCommand(strQry, sql.sqlCon);
                    sql.cmd.Parameters.AddWithValue("@id", emp_id_String);
                    sql.cmd.ExecuteNonQuery();
                    sql.sqlCon.Close();

                }
            } catch (SqlException sqlErr) {
                MessageBox.Show(sqlErr.Message);
            } catch (Exception err) {
                Console.WriteLine(err.ToString());
            }

            try {
                String strQry = "SELECT Employee.emp_id,Employee.emp_name,Employee.emp_add,Employee.title_id,Title.title_name,Employee.branch_id,Branch.branch_name FROM Employee INNER JOIN title ON title.title_id = Employee.title_id INNER JOIN Branch ON Branch.branch_id = Employee.branch_id";

                sqlConnection sql = new sqlConnection();
                sql.sqlCon.Open();
                sql.cmd = new SqlCommand(strQry, sql.sqlCon);
                sql.cmd.ExecuteNonQuery();
                sql.da = new SqlDataAdapter(sql.cmd);
                sql.sqlCon.Close();
                DataSet ds = new DataSet();
                sql.da.Fill(ds, "employee");
                Employee_list_grid.ItemsSource = ds.Tables["employee"].DefaultView;


            } catch (SqlException sqlErr) {
                MessageBox.Show(sqlErr.Message);
            } catch (Exception err) {
                Console.WriteLine(err.ToString());
            }
            try {
                DataRowView drv = (DataRowView)Employee_list_grid.SelectedItem;
                String emp_id_String = (drv["emp_id"]).ToString();
                String strQry = "SELECT * FROM web_login WHERE emp_id = @id";
                sqlConnection sql = new sqlConnection();
                sql.sqlCon.Open();
                sql.cmd = new SqlCommand(strQry, sql.sqlCon);
                sql.cmd.Parameters.AddWithValue("@id", emp_id_String);
                sql.cmd.ExecuteNonQuery();
                sql.sqlCon.Close();
                sql.da = new SqlDataAdapter(sql.cmd);
                DataSet ds = new DataSet();
                sql.da.Fill(ds, "web_login");
                Username_txt.Text = ds.Tables["web_login"].Rows[0]["username"].ToString();
                Password_txt.Text = ds.Tables["web_login"].Rows[0]["password"].ToString();



            } catch (SqlException sqlErr) {
                MessageBox.Show(sqlErr.Message);
            } catch (Exception err) {
                Console.WriteLine(err.ToString());
            }

        }

        private void Create_newemp_btn_Click(object sender, RoutedEventArgs e) {
            member register = new member();
            register.Show();
        }

        private void Branch_managerment_btn_Click(object sender, RoutedEventArgs e) {
            ShowHideMenu("sbShowBranch", hide_branch_btn, Branch_managerment_btn, branch_Manager);
            try {
                String strQry = @"select branch_id as 'รหัสาขา' , branch_name as 'ชื่อสาขา' , branch_add as 'ที่อยู่สาขา' , emp_id as 'ผูู้จัดการสาขา'
                                , latlong as 'พิกัด'
                                from Branch";
                sqlConnection sql = new sqlConnection();
                sql.sqlCon.Open();
                sql.cmd = new SqlCommand(strQry, sql.sqlCon);
                sql.cmd.ExecuteNonQuery();
                sql.da = new SqlDataAdapter(sql.cmd);
                DataSet ds = new DataSet();
                sql.da.Fill(ds, "Branch");
                sql.sqlCon.Close();
                branch_datagrid.ItemsSource = ds.Tables["Branch"].DefaultView;

            } catch (SqlException sqlErr) {
                MessageBox.Show(sqlErr.Message);
            } catch (Exception err) {
                Console.WriteLine(err.ToString());
            }
        }

        private void hide_branch_btn_Click(object sender, RoutedEventArgs e) {
            ShowHideMenu("sbHideBranch", hide_branch_btn, Branch_managerment_btn, branch_Manager);
        }

        private void selected_cell_branch_list_grid(object sender, MouseButtonEventArgs e) {

            try {
                DataRowView drv = (DataRowView)branch_datagrid.SelectedItem;
                String branch_id_String = (drv["รหัสาขา"]).ToString();
                String strQry = @"SELECT * FROM Branch WHERE branch_id = @id";
                sqlConnection sql = new sqlConnection();
                sql.sqlCon.Open();
                sql.cmd = new SqlCommand(strQry, sql.sqlCon);
                sql.cmd.Parameters.AddWithValue("@id", branch_id_String);
                sql.cmd.ExecuteNonQuery();
                sql.da = new SqlDataAdapter(sql.cmd);
                DataSet ds = new DataSet();
                sql.da.Fill(ds, "select_branch");
                sql.sqlCon.Close();
                branch_id_txb.Text = ds.Tables["select_branch"].Rows[0]["branch_id"].ToString();
                branch_name_txb.Text = ds.Tables["select_branch"].Rows[0]["branch_name"].ToString();
                branch_add_txb.Text = ds.Tables["select_branch"].Rows[0]["branch_add"].ToString();
                emp_id_txb.Text = ds.Tables["select_branch"].Rows[0]["emp_id"].ToString();
                latlong_txb.Text = ds.Tables["select_branch"].Rows[0]["latlong"].ToString();

            } catch (SqlException sqlErr) {
                MessageBox.Show(sqlErr.Message);
            } catch (Exception err) {
                // MessageBox.Show(err.Message);
            }

            try {
                DataRowView drv = (DataRowView)branch_datagrid.SelectedItem;
                String branch_id_String = (drv["รหัสาขา"]).ToString();
                String strQry = @"SELECT * FROM  package_price WHERE branch_id = @id";
                sqlConnection sql = new sqlConnection();
                sql.sqlCon.Open();
                sql.cmd = new SqlCommand(strQry, sql.sqlCon);
                sql.cmd.Parameters.AddWithValue("@id", branch_id_String);
                sql.cmd.ExecuteNonQuery();
                sql.sqlCon.Close();
                sql.da = new SqlDataAdapter(sql.cmd);
                DataSet ds = new DataSet();
                sql.da.Fill(ds, "package_price");
                package_price_datagrid.ItemsSource = ds.Tables["package_price"].DefaultView;

            } catch (SqlException sqlErr) {
                MessageBox.Show(sqlErr.Message);
            } catch (Exception err) {
                // MessageBox.Show(err.Message);
            }


        }

        private void update_branch_data_btn_Click(object sender, RoutedEventArgs e) {
            if (MessageBox.Show("คุณต้องการที่จะแก้ไขข้อมูลสาขาจริงๆหรอ ?", "ยืนยันการแก้ไข", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes) {
                try {
                    String strQry = @"UPDATE branch SET branch_name = @name,branch_add = @add , emp_id = @id ,latlong = @latlong WHERE branch_id = @branch_id ";
                    sqlConnection sql = new sqlConnection();
                    sql.sqlCon.Open();
                    sql.cmd = new SqlCommand(strQry, sql.sqlCon);
                    sql.cmd.Parameters.AddWithValue("@branch_id", branch_id_txb.Text);
                    sql.cmd.Parameters.AddWithValue("@name", branch_name_txb.Text);
                    sql.cmd.Parameters.AddWithValue("@add", branch_add_txb.Text);
                    sql.cmd.Parameters.AddWithValue("@id", emp_idComboBox.SelectedValuePath);
                    sql.cmd.Parameters.AddWithValue("@latlong", latlong_txb.Text);
                    sql.cmd.ExecuteNonQuery();
                    sql.sqlCon.Close();


                } catch (SqlException sqlErr) {
                    MessageBox.Show(sqlErr.Message);
                } catch (Exception err) {
                    // MessageBox.Show(err.Message);
                }

                try {
                    String strQry = @"select branch_id as 'รหัสาขา' , branch_name as 'ชื่อสาขา' , branch_add as 'ที่อยู่สาขา' , emp_id as 'ผูู้จัดการสาขา'
                                , latlong as 'พิกัด'
                                from Branch";
                    sqlConnection sql = new sqlConnection();
                    sql.sqlCon.Open();
                    sql.cmd = new SqlCommand(strQry, sql.sqlCon);
                    sql.cmd.ExecuteNonQuery();
                    sql.da = new SqlDataAdapter(sql.cmd);
                    DataSet ds = new DataSet();
                    sql.da.Fill(ds, "Branch");
                    sql.sqlCon.Close();
                    branch_datagrid.ItemsSource = ds.Tables["Branch"].DefaultView;

                } catch (SqlException sqlErr) {
                    MessageBox.Show(sqlErr.Message);
                } catch (Exception err) {
                    Console.WriteLine(err.ToString());
                }
            }
            branch_id_txb.Text = "";
            branch_name_txb.Text = "";
            branch_add_txb.Text = "";
            emp_idComboBox.SelectedIndex = -1;
            latlong_txb.Text = "";
        }

        private void Insert_branch_btn_Click(object sender, RoutedEventArgs e) {
            addbranch regis = new addbranch();
            regis.Show();
        }

        private void refresh_branch_datagrid_btn_Click(object sender, RoutedEventArgs e) {
            try {
                String strQry = @"select branch_id as 'รหัสาขา' , branch_name as 'ชื่อสาขา' , branch_add as 'ที่อยู่สาขา' , emp_id as 'ผูู้จัดการสาขา'
                                , latlong as 'พิกัด'
                                from Branch";
                sqlConnection sql = new sqlConnection();
                sql.sqlCon.Open();
                sql.cmd = new SqlCommand(strQry, sql.sqlCon);
                sql.cmd.ExecuteNonQuery();
                sql.da = new SqlDataAdapter(sql.cmd);
                DataSet ds = new DataSet();
                sql.da.Fill(ds, "Branch");
                sql.sqlCon.Close();
                branch_datagrid.ItemsSource = ds.Tables["Branch"].DefaultView;

            } catch (SqlException sqlErr) {
                MessageBox.Show(sqlErr.Message);
            } catch (Exception err) {
                Console.WriteLine(err.ToString());
            }
        }

        private void Delete_branch_btn_Click(object sender, RoutedEventArgs e) {
            if (MessageBox.Show("คุณแน่ใจใช้ไหมที่จะลบสาขานี้ออก", "ยืนยัน", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes) {
                try {
                    DataRowView drv = (DataRowView)branch_datagrid.SelectedItem;
                    String branch_id_String = (drv["รหัสาขา"]).ToString();
                    String strQry = "DELETE FROM branch WHERE branch_id = @id";
                    sqlConnection sql = new sqlConnection();
                    sql.sqlCon.Open();
                    sql.cmd = new SqlCommand(strQry, sql.sqlCon);
                    sql.cmd.Parameters.AddWithValue("@id", branch_id_String);
                    sql.cmd.ExecuteNonQuery();
                    sql.sqlCon.Close();
                    MessageBox.Show("ลบสำเร็จ");


                } catch (SqlException errSql) {
                    MessageBox.Show(errSql.Message);
                }
            }
        }

        private void cost_managerment_Click(object sender, RoutedEventArgs e) {
            try {
                DataRowView drv = (DataRowView)branch_datagrid.SelectedItem;
                String branch_id_String = (drv["รหัสาขา"]).ToString();
                if (branch_id_String == "") {
                    MessageBox.Show("กรุณาเลือกสาขาที่ต้องการเปลี่ยนแปลงข้อมูลก่อน");
                    return;
                } else {
                    package_price_editor package_Price = new package_price_editor(branch_id_String);
                    package_Price.Show();
                }

            } catch {

            }

        }

        private void emp_manage_btn_Click(object sender, RoutedEventArgs e) {
            ShowHideMenu("sbShowEmp", hide_emp_btn, emp_manage_btn, Emp_mana_stc);

            try {
                String strQry = @"SELECT cus_id as 'รหัสลูกค้า' , cus_name as 'ชื่อลูกค้า' , cus_id_card as 'รหัสบัตรประจำตัวลูกค้า' , num_of_services as 'จำนวนการใช้บริการ', tel_num as 'เบอร์โทรศัพท์' FROM Customers";
                sqlConnection sql = new sqlConnection();
                sql.sqlCon.Open();
                sql.cmd = new SqlCommand(strQry, sql.sqlCon);
                sql.cmd.ExecuteNonQuery();
                sql.da = new SqlDataAdapter(sql.cmd);
                DataSet ds = new DataSet();
                sql.da.Fill(ds, "Customers");
                customer_datagrid.ItemsSource = ds.Tables["customers"].DefaultView;
                sql.sqlCon.Close();

            } catch {

            }
        }

        private void hide_emp_btn_Click(object sender, RoutedEventArgs e) {
            ShowHideMenu("sbHideEmp", hide_emp_btn, emp_manage_btn, Emp_mana_stc);
        }

        private void selected_cell_customers_list_grid(object sender, MouseButtonEventArgs e) {
            try {
                DataRowView drv = (DataRowView)customer_datagrid.SelectedItem;
                String cus_id_String = (drv["รหัสลูกค้า"]).ToString();
                //MessageBox.Show(cus_id_String);
                String strQry = @"SELECT bil_id as 'รหัสใบเสร็จ', package_id as 'รหัสพัสดุ',dev_id as 'หมายเลขติดตามพัสดุ' , Package_price.cost as 'ราคาขนส่ง' 
                FROM Bill 
                INNER JOIN Package_price ON Bill.id = Package_price.id Where cus_id = @id";
                sqlConnection sql = new sqlConnection();
                sql.sqlCon.Open();
                sql.cmd = new SqlCommand(strQry, sql.sqlCon);
                sql.cmd.Parameters.AddWithValue("@id", cus_id_String);
                sql.cmd.ExecuteNonQuery();
                sql.da = new SqlDataAdapter(sql.cmd);
                DataSet ds = new DataSet();
                sql.da.Fill(ds, "bill");
                bill_datagrid.ItemsSource = ds.Tables["bill"].DefaultView;
                sql.sqlCon.Close();

                strQry = @"SELECT * FROM customers WHERE cus_id = @cus_id";
                sql.sqlCon.Open();
                sql.cmd = new SqlCommand(strQry, sql.sqlCon);
                sql.cmd.Parameters.AddWithValue("@cus_id", cus_id_String);
                sql.cmd.ExecuteNonQuery();
                sql.sqlCon.Close();
                sql.da = new SqlDataAdapter(sql.cmd);
                sql.da.Fill(ds, "customer_cur");
                cus_name_txt.Text = ds.Tables["customer_cur"].Rows[0]["cus_name"].ToString();
                id_card_txt.Text = ds.Tables["customer_cur"].Rows[0]["cus_id_card"].ToString();
                tel_txt.Text = ds.Tables["customer_cur"].Rows[0]["tel_num"].ToString();


            } catch (SqlException err) {
                MessageBox.Show(err.ToString());
            } catch {

            }

        }

        private void Update_cus_btn_Click(object sender, RoutedEventArgs e) {

            if (MessageBox.Show("คุณแน่ใขที่จะแก้ไขข้อมูลลูกค้าท่านนี้ใช้หรือไม่ ?", "ยืนยันการแก้ไข", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes) {
                try {
                    DataRowView drv = (DataRowView)customer_datagrid.SelectedItem;
                    String cus_id_String = (drv["รหัสลูกค้า"]).ToString();
                    //MessageBox.Show(cus_id_String);
                    String strQry = @"UPDATE customers SET cus_name = @name , cus_id_card = @id_card , tel_num = @tel 
                    WHERE cus_id = @cus_id";
                    sqlConnection sql = new sqlConnection();
                    sql.sqlCon.Open();
                    sql.cmd = new SqlCommand(strQry, sql.sqlCon);
                    sql.cmd.Parameters.AddWithValue("@name", cus_name_txt.Text);
                    sql.cmd.Parameters.AddWithValue("@id_card", id_card_txt.Text);
                    sql.cmd.Parameters.AddWithValue("@tel", tel_txt.Text);
                    sql.cmd.Parameters.AddWithValue("@cus_id", cus_id_String);
                    sql.cmd.ExecuteNonQuery();
                    sql.sqlCon.Close();
                    MessageBox.Show("แก้ไขข้อมูลสำเร็จ");

                } catch (SqlException err) {
                    MessageBox.Show(err.Message);
                } catch (Exception err) {
                    MessageBox.Show(err.ToString());
                }
            }
            try {
                String strQry = @"SELECT cus_id as 'รหัสลูกค้า' , cus_name as 'ชื่อลูกค้า' , cus_id_card as 'รหัสบัตรประจำตัวลูกค้า' , num_of_services as 'จำนวนการใช้บริการ', tel_num as 'เบอร์โทรศัพท์' FROM Customers";
                sqlConnection sql = new sqlConnection();
                sql.sqlCon.Open();
                sql.cmd = new SqlCommand(strQry, sql.sqlCon);
                sql.cmd.ExecuteNonQuery();
                sql.da = new SqlDataAdapter(sql.cmd);
                DataSet ds = new DataSet();
                sql.da.Fill(ds, "Customers");
                customer_datagrid.ItemsSource = ds.Tables["customers"].DefaultView;
                sql.sqlCon.Close();

            } catch {

            }

        }

        private void add_cus_btn_Click(object sender, RoutedEventArgs e) {
            if (MessageBox.Show("คุณต้องการที่จะเพิ่มข้อมูลลูกค้าใช้หรือไม่", "การยืนยัน", MessageBoxButton.YesNo) == MessageBoxResult.Yes) {
                try {
                    String strQry = @"INSERT INTO customers (cus_name,cus_id_card,num_of_services,tel_num)
                                      VALUES (@name,@id_card,@num,@tel_num)";
                    sqlConnection sql = new sqlConnection();
                    sql.sqlCon.Open();
                    sql.cmd = new SqlCommand(strQry, sql.sqlCon);
                    sql.cmd.Parameters.AddWithValue("@name", cus_name_txt_Copy.Text);
                    sql.cmd.Parameters.AddWithValue("@id_card", id_card_txt_Copy.Text);
                    sql.cmd.Parameters.AddWithValue("@num", 0);
                    sql.cmd.Parameters.AddWithValue("@tel_num", tel_txt_Copy.Text);
                    sql.cmd.ExecuteNonQuery();
                    sql.sqlCon.Close();
                    MessageBox.Show("เพิ่มข้อมูลลูกค้าสำเร็จ !!");
                    cus_name_txt_Copy.Text = "";
                    id_card_txt_Copy.Text = "";
                    tel_txt_Copy.Text = "";


                } catch (SqlException err) {
                    MessageBox.Show(err.Message);
                } catch (Exception err) {
                    MessageBox.Show(err.ToString());
                }

                try {
                    String strQry = @"SELECT cus_id as 'รหัสลูกค้า' , cus_name as 'ชื่อลูกค้า' , cus_id_card as 'รหัสบัตรประจำตัวลูกค้า' , num_of_services as 'จำนวนการใช้บริการ', tel_num as 'เบอร์โทรศัพท์' FROM Customers";
                    sqlConnection sql = new sqlConnection();
                    sql.sqlCon.Open();
                    sql.cmd = new SqlCommand(strQry, sql.sqlCon);
                    sql.cmd.ExecuteNonQuery();
                    sql.da = new SqlDataAdapter(sql.cmd);
                    DataSet ds = new DataSet();
                    sql.da.Fill(ds, "Customers");
                    customer_datagrid.ItemsSource = ds.Tables["customers"].DefaultView;
                    sql.sqlCon.Close();

                } catch {

                }
            }
        }

        private void remove_cus_btn_Click(object sender, RoutedEventArgs e) {
            if(MessageBox.Show("ต้องการลบข้อมูลค้านี้จริงหรือไม่","การยืนยัน",MessageBoxButton.YesNo,MessageBoxImage.Warning) == MessageBoxResult.Yes) {
                try {
                    DataRowView drv = (DataRowView)customer_datagrid.SelectedItem;
                    String cus_id_String = (drv["รหัสลูกค้า"]).ToString();
                    String strQry = "DELETE FROM customers WHERE cus_id = @id";
                    sqlConnection sql = new sqlConnection();
                    sql.sqlCon.Open();
                    sql.cmd = new SqlCommand(strQry, sql.sqlCon);
                    sql.cmd.Parameters.AddWithValue("@id", cus_id_String);
                    sql.cmd.ExecuteNonQuery();
                    sql.sqlCon.Close();

                } catch (SqlException err) {
                    MessageBox.Show("การลบข้อมูลลูกค้าที่มีการทำรายพัสดุอยู่ในระบบไม่สามารถทำได้","ไม่สามารถทำได้",MessageBoxButton.OK,MessageBoxImage.Error);
                } catch (Exception err) {
                    MessageBox.Show(err.ToString());
                }
            }
            try {
                String strQry = @"SELECT cus_id as 'รหัสลูกค้า' , cus_name as 'ชื่อลูกค้า' , cus_id_card as 'รหัสบัตรประจำตัวลูกค้า' , num_of_services as 'จำนวนการใช้บริการ', tel_num as 'เบอร์โทรศัพท์' FROM Customers";
                sqlConnection sql = new sqlConnection();
                sql.sqlCon.Open();
                sql.cmd = new SqlCommand(strQry, sql.sqlCon);
                sql.cmd.ExecuteNonQuery();
                sql.da = new SqlDataAdapter(sql.cmd);
                DataSet ds = new DataSet();
                sql.da.Fill(ds, "Customers");
                customer_datagrid.ItemsSource = ds.Tables["customers"].DefaultView;
                sql.sqlCon.Close();

            } catch {

            }
        }

        
    }
}
    

