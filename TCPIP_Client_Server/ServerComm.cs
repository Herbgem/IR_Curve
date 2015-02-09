using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.ComponentModel;
using System.IO;
using System.Data;
using System.Runtime.Serialization.Formatters.Binary;
using Utility;

namespace Server
{
    internal class ServerComm : IComm
    {
        #region Event
        //Events raised for communication with UI
        public event EventHandler<string> BroadcastConnectionEvent;
        public event EventHandler<ClientInfo> SendClientNamesToUIEvent;
        public event EventHandler<bool> ServerStatusEvent;
        //Events handled by class operation
        public event EventHandler<Tuple<Times, Socket>> ClientRequestDataEvent;
        public event EventHandler<string> LatestDateRequestEvent;

        #endregion Event

        #region EventHandler

        public void StartStopServerEventHandler(object sender, bool switcher)
        {
            if (switcher)
            {
                StartListening();
            }
            else
            {
                _cts.Cancel();
                ActiveSocketCleanUp(_activeSocket);
                _clientList.Clear();
                _updateList.Clear();
                _RootSocket.Close();
                _RootSocket.Dispose();
            }
        }
        public void SendDataToServerCommEventHandler(object sender, Tuple<DataTable, Socket> dt)
        {
            SendWrapper<DataTable>(dt.Item2, MessageType.DataTable, dt.Item1);
        }
        public void RequestDataOutOfBoundEventHandler(object sender, Tuple<string, Socket> tu)
        {
            SendWrapper<string>(tu.Item2, MessageType.Error, tu.Item1);
        }
        public void SendLatestDateToServerCommEventHandler(object sender, DateTime latestDate)
        {
            _rwLatestDateLock.EnterWriteLock();
            _latestDate = latestDate;
            _rwLatestDateLock.ExitWriteLock();
        }
        public void DisconnectClientEventHandler(object sender, string clientName)
        {
            ClientProfile cp = _clientList.First(ant => ant.ClientName == clientName);
            _activeSocket.RemoveAll(ant => ant == cp.WorkSocket);
            _clientList.Remove(cp);
            while (_updateList.Contains(new KeyValuePair<Socket, ClientProfile>(cp.WorkSocket, cp)))
                _updateList.TryRemove(cp.WorkSocket, out cp);
            cp.WorkSocket.Shutdown(SocketShutdown.Both);
            cp.WorkSocket.Disconnect(false);
            cp.WorkSocket.Close();
            cp.WorkSocket.Dispose();
        }

        #endregion EventHandler

        #region Methods

