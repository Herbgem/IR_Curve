using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace Utility
{
    public partial class ProgressForm : Form
    {
        #region Methods

        public ProgressForm()
        {
            InitializeComponent();
            _ct = _cts.Token;
            _progress = new Progress<int>(i =>
            {
                progressBar1.Value = i;
                lblProgress.Text = string.Format("Downloading...{0}%", i);
            });
        }

        public void Clear()
        {
            _totalTasks = 0;
            _completedTasks = 0;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
            
        }

        private void ProgressForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            CancelRequest(this, _cts);
        }

        #endregion Methods

        #region Event

        public EventHandler<CancellationTokenSource> CancelRequest;

        #endregion Event

        #region Fields

        private CancellationTokenSource _cts = new CancellationTokenSource();
        private CancellationToken _ct;
        private object _locker = new object();
        private Progress<int> _progress;
        private int _incomingTasks = 0;
        private int _completedTasks = 0;
        private int _totalTasks = 0;

        public CancellationToken CT { get { return _ct; } private set { } }
        public Progress<int> ProgressReport { get { return _progress; } private set { } }
        public int IncomingTasks { get { return _incomingTasks; } set { _incomingTasks = value; _totalTasks += _incomingTasks; } }
        public int TotalTasks { get { return _totalTasks; } private set { } }
        public int CompletedTasks { 
            get { return _completedTasks; } 
            set { 
                _completedTasks += value; 
                ((IProgress<int>)_progress).Report(_completedTasks * 100 / _totalTasks); 
            } }
        public object Locker { get { return _locker; } private set { } }

        #endregion Fields


    }
}
