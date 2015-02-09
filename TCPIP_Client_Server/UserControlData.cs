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
using System.Xml;
using System.IO;
using Utility;

namespace Server
{
    public partial class UserControlData : UserControl
    {
        #region Delegate

        private delegate void DataTableMod(ref DataTable dt, IDictionary<string, string> dic);

        #endregion Delegate

        #region Event

        public event EventHandler<Tuple<DateTime,DateTime,CancellationToken>> DownloadCommandEvent;
        public event EventHandler<IEnumerable<string>> PullDataRequestEvent;
        public event EventHandler<int> SetUpdateFrequencyEvent;
        public event EventHandler<bool> UpdateEvent;

        #endregion Event

        #region Event Handler

        private void btnPopulate_Click(object sender, EventArgs e)
        {
            if (this.dtpStartDate.Value > this.dtpEndDate.Value)
                using (new CenterWinDialog(Application.OpenForms.Cast<Form>().Last()))
                    MessageBox.Show("Start date cannot be later than end date", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                //Set up the progress form
                _progressForm = new ProgressForm();
                _progressForm.CancelRequest += WhenDownloadCancelled;

                DownloadCommandEvent(this, new Tuple<DateTime, DateTime, CancellationToken>(dtpStartDate.Value, dtpEndDate.Value, _progressForm.CT));

                _progressForm.ShowDialog();
            }
        }
        public void DownloadTaskStatusEventHandler(object sender, bool status)
        {
            if (status)
            {
                PullDataRequestEvent(this, _tableIndex.Keys.AsEnumerable());
                if (this.dtpEndDate.InvokeRequired)
                {
                    this.dtpEndDate.BeginInvoke(new Action(() => _endDate = this.dtpEndDate.Value));
                }
                _endDate = this.dtpEndDate.Value;
            }
            else
            {
                MessageBox.Show("Server failed to download the data", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void SendDataToUIEventHandler(object sender, Dictionary<string, DataTable> sendback)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action(
                    () =>
                    {
                        foreach (string key in sendback.Keys)
                        {
                            _tableIndex[key].DTable.Merge(sendback[key], false, MissingSchemaAction.AddWithKey);
                            _tableIndex[key].BSource.ResetBindings(true);
                        }
                    }
                )
                );
            }
            else
            {
                foreach (string key in sendback.Keys)
                {
                    if (_tableIndex[key].DTable != null)
                        _tableIndex[key].DTable.Clear();
                    _tableIndex[key].DTable.Merge(sendback[key], false, MissingSchemaAction.AddWithKey);
                }
            }
            if (!_progressForm.IsDisposed)
            {
                _progressForm.Clear();
                _progressForm.Dispose();
            }
        }
        public void UpdateNumOfProcessingTasksEventHandler(object sender, int num)
        {
            _progressForm.IncomingTasks = num;
        }
        public void UpdateNumOfCompletedTasksEventHandler(object sender, int num)
        {
            _progressForm.CompletedTasks = num;
        }
        public void UpdateCompletedEventHandler(object sender, bool completed)
        {
            _updateCompleted = completed;
            if (_updateCompleted)
                if (this.btnStopUpdate.InvokeRequired)
                    this.btnStopUpdate.BeginInvoke(
                        new Action(() =>
                        {
                            this.btnStopUpdate.Enabled = false;
                            this.btnPopulate.Enabled = true;
                        }));
                else
                {
                    this.btnStopUpdate.Enabled = false;
                    this.btnPopulate.Enabled = true;
                }
        }
        public void UpdateEndDateEventHandler(object sender, DateTime enddate)
        {
            _endDate = enddate;
        }
        private void btnSet_Click(object sender, EventArgs e)
        {
            _updateFrequency = (int)nudUpdateSec.Value;
            if (_updateCompleted)
            {
                this.btnStartUpdate.Enabled = true;
            }
        }
        private void btnStartUpdate_Click(object sender, EventArgs e)
        {
            if (_endDate.Date == DateTime.Now.Date)
            {
                using (new CenterWinDialog(Application.OpenForms.Cast<Form>().Last()))
                    MessageBox.Show("Data is up to date", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.btnStartUpdate.Enabled = false;
            }
            else
            {
                _updateCompleted = false;
                if (SetUpdateFrequencyEvent != null)
                    SetUpdateFrequencyEvent(this, _updateFrequency);
                if (UpdateEvent != null)
                    UpdateEvent(this, true);
                this.btnStartUpdate.Enabled = false;
                this.btnStopUpdate.Enabled = true;
                this.btnPopulate.Enabled = false;
            }
        }
        private void btnStopUpdate_Click(object sender, EventArgs e)
        {
            if (UpdateEvent != null)
                UpdateEvent(this, false);
            this.btnStopUpdate.Enabled = false;
            this.btnPopulate.Enabled = true;
            this._updateCompleted = true;
        }
        private void pnlData_Paint(object sender, PaintEventArgs e)
        {
            
        }
        private void dgvCMT_MouseEnter(object sender, EventArgs e)
        {
            this.dgvCMT.Focus();
        }
        private void dgvCMT_MouseLeave(object sender, EventArgs e)
        {
            this.lblStartDate.Focus();
        }
        private void dgvCMT_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.Value != null)
            {
                if (e.Value.GetType() == typeof(double))
                    e.CellStyle.Format = "N4";
            }
        }
        private void dgvLIBOR_MouseEnter(object sender, EventArgs e)
        {
            this.dgvLIBOR.Focus();
        }
        private void dgvLIBOR_MouseLeave(object sender, EventArgs e)
        {
            this.lblStartDate.Focus();
        }
        private void dgvIRS_MouseEnter(object sender, EventArgs e)
        {
            this.dgvIRS.Focus();
        }
        private void dgvIRS_MouseLeave(object sender, EventArgs e)
        {
            this.lblStartDate.Focus();
        }

        

        #endregion Event Handler

        #region Methods

        public UserControlData()
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;

            _IDs.ForEach(id => _tableIndex.Add(id, new BindDataTable(id)));
            DataGridViewSetUp(_tableIndex);
        }
        private void WhenDownloadCancelled(object sender, CancellationTokenSource cts)
        {
            cts.Cancel();
        }
        private void DataGridViewSetUp(Dictionary<string, BindDataTable> tableIndex)
        {

            this.dgvCMT.DataSource = tableIndex["CMT"].BSource;
            this.dgvLIBOR.DataSource = tableIndex["LIBOR"].BSource;
            this.dgvIRS.DataSource = tableIndex["IRS"].BSource;
        }

        #endregion Methods

        #region Type Class

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

        #endregion Type Class

        #region Fields

        private DateTime _startDate;
        private DateTime _endDate;
        private ProgressForm _progressForm = new ProgressForm();
        private int _updateFrequency;

        private bool _updateCompleted = true;
        private Dictionary<string, BindDataTable> _tableIndex = new Dictionary<string,BindDataTable>();
        private List<string> _IDs = new List<string>() { "CMT", "LIBOR", "IRS" };
        #endregion Fields

        private void dgvCMT_DataSourceChanged(object sender, EventArgs e)
        {
            this.dgvCMT.Refresh();
        }

        private void dgvCMT_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
            e.Column.SortMode = DataGridViewColumnSortMode.Automatic;
        }

        

        

    }
}
