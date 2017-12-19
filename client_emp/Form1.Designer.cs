namespace client_emp
{
    partial class Main_frm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.mainToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.functionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.registerPackageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.displayPackageToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.displayPackageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.inventoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deliveryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mainToolStripMenuItem,
            this.functionToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(905, 28);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // mainToolStripMenuItem
            // 
            this.mainToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.mainToolStripMenuItem.Name = "mainToolStripMenuItem";
            this.mainToolStripMenuItem.Size = new System.Drawing.Size(54, 24);
            this.mainToolStripMenuItem.Text = "Main";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(108, 26);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // functionToolStripMenuItem
            // 
            this.functionToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.registerPackageToolStripMenuItem,
            this.displayPackageToolStripMenuItem,
            this.inventoryToolStripMenuItem,
            this.deliveryToolStripMenuItem});
            this.functionToolStripMenuItem.Name = "functionToolStripMenuItem";
            this.functionToolStripMenuItem.Size = new System.Drawing.Size(77, 24);
            this.functionToolStripMenuItem.Text = "Function";
            // 
            // registerPackageToolStripMenuItem
            // 
            this.registerPackageToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.displayPackageToolStripMenuItem1});
            this.registerPackageToolStripMenuItem.Name = "registerPackageToolStripMenuItem";
            this.registerPackageToolStripMenuItem.Size = new System.Drawing.Size(150, 26);
            this.registerPackageToolStripMenuItem.Text = "Package";
            this.registerPackageToolStripMenuItem.Click += new System.EventHandler(this.registerPackageToolStripMenuItem_Click);
            // 
            // displayPackageToolStripMenuItem1
            // 
            this.displayPackageToolStripMenuItem1.Name = "displayPackageToolStripMenuItem1";
            this.displayPackageToolStripMenuItem1.Size = new System.Drawing.Size(230, 26);
            this.displayPackageToolStripMenuItem1.Text = "Package Management";
            this.displayPackageToolStripMenuItem1.Click += new System.EventHandler(this.displayPackageToolStripMenuItem1_Click);
            // 
            // displayPackageToolStripMenuItem
            // 
            this.displayPackageToolStripMenuItem.Name = "displayPackageToolStripMenuItem";
            this.displayPackageToolStripMenuItem.Size = new System.Drawing.Size(150, 26);
            this.displayPackageToolStripMenuItem.Text = "Employee";
            // 
            // inventoryToolStripMenuItem
            // 
            this.inventoryToolStripMenuItem.Name = "inventoryToolStripMenuItem";
            this.inventoryToolStripMenuItem.Size = new System.Drawing.Size(150, 26);
            this.inventoryToolStripMenuItem.Text = "Inventory";
            // 
            // deliveryToolStripMenuItem
            // 
            this.deliveryToolStripMenuItem.Name = "deliveryToolStripMenuItem";
            this.deliveryToolStripMenuItem.Size = new System.Drawing.Size(150, 26);
            this.deliveryToolStripMenuItem.Text = "Delivery";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(62, 24);
            this.aboutToolStripMenuItem.Text = "About";
            // 
            // Main_frm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(905, 514);
            this.Controls.Add(this.menuStrip1);
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Main_frm";
            this.Text = "Form1";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem mainToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem functionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem registerPackageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem displayPackageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem displayPackageToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem inventoryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deliveryToolStripMenuItem;
    }
}

