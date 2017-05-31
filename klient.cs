using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class Udp_Client
{
    public static void Main()
    {
        byte[] data = new byte[1024];
        string input, stringData;

        //Ввод IP-адреса сервера
        Console.Write("Укажите IP-адрес сервера: ");
        string addr = Console.ReadLine();
        //Если адрес пустой - принимаем за локалхост
        if (addr == "") addr = "127.0.0.1";

        //Создание нового UDP-клиента и установка удаленного узла по умолчанию
        UdpClient server = new UdpClient(addr, 9050);
        //создаем конечную точку по адресу сокета. Т.е. будем "слушать" порты 
        //и контролировать все сетевые интерфейсы
        IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);

        //Отправляем серверу сообщение о подключении
        string welcome = "Клиент успешно подключился!";
        data = Encoding.UTF8.GetBytes(welcome);
        server.Send(data, data.Length);

        //От сервера придут данные, типа "Подключение успешно"
        data = server.Receive(ref sender);

        //Обеспечиваем их отображение на экране
        Console.Write("Сообщение принято от {0}:", sender.ToString());
        stringData = Encoding.UTF8.GetString(data, 0, data.Length);
        Console.WriteLine(stringData);

        //Бесконечный цикл.
        while (true)
        {
            //Объявляем новый массив под пришедшие данные.
            data = new byte[1024];
            //Ожидаем ввода с клавиатуры (строки) и заносим в переменную input
            Console.Write("\r\n>");
            input = Console.ReadLine();

            //Перевод отсылаемой строки в байты
            data = Encoding.UTF8.GetBytes(input);
            //Отсылаем серверу строку (переведенную в байты)
            server.Send(data, data.Length);

            //Если пришла команла exit - выходим из цикла (далее - закрываем сокет и т.д.)
            if (input == "exit") break;

            //Получение данных...
            data = server.Receive(ref sender);
            //Перевод принятых байтов в строку
            stringData = Encoding.UTF8.GetString(data, 0, data.Length);
            Console.Write("<");
            //Отображение на экране принятой строки (размер файла)
            Console.WriteLine(stringData);
        }
        //После выхода из цикла...
        Console.WriteLine("Остановка клиента...");
        //Закрываем UDP-соединение
        server.Close();
    }
}