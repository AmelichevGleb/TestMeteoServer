using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.IO;

namespace ConsoleApp1
{
    internal class Program
    {
        private static PortScanner portScanner = new PortScanner();
        private static ByteTest _bT = new ByteTest();


        private static bool exit = true;

        static void Main(string[] args)
        {

            while (exit)
            {
                Console.WriteLine("1 - тесты байтов");
                Console.WriteLine("2 - тесты портов");
                Console.WriteLine("3 - тестовая метеостанция");
                Console.WriteLine("4 - неведомый сервер      \n Принимает данные с клиента (работает 127.0.0.1   4444) ");
                Console.WriteLine("5 - Клиент-байт    \n    Отправляет на 127.0.0.1 5555 рандомные байты (сигнал ЧС)");
                Console.WriteLine("6 - отправка сообщения по TCP порта и ip");
                Console.WriteLine("7 - выход");
                Console.Write("ввод символа: ");
                int choice = Convert.ToInt32(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        //тестовая проверка заполения массива байт и сравнение с оригиналом
                        Console.Clear();
                        Console.WriteLine("тест байт");
                        Console.WriteLine(_bT.TestCompletion());
                        Console.WriteLine("Нажмите любую кнопку...");
                        Console.ReadLine();
                        Console.Clear();
                        break;
                    case 2:
                        //проверка портов по заданному IP и запись результатов в файл
                        Console.Clear();
                        Console.WriteLine("тест портов");
                        Console.WriteLine("Введите IP устройства для проверки");
                        string IP = Console.ReadLine();
                        portScanner.Scanner(IP);
                        break;
                    case 3:
                        //тестовый сервер кидает данные (как будто метеостанция) 
                        Console.Clear();
                        Console.WriteLine("Тестовый сервер: ");
                        IPHostEntry ipHost = Dns.GetHostEntry("127.0.0.1");
                        IPAddress ipAddr = ipHost.AddressList[0];
                        IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, 2222);
                        byte[] Message = new byte[] { 0x01, 0x03, 0xB4, 0x82, 0x80, 0x00, 0x00, 0x00, 0x00, 0x09, 0xA1, 0x27, 0x1D, 0x00, 0x19, 0x00, 0x00, 0x01, 0x01, 0x00, 0x1D, 0x00, 0x00, 0x00, 0x92 };
                        Socket sListener = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                        try
                        {
                            sListener.Bind(ipEndPoint);
                            sListener.Listen(10);
                            while (true)
                            {
                                Console.WriteLine("Ожидаем соединение через порт {0}", ipEndPoint);
                                Socket handler = sListener.Accept();
                                string data = null;
                                byte[] bytes = new byte[1024];
                                int bytesRec = handler.Receive(bytes);
                                data += Encoding.UTF8.GetString(bytes, 0, bytesRec);
                                Console.WriteLine("РЕЗУЛЬТАТ !!!!   Array Output, Size: {0} Data: " + BitConverter.ToString(bytes), bytes.Length);
                                string reply = "Спасибо за запрос в " + data.Length.ToString()
                                        + " символов";
                                byte[] msg = Encoding.UTF8.GetBytes(reply);
                                handler.Send(Message);
                                if (data.IndexOf("<TheEnd>") > -1)
                                {
                                    Console.WriteLine("Сервер завершил соединение с клиентом.");
                                    break;
                                }
                                handler.Shutdown(SocketShutdown.Both);
                                handler.Close();
                            }
                        }

                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.ToString());
                        }
                        finally
                        {
                            Console.ReadLine();
                        }
                        break;
                    case 4:
                        //Teстовый сервер, выводит информацию о том, что приходит и с какого IP адреса 
                        Console.Clear();
                        Console.WriteLine("Тестовый сервер: ");
                        IPHostEntry ipHost1 = Dns.GetHostEntry("127.0.0.1");
                        IPAddress ipAddr1 = ipHost1.AddressList[0];
                        byte[] Message1 = new byte[] { 0x01, 0x03, 0xB4, 0x82, 0x80, 0x00, 0x00, 0x00, 0x00, 0x09, 0xA1, 0x27, 0x1D, 0x00, 0x19, 0x00, 0x00, 0x01, 0x01, 0x00, 0x1D, 0x00, 0x00, 0x00, 0x92 };
                        Socket sListener1 = new Socket(ipAddr1.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                        bool flag = false;
                        try
                        {
                            TcpListener server = new TcpListener(IPAddress.Any, 4444);
                            server.Start();  // запускаем сервер
                            while (true)
                            {
                                TcpClient client = server.AcceptTcpClient();  // ожидаем подключение клиента
                                NetworkStream ns = client.GetStream(); // для получения и отправки сообщений
                               
                                while (client.Connected)  // пока клиент подключен, ждем приходящие сообщения
                                {
                                    if (client.Connected == true && flag == false)
                                    {
                                        Console.WriteLine("Клиент подкл");
                                        flag = true;
                                        Console.WriteLine(ipAddr1);
                                    }
                                  
                                    byte[] msg = new byte[8096];     // готовим место для принятия сообщения
                                    int count = ns.Read(msg, 0, msg.Length);   // читаем сообщение от клиента
                                    if (count != 0)
                                    {
                                        Console.WriteLine(Encoding.Default.GetString(msg, 0, count)); // выводим на экран полученное сообщение в виде строки
                                        byte[] hello = new byte[100];   // любое сообщение должно быть сериализовано
                                        hello = Encoding.Default.GetBytes("hello world");  // конвертируем строку в массив байт
                                        ns.Write(hello, 0, hello.Length);     // отправляем сообщение
                                    }
                                    else { 
                                        Console.WriteLine("Клиент откл");
                                        flag = false;
                                        break;
                                    }
                                    
                                }
                            }
                        }

                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.ToString());
                        }
                        finally
                        {
                            Console.ReadLine();
                        }
                        break;

