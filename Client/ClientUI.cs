using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    public partial class ClientUI : Form
    {
        public ClientUI()
        {
            InitializeComponent();
            this.panel1.Controls.Add(_CMT._UCDataView);
            this.panel1.Controls.Add(_LIBOR._UCDataView);
            this.panel1.Controls.Add(_IRS._UCDataView);
        }

        private void ClientUI_Load(object sender, EventArgs e)
        {
            //_CMT._UCDataView.BringToFront();
        }

        private class ClientWrapper
        {
            public ClientWrapper(string IRType)
            {
                _UCDataView = new UserControlDataView(IRType);
                _clientComm = new ClientComm(IRType);

                _UCDataView.ConnectRequestEvent += _clientComm.ConnectRequestEventHandler;
                _UCDataView.DownloadRequestEvent += _clientComm.DownloadRequestEventHandler;
                _UCDataView.UpdateRequestEVent += _clientComm.UpdateRequestEventHandler;

                _clientComm.ConnectionEstablishedEvent += _UCDataView.ConnectionEstablishedEventHandler;
                _clientComm.SendConnectionInfoEvent += _UCDataView.SendConnectionInfoEventHandler;
                _clientComm.SendDataTableToUIEvent += _UCDataView.SendDataTableToUIEventHandler;
                _clientComm.ErrorReportToUIEvent += _UCDataView.ErrorReportToUIEventhandler;
            }

            public UserControlDataView _UCDataView;
            public ClientComm _clientComm;
        }

        #region Fields

        ClientWrapper _CMT = new ClientWrapper("CMT");
        ClientWrapper _LIBOR = new ClientWrapper("LIBOR");
        ClientWrapper _IRS = new ClientWrapper("IRS");

        #endregion Fields

        private void lIBORToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _LIBOR._UCDataView.BringToFront();
        }

        private void iRSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _IRS._UCDataView.BringToFront();
        }

        private void cMTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _CMT._UCDataView.BringToFront();
        }
    }
}
