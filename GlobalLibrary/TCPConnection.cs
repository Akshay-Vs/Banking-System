using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace Global.TCPConnection
{
    public class TCPConnection
    {
        public readonly IPAddress ipAddress;
        public readonly int port;
        internal TcpClient Client;
        internal NetworkStream Stream;
        internal TcpListener Listener;

        # region Getters and setters

        public TcpListener listener { 
            get { return Listener; }  
        }

        public TcpClient client
        {
            get { return Client; }
        }

        public NetworkStream stream { 
            get { return Stream; }
        }

        #endregion

        public TCPConnection(string ip, int _port)
        {
            ipAddress = IPAddress.Parse(ip);
            port = _port;
        }

        public void Write(string message)
        {
            byte[] buffer = Encode(message);
            Stream.Write(buffer);
        }

        public string Read() 
        {
            byte[] buffer = new byte[1024];
            int bytesReceived = Stream.Read(buffer, 0, buffer.Length);
            string response = Encode(buffer, 0, bytesReceived);
            return response; 
        }

        public void closeStream() => Stream.Close();
        public void closeClient() => Client.Close();

        private static byte[] Encode(string message) => Encoding.UTF8.GetBytes(message);
        private static string Encode(byte[] buffer, int index, int count) => Encoding.UTF8.GetString(buffer, index, count);
    }

    public class TCPServer : TCPConnection
    {
        private EndPoint endPoint;
        public TCPServer(string ip, int port) : base(ip, port)
        {
            //Initialize network stream for read/write
            Listener = new TcpListener(ipAddress, port);
            Listener.Start();
            Stream = stream;
        }

        public string StartListen()
        {
            Client = listener.AcceptTcpClient();
            Stream = Client.GetStream();
            endPoint = Client.Client.RemoteEndPoint;
            return $"Connection Established with {endPoint}";
        }

        public string StopListen()
        {
            Stream.Close();
            Client.Close();
            return $"Connection Terminated with {endPoint}";
        }

        public void End() => Listener.Stop();
    }

    public class TCPClient: TCPConnection
    {
        public TCPClient(string ip, int port) : base(ip, port)
        {
                // Initialize TCP Client connection
                Client = new TcpClient();
                Client.Connect(ipAddress, port);
                Stream = Client.GetStream();
        }
    }
    
}
