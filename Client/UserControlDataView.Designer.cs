namespace Client
{
    partial class UserControlDataView
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
            this.panel2 = new System.Windows.Forms.Panel();
            this.lblIRType = new System.Windows.Forms.Label();
            this.gbConnection = new System.Windows.Forms.GroupBox();
            this.lblConnStatus = new System.Windows.Forms.Label();
            this.btnDisconnect = new System.Windows.Forms.Button();
            this.txtConnInfo = new System.Windows.Forms.TextBox();
            this.btnConnect = new System.Windows.Forms.Button();
            this.gbData = new System.Windows.Forms.GroupBox();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnDownload = new System.Windows.Forms.Button();
            this.lblDataTo = new System.Windows.Forms.Label();
            this.lblDataFrom = new System.Windows.Forms.Label();
            this.dtpDataTo = new System.Windows.Forms.DateTimePicker();
            this.dtpDataFrom = new System.Windows.Forms.DateTimePicker();
            this.gbLineChart = new System.Windows.Forms.GroupBox();
            this.btnDraw = new System.Windows.Forms.Button();
            this.dtpLCTo = new System.Windows.Forms.DateTimePicker();
            this.lblLCTo = new System.Windows.Forms.Label();
            this.dtpLCFrom = new System.Windows.Forms.DateTimePicker();
            this.lblLCFrom = new System.Windows.Forms.Label();
            this.dgvData = new System.Windows.Forms.DataGridView();
            this.panel2.SuspendLayout();
            this.gbConnection.SuspendLayout();
            this.gbData.SuspendLayout();
            this.gbLineChart.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvData)).BeginInit();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lblIRType);
            this.panel2.Controls.Add(this.gbConnection);
            this.panel2.Controls.Add(this.gbData);
            this.panel2.Controls.Add(this.gbLineChart);
            this.panel2.Controls.Add(this.dgvData);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(766, 645);
            this.panel2.TabIndex = 15;
            // 
            // lblIRType
            // 
            this.lblIRType.AutoSize = true;
            this.lblIRType.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblIRType.Location = new System.Drawing.Point(33, 17);
            this.lblIRType.Name = "lblIRType";
            this.lblIRType.Size = new System.Drawing.Size(45, 16);
            this.lblIRType.TabIndex = 22;
            this.lblIRType.Text = "label1";
            // 
            // gbConnection
            // 
            this.gbConnection.Controls.Add(this.lblConnStatus);
            this.gbConnection.Controls.Add(this.btnDisconnect);
            this.gbConnection.Controls.Add(this.txtConnInfo);
            this.gbConnection.Controls.Add(this.btnConnect);
            this.gbConnection.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbConnection.Location = new System.Drawing.Point(63, 546);
            this.gbConnection.Name = "gbConnection";
            this.gbConnection.Size = new System.Drawing.Size(636, 75);
            this.gbConnection.TabIndex = 21;
            this.gbConnection.TabStop = false;
            this.gbConnection.Text = "Connection";
            // 
            // lblConnStatus
            // 
            this.lblConnStatus.AutoSize = true;
            this.lblConnStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblConnStatus.Location = new System.Drawing.Point(201, 16);
            this.lblConnStatus.Name = "lblConnStatus";
            this.lblConnStatus.Size = new System.Drawing.Size(121, 16);
            this.lblConnStatus.TabIndex = 24;
            this.lblConnStatus.Text = "Connection Status :";
            // 
            // btnDisconnect
            // 
            this.btnDisconnect.AutoSize = true;
            this.btnDisconnect.Enabled = false;
            this.btnDisconnect.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDisconnect.Location = new System.Drawing.Point(101, 34);
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new System.Drawing.Size(85, 26);
            this.btnDisconnect.TabIndex = 23;
            this.btnDisconnect.Text = "Disconnect";
            this.btnDisconnect.UseVisualStyleBackColor = true;
            this.btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);
            // 
            // txtConnInfo
            // 
            this.txtConnInfo.Location = new System.Drawing.Point(204, 40);
            this.txtConnInfo.Name = "txtConnInfo";
            this.txtConnInfo.ReadOnly = true;
            this.txtConnInfo.Size = new System.Drawing.Size(417, 22);
            this.txtConnInfo.TabIndex = 22;
            this.txtConnInfo.Text = "Disconnected";
            // 
            // btnConnect
            // 
            this.btnConnect.AutoSize = true;
            this.btnConnect.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnConnect.Location = new System.Drawing.Point(20, 34);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(75, 26);
            this.btnConnect.TabIndex = 21;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // gbData
            // 
            this.gbData.Controls.Add(this.btnUpdate);
            this.gbData.Controls.Add(this.btnDownload);
            this.gbData.Controls.Add(this.lblDataTo);
            this.gbData.Controls.Add(this.lblDataFrom);
            this.gbData.Controls.Add(this.dtpDataTo);
            this.gbData.Controls.Add(this.dtpDataFrom);
            this.gbData.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbData.Location = new System.Drawing.Point(63, 442);
            this.gbData.Name = "gbData";
            this.gbData.Size = new System.Drawing.Size(300, 86);
            this.gbData.TabIndex = 19;
            this.gbData.TabStop = false;
            this.gbData.Text = "Data";
            // 
            // btnUpdate
            // 
            this.btnUpdate.AutoSize = true;
            this.btnUpdate.Enabled = false;
            this.btnUpdate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUpdate.Location = new System.Drawing.Point(190, 51);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 26);
            this.btnUpdate.TabIndex = 19;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnDownload
            // 
            this.btnDownload.AutoSize = true;
            this.btnDownload.Enabled = false;
            this.btnDownload.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDownload.Location = new System.Drawing.Point(190, 23);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(79, 26);
            this.btnDownload.TabIndex = 18;
            this.btnDownload.Text = "Download";
            this.btnDownload.UseVisualStyleBackColor = true;
            this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
            // 
            // lblDataTo
            // 
            this.lblDataTo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDataTo.AutoSize = true;
            this.lblDataTo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDataTo.Location = new System.Drawing.Point(17, 53);
            this.lblDataTo.Name = "lblDataTo";
            this.lblDataTo.Size = new System.Drawing.Size(31, 16);
            this.lblDataTo.TabIndex = 17;
            this.lblDataTo.Text = "To :";
            // 
            // lblDataFrom
            // 
            this.lblDataFrom.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDataFrom.AutoSize = true;
            this.lblDataFrom.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDataFrom.Location = new System.Drawing.Point(17, 29);
            this.lblDataFrom.Name = "lblDataFrom";
            this.lblDataFrom.Size = new System.Drawing.Size(45, 16);
            this.lblDataFrom.TabIndex = 16;
            this.lblDataFrom.Text = "From :";
            // 
            // dtpDataTo
            // 
            this.dtpDataTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDataTo.Location = new System.Drawing.Point(62, 53);
            this.dtpDataTo.Name = "dtpDataTo";
            this.dtpDataTo.Size = new System.Drawing.Size(106, 22);
            this.dtpDataTo.TabIndex = 15;
            // 
            // dtpDataFrom
            // 
            this.dtpDataFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDataFrom.Location = new System.Drawing.Point(62, 29);
            this.dtpDataFrom.Name = "dtpDataFrom";
            this.dtpDataFrom.Size = new System.Drawing.Size(106, 22);
            this.dtpDataFrom.TabIndex = 14;
            // 
            // gbLineChart
            // 
            this.gbLineChart.Controls.Add(this.btnDraw);
            this.gbLineChart.Controls.Add(this.dtpLCTo);
            this.gbLineChart.Controls.Add(this.lblLCTo);
            this.gbLineChart.Controls.Add(this.dtpLCFrom);
            this.gbLineChart.Controls.Add(this.lblLCFrom);
            this.gbLineChart.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbLineChart.Location = new System.Drawing.Point(391, 442);
            this.gbLineChart.Name = "gbLineChart";
            this.gbLineChart.Size = new System.Drawing.Size(308, 86);
            this.gbLineChart.TabIndex = 18;
            this.gbLineChart.TabStop = false;
            this.gbLineChart.Text = "Line Chart";
            // 
            // btnDraw
            // 
            this.btnDraw.AutoSize = true;
            this.btnDraw.Enabled = false;
            this.btnDraw.Location = new System.Drawing.Point(205, 49);
            this.btnDraw.Name = "btnDraw";
            this.btnDraw.Size = new System.Drawing.Size(75, 26);
            this.btnDraw.TabIndex = 21;
            this.btnDraw.Text = "Draw";
            this.btnDraw.UseVisualStyleBackColor = true;
            this.btnDraw.Click += new System.EventHandler(this.btnDraw_Click);
            // 
            // dtpLCTo
            // 
            this.dtpLCTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpLCTo.Location = new System.Drawing.Point(67, 51);
            this.dtpLCTo.Name = "dtpLCTo";
            this.dtpLCTo.Size = new System.Drawing.Size(115, 22);
            this.dtpLCTo.TabIndex = 20;
            // 
            // lblLCTo
            // 
            this.lblLCTo.AutoSize = true;
            this.lblLCTo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLCTo.Location = new System.Drawing.Point(16, 53);
            this.lblLCTo.Name = "lblLCTo";
            this.lblLCTo.Size = new System.Drawing.Size(31, 16);
            this.lblLCTo.TabIndex = 19;
            this.lblLCTo.Text = "To :";
            // 
            // dtpLCFrom
            // 
            this.dtpLCFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpLCFrom.Location = new System.Drawing.Point(67, 25);
            this.dtpLCFrom.Name = "dtpLCFrom";
            this.dtpLCFrom.Size = new System.Drawing.Size(115, 22);
            this.dtpLCFrom.TabIndex = 18;
            // 
            // lblLCFrom
            // 
            this.lblLCFrom.AutoSize = true;
            this.lblLCFrom.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLCFrom.Location = new System.Drawing.Point(16, 25);
            this.lblLCFrom.Name = "lblLCFrom";
            this.lblLCFrom.Size = new System.Drawing.Size(45, 16);
            this.lblLCFrom.TabIndex = 17;
            this.lblLCFrom.Text = "From :";
            // 
            // dgvData
            // 
            this.dgvData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvData.Location = new System.Drawing.Point(33, 42);
            this.dgvData.Name = "dgvData";
            this.dgvData.Size = new System.Drawing.Size(695, 380);
            this.dgvData.TabIndex = 9;
            // 
            // UserControlDataView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel2);
            this.Name = "UserControlDataView";
            this.Size = new System.Drawing.Size(766, 645);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.gbConnection.ResumeLayout(false);
            this.gbConnection.PerformLayout();
            this.gbData.ResumeLayout(false);
            this.gbData.PerformLayout();
            this.gbLineChart.ResumeLayout(false);
            this.gbLineChart.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvData)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.GroupBox gbConnection;
        private System.Windows.Forms.Label lblConnStatus;
        private System.Windows.Forms.Button btnDisconnect;
        private System.Windows.Forms.TextBox txtConnInfo;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.GroupBox gbData;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnDownload;
        private System.Windows.Forms.Label lblDataTo;
        private System.Windows.Forms.Label lblDataFrom;
        private System.Windows.Forms.DateTimePicker dtpDataTo;
        private System.Windows.Forms.DateTimePicker dtpDataFrom;
        private System.Windows.Forms.GroupBox gbLineChart;
        private System.Windows.Forms.DateTimePicker dtpLCTo;
        private System.Windows.Forms.Label lblLCTo;
        private System.Windows.Forms.DateTimePicker dtpLCFrom;
        private System.Windows.Forms.Label lblLCFrom;
        private System.Windows.Forms.DataGridView dgvData;
        private System.Windows.Forms.Button btnDraw;
        private System.Windows.Forms.Label lblIRType;

    }
}
