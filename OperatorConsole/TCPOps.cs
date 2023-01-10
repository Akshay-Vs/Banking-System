using Global.TCPConnection;
using Newtonsoft.Json;
using System.Collections.Generic;
using Global.Types;

namespace Operator
{
    class TCPOps
    {
        public static string ip;
        public static int port;
        private static TCPConnection connection;


        public static string Login(string userName, string password)
        {
            connection = new TCPClient(ip, port);
            var body = new Dictionary<string, string>
            {
                { "id", "login"},
                { "userName", userName },
                { "password", password }
            };
            var json = JsonConvert.SerializeObject(body);
            connection.Write(json);
            return connection.Read();
        }

        public static string New(string data)
        {
            connection = new TCPClient(ip, port);
            connection.Write(data);
            return connection.Read();
        }

        public static void Close()
        {
            return;
        }
    }
}
