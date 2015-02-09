using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Utility;

namespace Server
{
    public partial class UserControlMain : UserControl
    {
        #region Event

        public event EventHandler<bool> StartStopServerEvent;
        public event EventHandler<string> DisconnectClientEvent;

        #endregion Event

        #region EventHandler

        public void BroadcastConnectionEventHandler(object sender, string msg)
        {
            if (this.txtActMsg.InvokeRequired)
            {
                this.BeginInvoke(new Action(() => this.txtActMsg.AppendText(msg + "\r\n")));
            }
            else
                this.txtActMsg.AppendText(msg + "\r\n");
        }
        public void SendClientNamesToUIEventHandler(object sender, ClientInfo clientInfo)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke( 
                    new Action(
                        () => {
                            if (!_clientNames.Select(t => t._clientInfo.ClientID).Contains(clientInfo.ClientID))
                                this._clientNames.Add(
                                    new ClientNamesAndType(clientInfo)
                                    );
                            else
                                this._clientNames.Remove(_clientNames.Where(t => t._clientInfo.ClientID == clientInfo.ClientID).First());
                        }));
            }
        }
        public void ReportErrorToUIEventHandler(object sender, Exception ex)
        {
            using (new CenterWinDialog(Application.OpenForms.Cast<Form>().Last()))
                MessageBox.Show("Data is up to date", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        public void ServerStatusEventHandler(object sender, bool isRunning)
        {
            _isRunning = isRunning;
            if (_isRunning)
            {
                this.BeginInvoke(new Action(
                    () =>
                    {
                        this.btnStart.Enabled = false;
                        this.btnStop.Enabled = true;

                        this.txtActMsg.AppendText("Server is listening.\r\n");
                    }));
            }
        }
        private void btnStart_Click(object sender, EventArgs e)
        {
            Task.Run(() => StartStopServerEvent(this, true));

        }
        private void btnStop_Click(object sender, EventArgs e)
        {
            this.btnStop.Enabled = false;
            this.btnStart.Enabled = true;

            _clientNames.Clear();
            this.txtActMsg.AppendText("Server is closed.\r\n");
            StartStopServerEvent(this, false);
        }
        private void dgvClientNames_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
            e.Column.Width = 80;
        }
        private void UserControlMain_Load(object sender, EventArgs e)
        {
            this.btnStop.Enabled = false;
        }

        #endregion  EventHandler

        #region Methods

        public UserControlMain()
        {
            InitializeComponent();

            this.Dock = DockStyle.Fill;
            this.dgvClientNames.DataSource = this._clientNames;
        }

        #endregion Methods

        #region Field

        private bool _isRunning = false;

        private BindingList<ClientNamesAndType> _clientNames = new BindingList<ClientNamesAndType>();

        #endregion Field

        #region Class ClientNamesAndType
        private class ClientNamesAndType
        {
            public ClientNamesAndType(ClientInfo ci) 
            {
                _clientInfo = ci;
                ClientName = _clientInfo.ClientName;
                IRType = _clientInfo.IRType;
            }
            public ClientInfo _clientInfo;

            [System.ComponentModel.DisplayName("Clients")]
            public string ClientName { get; set; }
            [System.ComponentModel.DisplayName("IR Type")]
            public string IRType { get; set; }
        }

        #endregion Class ClientNamesAndType

        private void btnDisconnectClient_Click(object sender, EventArgs e)
        {
            string clientName = (string)this.dgvClientNames.SelectedRows[0].Cells[0].Value;
            _clientNames.Remove(_clientNames.First(ant => ant.ClientName == clientName));
            DisconnectClientEvent(this, clientName);
        }





    }
}
