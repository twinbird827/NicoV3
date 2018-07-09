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
                return Instance["MAIL_ADDRESS"];
            }
            set
            {
                Instance["MAIL_ADDRESS"] = value;
            }
        }

        public static string Password
        {
            get
            {
                var tmp = Instance["PASSWORD"];

                return string.IsNullOrWhiteSpace(tmp)
                    ? tmp
                    : Encrypter.DecryptString(tmp, Constants.ApplicationId);
            }
            set
            {
                Instance["PASSWORD"] = Encrypter.EncryptString(value, Constants.ApplicationId);
            }
        }

        public static string Encoding
        {
            get
            {
                return Instance["ENCODING", "UTF-8"];
            }
            private set
            {
                Instance["ENCODING"] = value;
            }
        }

        public static string BrowserPath
        {
            get
            {
                return Instance["BROWSER_PATH", @"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe"];
            }
            private set
            {
                Instance["ENCODING"] = value;
            }
        }

    }
}
