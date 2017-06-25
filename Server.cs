using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace server
{
    class Program
    {
        static void Main(string[] args) {
            TcpListener serverSocket = new TcpListener(1255);
            serverSocket.Start();

            Console.WriteLine("Сервер запущен");

            while (true) {
                try {
                    TcpClient socket = serverSocket.AcceptTcpClient();
                    NetworkStream nwStream = socket.GetStream();
                    BinaryReader reader = new BinaryReader(nwStream);
                    BinaryWriter writer = new BinaryWriter(nwStream);

                    string filePath = reader.ReadString();

                    Console.WriteLine("Запрошена информация о файле " + filePath);
                    FileInfo file = new FileInfo(filePath);

                    string response = file.Exists ? file.FullName + ":\nДата создания: " + file.CreationTime + "\nРазмер: " + file.Length + " байт" : "Файл не найден";
                    writer.Write(response);
                    socket.Close();
                } catch(Exception e) { Console.WriteLine(e.Message); }
            }
            serverSocket.Stop();
            Console.ReadKey();
        }
    }
}
