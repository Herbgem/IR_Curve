using System;
using System.Net.Sockets;

namespace Utility
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class ClientInfo
    {
        public ClientInfo(string IRType)
        {
            _clientName = System.IO.Path.GetFileNameWithoutExtension(System.IO.Path.GetRandomFileName());
            _IRType = IRType;
        }

        public ClientInfo(ClientInfo ci)
        {
            _IRType = ci.IRType;
            _clientName = ci.ClientName;
            _clientID = ci.ClientID;
        }

        private string _IRType;
        private string _clientName;
        private Guid _clientID = Guid.NewGuid();
        private bool _isConnected = false;

        public string IRType { get { return _IRType; } private set { } }
        public string ClientName { get { return _clientName; } private set { } }
        
        public Guid ClientID { get { return _clientID; } private set { } }
        public bool IsConnected { get { return _isConnected; } set { _isConnected = value; } }
    }
}
