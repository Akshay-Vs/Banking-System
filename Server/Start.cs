using System;
using System.Diagnostics;
using Global.IO;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace Server
{
    class Program
    {

        public static void Main(string[] args)
        {
            IPAddress iPAddress = IPAddress.Parse("127.0.0.1");
            int port = 8080;

            TcpListener listner = new TcpListener(iPAddress, port);
            listner.Start();

            IO.WriteLine($"Started listening on {port} at {iPAddress}");
            while (true)
            {
                TcpClient client = listner.AcceptTcpClient();
                IO.WriteLine($"Connection Established with {client.Client.RemoteEndPoint}");

                NetworkStream stream = client.GetStream();

                byte[] incommingBytes = new byte[1024];
                int bytesReceived = stream.Read(incommingBytes, 0, incommingBytes.Length);
                string request = Encoding.UTF8.GetString(incommingBytes, 0, bytesReceived);
                byte[] buffer;

                

                //Console.WriteLine(json);

                string welcome = $"Welcome {request}";
                buffer = Encoding.UTF8.GetBytes(welcome);
                stream.Write(buffer);
                stream.Close();
                client.Close();
            }

        }


    }
}
