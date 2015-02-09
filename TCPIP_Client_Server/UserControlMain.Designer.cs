namespace Server
{
    partial class UserControlMain
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
            this.gbActivityMsg = new System.Windows.Forms.GroupBox();
            this.txtActMsg = new System.Windows.Forms.TextBox();
            this.gbClientList = new System.Windows.Forms.GroupBox();
            this.dgvClientNames = new System.Windows.Forms.DataGridView();
            this.btnDisconnectClient = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.gbActivityMsg.SuspendLayout();
            this.gbClientList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvClientNames)).BeginInit();
            this.SuspendLayout();
            // 
            // gbActivityMsg
            // 
            this.gbActivityMsg.Controls.Add(this.txtActMsg);
            this.gbActivityMsg.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbActivityMsg.Location = new System.Drawing.Point(14, 18);
            this.gbActivityMsg.Name = "gbActivityMsg";
            this.gbActivityMsg.Size = new System.Drawing.Size(524, 460);
            this.gbActivityMsg.TabIndex = 1;
            this.gbActivityMsg.TabStop = false;
            this.gbActivityMsg.Text = "Activity Message";
            // 
            // txtActMsg
            // 
            this.txtActMsg.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtActMsg.BackColor = System.Drawing.SystemColors.Window;
            this.txtActMsg.Location = new System.Drawing.Point(23, 30);
            this.txtActMsg.Multiline = true;
            this.txtActMsg.Name = "txtActMsg";
            this.txtActMsg.ReadOnly = true;
            this.txtActMsg.Size = new System.Drawing.Size(474, 408);
            this.txtActMsg.TabIndex = 2;
            // 
            // gbClientList
            // 
            this.gbClientList.Controls.Add(this.dgvClientNames);
            this.gbClientList.Controls.Add(this.btnDisconnectClient);
            this.gbClientList.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbClientList.Location = new System.Drawing.Point(559, 18);
            this.gbClientList.Name = "gbClientList";
            this.gbClientList.Size = new System.Drawing.Size(237, 516);
            this.gbClientList.TabIndex = 2;
            this.gbClientList.TabStop = false;
            this.gbClientList.Text = "Client List";
            // 
            // dgvClientNames
            // 
            this.dgvClientNames.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvClientNames.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvClientNames.Location = new System.Drawing.Point(17, 30);
            this.dgvClientNames.Name = "dgvClientNames";
            this.dgvClientNames.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvClientNames.Size = new System.Drawing.Size(203, 408);
            this.dgvClientNames.TabIndex = 7;
            this.dgvClientNames.ColumnAdded += new System.Windows.Forms.DataGridViewColumnEventHandler(this.dgvClientNames_ColumnAdded);
            // 
            // btnDisconnectClient
            // 
            this.btnDisconnectClient.AutoSize = true;
            this.btnDisconnectClient.Location = new System.Drawing.Point(27, 473);
            this.btnDisconnectClient.Name = "btnDisconnectClient";
            this.btnDisconnectClient.Size = new System.Drawing.Size(121, 26);
            this.btnDisconnectClient.TabIndex = 6;
            this.btnDisconnectClient.Text = "Disconnect Client";
            this.btnDisconnectClient.UseVisualStyleBackColor = true;
            this.btnDisconnectClient.Click += new System.EventHandler(this.btnDisconnectClient_Click);
            // 
            // btnStart
            // 
            this.btnStart.AutoSize = true;
            this.btnStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStart.Location = new System.Drawing.Point(37, 494);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(88, 26);
            this.btnStart.TabIndex = 3;
            this.btnStart.Text = "Start Server";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnStop
            // 
            this.btnStop.AutoSize = true;
            this.btnStop.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStop.Location = new System.Drawing.Point(152, 494);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(89, 26);
            this.btnStop.TabIndex = 4;
            this.btnStop.Text = "Stop Server";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // UserControlMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.gbClientList);
            this.Controls.Add(this.gbActivityMsg);
            this.Name = "UserControlMain";
            this.Size = new System.Drawing.Size(817, 579);
            this.Load += new System.EventHandler(this.UserControlMain_Load);
            this.gbActivityMsg.ResumeLayout(false);
            this.gbActivityMsg.PerformLayout();
            this.gbClientList.ResumeLayout(false);
            this.gbClientList.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvClientNames)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox gbActivityMsg;
        private System.Windows.Forms.TextBox txtActMsg;
        private System.Windows.Forms.GroupBox gbClientList;
        private System.Windows.Forms.Button btnDisconnectClient;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.DataGridView dgvClientNames;

    }
}
