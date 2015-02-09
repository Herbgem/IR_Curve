using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using RDotNet;
using Utility;

namespace Client
{
    public partial class UserControlDataView : UserControl
    {
        #region Event

        public event EventHandler<bool> ConnectRequestEvent;
        public event EventHandler<Times> DownloadRequestEvent;
        public event EventHandler<string> UpdateRequestEVent;

        #endregion Event

        #region Event Handler

        public void ConnectionEstablishedEventHandler(object sender, bool isConnected)
        {
            _isConnected = isConnected;
            if (!_isConnected)
            {
                this.txtConnInfo.BeginInvoke(new Action(() =>
                    {
                        DisconnectCleanWork();
                    }
                    ));
            }
            else
            {
                this.txtConnInfo.BeginInvoke(new Action(() =>
                    {
                        this.btnDownload.Enabled = true;
                        if (_isPopulated == true)
                            this.btnUpdate.Enabled = true;
                    }));
            }
        }
        public void SendConnectionInfoEventHandler(object sender, Socket returnedSocket)
        {
            _rootSocket = returnedSocket;
            if (this.txtConnInfo.InvokeRequired)
            {
                this.txtConnInfo.BeginInvoke(new Action(() =>
                    {
                        this.txtConnInfo.Text = string.Format(@"Client is connected to {0}", returnedSocket.RemoteEndPoint);
                        this.btnDisconnect.Enabled = true;
                    }
                    ));
            }
            else
            {
                this.txtConnInfo.Text = string.Format(@"Client is connected to {0}", returnedSocket.RemoteEndPoint);
                this.btnDisconnect.Enabled = true;
            }
        }
        public void SendDataTableToUIEventHandler(object sender, DataTable dt)
        {
            _isPopulated = true;
            _rwDataLock.TryEnterWriteLock(-1);
            
            this.Invoke(new Action(
                () =>
                {
                    _bindingTable.DTable.Merge(dt, false, MissingSchemaAction.AddWithKey);
                    if (_isPopulated == true)
                    {
                        this.btnUpdate.Enabled = true;
                        this.btnDraw.Enabled = true;
                    }
                }));
            _rwDataLock.ExitWriteLock();
        }
        public void ErrorReportToUIEventhandler(object sender, string error)
        {
            this.Invoke(new Action(() =>
            {
                using (new CenterWinDialog(Application.OpenForms.Cast<Form>().Last()))
                    MessageBox.Show(error, "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }));
            
            if (!_isConnected)
                this.btnConnect.BeginInvoke(new Action(() => this.btnConnect.Enabled = true));
        }
        private void btnConnect_Click(object sender, EventArgs e)
        {
            ConnectRequestEvent(this, true);
            this.btnConnect.Enabled = false;
        }
        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            ConnectRequestEvent(this, false);
            DisconnectCleanWork();
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            DownloadRequestEvent(this, new Times(this.dtpDataFrom.Value, this.dtpDataTo.Value, _IRType));
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            UpdateRequestEVent(this, string.Empty);
        }

        void graph_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.btnDraw.Enabled = true;
        }

        #endregion Event Handler

        #region Methods

        public UserControlDataView(string IRType)
        {
            InitializeComponent();

            _IRType = IRType;
            _bindingTable = new BindDataTable(IRType);

            this.dgvData.DataSource = _bindingTable.BSource;
            this.lblIRType.Text = IRType;

            REngine.SetEnvironmentVariables();
            _rEngine = REngine.GetInstance();
            _rEngine.Initialize();
        }
        private void DisconnectCleanWork()
        {
            this.txtConnInfo.Text = "Disconnected";

            this.btnDisconnect.Enabled = false;
            this.btnConnect.Enabled = true;
            this.btnDownload.Enabled = false;
            this.btnUpdate.Enabled = false;
        }
        private class BindDataTable
        {
            public BindDataTable(string id)
            {
                _dTable = new DataTable(id);
                BSource.DataSource = _dTable;
            }

            public DataTable _dTable;
            public DataTable DTable { get { return _dTable; } set { _dTable = value; BSource.ResetBindings(false); } }
            public BindingSource BSource = new BindingSource();
        }

        #endregion Methods

        #region Fields

        private ReaderWriterLockSlim _rwDataLock = new ReaderWriterLockSlim();

        private bool _isConnected = false;
        private bool _isPopulated = false;
        private Socket _rootSocket;

        private string _IRType;
        private BindDataTable _bindingTable;

        private DateTime _startDate = new DateTime(1990, 1, 1);
        private DateTime _endDate = new DateTime(1990, 1, 1);

        private REngine _rEngine;

        #endregion Fields

        

        private void btnDraw_Click(object sender, EventArgs e)
        {
            SetVariableInR(_bindingTable.DTable, this.dtpLCFrom.Value, this.dtpLCTo.Value);
            string filePath = string.Format(@"{0}\{1}.png", Application.StartupPath, _IRType);
            string saveFile = string.Format(@"ggsave(""{0}\{1}.png"")", Application.StartupPath, _IRType).Replace(@"\", @"/");
            string plotCmd = string.Format(@"ggplot(data = {0}, aes(x=Date, y=Value)) + geom_line(aes(colour=Level)) + ylab(""Value(%)"")", _IRType);
            _rEngine.Evaluate("library(ggplot2)");
            _rEngine.Evaluate(plotCmd);

            if (System.IO.File.Exists(filePath))
                System.IO.File.Delete(filePath);
            _rEngine.Evaluate(saveFile);
            _rEngine.Evaluate("graphics.off()");
            GC.Collect();
            GC.WaitForPendingFinalizers();

            GraphForm graph = new GraphForm(_IRType);
            graph.FormClosed += graph_FormClosed;
            using (var fs = System.IO.File.Open(filePath, System.IO.FileMode.Open))
                graph.Picture.Image = Image.FromStream(fs);
            graph.Show();

            this.btnDraw.Enabled = false;
        }

        

        private void SetVariableInR(DataTable dt, DateTime startDate, DateTime endDate)
        {
            _rEngine.Evaluate(string.Format("{0} <- data.frame()", _IRType));

            DataColumn date = dt.Columns.Cast<DataColumn>().First(col => col.DataType == typeof(DateTime));
            dt.Rows.Cast<DataRow>().OrderByDescending(row => row[date]);

            var dates = from row in dt.Rows.Cast<DataRow>()
                         where (DateTime)row[date] >= startDate && (DateTime)row[date] <= endDate
                         select ((DateTime)row[date]).ToShortDateString();

            CharacterVector dateVector = _rEngine.CreateCharacterVector(dates);
            _rEngine.SetSymbol(date.ColumnName, dateVector);
            _rEngine.Evaluate(string.Format(@"{0} <- as.Date({0}, ""%m/%d/%Y"")", date.ColumnName));
            

            foreach (DataColumn dc in dt.Columns)
            {
                switch (dc.DataType.ToString())
                {
                    case "System.Double":
                        var templs = from row in dt.Rows.Cast<DataRow>()
                                      where (DateTime)row[date] >= startDate && (DateTime)row[date] <= endDate
                                      select row[dc].GetType() == typeof(DBNull) ? double.NaN : (double)row[dc];

                        NumericVector tempvec = _rEngine.CreateNumericVector(templs);
                        _rEngine.SetSymbol(dc.ColumnName, tempvec);
                        _rEngine.Evaluate(string.Format(@"{0} <- data.frame(Date={1}, Value={2}, Level=""{3}"")", "tempDF", date.ColumnName, dc.ColumnName, dc.ColumnName));
                        _rEngine.Evaluate("tempDF");
                        _rEngine.Evaluate(string.Format(@"{0} <- rbind.data.frame({1}, {2})", _IRType, _IRType, "tempDF"));
                        break;
                }
            }
        }
    }
}