        public ServerComm()
        {
            _IPEndPoint = new IPEndPoint(_IPAddress, _IPPort);
        }
        public ServerComm(IPAddress IPAddress, int IPPort)
        {
            this._IPAddress = IPAddress;
            this._IPPort = IPPort;
            _IPEndPoint = new IPEndPoint(_IPAddress, _IPPort);
        }  
        public void StartListening()
        {
            //Set up the server socket
            _RootSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _RootSocket.ReceiveBufferSize = _bufferSize;
            _RootSocket.SendBufferSize = _bufferSize;
            _RootSocket.Bind(_IPEndPoint);
            //Start listening
            _RootSocket.Listen(100);
            ServerStatusEvent(this, true);
            //Create new cancellation token source instance
            _cts = new CancellationTokenSource();
            //Send Update to Clients
            Task.Run(() =>
            {
                while (true)
                {
                    PushUpdateToClient(_updateList);
                }
            }, _cts.Token);
            //Accept connection request
            while (!_cts.IsCancellationRequested)
            {
                _acceptSignal.Reset();
                try
                {
                    _RootSocket.BeginAccept(new AsyncCallback(AcceptCallBack), _RootSocket);
                }
                catch (ObjectDisposedException ex)
                {
                    //Exception handler goes in here
                }
                _acceptSignal.Wait();
            }
        }
        private void AcceptCallBack(IAsyncResult ar)
        {
            _acceptSignal.Set();

            try
            {
                Socket server = (Socket)ar.AsyncState;
                Socket handler = server.EndAccept(ar);
                _activeSocket.Add(handler);
                while (true)
                {
                    if (handler.Available > 0)
                        Receive(handler);
                }
            }
            catch (ObjectDisposedException ex)
            {
                //Exception handler goes in here
            }
        }
        public void Receive(Socket handler)
        {
            StateObject state = new StateObject();
            state.WorkSocket = handler;
            try
            {
                handler.BeginReceive(state._oneTimeBuffer, 0, state.OneTimeBufferSize, 0, new AsyncCallback(ReceiveCallback), state);
            }
            catch(ObjectDisposedException ex)
            {
                //Exception handler goes in here
            }
        }
        public void ReceiveCallback(IAsyncResult ar)
        {
            StateObject state = (StateObject)ar.AsyncState;
            Socket handler = state.WorkSocket;

            int byteRead = handler.EndReceive(ar);

            if (byteRead > 0)
            {
                if (state._msgBuffer == null)
                {
                    Console.WriteLine("Inside server receivecallback");
                    state._byteRead += byteRead;
                    if (state._byteRead >= _totalHeaderSize)
                    {


                        byte[] packetTypeArr = new byte[_typeByteSize];
                        byte[] msgLenArr = new byte[_msgLenByteSize];

                        Buffer.BlockCopy(state._oneTimeBuffer, 0, packetTypeArr, 0, _typeByteSize);

                        Buffer.BlockCopy(state._oneTimeBuffer, _typeByteSize, msgLenArr, 0, _msgLenByteSize);

                        int msgLength = BitConverter.ToInt32(msgLenArr, 0);
                        int packetType = BitConverter.ToInt32(packetTypeArr, 0);
                        Console.WriteLine("type: {0}", packetType);

                        state._packetType = packetType;
                        Console.WriteLine(msgLength);
                        state._msgBuffer = new byte[msgLength];
                        Buffer.BlockCopy(state._oneTimeBuffer, _totalHeaderSize, state._msgBuffer, 0, state._byteRead - _totalHeaderSize);

                        if (msgLength == (state._byteRead - _totalHeaderSize))
                        {
                            ActionFilter(state._packetType, state._msgBuffer, handler);
                        }
                        else if (msgLength > (state._byteRead - _totalHeaderSize))
                        {
                            Array.Clear(state._oneTimeBuffer, 0, state._oneTimeBuffer.Length);
                            try
                            {
                                handler.BeginReceive(state._oneTimeBuffer, 0, state.OneTimeBufferSize, 0, new AsyncCallback(ReceiveCallback), state);
                            }
                            catch (ObjectDisposedException ex)
                            {
                                //Exception handler goes in here
                            }
                        }
                        else
                        {
                            throw new Exception("Message lost");
                        }
                    }
                    else
                    {
                        handler.BeginReceive(state._oneTimeBuffer, state._byteRead, state.OneTimeBufferSize - state._byteRead, 0, new AsyncCallback(ReceiveCallback), state);
                    }
                }
                else
                {
                    Buffer.BlockCopy(state._oneTimeBuffer, 0, state._msgBuffer, state._byteRead - _totalHeaderSize, byteRead);
                    state._byteRead += byteRead;

                    if (state._msgBuffer.Length == state._byteRead - _totalHeaderSize)
                    {
                        ActionFilter(state._packetType, state._msgBuffer, handler);
                    }
                    else if (state._msgBuffer.Length > state._byteRead - _totalHeaderSize)
                    {
                        Array.Clear(state._oneTimeBuffer, 0, state._oneTimeBuffer.Length);
                        try
                        {
                            handler.BeginReceive(state._oneTimeBuffer, 0, state.OneTimeBufferSize, 0, new AsyncCallback(ReceiveCallback), state);
                        }
                        catch
                        {
                            //Exception handler goes in here
                        }
                    }
                    else
                    {
                        throw new Exception("Message lost");
                    }
                }
            }
        }
        public void Send(Socket workSocket, byte[] packet)
        {
            workSocket.BeginSend(packet, 0, packet.Length, 0, new AsyncCallback(SendCallback), workSocket);
        }
        public void SendCallback(IAsyncResult ar)
        {
            Socket workSocket = (Socket)ar.AsyncState;
            try
            {
                lock (locker)
                {
                    int byteSent = workSocket.EndSend(ar);
                }
            }
            catch(ObjectDisposedException ex)
            {
                //
            }
            catch (SocketException ex)
            {
                //
            }
        }
        private byte[] SendMsgFormat<T>(MessageType type, T msg)
        {
            if (msg == null)
                return null;
            //Convert string object type to byte array
            byte[] typeArray = BitConverter.GetBytes((Int32)type);
            //Convert generic type object msg to byte array
            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            formatter.Serialize(ms, msg);
            byte[] msgArray = ms.ToArray();
            ms.Close();
            ms.Dispose();
            //Convert the length of object msg to byte array
            byte[] lenArray = BitConverter.GetBytes(msgArray.Length);
            //Concatenate three arrays in the order of type, len and msg
            byte[] fullArray = typeArray.Concat(lenArray).Concat(msgArray).ToArray();

            return fullArray;
        }
        private void SendWrapper<T>(Socket workSocket, MessageType type, T msg)
        {
            byte[] packet = SendMsgFormat<T>(type, msg);
            Send(workSocket, packet);
        }
        private void ActionFilter(int packetType, byte[] msg, Socket handler)
        {
            string key = ((MessageType)packetType).ToString();

            object box;

            BinaryFormatter bformatter = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream(msg))
            {
                box = bformatter.Deserialize(ms);
            }

