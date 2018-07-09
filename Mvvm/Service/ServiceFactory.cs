using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NicoV3.Mvvm.Service
{
    public class ServiceFactory
    {
        public static IMessageService MessageService { get; set; } = new ConsoleMessageService();
    }
}
