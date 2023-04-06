using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADO.net.Service
{
    internal interface ILogger
    {
        public void Log(string message, string level);
        public void Log(string message, string level, string className, string methodName);
    }
}
