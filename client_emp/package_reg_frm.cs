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
    public partial class package_reg_frm : Form
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
        public package_reg_frm(String emp_id,String branch_id)
        {
            InitializeComponent();
            connectMSSQL();
            this.emp_id = emp_id;
            this.branch_id = branch_id;
            emp_id_txt.Text = emp_id;
            branch_txt.Text = branch_id;
        }


        private void package_reg_frm_Load(object sender, EventArgs e)
        {
            String strQry = "SELECT * FROM Package_type ";
            try {
                cmd.CommandText = strQry;
            }catch(Exception err) {
                MessageBox.Show("ข้อผิดพลาดไม่สามารถเชื่อมต่อ ฐานข้อมูล กรุณาปิดโปรแกรมแล้วเปิดใหม่");
                Console.WriteLine(err.ToString());
            }
            da = new SqlDataAdapter(cmd);
            ds = new DataSet();
            da.Fill(ds, "Package_type");
            comboBox1.DataSource = ds.Tables["Package_type"];
            
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DateTime reg_date = dateTimePicker1.Value.Date + dateTimePicker2.Value.TimeOfDay;
            
            

            String strQry1 = "INSERT INTO Package (package_id,regis_date,emp_id,brach_id ) VALUES ("+ Convert.ToInt32(pakage_id_txt.Text) + ",'"+ reg_date.ToString() +"','"+emp_id +"','"+ branch_id +"')";
            String strQry2 = @"INSERT INTO Package_detail (package_id,package_name,sender_name,sender_add,rec_name,rec_add,package_type_id) 
                                    VALUES ("+ Convert.ToInt32(pakage_id_txt.Text) + ",'"+package_name_txt.Text +"','"+ sender_txt.Text+" ','"+sender_add_txt.Text +" ','"+rec_txt.Text +" ','"+rec_add_txt.Text +" ','"+ comboBox1.SelectedValue +"')";
            
            try{
                cmd.CommandText = strQry1;
                cmd.ExecuteNonQuery();
                cmd.CommandText = strQry2;
                cmd.ExecuteNonQuery();
                MessageBox.Show("ลงทะเบียนพัสดุเรียบร้อย");
            }
            catch (Exception err){
                MessageBox.Show("ข้อผิดพลาดไม่สามารถเชื่อมต่อ ฐานข้อมูล กรุณาปิดโปรแกรมแล้วเปิดใหม่");
                Console.WriteLine(err.ToString());
            }
        }
    }
}
