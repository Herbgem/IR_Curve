using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using System.Net.Sockets;
using Utility;
//
using System.Windows.Forms;

namespace Server
{
    internal class Operation
    {
        #region Delegate

        private delegate void DataTableMod(ref DataTable dt, IDictionary<string, string> dic);

        #endregion Delegate

        #region Event
        //Event raised for user control data
        public event EventHandler<int> UpdateNumOfProcessingTasksEvent;
        public event EventHandler<int> UpdateNumOfCompletedTasksEvent;
        public event EventHandler<bool> UpdateCompletedEvent;
        public event EventHandler<DateTime> UpdateEndDateEvent;
        public event EventHandler<bool> DownloadTaskStatusEvent;
        public event EventHandler<Dictionary<string, DataTable>> SendDataToUIEvent;
        //Event raised for user control main
        public event EventHandler<Exception> ReportErrorToUIEvent;
        //Event raised for ServerComm
        public event EventHandler<Tuple<string, Socket>> RequestDataOutOfBoundEvent;
        public event EventHandler<Tuple<DataTable, Socket>> SendDataToServerCommEvent;
        public event EventHandler<DateTime> SendLatestDateToServerCommEvent;
        #endregion Event

        #region EventHandler

        public void DownloadCommandEventHandler(object sender, Tuple<DateTime, DateTime, CancellationToken> request)
        {
            AcceptDownloadTask(request.Item1, request.Item2, request.Item3);
        }
        public void PullDataRequestEventHandler(object sender, IEnumerable<string> ids)
        {
            AcceptDataPulling(ids);
        }
        public void SetUpdateFrequencyEventHandler(object sender, int updateFrequency)
        {
            _updateFrequency = updateFrequency;
            _updateStatus = true;
        }
        public void UpdateEventHandler(object sender, bool updateStatue)
        {
            _updateStatus = updateStatue;
            if (_updateStatus)
            {
                //Kill previous update loop if exists
                _updateCTS.Cancel();
                _updateCTS = new CancellationTokenSource();
                UpdateLoop(ref _endDate, _updateFrequency, _updateCTS.Token);
            }
            else
            {
                _updateCTS.Cancel();
            }
        }
        public void UpdateEndDateEventHandler(object sender, DateTime enddate)
        {
            _rwEndDateLock.EnterWriteLock();
            _endDate = enddate;
            _rwEndDateLock.ExitWriteLock();
        }
        public void ClientRequestDataEventHandler(object sender, Tuple<Times, Socket> t)
        {
            DataTable queryResult = new DataTable();
            _rwDataLock.EnterReadLock();

            try
            {
                queryResult = _tableIndex[t.Item1.IRType].Clone();
                var query = from row in _tableIndex[t.Item1.IRType].AsEnumerable()
                            where row.Field<DateTime>("date") >= t.Item1.StartDate
                            && row.Field<DateTime>("date") <= t.Item1.EndDate
                            select row;

                if (query.Count() != 0)
                {
                    queryResult.Merge(query.CopyToDataTable<DataRow>());
                    SendDataToServerCommEvent(this, new Tuple<DataTable, Socket>(queryResult, t.Item2));
                }
            }
            catch (Exception ex)
            {
                ReportErrorToUIEvent(this, ex);
            }
            finally
            {
                _rwDataLock.ExitReadLock();
            }
        }
        public void LatestDateRequestEventHandler(object sender, string placeHolder)
        {
            _rwEndDateLock.EnterReadLock();
            SendLatestDateToServerCommEvent(this, _endDate);
            _rwEndDateLock.ExitReadLock();
        }

        #endregion EventHandler

        #region Methods

