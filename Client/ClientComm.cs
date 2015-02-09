using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Data;
using System.Windows.Forms;
using System.Runtime.Serialization.Formatters.Binary;
using Utility;

namespace Client
{
    class ClientComm : IComm
    {
        #region Event
        /// <summary>
        /// Send a Value which indicates whether a connection is established 
        /// </summary>
        public event EventHandler<bool> ConnectionEstablishedEvent;
        /// <summary>
        /// Pass a Socket object to UI when connected
        /// </summary>
        public event EventHandler<Socket> SendConnectionInfoEvent;
        public event EventHandler<DataTable> SendDataTableToUIEvent;
        public event EventHandler<string> ErrorReportToUIEvent;

        #endregion Event

        #region Event Handler

        public void ConnectRequestEventHandler(object sender, bool makeConnect)
        {
            if (makeConnect)
                Connect();
            else
                Disconnect();
        }
        public void DownloadRequestEventHandler(object sender, Times timeRange)
        {
            SendWrapper<Times>(_RootSocket, MessageType.BulkData, timeRange);
        }
        public void UpdateRequestEventHandler(object sender, string placeHolder)
        {
            SendWrapper<DateTime>(_RootSocket, MessageType.Update, _dt.AsEnumerable().Max(r => r.Field<DateTime>("date")));
        }

        #endregion Event Handler

        #region Methods

        public ClientComm(string IRType)
        {
            _IPEndPoint = new IPEndPoint(_IPAddress, _IPPort);
            _clientInfo = new ClientInfo(IRType);
        }

        public ClientComm(IPAddress IPAddress, int IPPort, string IRType)
        {
            this._IPAddress = IPAddress;
            this._IPPort = IPPort;
            _IPEndPoint = new IPEndPoint(_IPAddress, _IPPort);
            _clientInfo = new ClientInfo(IRType);
        }

        public void Connect()
        {
            _RootSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //_RootSocket.Bind(new IPEndPoint(IPAddress.Any, 0));
            _RootSocket.UseOnlyOverlappedIO = true;
            //_RootSocket.ExclusiveAddressUse
            _RootSocket.BeginConnect(_IPEndPoint, new AsyncCallback(ConnectCallback), _RootSocket);
        }
        public void Disconnect()
        {
            SendWrapper<ClientInfo>(_RootSocket, MessageType.Disconnect, _clientInfo);
            _RootSocket.Shutdown(SocketShutdown.Both);
            _RootSocket.Disconnect(false);
            _RootSocket.Close();
            _RootSocket.Dispose();
        }
        private void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                Socket rootSocket = (Socket)ar.AsyncState;

                rootSocket.EndConnect(ar);
                _clientInfo.IsConnected = true;
                //Send the client info to the server
                SendWrapper<ClientInfo>(rootSocket, MessageType.ClientInfo, _clientInfo);
                //Communicate with UI
                ConnectionEstablishedEvent(this, true);
                SendConnectionInfoEvent(this, rootSocket);
                //Begin receive loop


