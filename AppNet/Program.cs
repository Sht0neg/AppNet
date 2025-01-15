using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace AppNet
{
    internal class Server
    {
        TcpListener _listener;
        Server() {
            _listener = new TcpListener(IPAddress.Any, 5000);
        }

        void Start() {
            _listener.Start();
            Console.WriteLine("Сервер запущен на порту 5000");
            while (true)
            {
                var client = _listener.AcceptTcpClient();
                Console.WriteLine("Клиент подключился");
                Thread thread = new Thread(HandlerSocket);
                thread.Start(client);
            }
        }
        void HandlerSocket(object socket) {
            using (var client = (TcpClient)socket) { 
                var stream = client.GetStream();
                byte[] buffer = new byte[1024];
                int bytesRead;

                while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    dynamic responseData = Newtonsoft.Json.JsonConvert.DeserializeObject(message);
                    Console.WriteLine($"Сообщение получено: {responseData.messages}");
                    byte[] res = Encoding.UTF8.GetBytes("Сообщение получено");
                    stream.Write(res, 0, res.Length);
                    break;
                }
            }
        }
        static void Main(string[] args)
        {
            var server = new Server();
            server.Start();
        }
    }
}
