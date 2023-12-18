using System;
using System.Net;
using System.Net.Sockets;
using System.Reflection.Emit;
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
                Console.WriteLine("4 - выход");
                Console.Write("ввод символа: ");
                int choice = Convert.ToInt32(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        Console.Clear();
                        Console.WriteLine("тест байт");
                        byte[] _Message = new byte[] { 0x01, 0x03, 0xB4, 0x82, 0x80 };
                        Console.WriteLine(_bT.TestCompletion());
                        //Console.WriteLine(_bT.testByte(_Message));
                        Console.WriteLine("Нажмите любую кнопку...");
                        Console.ReadLine();
                        Console.Clear();
                        break;
                    case 2:
                        Console.Clear();
                        Console.WriteLine("тест портов");
                        Console.WriteLine("Введите IP устройства для проверки");
                        string IP = Console.ReadLine();
                        portScanner.Scanner(IP);
                        break;
                    case 3:
                        Console.Clear();
                        Console.WriteLine("Тестовый сервер: ");
                        // Устанавливаем для сокета локальную конечную точку
                        IPHostEntry ipHost = Dns.GetHostEntry("localhost");
                        IPAddress ipAddr = ipHost.AddressList[0];
                        IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, 11000);
                        byte[] Message = new byte[] { 0x01, 0x03, 0xB4, 0x82, 0x80, 0x00, 0x00, 0x00, 0x00, 0x09, 0xA1, 0x27, 0x1D, 0x00, 0x19, 0x00, 0x00, 0x01, 0x01, 0x00, 0x1D, 0x00, 0x00, 0x00, 0x92 };
                        // Создаем сокет Tcp/Ip
                        Socket sListener = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                        // Назначаем сокет локальной конечной точке и слушаем входящие сокеты
                        try
                        {
                            sListener.Bind(ipEndPoint);
                            sListener.Listen(10);

                            // Начинаем слушать соединения
                            while (true)
                            {
                                Console.WriteLine("Ожидаем соединение через порт {0}", ipEndPoint);

                                // Программа приостанавливается, ожидая входящее соединение
                                Socket handler = sListener.Accept();
                                string data = null;

                                // Мы дождались клиента, пытающегося с нами соединиться

                                byte[] bytes = new byte[1024];
                                int bytesRec = handler.Receive(bytes);

                                data += Encoding.UTF8.GetString(bytes, 0, bytesRec);

                                // Показываем данные на консоли
                                Console.WriteLine("РЕЗУЛЬТАТ !!!!   Array Output, Size: {0} Data: " + BitConverter.ToString(bytes), bytes.Length);

                                // Отправляем ответ клиенту\
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
                        exit = false;
                        break;
                }
            }
        }
    }
}