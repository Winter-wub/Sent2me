using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace client_emp
{
    
    public partial class Main_frm : Form
    {
        public String emp_id;
        public String branch_id;
        
        public Main_frm(String emp_id,String branch_id)
        {
            this.emp_id = emp_id;
            this.branch_id = branch_id;
            InitializeComponent(); 
        }
        
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void registerPackageToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void displayPackageToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Package_frm package_Frm = new Package_frm(emp_id,branch_id);
            package_Frm.MdiParent = this;
            package_Frm.Text = "Package Management";
           // package_Frm.setText(emp_id, brach_id);
            package_Frm.Show();
            
        }
    }
}
