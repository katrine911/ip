using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace client
{
    class Program
    {
        static void Main(string[] args) {
            while (true) {
                try {
                    string filePath = Console.ReadLine();
                    TcpClient socket = new TcpClient("7.31.197.143", 1255);
                    NetworkStream nwStream = socket.GetStream();
                    BinaryReader reader = new BinaryReader(nwStream);
                    BinaryWriter writer = new BinaryWriter(nwStream);
                    writer.Write(filePath);

                    Console.WriteLine(reader.ReadString() + "\n");
                    socket.Close();
                }
                catch (Exception e) { Console.WriteLine(e.Message); }
            }
            Console.ReadKey();
        }
    }
}
