using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.IO;
 
public class UDP_Server
{
    public static void Main()
    {
        int recv;
        byte[] data = new byte[1024];
        //Сетевая конечная точка в виде адреса и номера порта
        IPEndPoint ipep = new IPEndPoint(IPAddress.Any, 9050);
        //Создание нового сокета (схема адресации, тип сокета, протокол)
        //параметр AddressFamily задает схему адресации. В нашем случае это адреса IPv4
        //параметр SocketType указывает, какой тип сокета применяется. 
        //Datagram - поддерживает двусторонний поток данных. Не гарантируется, что этот поток будет последовательным, 
        //надежным, и что данные не будут дублироваться. Важной характеристикой данного сокета является то,
        //что границы записи данных предопределены.
        //последний параметр, ProtocolType, задает тип протокола
        Socket SrvSock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        //Связываем новый сокет с локальной конечной точкой
        SrvSock.Bind(ipep);
        Console.WriteLine("Ожидаем соединения с клиентом...");
        //создаем конечную точку по адресу сокета. Т.е. будем "слушать" порты 
        //и контролировать все сетевые интерфейсы
        IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
        //Получаем конечную удаленную точку
        EndPoint Remote = (EndPoint)(sender);

        //Получаем сообщение от клиента (типа, "Клиент успешно подключился!")
        recv = SrvSock.ReceiveFrom(data, ref Remote);

        //Отображаем сообщение о успешно подключенном клиенте
        Console.Write("Сообщение получено от {0}:", Remote.ToString());
        Console.WriteLine(Encoding.UTF8.GetString(data, 0, recv));

        //Отправляем клиенту сообщение об успешном подключении
        string welcome = "Подключение к серверу успешно!";
        data = Encoding.UTF8.GetBytes(welcome);
        SrvSock.SendTo(data, data.Length, SocketFlags.None, Remote);

        //Бесконечный цикл.
        while (true)
        {
            //Объявляем новый массив под пришедшие данные.
            data = new byte[1024];
            //Получение данных...
            recv = SrvSock.ReceiveFrom(data, ref Remote);
            //Перевод принятых байтов в строку
            string str = Encoding.UTF8.GetString(data, 0, recv);

            //Если пришла команда выхода
            //Завершаем работу сокета и программы...
            if (str == "exit") break;

            Console.WriteLine("Получили данные: " + str);
            //Записываем в переменную size новый размер файла (после процедуры prntofile)
            double size = prntofile(str);
            //Отображаем форматированное сообщение
            string input = "Строка успешно записана!\nНовый размер файла: " + size + " байт.";
            Console.WriteLine(">" + input + "\n");

            //Перевод отсылаемой строки в байты
            data = Encoding.UTF8.GetBytes(input);
            //Отсылаем серверу строку (переведенную в байты)
            SrvSock.SendTo(data, data.Length, SocketFlags.None, Remote);
        }
    }


    //Процедура записи в файл и вычисления размера
    public static double prntofile(string str)
    {
        //Определяем имя файла
        string fil = "Mystr_Srv.txt";
        StreamWriter sw;
        FileInfo fi = new FileInfo(fil);
        //Открываем файл на appendtext
        sw = fi.AppendText();
        //Записываем в файл строку
        sw.WriteLine(str);
        //Закрываем файл.
        sw.Close();
        //Вычисляем размер файла (в байтах).
        double size = fi.Length;
        //Возвращаем размер файла.
        return size;
    }
}