            switch(key)
            {
                case "ClientInfo":
                    ClientInfo ci1 = (ClientInfo)box;
                    _clientList.Add(new ClientProfile(ci1, handler));
                    BroadcastConnectionEvent(this, string.Format("{0} is connected.", ci1.ClientName));
                    SendClientNamesToUIEvent(this, ci1);
                    break;

                case "BulkData":
                    Times t2 = (Times)box;
                    BroadcastConnectionEvent(this, string.Format
                        ("{0} downloads data from {1} to {2}.", 
                        _clientList.First(ant => ant.WorkSocket == handler).ClientName, t2.StartDate.ToShortDateString(), t2.EndDate.ToShortDateString()));
                    ClientRequestDataEvent(this, new Tuple<Times,Socket>(t2, handler));
                    break;

                case "Update":
                    DateTime t3 = (DateTime)box;
                    ClientProfile cp = _clientList.First(t => t.WorkSocket == handler);
                    cp.EndDate = t3;
                    BroadcastConnectionEvent(this, string.Format("{0} requests update.", cp.ClientName));
                    _updateList.TryAdd(handler, cp);
                    break;

                case "Disconnect":
                    ClientInfo ci4 = (ClientInfo)box;
                    _clientList.Remove(_clientList.First(t => t.ClientID == ci4.ClientID));
                    BroadcastConnectionEvent(this, string.Format("{0} is disconnected", ci4.ClientName));
                    SendClientNamesToUIEvent(this, ci4);
                    break;
            }
        }
        private void ActiveSocketCleanUp(ICollection<Socket> socketList)
        {
            foreach (Socket soc in socketList)
            {
                soc.Shutdown(SocketShutdown.Both);
                soc.Disconnect(false);
                soc.Close();
                soc.Dispose();
            }
            socketList.Clear();
        }
        private void PushUpdateToClient(IDictionary<Socket, ClientProfile> cps)
        {
            if (cps.Count() > 0)
            {
                LatestDateRequestEvent(this, string.Empty);
                cps.Values.AsParallel().ForAll(
                    new Action<ClientProfile>(
                        ant =>
                        {
                            _rwLatestDateLock.EnterReadLock();
                            if (ant.EndDate.Date < _latestDate.Date)
                            {
                                Console.WriteLine(ant.EndDate.Date);
                                ant.EndDate = ant.EndDate.AddDays(1);
                                ClientRequestDataEvent(this, new Tuple<Times, Socket>(new Times(ant.EndDate, ant.EndDate, ant.IRType), ant.WorkSocket));
                            }
                            _rwLatestDateLock.ExitReadLock();
                        }));
            }
        }
        private void ClientDisconnectCleanUp(Socket workSocket)
        {
            ClientProfile disconnectClient = _clientList.First(t => t.WorkSocket == workSocket);
            _clientList.Remove(disconnectClient);
            BroadcastConnectionEvent(this, string.Format("{0} is disconnected", disconnectClient.ClientName));
            SendClientNamesToUIEvent(this, disconnectClient);
            disconnectClient.WorkSocket.Shutdown(SocketShutdown.Both);
            disconnectClient.WorkSocket.Disconnect(false);
            disconnectClient.WorkSocket.Close();
            disconnectClient.WorkSocket.Dispose();
        }