                while (true)
                {
                    if (_msgOverflowBuffer.Length > 0)
                    {
                        Console.WriteLine("length of msbuffer: {0}", _msgOverflowBuffer.Length);
                        _receiveDone.Reset();
                        Receive(_msgOverflowBuffer, _RootSocket);
                        _receiveDone.Wait();
                    }
                    else if (_RootSocket.Available > 0)
                    {
                        _receiveDone.Reset();
                        Receive(_RootSocket);
                        _receiveDone.Wait();
                    }
                }
                
            }
            catch (ObjectDisposedException ex)
            {
                ConnectionEstablishedEvent(this, false);
            }
            catch (SocketException ex)
            {
                if (ex.SocketErrorCode == SocketError.ConnectionRefused)
                {
                    ErrorReportToUIEvent(this, "Server is closed");
                }
            }
        }

        public void Receive(Socket workSocket)
        {
            StateObject state = new StateObject();
            state.WorkSocket = workSocket;
            workSocket.BeginReceive(state._oneTimeBuffer, 0, state.OneTimeBufferSize, 0, new AsyncCallback(ReceiveCallback), state);
        }

        public void Receive(MemoryStream ms, Socket workSocket)
        {
            int msgMSLen = (int)ms.Length;

            StateObject state = new StateObject();
            state.WorkSocket = workSocket;
            ms.Seek(0, SeekOrigin.Begin);
            ms.Read(state._oneTimeBuffer, 0, msgMSLen);
            ms.SetLength(0);
            //ms.Seek(0, SeekOrigin.Begin);
            
            state._byteRead = msgMSLen;

            ReceiveCallback(state);
        }

        public void ReceiveCallback(IAsyncResult ar)
        {
            StateObject state = (StateObject)ar.AsyncState;
            Socket handler = state.WorkSocket;

            int byteRead = handler.EndReceive(ar);
            Console.WriteLine("inside client receive callback: {0}", byteRead);

            if (state._msgBuffer == null)
            {
                state._byteRead += byteRead;
                if (state._byteRead >= _totalHeaderSize)
                {
                    byte[] packetTypeArr = new byte[_typeByteSize];
                    byte[] msgLenArr = new byte[_msgLenByteSize];

                    Buffer.BlockCopy(state._oneTimeBuffer, 0, packetTypeArr, 0, _typeByteSize);

                    Buffer.BlockCopy(state._oneTimeBuffer, _typeByteSize, msgLenArr, 0, _msgLenByteSize);

                    int msgLength = BitConverter.ToInt32(msgLenArr, 0);
                    int packetType = BitConverter.ToInt32(packetTypeArr, 0);

                    state._packetType = packetType;
                    state._msgBuffer = new byte[msgLength];


                    if (msgLength == (state._byteRead - _totalHeaderSize))
                    {
                        Buffer.BlockCopy(state._oneTimeBuffer, _totalHeaderSize, state._msgBuffer, 0, msgLength);
                        Task.Run(() => ActionFilter(state._packetType, state._msgBuffer, handler));
                    }
                    else if (msgLength > (state._byteRead - _totalHeaderSize))
                    {
                        Buffer.BlockCopy(state._oneTimeBuffer, _totalHeaderSize, state._msgBuffer, 0, state._byteRead - _totalHeaderSize);
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
                        // more work here
                        Buffer.BlockCopy(state._oneTimeBuffer, _totalHeaderSize, state._msgBuffer, 0, msgLength);
                        Buffer.BlockCopy(state._oneTimeBuffer, _totalHeaderSize + msgLength, _oneTimeOverflowBuffer, 0, state._byteRead - _totalHeaderSize - msgLength);
                        _msgOverflowBuffer.Write(_oneTimeOverflowBuffer, 0, state._byteRead - _totalHeaderSize - msgLength);
                        Array.Clear(_oneTimeOverflowBuffer, 0, _oneTimeOverflowBuffer.Length);
                        Task.Run(() => ActionFilter(state._packetType, state._msgBuffer, handler));
                    }
                }
                else
                {
                    handler.BeginReceive(state._oneTimeBuffer, state._byteRead, state.OneTimeBufferSize - state._byteRead, 0, new AsyncCallback(ReceiveCallback), state);
                }
            }
            else
            {
                if (state._msgBuffer.Length == state._byteRead + byteRead - _totalHeaderSize)
                {
                    Buffer.BlockCopy(state._oneTimeBuffer, 0, state._msgBuffer, state._byteRead - _totalHeaderSize, byteRead);

                    Task.Run(() => ActionFilter(state._packetType, state._msgBuffer, handler));
                }
                else if (state._msgBuffer.Length > state._byteRead + byteRead - _totalHeaderSize)
                {
                    Buffer.BlockCopy(state._oneTimeBuffer, 0, state._msgBuffer, state._byteRead - _totalHeaderSize, byteRead);
                    state._byteRead += byteRead;
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
                    //More work here
                    Buffer.BlockCopy(state._oneTimeBuffer, 0, state._msgBuffer, state._byteRead - _totalHeaderSize, state._msgBuffer.Length - state._byteRead + _totalHeaderSize);
                    Buffer.BlockCopy(state._oneTimeBuffer, state._msgBuffer.Length - state._byteRead + _totalHeaderSize, _oneTimeOverflowBuffer, 0, byteRead - state._msgBuffer.Length + state._byteRead - _totalHeaderSize);
                    _msgOverflowBuffer.Write(_oneTimeOverflowBuffer, 0, byteRead - state._msgBuffer.Length + state._byteRead - _totalHeaderSize);
                    Array.Clear(_oneTimeOverflowBuffer, 0, _oneTimeOverflowBuffer.Length);
                    Task.Run(() => ActionFilter(state._packetType, state._msgBuffer, handler));
                }
            }
        }
        public void ReceiveCallback(StateObject so)
        {
            StateObject state = so;
            Socket handler = state.WorkSocket;

            if (state._byteRead >= _totalHeaderSize)
            {
                byte[] packetTypeArr = new byte[_typeByteSize];
                byte[] msgLenArr = new byte[_msgLenByteSize];

                Buffer.BlockCopy(state._oneTimeBuffer, 0, packetTypeArr, 0, _typeByteSize);

                Buffer.BlockCopy(state._oneTimeBuffer, _typeByteSize, msgLenArr, 0, _msgLenByteSize);

                int msgLength = BitConverter.ToInt32(msgLenArr, 0);
                int packetType = BitConverter.ToInt32(packetTypeArr, 0);

                state._packetType = packetType;
                state._msgBuffer = new byte[msgLength];


                if (msgLength == (state._byteRead - _totalHeaderSize))
                {
                    Buffer.BlockCopy(state._oneTimeBuffer, _totalHeaderSize, state._msgBuffer, 0, msgLength);
                    Task.Run(() => ActionFilter(state._packetType, state._msgBuffer, handler));
                }
                else if (msgLength > (state._byteRead - _totalHeaderSize))
                {
                    Buffer.BlockCopy(state._oneTimeBuffer, _totalHeaderSize, state._msgBuffer, 0, state._byteRead - _totalHeaderSize);
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
                    // more work here
                    Buffer.BlockCopy(state._oneTimeBuffer, _totalHeaderSize, state._msgBuffer, 0, msgLength);
                    Buffer.BlockCopy(state._oneTimeBuffer, _totalHeaderSize + msgLength, _oneTimeOverflowBuffer, 0, state._byteRead - _totalHeaderSize - msgLength);
                    _msgOverflowBuffer.Write(_oneTimeOverflowBuffer, 0, state._byteRead - _totalHeaderSize - msgLength);
                    Array.Clear(_oneTimeOverflowBuffer, 0, _oneTimeOverflowBuffer.Length);
                    Task.Run(() => ActionFilter(state._packetType, state._msgBuffer, handler));
                }
            }
            else
            {
                handler.BeginReceive(state._oneTimeBuffer, state._byteRead, state.OneTimeBufferSize - state._byteRead, 0, new AsyncCallback(ReceiveCallback), state);
            }
        }

        public void Send(Socket workSocket, byte[] packet)
        {
            if (_clientInfo.IsConnected)
            {
                try
                {
                    workSocket.BeginSend(packet, 0, packet.Length, 0, new AsyncCallback(SendCallback), workSocket);
                }
                catch (ObjectDisposedException ex)
                {
                    ConnectionEstablishedEvent(this, false);
                }
                catch (SocketException ex)
                {
                    if (ex.SocketErrorCode == SocketError.ConnectionAborted)
                    {
                        ConnectionEstablishedEvent(this, false);
                    }
                    else
                        throw;
                }
            }
        }
        public void SendCallback(IAsyncResult ar)
        {
            try
            {
                Socket workSocket = (Socket)ar.AsyncState;

                int byteSent = workSocket.EndSend(ar);
                if (byteSent == 0)
                {
                    ConnectionEstablishedEvent(this, false);
                }
            }
            catch (ObjectDisposedException ex)
            {
                MessageBox.Show("inside Sendcallback" + ex.Message + "\r\n" + ex.StackTrace);
            }
        }
        /// <summary>
        /// Concatenate message type, length of message and the body of message into one byte array
        /// </summary>
        /// <remarks> If the recipient is bigendian, order of every array needs to be reversed</remarks>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
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
            _receiveDone.Set();

            if (packetType == 0)
            {
                return;
            }
            string key = ((MessageType)packetType).ToString();

            object box;

            BinaryFormatter bformatter = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream(msg))
            {
                box = bformatter.Deserialize(ms);
            }

            switch (key)
            {
                case "ClientInfo":
                    break;

                case "BulkData":
                    break;

                case "Disconnect":
                    break;
                case "DataTable":
                    _rwDataTableLock.TryEnterWriteLock(-1);
                    _dt.Merge((DataTable)box, false, MissingSchemaAction.AddWithKey);
                    Console.WriteLine("rows: {0}", _dt.Rows.Count);
                    SendDataTableToUIEvent(this, _dt);
                    _rwDataTableLock.ExitWriteLock();
                    break;
                case "Error":
                    string error = (string)box;
                    ErrorReportToUIEvent(this, error);
                    break;
            }
        }
        private void RootSocketCleanUp()
        {
            ConnectionEstablishedEvent(this, false);
            _RootSocket.Shutdown(SocketShutdown.Both);
            _RootSocket.Disconnect(false);
            _RootSocket.Close();
            _RootSocket.Dispose();

            _msgOverflowBuffer.Close();
            _msgOverflowBuffer.Dispose();
        }

        #endregion Methods

        public class StateObject
        {
            public byte[] _msgBuffer;
            public int _byteRead = 0;
            private const int _oneTimeBufferSize = 400 * 1024;
            public byte[] _oneTimeBuffer = new byte[_oneTimeBufferSize];
            public int _packetType;

            public Socket WorkSocket { get; set; }
            public int OneTimeBufferSize { get { return _oneTimeBufferSize; } private set { } }
        }

        #region Fields

        private ClientInfo _clientInfo;
        private DataTable _dt = new DataTable();
        private ManualResetEventSlim _receiveDone = new ManualResetEventSlim(false);
        private ReaderWriterLockSlim _rwDataTableLock = new ReaderWriterLockSlim();

        private Socket _RootSocket;
        private IPEndPoint _IPEndPoint;
        private int _IPPort = 1203;
        private IPAddress _IPAddress = IPAddress.Loopback;

        private static int _typeByteSize = 4;
        private static int _msgLenByteSize = 4;
        private static int _totalHeaderSize = _typeByteSize + _msgLenByteSize;
        private byte[] _oneTimeOverflowBuffer = new byte[5* 1024 * 1024];
        private MemoryStream _msgOverflowBuffer = new MemoryStream();

        private enum MessageType
        { ClientInfo = 1, BulkData = 2, Update = 3, Disconnect = 4, DataTable = 5, Error = 404 };

        #endregion Fields
    }
}
