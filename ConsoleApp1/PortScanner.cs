using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class PortScanner
    {
        private static Files file = new Files();
        private static string nameFile = "port.txt"; 

        public void Scanner(string IP)
        {
            file.CreateFile(nameFile);
            Console.Write("начальное значение: ");
            int startPort = Convert.ToInt32(Console.ReadLine());
            Console.Write("конечное значение: ");
            int endPort = Convert.ToInt32(Console.ReadLine());
            for (int i = startPort; i <= endPort; i++)
            {
                using (TcpClient Scan = new TcpClient())
                {
                    try
                    {
                        Scan.Connect(IP, i);
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"[{i}] | OPEN");
                        string temp = IP +"  "+ i.ToString() +   "| OPEN "+ DateTime.Now;
                        file.ReadFile(nameFile, temp);
                        Console.ResetColor();
                    }
                    catch
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"[{i}] | CLOSED");
                        string temp = IP + "  " + i.ToString() + "| CLOSED " + DateTime.Now;
                        file.ReadFile(nameFile, temp);
                        Console.ResetColor();
                    }
                }
            }
            Console.WriteLine("Нажмите любую кнопку...");
            Console.ReadLine();
        }
    }
}
