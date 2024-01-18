using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

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
                Console.WriteLine("4 - неведомый сервер");
                Console.WriteLine("5 - выход");
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
                        IPHostEntry ipHost = Dns.GetHostEntry("localhost");
                        IPAddress ipAddr = ipHost.AddressList[0];
                        IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, 11000);
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
                        Console.Clear();
                        Console.WriteLine("Тестовый сервер: ");
                        IPHostEntry ipHost1 = Dns.GetHostEntry("localhost");
                        IPAddress ipAddr1 = ipHost1.AddressList[0];
                        TcpListener tcpListener = new TcpListener(ipAddr1, 11000);
                        byte[] Message1 = new byte[] { 0x01, 0x03, 0xB4, 0x82, 0x80, 0x00, 0x00, 0x00, 0x00, 0x09, 0xA1, 0x27, 0x1D, 0x00, 0x19, 0x00, 0x00, 0x01, 0x01, 0x00, 0x1D, 0x00, 0x00, 0x00, 0x92 };
                        Socket sListener1 = new Socket(ipAddr1.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                        bool flag = false;
                        try
                        {
                            TcpListener server = new TcpListener(IPAddress.Any, 11000);
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
                        exit = false;
                        break;
                }
            }
        }
    }
}