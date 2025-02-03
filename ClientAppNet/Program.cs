using System.Net.Sockets;
using System.Text;

namespace ClientAppNet
{
    internal class Client
    {
        static void Main(string[] args)
        {
            var requestData = new
            {
                model = "openai/gpt-3.5-turbo",
                messages = "lox",
                temperature = 0.7,
                n = 1,
                max_tokens = 3000
            };
            var jsonRequest = Newtonsoft.Json.JsonConvert.SerializeObject(requestData);
            try {
                using (var client = new TcpClient("127.0.0.1", 5000)) {
                    Console.WriteLine("Соединение установлено");
                    var stream = client.GetStream();
                    int i = 0;
                    while (i < 5)
                    {
                        i++;
                        //Console.WriteLine("напишите сообщение для отправлки или exit для выхода: ");
                        //string message = Console.ReadLine();
                        //if (message.ToLower() == "exit") { break; }
                        byte[] data = Encoding.UTF8.GetBytes(jsonRequest);
                        stream.Write(data, 0, data.Length);

                        byte[] buffer = new byte[1024];
                        int countBytes = stream.Read(buffer, 0, buffer.Length);
                        var res = Encoding.UTF8.GetString(buffer);
                        Console.WriteLine($"Ответ от сервера {res}");
                        break;
                    }
                }
            }
            catch (Exception e) { Console.WriteLine($"Ошибка {e.Message}"); }
        }
    }
}
