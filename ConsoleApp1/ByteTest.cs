using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class ByteTest
    {
        byte[] Message = new byte[] { 0x01, 0x03, 0x00, 0x00, 0x00, 0x5A, 0xC5, 0xF1 };  // правильный массив
        byte[] Message2 = new byte[8]; //Тестовый массив для команды запроса 

        /*
            1 <---> 1
            3 <---> 3
            0 <---> 0
            0 <---> 0
            0 <---> 0
            90 <---> 90
            197 <---> 197
            241 <---> 241
        */

        public bool TestСomparisonByte(byte[] _message)
        {
            if (Message.Length == _message.Length)
            {
                var temp = 0;
                for (int i = 0; i < Message.Length; i++)
                {
                    Console.WriteLine(Message[i] + " <---> " + _message[i]);
                    if (Message[i] == _message[i])
                    {
                        temp++;
                    }
                }
                if (temp == _message.Length) { return true; }
                else { return false; }
            }
            else
            {
                return false;
            }
        }
        public bool TestCompletion()
        {
            for (int i = 0;i< Message2.Length;i++)
            {
                Message2[i]= Convert.ToByte(Console.ReadLine());
              //  Console.WriteLine(Message[i] + " <---> " + Message2[i]);

            }
            Console.WriteLine("ТЕСТ BT 2 Array Output, Size: {0} Data: " + BitConverter.ToString(Message2), Message2.Length);
            return TestСomparisonByte(Message2);
        }
    }
}