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

namespace client_emp
{
    public partial class Package_frm : Form
    {
        String strconn;
        SqlConnection conn;
        SqlCommand cmd;
        SqlDataAdapter da;
        DataSet ds;
        String emp_id;
        String branch_id;
        

        private void connectMSSQL()
        {
            try
            {
                strconn = "Data Source=.;Initial Catalog=sent2me;Persist Security Info=True;User ID=sa;Password=1234";
                conn = new SqlConnection(strconn);
                conn.Open();
                cmd = new SqlCommand("", conn);
            }
            catch (Exception err)
            {
                MessageBox.Show("ข้อผิดพลาดไม่สามารถเชื่อมต่อ ฐานข้อมูล กรุณาปิดโปรแกรมแล้วเปิดใหม่");
                Console.WriteLine(err.ToString());

            }
        }

        public Package_frm(String emp_id,String branch_id)
        {
            InitializeComponent();
            this.emp_id = emp_id;
            this.branch_id = branch_id;

            label1.Text = "พนักงาน :"+emp_id;
            label2.Text = "สาขา :"+branch_id;
            connectMSSQL();
            String strQry = @"SELECT 
                              Package.package_id,Package.regis_date,Package.brach_id,branch_name,package_name,sender_name,rec_name,rec_add,Package_type.package_type_id,employee.emp_id
                              FROM Package 
                                INNER JOIN Package_detail ON Package_detail.package_id = Package.package_id
                                INNER JOIN Package_type ON Package_type.package_type_id = Package_detail.package_type_id
                                INNER JOIN Branch ON Branch.branch_id = Package.brach_id
                                INNER JOIN Employee on Employee.emp_id = Package.emp_id";
            try
            {
                cmd.CommandText = strQry;
            }
            catch (Exception err)
            {
                MessageBox.Show("ข้อผิดพลาดไม่สามารถเชื่อมต่อ ฐานข้อมูล กรุณาปิดโปรแกรมแล้วเปิดใหม่");
                Console.WriteLine(err.ToString());
            }
            da = new SqlDataAdapter(cmd);
            ds = new DataSet();
            da.Fill(ds, "package");

            dataGridView1.DataSource = ds.Tables["package"];
            
            String[] header_table = {"รหัสพัสดุ","วันที่ลงทะเบียน","รหัสสาขา","สาขา","ชื่อพัสดุ(ถ้ามี)","ชื่อผู่ส่ง","ชื่อผู้รับ","ที่อยู่ผู้รับ","ประเภทพัสดุ","พนักงานลงทะเบียน"};

            for (int i=0; i < header_table.Length; i++)
            {
                dataGridView1.Columns[i].HeaderText = header_table[i];
            }
            conn.Close();

        }

        private void Package_frm_Load(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Maximized;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            package_reg_frm form = new package_reg_frm(emp_id,branch_id);
            form.Show();

        }
    }
}
