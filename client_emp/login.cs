using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;


namespace client_emp
{
    public partial class login : Form {
        String strconn;
        SqlConnection conn;
        SqlCommand cmd;
        SqlDataAdapter da;
        DataSet ds;
       
        
        public login()
        {
            InitializeComponent();
            connectMSSQL();
        }

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

        private void submit_btn_Click(object sender, EventArgs e)
        {
            String  strQry = @"SELECT * FROM Web_login inner join Employee on Web_login.emp_id = Employee.emp_id WHERE username = '" + username_txt.Text+"'";
            try
            {
                cmd.CommandText = strQry;
            }
            catch (Exception err)
            {
                MessageBox.Show("ข้อผิดพลาดไม่สามารถเชื่อมต่อ ฐานข้อมูล กรุณาลองใหม่อีกครั้ง");
                Console.WriteLine(err.ToString());
                connectMSSQL();
            }
            da = new SqlDataAdapter(cmd);
            ds = new DataSet();

            da.Fill(ds, "employee");
            if(ds.Tables["employee"].Rows.Count == 0)
            {
                MessageBox.Show("Username or Password incorrect");
            }
            else
            {
                String data_password = ds.Tables["employee"].Rows[0]["password"].ToString();
                String brach_id = ds.Tables["employee"].Rows[0]["branch_id"].ToString();
                String emp_id = ds.Tables["employee"].Rows[0]["emp_id"].ToString();
                if (password_txt.Text == data_password)
                {
                    Main_frm form = new Main_frm(emp_id,brach_id);
                    MessageBox.Show("Login Successful");
                    Visible = false;
                    form.Visible = true;
                    
                    form.Text = "Main Pragrame USER : " + username_txt.Text;
                    conn.Close();
                }
                else
                {
                    MessageBox.Show("Username or Password incorrect");
                }
            }

        }
    }
}
