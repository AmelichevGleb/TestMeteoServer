using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class Files
    {
        private string fileLogs = "logs.txt";
        private string fileApu = "Port.txt";
        public void InitFile()
        {
            try
            {
                if (CheckFile(fileLogs) == false)
                {
                    CreateFile(fileLogs);
                }

                if (CheckFile(fileLogs) == false) { Console.WriteLine("Неудается создать файл Logs"); }
                else
                {
                    CreateFile(fileApu);
                }
            }
            catch (Exception ex)
            {
                ReadExeption(ex);
            }
        }
        //Проверка файла на наличие
        public bool CheckFile(string _name)
        {
            string path = Environment.CurrentDirectory + @"\" + _name;
            if (System.IO.File.Exists(path))
            {
                return true;
            }
            else
                return false;
        }
        //Создание файла
        public bool CreateFile(string _name)
        {
            try
            {
                string path = Environment.CurrentDirectory + @"\" + _name;
                using (StreamWriter w = System.IO.File.AppendText(_name))
                    w.Close();
                return true;
            }
            catch (Exception ex)
            {
                ReadExeption(ex);
                return false;
            }
        }
        //Запись ошибки в файл
        public void ReadExeption(Exception _ex)
        {
            Console.WriteLine("Exception: " + _ex.Message.ToString());
            ReadFile(_ex.ToString(), true);
        }
        //Запись ошибки в файл logs.txt
        public void ReadFile(string _text, bool state)
        {
            string path = Environment.CurrentDirectory + @"\" + fileLogs;
            Console.WriteLine(_text + DateTime.Now);
            System.IO.File.AppendAllText(fileLogs, "-" + " " + Convert.ToString(DateTime.Now) + " " + _text + "\n");
        }
        //Запись информации в файл
        public void ReadFile(string _name, string _text)
        {
            string path = Environment.CurrentDirectory + @"\" + _name;
            System.IO.File.AppendAllText(_name, _text + "\n");
        }
    }
}
