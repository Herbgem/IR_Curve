namespace Client
{
    partial class ClientUI
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
            this.cMTToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lIBORToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.iRSToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cMTToolStripMenuItem,
            this.lIBORToolStripMenuItem,
            this.iRSToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(644, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // cMTToolStripMenuItem
            // 
            this.cMTToolStripMenuItem.Name = "cMTToolStripMenuItem";
            this.cMTToolStripMenuItem.Size = new System.Drawing.Size(45, 20);
            this.cMTToolStripMenuItem.Text = "CMT";
            this.cMTToolStripMenuItem.Click += new System.EventHandler(this.cMTToolStripMenuItem_Click);
            // 
            // lIBORToolStripMenuItem
            // 
            this.lIBORToolStripMenuItem.Name = "lIBORToolStripMenuItem";
            this.lIBORToolStripMenuItem.Size = new System.Drawing.Size(51, 20);
            this.lIBORToolStripMenuItem.Text = "LIBOR";
            this.lIBORToolStripMenuItem.Click += new System.EventHandler(this.lIBORToolStripMenuItem_Click);
            // 
            // iRSToolStripMenuItem
            // 
            this.iRSToolStripMenuItem.Name = "iRSToolStripMenuItem";
            this.iRSToolStripMenuItem.Size = new System.Drawing.Size(35, 20);
            this.iRSToolStripMenuItem.Text = "IRS";
            this.iRSToolStripMenuItem.Click += new System.EventHandler(this.iRSToolStripMenuItem_Click);
            // 
            // panel1
            // 
            this.panel1.AutoSize = true;
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 24);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(644, 424);
            this.panel1.TabIndex = 1;
            // 
            // ClientUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(644, 448);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "ClientUI";
            this.ShowIcon = false;
            this.Text = "ClientUI";
            this.Load += new System.EventHandler(this.ClientUI_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem cMTToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem lIBORToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem iRSToolStripMenuItem;
        private System.Windows.Forms.Panel panel1;

    }
}

