using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

namespace Server
{
    class SocketServer
    {
        public SocketServer() 
        {
            _hostIPAddress = IPAddress.Parse("127.0.0.1");
            _portNumber = 1203;
            _backlog = 100;
        }

        public SocketServer(IPAddress hostIPAddress, int portNumber, int backlog)
        {
            _hostIPAddress = hostIPAddress;
            _portNumber = portNumber;
            _backlog = backlog;
        }

        public void StartListening()
        {
            _server.Bind(new IPEndPoint(_hostIPAddress, _portNumber));
            _server.Listen(_backlog);

            while(!_close)
            {
                _clientAccepted.Reset();
                _server.BeginAccept(new AsyncCallback(AcceptCallBack), _server);
                _clientAccepted.Wait();
            }
        }

        private void AcceptCallBack(IAsyncResult iar)
        {
            _clientAccepted.Set();

            Socket server = (Socket)iar;
            Socket handler = server.EndAccept(iar);
        }

        private IPAddress _hostIPAddress;
        private int _portNumber;
        private int _backlog;

        private bool _close = false;
        private Socket _server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private ManualResetEventSlim _clientAccepted = new ManualResetEventSlim(false);

        private class StateObject
        {
            internal Socket _workSocket;
            internal static int _bufferSize = 65535;
            internal byte[] buffer = new byte[_bufferSize];
        }
    }
}
