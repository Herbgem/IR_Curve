using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Server
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
            this.ShowIcon = false;
            ProgramSettings.Initialize();

            _UCData.DownloadCommandEvent += _op.DownloadCommandEventHandler;
            _UCData.PullDataRequestEvent += _op.PullDataRequestEventHandler;
            _UCData.SetUpdateFrequencyEvent += _op.SetUpdateFrequencyEventHandler;
            _UCData.UpdateEvent += _op.UpdateEventHandler;

            _op.DownloadTaskStatusEvent += _UCData.DownloadTaskStatusEventHandler;
            _op.SendDataToUIEvent += _UCData.SendDataToUIEventHandler;
            _op.UpdateNumOfProcessingTasksEvent += _UCData.UpdateNumOfProcessingTasksEventHandler;
            _op.UpdateNumOfCompletedTasksEvent += _UCData.UpdateNumOfCompletedTasksEventHandler;
            _op.UpdateCompletedEvent += _UCData.UpdateCompletedEventHandler;
            _op.UpdateEndDateEvent += _UCData.UpdateEndDateEventHandler;

            _op.SendDataToServerCommEvent += _serverComm.SendDataToServerCommEventHandler;
            //_op.RequestDataOutOfBoundEvent += _serverComm.RequestDataOutOfBoundEventHandler;
            _op.SendLatestDateToServerCommEvent += _serverComm.SendLatestDateToServerCommEventHandler;

            _op.ReportErrorToUIEvent += _UCMain.ReportErrorToUIEventHandler;

            _serverComm.BroadcastConnectionEvent += _UCMain.BroadcastConnectionEventHandler;
            _serverComm.SendClientNamesToUIEvent += _UCMain.SendClientNamesToUIEventHandler;
            _serverComm.ServerStatusEvent += _UCMain.ServerStatusEventHandler;

            _serverComm.ClientRequestDataEvent += _op.ClientRequestDataEventHandler;
            _serverComm.LatestDateRequestEvent += _op.LatestDateRequestEventHandler;

            _UCMain.StartStopServerEvent += _serverComm.StartStopServerEventHandler;
            _UCMain.DisconnectClientEvent += _serverComm.DisconnectClientEventHandler;
            
        }

        private void dataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this._UCData.BringToFront();
        }

        private void mainToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this._UCMain.BringToFront();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            this.panel1.Controls.Add(_UCMain);
            this.panel1.Controls.Add(_UCData);
            this._UCMain.BringToFront();
        }

        #region Fields

        Operation _op = new Operation();
        ServerComm _serverComm = new ServerComm();

        UserControlData _UCData = new UserControlData();
        UserControlMain _UCMain = new UserControlMain();

        #endregion Fields

    }
}