        #endregion Methods

        #region Class StateObject

        private class StateObject
        {
            public byte[] _msgBuffer;
            public int _byteRead = 0;
            private const int _oneTimeBufferSize = 16 * 1024;
            public byte[] _oneTimeBuffer = new byte[_oneTimeBufferSize];
            public int _packetType;
            
            public Socket WorkSocket { get; set; }
            public int OneTimeBufferSize { get { return _oneTimeBufferSize; } private set { } }
        }

        #endregion Class StateObject

        #region Class ClientInfo

        //private class ClientInfo
        //{
        //    public ClientInfo(string IRType)
        //    {
        //        _clientName = System.IO.Path.GetFileNameWithoutExtension(System.IO.Path.GetRandomFileName());
        //        _IRType = IRType;
        //    }

        //    private string _IRType;
        //    private string _clientName;
        //    private Guid _clientID = Guid.NewGuid();
        //    private bool _isConnected = false;

        //    public string IRType { get { return _IRType; } private set { } }
        //    public string ClientName { get { return _clientName; } private set { } }
        //    public Guid ClientID { get { return _clientID; } private set { } }
        //    public bool IsConnected { get { return _isConnected; } set { _isConnected = value; } }
        //}

        #endregion Class ClientInfo

        #region Class RequestType

        internal static class RequestType
        {
            public static string Echo = "Echo".ToUpper();
            public static string DataBulk = "DataBulk".ToUpper();
            public static string Update = "Update".ToUpper();
        }

        #endregion Class RequestType

        private class ClientProfile : ClientInfo
        {
            public ClientProfile(ClientInfo ci, Socket workSocket) : base(ci)
            {
                WorkSocket = workSocket;
            }
            public Socket WorkSocket { get; set; }
            public DateTime EndDate { get; set; }
        }

        #region Fields


        private Socket _RootSocket;
        private IPEndPoint _IPEndPoint;
        private int _bufferSize = 8 * 1024;
        private int _IPPort = 1203;
        private IPAddress _IPAddress = IPAddress.Any;

        private static int _typeByteSize = 4;
        private static int _msgLenByteSize = 4;
        private static int _totalHeaderSize = _typeByteSize + _msgLenByteSize;


        private ManualResetEventSlim _acceptSignal = new ManualResetEventSlim(false);
        private CancellationTokenSource _cts;

        
        private List<Socket> _activeSocket = new List<Socket>();
        private BindingList<ClientProfile> _clientList = new BindingList<ClientProfile>();
        private ConcurrentDictionary<Socket, ClientProfile> _updateList = new ConcurrentDictionary<Socket,ClientProfile>();

        private ReaderWriterLockSlim _rwLatestDateLock = new ReaderWriterLockSlim();
        private DateTime _latestDate = new DateTime(1,1,1);

        private static object locker=  new object();

        private enum MessageType
        { ClientInfo = 1, BulkData = 2, Update = 3, Disconnect = 4, DataTable = 5, Error = 404 };

        #endregion Fields
    }
}