                    case 5:
                        // Клиент кидает на сервер байты

                        byte[] B4c = new byte[] { 0x00, 0x02, 0x01, 0xE2 };
                        byte[] Btest = new byte[] { 0x00, 0x00, 0x01, 0xE3 };
                        byte[] Btest1 = new byte[] { 0x00, 0x00, 0x01, 0xE4 };
                        byte[] Btest2 = new byte[] { 0x00, 0x00, 0x01, 0xE5 };
                        byte[] Btest3 = new byte[] { 0x00, 0x00, 0x01, 0xE6 };
                        byte[][] lists = new byte[][] { B4c, Btest, Btest1, Btest2, Btest3 };

                        string ip = "127.0.0.1";
                        int port = 5555;
 
                        Console.Clear();
                        // Console.WriteLine("Ввести IP");
                        // string ip = Console.ReadLine();
                        Console.WriteLine("ip = {0}", ip);
                       // Console.WriteLine("Ввести Port");
                      //  string port = Console.ReadLine();
                        Console.WriteLine("port = {0}", port);
                        // Инициализация
                        TcpClient newClient = new TcpClient();
                        bool connect = false;
                        while (true)
                        {
                            try
                            {
                                newClient.Connect(ip, Convert.ToInt32(port));
                                Console.WriteLine("Мы подключены");

                                connect = true;
                                while (true)
                                {
                                    try
                                    {
                                        NetworkStream tcpStream = newClient.GetStream();
                                        Random rd = new Random();
                                        int randomIndex = rd.Next(0, lists.Length);
                                        tcpStream.Write(lists[randomIndex], 0, lists[randomIndex].Length);
                                        Console.WriteLine(BitConverter.ToString(lists[randomIndex]));
                                        Thread.Sleep(400);
                                        byte[] msg = new byte[8096];
                                        int count = tcpStream.Read(msg, 0, msg.Length);
                                        if (count == 0) { Console.WriteLine("было отключение"); }
                                    }
                                    catch (IOException e)
                                    {
                                        break;
                                    }

                                }

                            }
                            catch (ObjectDisposedException e)
                            {
                                Console.WriteLine("АААА");
                                newClient = new TcpClient();
                               
                            }
                            catch (SocketException ex)
                            {
                                Console.WriteLine("ТУт? {0}", ex.ErrorCode);
                                if (ex.ErrorCode == 10056)
                                {
                                    newClient.Close();
                                    Console.WriteLine("ТУт? {0}", ex.ErrorCode);
                                }

                                if (ex.ErrorCode == 10061)
                                {
                                    Console.WriteLine("попытка подключиться ...");
                                    Thread.Sleep(800);
                                }
                                //Console.WriteLine(ex.ErrorCode);
                                //Console.WriteLine("Попытка подключение к {0}:{1}",ip, port);

                                Thread.Sleep(400);
                                connect = false;
                            }
                            // finally { Console.WriteLine("Попытка повторно подключится"); connect = false; }
                        }
                            break;
                    case 6:
                        // Клиент кидает на сервер байты


