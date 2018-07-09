using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NicoV3.Mvvm.Service
{
    public class ConsoleMessageService : IMessageService
    {
        public void Error(string message)
        {
            Console.WriteLine(string.Format("[ERROR][{0:yy/MM/dd HH:mm:ss}]{1}", DateTime.Now, message));
        }

        public void Info(string message)
        {
            Console.WriteLine(string.Format("[INFO][{0:yy/MM/dd HH:mm:ss}]{1}", DateTime.Now, message));
        }

        public void Debug(string message)
        {
            Console.WriteLine(string.Format("[DEBUG][{0:yy/MM/dd HH:mm:ss}]{1}", DateTime.Now, message));
        }
    }
}
