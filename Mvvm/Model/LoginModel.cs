using NicoV3.Common;
using NicoV3.Mvvm.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WpfUtilV1.Common;

namespace NicoV3.Mvvm.Model
{
    public class LoginModel : HttpModel
    {
        /// <summary>
        /// ｼﾝｸﾞﾙﾄﾝﾊﾟﾀｰﾝのため、ﾌﾟﾗｲﾍﾞｰﾄｺﾝｽﾄﾗｸﾀ。
        /// </summary>
        private LoginModel()
        {
            this.Method = "POST";
            this.ContentType = "application/x-www-form-urlencoded";
        }

        /// <summary>
        /// LoginModelｲﾝｽﾀﾝｽを取得します。
        /// </summary>
        public static LoginModel Instance { get; private set; } = new LoginModel();

        /// <summary>
        /// ﾛｸﾞｲﾝしているかどうか
        /// </summary>
        public bool IsLogin
        {
            get { return _IsLogin; }
            set { SetProperty(ref _IsLogin, value); }
        }
        private bool _IsLogin = false;

        /// <summary>
        /// ﾌﾟﾚﾐｱﾑかどうか
        /// </summary>
        public bool IsPremium
        {
            get { return _IsPremium; }
            set { SetProperty(ref _IsPremium, value); }
        }
        private bool _IsPremium = false;

        /// <summary>
        /// ﾌﾟﾚﾐｱﾑかどうか
        /// </summary>
        public string Token
        {
            get
            {
                if (LastTokenGetDate.AddMinutes(10).Ticks < DateTime.Now.Ticks)
                {
                    // 10分経過毎にﾄｰｸﾝを更新する。
                    _Token = GetToken();
                    //LastTokenGetDate = DateTime.Now;
                }
                return _Token;
            }
        }
        private string _Token = default(string);

        public DateTime LastTokenGetDate
        {
            get { return _LastTokenGetDate; }
            set { SetProperty(ref _LastTokenGetDate, value); }
        }
        private DateTime _LastTokenGetDate = default(DateTime);

        /// <summary>
        /// ｸｯｷｰｺﾝﾃﾅ
        /// </summary>
        public CookieContainer Cookie
        {
            get { return _Cookie; }
            set { SetProperty(ref _Cookie, value); }
        }
        private CookieContainer _Cookie = new CookieContainer();

        /// <summary>
        /// 規定のﾕｰｻﾞ、ﾊﾟｽﾜｰﾄﾞを用いて、ﾛｸﾞｲﾝ処理を実行します。
        /// </summary>
        /// <remarks>既にﾛｸﾞｲﾝ済みの場合は中断します。</remarks>
        public void Login()
        {
            if (IsLogin)
            {
                // ﾛｸﾞｲﾝ済みの場合は中断
                return;
            }

            // ﾛｸﾞｲﾝ処理
            Login(Variables.MailAddress, Variables.Password);
        }

        /// <summary>
        /// 指定したﾕｰｻﾞ、ﾊﾟｽﾜｰﾄﾞを用いて、ﾛｸﾞｲﾝ処理を実行します。
        /// </summary>
        /// <param name="mail">ﾒｰﾙｱﾄﾞﾚｽ</param>
        /// <param name="password">ﾊﾟｽﾜｰﾄﾞ</param>
        /// <remarks>既にﾛｸﾞｲﾝ済みの場合もﾛｸﾞｲﾝし直します。</remarks>
        public void Login(string mail, string password)
        {
            // ﾛｸﾞｲﾝﾃｽﾄ前にﾌﾟﾛﾊﾟﾃｨを初期化する。
            Initialize();

            if (string.IsNullOrWhiteSpace(mail) || string.IsNullOrWhiteSpace(password))
            {
                // ﾛｸﾞｲﾝ情報が指定されていない場合は中断
                ServiceFactory.MessageService.Error("ログイン情報が入力されていません。");
                return;
            }

            try
            {
                // ﾘｸｴｽﾄを取得する。
                var req = GetRequest(Constants.LoginUrl, ToLoginParameter(mail, password));
                var res = GetResponse(req, false);

                // ﾘｸｴｽﾄからｸｯｷｰ取得
                CookieCollection cookies = req.CookieContainer.GetCookies(req.RequestUri);

                // ﾚｽﾎﾟﾝｽを用いてﾛｸﾞｲﾝ処理を実行する。
                if (LoginWithResponse(HttpUtil.GetResponseString(res)))
                {
                    // ﾛｸﾞｲﾝが成功したら、ﾛｸﾞｲﾝ情報を保存する。
                    Variables.MailAddress = mail;
                    Variables.Password = password;

                    // 取得したｸｯｷｰをIEに流用
                    HttpUtil.InternetSetCookie(Constants.CookieUrl, cookies);

                    // 自身のｸｯｷｰｺﾝﾃﾅに追加
                    Cookie.Add(cookies);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());   // TODO
                Initialize();
                ServiceFactory.MessageService.Error("何らかの原因でエラーが発生しました。");
            }

        }

        /// <summary>
        /// ﾚｽﾎﾟﾝｽを用いてﾛｸﾞｲﾝ処理を実行します。
        /// </summary>
        /// <param name="expression">ﾚｽﾎﾟﾝｽﾃﾞｰﾀ</param>
        /// <returns>ﾛｸﾞｲﾝ成功：true / 失敗：false</returns>
        private bool LoginWithResponse(string expression)
        {
            ServiceFactory.MessageService.Debug(expression);
            if (expression.Contains("メールアドレスまたはパスワードが間違っています"))
            {
                ServiceFactory.MessageService.Error("入力されたログイン情報が間違っています。");
            }
            else if (expression.Contains("closed=1&"))
            {
                ServiceFactory.MessageService.Error("入力されたアカウント情報が間違っています。");
            }
            else if (expression.Contains("error=access_locked"))
            {
                ServiceFactory.MessageService.Error("連続アクセス検出のためアカウントロック中\r\nしばらく時間を置いてから試行してください。");
            }
            else if (expression.Contains("is_premium=0&") || expression.Contains("is_premium=1&"))
            {
                // ﾛｸﾞｲﾝ成功
                // ﾛｸﾞｲﾝﾌﾗｸﾞを立てる。
                IsLogin = true;
                IsPremium = expression.Contains("is_premium=1&");
                return true;
            }
            else
            {
                ServiceFactory.MessageService.Error("何らかの原因でログインできませんでした\r\nしばらく時間を置いてから試行してください。");
            }

            return false;
        }

        /// <summary>
        /// ﾌﾟﾛﾊﾟﾃｨを初期化します。
        /// </summary>
        private void Initialize()
        {
            IsLogin = false;
            IsPremium = false;
            Cookie = null;
            Cookie = new CookieContainer();
        }

        private string GetToken()
        {
            var txt = GetSmileVideoHtmlText(Constants.TokenUrl);
            return Regex.Match(txt, "data-csrf-token=\"(?<token>[^\"]+)\"").Groups["token"].Value;
        }

        /// <summary>
        /// ﾛｸﾞｲﾝﾊﾟﾗﾒｰﾀに変換します。
        /// </summary>
        /// <param name="mailaddress">ﾒｰﾙｱﾄﾞﾚｽ</param>
        /// <param name="password">ﾊﾟｽﾜｰﾄﾞ</param>
        /// <returns></returns>
        private string ToLoginParameter(string mailaddress, string password)
        {
            return string.Format(
                Constants.LoginParameter,
                HttpUtil.ToUrlEncode(mailaddress),
                HttpUtil.ToUrlEncode(password)
            );
        }

    }
}
