using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace Utility
{
    public interface IComm
    {
        void Receive(Socket workSocket);
        void ReceiveCallback(IAsyncResult ar);
        void Send(Socket workSocket, byte[] msg);
        void SendCallback(IAsyncResult ar);
    }
}
