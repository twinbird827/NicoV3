using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfUtilV1.Common;

namespace NicoV3.Common
{
    class Variables : Ini
    {
        private static Variables Instance { get; set; } = new Variables();

        protected Variables() : base(Constants.AppConfigPath, Constants.AppConfigDefaultSection)
        {
        }

        public static string MailAddress
        {
            get
            {
                return Instance[Instance.Section, "MAIL_ADDRESS"];
            }
            set
            {
                Instance[Instance.Section, "MAIL_ADDRESS"] = value;
            }
        }

        public static string Password
        {
            get
            {
                var tmp = Instance[Instance.Section, "PASSWORD"];

                return string.IsNullOrWhiteSpace(tmp)
                    ? tmp
                    : Encrypter.DecryptString(tmp, Constants.ApplicationId);
            }
            set
            {
                Instance[Instance.Section, "PASSWORD"] = Encrypter.EncryptString(value, Constants.ApplicationId);
            }
        }

        public static string Encoding
        {
            get
            {
                return Instance[Instance.Section, "ENCODING", "UTF-8"];
            }
            private set
            {
                Instance[Instance.Section, "ENCODING"] = value;
            }
        }

        public static string BrowserPath
        {
            get
            {
                return Instance[Instance.Section, "BROWSER_PATH", @"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe"];
            }
            private set
            {
                Instance[Instance.Section, "BROWSER_PATH"] = value;
            }
        }

        public static long MylistUpdateInterval
        {
            get
            {
                return long.Parse(Instance[Instance.Section, "MYLIST_UPDATE_TIMER", @"600000"]);
            }
            private set
            {
                Instance[Instance.Section, "MYLIST_UPDATE_TIMER"] = value.ToString();
            }
        }

        public static DateTime MylistUpdateDatetime
        {
            get
            {
                var tmp = Instance[Instance.Section, "MYLIST_UPDATE_DATETIME", ""];
                return string.IsNullOrWhiteSpace(tmp) ? DateTime.Now : DateTime.Parse(tmp);
            }
            set
            {
                Instance[Instance.Section, "MYLIST_UPDATE_DATETIME"] = value.ToString();
            }
        }

    }
}
