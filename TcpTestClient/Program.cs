using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace TcpTestClient
{
    class Program
    {
        static void Main(string[] args)
        {
            IPAddress[] addresses = Dns.GetHostAddresses("38.98.173.2");
            //IPAddress[] addresses = Dns.GetHostAddresses("127.0.0.1");
            IPEndPoint _ipEndpoint = new IPEndPoint(addresses[0], 58642);

            IPEndPoint ipep = new IPEndPoint(IPAddress.Parse("38.98.173.2"), 58642);

            Socket _socket = new Socket(_ipEndpoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            IAsyncResult result = _socket.BeginConnect(_ipEndpoint, null, null);
            bool success = result.AsyncWaitHandle.WaitOne(2000, true);
            if (!success)
            {
                _socket.Close();
                Console.WriteLine("failed to connect");
                //throw new ApplicationException("Timeout trying to connect to server!");
            }
            if (_socket.Connected)
            {
                Console.WriteLine("connected!");
                //send a byte to let the server know that this is the data loader program communicating with it
                byte[] clientTypeByte = new byte[1024];
                for(int i=0;i<1024;i++)
                    clientTypeByte[i] = (byte)(i%256);
                _socket.Send(clientTypeByte);
                byte[] dataFromServer = new byte[1024];
                int bytesReceived = _socket.Receive(dataFromServer);
                _socket.Close();
            }

            Console.ReadLine();
        }
    }
}
