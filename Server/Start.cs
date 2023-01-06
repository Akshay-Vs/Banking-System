using Newtonsoft.Json;
using Global.TCPConnection;
using Global.IO;
using System;

namespace ConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            // Set the IP address and port number for the server
            string ipAddress = "127.0.0.1";
            int port = 8080;
            
            TCPServer server = new TCPServer(ipAddress.ToString(), port);
            IO.WriteLine($"Listening for clients on  {ipAddress}, {port}");

            while (true)
            {
                //Start Listening
                string log = server.StartListen();
                IO.WriteLine(log);

                var request = JsonConvert.DeserializeObject<dynamic>(server.Read());
                string response = String.Empty;

                // Logics
                if (request.id == "login")
                {
                    response = $"Welcome {request.userName}";
                }
                else if (request.id == "close") break;

                server.Write(response);

                //Stop Listening
                log = server.StopListen();
                IO.WriteLine(log);
            }

            //Exit Listener
            server.End();
        }
    }
}
