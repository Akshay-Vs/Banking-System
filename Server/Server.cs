using Newtonsoft.Json;
using Global.TCPConnection;
using Global.IO;
using Global.Database;
using Global.Types;
using System;

namespace ConsoleApplication
{
    class Server
    {
        static void Main(string[] args)
        {
            string ipAddress = "127.0.0.1";
            string connectionString = "mongodb://localhost:27017";
            int port = 8080;
            
            TCPServer server = new TCPServer(ipAddress.ToString(), port);
            IO.WriteLine($"Listening for clients on  {ipAddress}, {port}");

            //instantiating database
            DataBase dataBase = new DataBase(connectionString, "BankingSystem", "Users");
            IO.WriteLine($"Connected to database {dataBase}");

            while (true)
            {
                try {
                    //Start Listening
                    string log = server.StartListen();
                    IO.WriteLine(log);

                    var request = JsonConvert.DeserializeObject<dynamic>(server.Read());
                    string response = string.Empty;

                    // Logics
                    if (request.id == "login")
                    {
                        response = $"Welcome {request.userName}";
                    }
                    else if (request.id == "new")
                    {
                        Console.WriteLine(request);
                        string requestJson = JsonConvert.SerializeObject(request);
                        dataBase.Insert(requestJson);
                    }
                    else if (request.id == "close") break;

                    server.Write(response);

                    //Stop Listening
                    log = server.StopListen();
                    IO.WriteLine(log);
                }
                catch (Exception e) 
                {
                    IO.WriteError("Operation Failed due to: ", e);
                    server.Write("Operation Failed");
                }
            }

            //Exit Listener
            server.End();
        }
    }
}