        public Operation()
        {
            UpdateEndDateEvent += UpdateEndDateEventHandler;
        }
        public void AcceptDownloadTask(DateTime startDate, DateTime endDate, CancellationToken ct)
        {
            _startDate = startDate;

            _endDate = endDate;

            Task<DataTable> cmtTask = DownloadProcedureAsync(_startDate, _endDate, _fred.TreasureIDs, ct, new DataTableMod(CalculateZeroRate));
            Task<DataTable> liborTask = DownloadProcedureAsync(_startDate, _endDate, _fred.LiborIDs, ct);
            Task<DataTable> irsTask = DownloadProcedureAsync(_startDate, _endDate, _fred.IRSIDs, ct ,new DataTableMod(CalculateZeroRate));

            Task aggregateTasks = Task.WhenAll(cmtTask, liborTask, irsTask);
            aggregateTasks.GetAwaiter().OnCompleted(
                () =>
                {
                    if (aggregateTasks.Status == TaskStatus.RanToCompletion)
                    {
                        _rwDataLock.EnterWriteLock();
                        _tableIndex["CMT"] = cmtTask.Result;
                        _tableIndex["LIBOR"] = liborTask.Result;
                        _tableIndex["IRS"] = irsTask.Result;
                        _rwDataLock.ExitWriteLock();
                        DownloadTaskStatusEvent(this, true);
                    }
                    else
                    {
                        DownloadTaskStatusEvent(this, false);
                        MessageBox.Show(aggregateTasks.Exception.InnerException.Message + "\r\n" + aggregateTasks.Exception.InnerException.StackTrace);
                    }
                });
        }
        public void AcceptDataPulling(IEnumerable<string> ids) 
        {
            var tables = from table in _tableIndex
                           join id in ids
                           on table.Key equals id
                           select new { table.Key, table.Value };
            Dictionary<string, DataTable> sendback = tables.ToDictionary(ant => ant.Key, ant => ant.Value);
            SendDataToUIEvent(this, sendback);
        }
        private async Task<DataTable> DownloadProcedureAsync(DateTime startDate, DateTime endDate, Dictionary<string, string> ids, CancellationToken ct ,DataTableMod dtm = null)
        {
            object locker = new object();
            DataTable dt = new DataTable();
            List<Task> continuationTasks = new List<Task>();
            UpdateNumOfProcessingTasksEvent(this, ids.Count);

            foreach (string id in ids.Values)
            {
                Task task = SingleDownloadTask(id, startDate, endDate, ct).ContinueWith(ant =>
                {
                    lock (locker)
                    {
                        if (dt.Columns.Count == 0)
                        {
                            dt = ant.Result.Copy();
                            
                        }
                        else
                        {
                            dt.Merge(ant.Result, false, MissingSchemaAction.AddWithKey);
                            
                        }
                        UpdateNumOfCompletedTasksEvent(this, 1);
                    }
                }, TaskContinuationOptions.OnlyOnRanToCompletion);

                continuationTasks.Add(task);
            }
            await Task.WhenAll(continuationTasks).ContinueWith(
                (t) =>
                {
                    //Reorder the columns of _cmtTable to match the order set in the TreasureIDs
                    
                    if (dt.Columns.Count != 0)
                    {
                        ids.Values.ToList().ForEach(id => dt.Columns[id].SetOrdinal(dt.Columns.Count - 1));
                        if (dtm != null)
                        {
                            dtm(ref dt, ids);
                        }
                    }
                }, TaskContinuationOptions.OnlyOnRanToCompletion
                );
            
            return dt;
        }
        private void CalculateZeroRate(ref DataTable dt, IDictionary<string, string> dic)
        {
            dt = new ZeroCalculation(dt, dic).Run();
        }
        private Task<DataTable> SingleDownloadTask(string id, DateTime startDate, DateTime endDate, CancellationToken ct)
        {
            FredRequest fr = new FredRequest();
            Task<DataTable> task = Task.Factory.StartNew<DataTable>(() =>
            {
                DataTable table = new DataTable();
                table.TableName = id;
                table.Columns.Add(id, typeof(double));

                MemoryStream sr;

                if (fr.Download(fr.FormatRequest(id, startDate, endDate), id, out sr))
                {
                    table.Merge(Readxml(sr, id), false, MissingSchemaAction.AddWithKey);
                    sr.Close();
                    sr.Dispose();
                    return table;
                }
                else
                    return table;
            }, ct);
            return task;
        }
        private DataTable Readxml(string filefullname)
        {
            XmlReader r = XmlReader.Create(filefullname);
            ZeroDataStruct ir = new ZeroDataStruct(r, Path.GetFileNameWithoutExtension(filefullname));
            r.Close();
            return ir.Datasource;
        }
        private DataTable Readxml(Stream sr, string id)
        {
            sr.Seek(0, SeekOrigin.Begin);
            XmlReader r = XmlReader.Create(sr);
            ZeroDataStruct ir = new ZeroDataStruct(r, id);
            r.Close();
            return ir.Datasource;
        }
        private void UpdateLoop(ref DateTime endDate, int frequency ,CancellationToken ct)
        {
            _rwEndDateLock.EnterReadLock();
            DateTime date = endDate;
            _rwEndDateLock.ExitReadLock();
            Task.Run(
                async () =>
                {
                    while (date <= DateTime.Now)
                    {
                        
                        Thread.Sleep(frequency * 1000);
                        Task<DataTable> cmtTask = DownloadProcedureAsync(date, date, _fred.TreasureIDs, ct, new DataTableMod(CalculateZeroRate));
                        Task<DataTable> liborTask = DownloadProcedureAsync(date, date, _fred.LiborIDs, ct);
                        Task<DataTable> irsTask = DownloadProcedureAsync(date, date, _fred.IRSIDs, ct, new DataTableMod(CalculateZeroRate));

                        var aggregateTasks = Task<DataTable>.WhenAll(cmtTask, liborTask, irsTask);

                        try
                        {
                            Task continuation = aggregateTasks.ContinueWith(
                            t =>
                            {
                                _rwDataLock.EnterWriteLock();
                                _tableIndex["CMT"].Merge(cmtTask.Result, false, MissingSchemaAction.AddWithKey);
                                _tableIndex["LIBOR"].Merge(liborTask.Result, false, MissingSchemaAction.AddWithKey);
                                _tableIndex["IRS"].Merge(irsTask.Result, false, MissingSchemaAction.AddWithKey);
                                _rwDataLock.ExitWriteLock();

                                SendDataToUIEvent(this, _tableIndex);
                                
                            }, TaskContinuationOptions.OnlyOnRanToCompletion
                            );

                            await aggregateTasks;

                            if (continuation != null)
                            {
                                await continuation;
                            }
                        }
                        catch (AggregateException ex)
                        {
                            MessageBox.Show(ex.InnerException.Message + ex.StackTrace);
                        }

                        UpdateEndDateEvent(this, date);
                        date = date.AddDays(1);
                    }
                    UpdateCompletedEvent(this, true);
                }, ct
            );
        }

        #endregion Methods

        #region Fields
        private ReaderWriterLockSlim _rwDataLock = new ReaderWriterLockSlim();
        private ReaderWriterLockSlim _rwEndDateLock = new ReaderWriterLockSlim();

        private FredRequest _fred = new FredRequest();

        private int _updateFrequency;
        private bool _updateStatus = false;
        private CancellationTokenSource _updateCTS = new CancellationTokenSource();
        private static DateTime _startDate;
        private static DateTime _endDate = new DateTime(1990, 07, 04);

        private Dictionary<string, DataTable> _tableIndex =
            new Dictionary<string, DataTable>() {
            { "CMT", new DataTable("CMT")}, {"LIBOR", new DataTable("LIBOR")}, 
            {"IRS", new DataTable("IRS")}
            };
        #endregion Fields
    }
}
