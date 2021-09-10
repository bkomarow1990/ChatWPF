using ChatLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace ChatWPFServer
{
    class Program
    {
        static IPAddress iPAddress = IPAddress.Parse("127.0.0.1");
        static int port = 7777;
        static Dictionary<string, TcpClient> clientsEndPoints = new Dictionary<string, TcpClient>();
        static void Join(string username, TcpClient client) {
            clientsEndPoints.Add(username, client);
        }
        static void Leave(string username)
        {
            if (clientsEndPoints.ContainsKey(username))
            {
                Console.WriteLine($"Incorrect username for leave {username}");
                return;
            }
            clientsEndPoints.Remove(username);
        }
        static void Send(string userName, string message) {
            MessageInfo messageInfo = new MessageInfo { Message = message, Time = DateTime.Now, UserName = userName };
            foreach (TcpClient client in clientsEndPoints)
            {
                using (NetworkStream stream = client.GetStream())
                {
                    // серіалізуємо об'єкт класа
                    // та відправляємо його на сервер
                    serializer.Serialize(stream, info);
                }
            }
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Sended!");
        }
        static void Main(string[] args)
        {
            IPEndPoint localEndPoint = new IPEndPoint(iPAddress, port);
            IPEndPoint clientSEndPoint = new IPEndPoint(IPAddress.Any, 0);
            NetworkStream stream = null;
            
            // створюємо екземпляр сервера вказуючи кінцеву точку для приєднання
            TcpListener server = new TcpListener(localEndPoint);
            // запускаємо прослуховування вказаної кінцевої точки
            server.Start(10);

            while (true)
            {
                try
                {
                    Console.WriteLine("\tWaiting for Messages...");
                    // отримуємо зв'язок з клієнтом
                    TcpClient cl = server.AcceptTcpClient();
                    BinaryFormatter serializer = new BinaryFormatter();
                    var info = (MessageTransferInfo)serializer.Deserialize(cl.GetStream());
                    clientsEndPoints.Add(info.Username,cl); // waiting...

                    // отримуємо дані від клієнта
                    // та десеріалізуємо об'єкт
                    
                    switch (info.Action)
                    {
                        case Actions.JOIN:
                            Join(info.Username, cl);
                            break;
                        case Actions.LEAVE:
                            Leave(info.Username);
                            break;
                        case Actions.SEND:
                            Send(info.Username, info.Message);
                            break;
                        default:
                            break;
                    }
                    //Console.ForegroundColor = ConsoleColor.Cyan;
                    //Console.WriteLine($"Got a file: {info.Name} from {client.Client.RemoteEndPoint}");
                    //Console.ForegroundColor = ConsoleColor.DarkGreen;
                    //Console.WriteLine("Saving...");

                    // зберігаємо отриманий файл на сервері
                   
                    
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    Console.ResetColor();
                }
            }

            // зупиняємо роботу сервера
            server.Stop();
        }
    }
}
