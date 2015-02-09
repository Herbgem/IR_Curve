namespace Server
{
    partial class UserControlData
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pnlData = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnStopUpdate = new System.Windows.Forms.Button();
            this.btnStartUpdate = new System.Windows.Forms.Button();
            this.btnSet = new System.Windows.Forms.Button();
            this.lblSecond = new System.Windows.Forms.Label();
            this.nudUpdateSec = new System.Windows.Forms.NumericUpDown();
            this.lblUpdateFrequncy = new System.Windows.Forms.Label();
            this.btnPopulate = new System.Windows.Forms.Button();
            this.dtpEndDate = new System.Windows.Forms.DateTimePicker();
            this.lblEndDate = new System.Windows.Forms.Label();
            this.lblStartDate = new System.Windows.Forms.Label();
            this.dtpStartDate = new System.Windows.Forms.DateTimePicker();
            this.lblIRS = new System.Windows.Forms.Label();
            this.lblLIBOR = new System.Windows.Forms.Label();
            this.lblCMT = new System.Windows.Forms.Label();
            this.dgvIRS = new System.Windows.Forms.DataGridView();
            this.dgvLIBOR = new System.Windows.Forms.DataGridView();
            this.dgvCMT = new System.Windows.Forms.DataGridView();
            this.pnlData.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudUpdateSec)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvIRS)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLIBOR)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCMT)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlData
            // 
            this.pnlData.Controls.Add(this.groupBox1);
            this.pnlData.Controls.Add(this.lblIRS);
            this.pnlData.Controls.Add(this.lblLIBOR);
            this.pnlData.Controls.Add(this.lblCMT);
            this.pnlData.Controls.Add(this.dgvIRS);
            this.pnlData.Controls.Add(this.dgvLIBOR);
            this.pnlData.Controls.Add(this.dgvCMT);
            this.pnlData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlData.Location = new System.Drawing.Point(0, 0);
            this.pnlData.Name = "pnlData";
            this.pnlData.Size = new System.Drawing.Size(817, 579);
            this.pnlData.TabIndex = 2;
            this.pnlData.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlData_Paint);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnStopUpdate);
            this.groupBox1.Controls.Add(this.btnStartUpdate);
            this.groupBox1.Controls.Add(this.btnSet);
            this.groupBox1.Controls.Add(this.lblSecond);
            this.groupBox1.Controls.Add(this.nudUpdateSec);
            this.groupBox1.Controls.Add(this.lblUpdateFrequncy);
            this.groupBox1.Controls.Add(this.btnPopulate);
            this.groupBox1.Controls.Add(this.dtpEndDate);
            this.groupBox1.Controls.Add(this.lblEndDate);
            this.groupBox1.Controls.Add(this.lblStartDate);
            this.groupBox1.Controls.Add(this.dtpStartDate);
            this.groupBox1.Location = new System.Drawing.Point(80, 467);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(713, 90);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            // 
            // btnStopUpdate
            // 
            this.btnStopUpdate.AutoSize = true;
            this.btnStopUpdate.Enabled = false;
            this.btnStopUpdate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStopUpdate.Location = new System.Drawing.Point(601, 48);
            this.btnStopUpdate.Name = "btnStopUpdate";
            this.btnStopUpdate.Size = new System.Drawing.Size(94, 26);
            this.btnStopUpdate.TabIndex = 17;
            this.btnStopUpdate.Text = "Stop Update";
            this.btnStopUpdate.UseVisualStyleBackColor = true;
            this.btnStopUpdate.Click += new System.EventHandler(this.btnStopUpdate_Click);
            // 
            // btnStartUpdate
            // 
            this.btnStartUpdate.AutoSize = true;
            this.btnStartUpdate.Enabled = false;
            this.btnStartUpdate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStartUpdate.Location = new System.Drawing.Point(491, 49);
            this.btnStartUpdate.Name = "btnStartUpdate";
            this.btnStartUpdate.Size = new System.Drawing.Size(93, 26);
            this.btnStartUpdate.TabIndex = 16;
            this.btnStartUpdate.Text = "Start Update";
            this.btnStartUpdate.UseVisualStyleBackColor = true;
            this.btnStartUpdate.Click += new System.EventHandler(this.btnStartUpdate_Click);
            // 
            // btnSet
            // 
            this.btnSet.AutoSize = true;
            this.btnSet.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSet.Location = new System.Drawing.Point(395, 49);
            this.btnSet.Name = "btnSet";
            this.btnSet.Size = new System.Drawing.Size(75, 26);
            this.btnSet.TabIndex = 15;
            this.btnSet.Text = "Set";
            this.btnSet.UseVisualStyleBackColor = true;
            this.btnSet.Click += new System.EventHandler(this.btnSet_Click);
            // 
            // lblSecond
            // 
            this.lblSecond.AutoSize = true;
            this.lblSecond.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSecond.Location = new System.Drawing.Point(566, 20);
            this.lblSecond.Name = "lblSecond";
            this.lblSecond.Size = new System.Drawing.Size(53, 16);
            this.lblSecond.TabIndex = 14;
            this.lblSecond.Text = "second";
            // 
            // nudUpdateSec
            // 
            this.nudUpdateSec.Location = new System.Drawing.Point(525, 20);
            this.nudUpdateSec.Name = "nudUpdateSec";
            this.nudUpdateSec.Size = new System.Drawing.Size(40, 20);
            this.nudUpdateSec.TabIndex = 13;
            // 
            // lblUpdateFrequncy
            // 
            this.lblUpdateFrequncy.AutoSize = true;
            this.lblUpdateFrequncy.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUpdateFrequncy.Location = new System.Drawing.Point(392, 21);
            this.lblUpdateFrequncy.Name = "lblUpdateFrequncy";
            this.lblUpdateFrequncy.Size = new System.Drawing.Size(126, 16);
            this.lblUpdateFrequncy.TabIndex = 12;
            this.lblUpdateFrequncy.Text = "Update Frequency :";
            // 
            // btnPopulate
            // 
            this.btnPopulate.AutoSize = true;
            this.btnPopulate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPopulate.Location = new System.Drawing.Point(271, 48);
            this.btnPopulate.Name = "btnPopulate";
            this.btnPopulate.Size = new System.Drawing.Size(75, 26);
            this.btnPopulate.TabIndex = 11;
            this.btnPopulate.Text = "Populate";
            this.btnPopulate.UseVisualStyleBackColor = true;
            this.btnPopulate.Click += new System.EventHandler(this.btnPopulate_Click);
            // 
            // dtpEndDate
            // 
            this.dtpEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpEndDate.Location = new System.Drawing.Point(158, 52);
            this.dtpEndDate.Name = "dtpEndDate";
            this.dtpEndDate.Size = new System.Drawing.Size(96, 20);
            this.dtpEndDate.TabIndex = 10;
            // 
            // lblEndDate
            // 
            this.lblEndDate.AutoSize = true;
            this.lblEndDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEndDate.Location = new System.Drawing.Point(82, 53);
            this.lblEndDate.Name = "lblEndDate";
            this.lblEndDate.Size = new System.Drawing.Size(70, 16);
            this.lblEndDate.TabIndex = 9;
            this.lblEndDate.Text = "End Date :";
            // 
            // lblStartDate
            // 
            this.lblStartDate.AutoSize = true;
            this.lblStartDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStartDate.Location = new System.Drawing.Point(79, 19);
            this.lblStartDate.Name = "lblStartDate";
            this.lblStartDate.Size = new System.Drawing.Size(73, 16);
            this.lblStartDate.TabIndex = 8;
            this.lblStartDate.Text = "Start Date :";
            // 
            // dtpStartDate
            // 
            this.dtpStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpStartDate.Location = new System.Drawing.Point(159, 18);
            this.dtpStartDate.Name = "dtpStartDate";
            this.dtpStartDate.Size = new System.Drawing.Size(96, 20);
            this.dtpStartDate.TabIndex = 7;
            // 
            // lblIRS
            // 
            this.lblIRS.AutoSize = true;
            this.lblIRS.Location = new System.Drawing.Point(25, 325);
            this.lblIRS.MaximumSize = new System.Drawing.Size(50, 0);
            this.lblIRS.Name = "lblIRS";
            this.lblIRS.Size = new System.Drawing.Size(45, 39);
            this.lblIRS.TabIndex = 5;
            this.lblIRS.Text = "Interest Rate Swap";
            // 
            // lblLIBOR
            // 
            this.lblLIBOR.AutoSize = true;
            this.lblLIBOR.Location = new System.Drawing.Point(25, 171);
            this.lblLIBOR.MaximumSize = new System.Drawing.Size(50, 0);
            this.lblLIBOR.Name = "lblLIBOR";
            this.lblLIBOR.Size = new System.Drawing.Size(39, 13);
            this.lblLIBOR.TabIndex = 4;
            this.lblLIBOR.Text = "LIBOR";
            // 
            // lblCMT
            // 
            this.lblCMT.AutoSize = true;
            this.lblCMT.Location = new System.Drawing.Point(25, 14);
            this.lblCMT.MaximumSize = new System.Drawing.Size(50, 0);
            this.lblCMT.Name = "lblCMT";
            this.lblCMT.Size = new System.Drawing.Size(49, 39);
            this.lblCMT.TabIndex = 3;
            this.lblCMT.Text = "Constatn Maturity Rate";
            // 
            // dgvIRS
            // 
            this.dgvIRS.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvIRS.Location = new System.Drawing.Point(80, 325);
            this.dgvIRS.Name = "dgvIRS";
            this.dgvIRS.Size = new System.Drawing.Size(713, 129);
            this.dgvIRS.TabIndex = 2;
            this.dgvIRS.MouseEnter += new System.EventHandler(this.dgvIRS_MouseEnter);
            this.dgvIRS.MouseLeave += new System.EventHandler(this.dgvIRS_MouseLeave);
            // 
            // dgvLIBOR
            // 
            this.dgvLIBOR.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvLIBOR.Location = new System.Drawing.Point(80, 171);
            this.dgvLIBOR.Name = "dgvLIBOR";
            this.dgvLIBOR.Size = new System.Drawing.Size(713, 129);
            this.dgvLIBOR.TabIndex = 1;
            this.dgvLIBOR.MouseEnter += new System.EventHandler(this.dgvLIBOR_MouseEnter);
            this.dgvLIBOR.MouseLeave += new System.EventHandler(this.dgvLIBOR_MouseLeave);
            // 
            // dgvCMT
            // 
            this.dgvCMT.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCMT.Location = new System.Drawing.Point(80, 14);
            this.dgvCMT.Name = "dgvCMT";
            this.dgvCMT.Size = new System.Drawing.Size(713, 129);
            this.dgvCMT.TabIndex = 0;
            this.dgvCMT.DataSourceChanged += new System.EventHandler(this.dgvCMT_DataSourceChanged);
            this.dgvCMT.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvCMT_CellFormatting);
            this.dgvCMT.ColumnAdded += new System.Windows.Forms.DataGridViewColumnEventHandler(this.dgvCMT_ColumnAdded);
            this.dgvCMT.MouseEnter += new System.EventHandler(this.dgvCMT_MouseEnter);
            this.dgvCMT.MouseLeave += new System.EventHandler(this.dgvCMT_MouseLeave);
            // 
            // UserControlData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlData);
            this.Name = "UserControlData";
            this.Size = new System.Drawing.Size(817, 579);
            this.pnlData.ResumeLayout(false);
            this.pnlData.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudUpdateSec)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvIRS)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLIBOR)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCMT)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlData;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnPopulate;
        private System.Windows.Forms.DateTimePicker dtpEndDate;
        private System.Windows.Forms.Label lblEndDate;
        private System.Windows.Forms.Label lblStartDate;
        private System.Windows.Forms.DateTimePicker dtpStartDate;
        private System.Windows.Forms.Label lblIRS;
        private System.Windows.Forms.Label lblLIBOR;
        private System.Windows.Forms.Label lblCMT;
        private System.Windows.Forms.DataGridView dgvIRS;
        private System.Windows.Forms.DataGridView dgvLIBOR;
        private System.Windows.Forms.DataGridView dgvCMT;
        private System.Windows.Forms.Button btnStartUpdate;
        private System.Windows.Forms.Button btnSet;
        private System.Windows.Forms.Label lblSecond;
        private System.Windows.Forms.NumericUpDown nudUpdateSec;
        private System.Windows.Forms.Label lblUpdateFrequncy;
        private System.Windows.Forms.Button btnStopUpdate;

    }
}
