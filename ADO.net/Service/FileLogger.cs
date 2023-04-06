
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADO.net.Service
{
    internal class FileLogger : ILogger
    {
        private readonly string _filename;

        public FileLogger()
        {
            _filename = "logs.txt";
        }

        public void Log(string message, string level)
        {
            this.Log(message, level, "", "");
        }
        public void Log(string message, string level, string className, string methodName)
        {
            File.AppendAllText(_filename, $"{DateTime.Now}  {level}  {className}.{methodName}\r\n");
        }
    }
}