                        Console.Clear();
                        Console.ReadLine();
                        Console.WriteLine("Ввести IP");
                        string ip1 = Console.ReadLine();
                        Console.WriteLine("ip = {0}", ip1);
                        Console.WriteLine("Ввести Port");
                        string port1 = Console.ReadLine();
                        Console.WriteLine("port = {0}", port1);
                        // Инициализация
                        TcpClient newClient1 = new TcpClient();
                        bool connect1 = false;
                        while (true)
                        {
                            try
                            {
                                newClient1.Connect(ip1, Convert.ToInt32(port1));
                                Console.WriteLine("Мы подключены");

                                connect = true;
                                while (true)
                                {
                                    try
                                    {
                                        NetworkStream tcpStream = newClient1.GetStream();
                                        byte[] message = Encoding.UTF8.GetBytes("127.0.0.1;2222;");
                                        tcpStream.Write(message, 0, message.Length);
                                        //Console.WriteLine(" ---> {0}",BitConverter.ToString(message));
                                   
                                        byte[] msg = new byte[8096];
                                        int count = tcpStream.Read(msg, 0, msg.Length);
                                        var t = Encoding.ASCII.GetString(msg);
                                        Console.WriteLine("<---------- {0}", t);
                                        Console.WriteLine("Текст");
                                        if (count == 0) { Console.WriteLine("было отключение"); }
                                        Thread.Sleep(1000);
                                  }
                                    catch (IOException e)
                                    {
                                        break;
                                    }

                                }
                                /*
                                byte[] msg = new byte[8096];
                                int count = tcpStream.Read(msg, 0, msg.Length);

                                if (count == 0) { }
                                
                                var t = newClient.Connected;
                                if (connect == false)
                                { newClient.Connect(ip, Convert.ToInt32(port));  t = newClient.Connected; if (newClient.Connected == true) connect = true; }
                                }
                                else
                                {
                                
                                    newClient.Close();
                                    newClient = new TcpClient();
                                    connect = false;
                                    Console.WriteLine("Были отключены ");
                                   // newClient.Connect(ip, Convert.ToInt32(port));

                                }*/
                            }
                            catch (ObjectDisposedException e)
                            {
                                Console.WriteLine("АААА");
                                newClient1 = new TcpClient();

                            }
                            catch (SocketException ex)
                            {
                                Console.WriteLine("ТУт? {0}", ex.ErrorCode);
                                if (ex.ErrorCode == 10056)
                                {
                                    newClient1.Close();
                                    Console.WriteLine("ТУт? {0}", ex.ErrorCode);
                                }

                                if (ex.ErrorCode == 10061)
                                {
                                    Console.WriteLine("попытка подключиться ...");
                                    Thread.Sleep(800);
                                }
                                //Console.WriteLine(ex.ErrorCode);
                                //Console.WriteLine("Попытка подключение к {0}:{1}",ip, port);

                                Thread.Sleep(400);
                                connect = false;
                            }
                            // finally { Console.WriteLine("Попытка повторно подключится"); connect = false; }
                        }
                        break;

                    case 7:
                        exit = false;
                        break;
                }
            }
        }
    }
}