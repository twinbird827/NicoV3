using NicoV3.Common;
using NicoV3.Mvvm.Model;
using WpfUtilV1.Mvvm.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfUtilV1.Mvvm;

namespace NicoV3.Mvvm.ViewModel
{
    public class SettingViewModel : WorkSpaceViewModel
    {
        public static SettingViewModel Instance { get; private set; } = new SettingViewModel();

        private SettingViewModel()
        {
            // ﾛｸﾞｲﾝ情報の初期値
            MailAddress = Variables.MailAddress;
            Password = Variables.Password;

            // なぜか Password が string.Empty だと上手くﾊﾞｲﾝﾃﾞｨﾝｸﾞできないので対処
            Password = string.IsNullOrEmpty(Password) ? null : Password;

            // ﾊﾞｰｼﾞｮﾝ情報の初期値
            var assm = System.Reflection.Assembly.GetExecutingAssembly();
            var asmcpy = Attribute.GetCustomAttribute(assm, typeof(AssemblyCopyrightAttribute)) as AssemblyCopyrightAttribute;
            Version = assm.GetName().Version.ToString();
            UpdateDate = (new FileInfo(assm.Location)).LastWriteTime;
            Copyright = asmcpy != null ? asmcpy.Copyright : "";

        }

        /// <summary>
        /// ﾒｰﾙｱﾄﾞﾚｽ
        /// </summary>
        public string MailAddress
        {
            get { return _MailAddress; }
            set { SetProperty(ref _MailAddress, value); }
        }
        private string _MailAddress;

        /// <summary>
        /// ﾊﾟｽﾜｰﾄﾞ
        /// </summary>
        public string Password
        {
            get { return _Password; }
            set { SetProperty(ref _Password, value); }
        }
        private string _Password;

        /// <summary>
        /// ﾊﾟｽﾜｰﾄﾞ
        /// </summary>
        public string Version
        {
            get { return _Version; }
            set { SetProperty(ref _Version, value); }
        }
        private string _Version;

        /// <summary>
        /// ﾊﾟｽﾜｰﾄﾞ
        /// </summary>
        public DateTime UpdateDate
        {
            get { return _UpdateDate; }
            set { SetProperty(ref _UpdateDate, value); }
        }
        private DateTime _UpdateDate;

        /// <summary>
        /// ﾊﾟｽﾜｰﾄﾞ
        /// </summary>
        public string Copyright
        {
            get { return _Copyright; }
            set { SetProperty(ref _Copyright, value); }
        }
        private string _Copyright;

        /// <summary>
        /// ﾛｸﾞｲﾝ処理
        /// </summary>
        public ICommand OnLogin
        {
            get
            {
                return _OnLogin = _OnLogin ?? new RelayCommand(
                    _ =>
                    {
                        // ﾛｸﾞｲﾝ実行
                        LoginModel.Instance.Login(MailAddress, Password);
                        ServiceFactory.MessageService.Debug(LoginModel.Instance.IsLogin.ToString());
                    },
                    _ => {
                        return
                            !string.IsNullOrWhiteSpace(MailAddress) &&
                            !string.IsNullOrWhiteSpace(Password);
                    });
            }
        }

        public ICommand _OnLogin;

    }
}
