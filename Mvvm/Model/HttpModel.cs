using NicoV3.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using WpfUtilV1.Common;
using WpfUtilV1.Mvvm;

namespace NicoV3.Mvvm.Model
{
    [DataContract]
    public class HttpModel : BindableBase
    {
        /// <summary>
        /// Httpﾒｿｯﾄﾞ
        /// </summary>
        protected virtual string Method { get; set; }

        /// <summary>
        /// Httpｺﾝﾃﾝﾂﾀｲﾌﾟ
        /// </summary>
        protected virtual string ContentType { get; set; }

        /// <summary>
        /// 指定したUrlとﾊﾟﾗﾒｰﾀでHTTPﾘｸｴｽﾄを取得します。
        /// </summary>
        /// <param name="url">URL</param>
        /// <param name="parameter">ﾊﾟﾗﾒｰﾀ</param>
        /// <returns><code>HTTPﾘｸｴｽﾄ</code></returns>
        protected HttpWebRequest GetRequest(string url, string parameter = null)
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = Method;
            req.ContentType = ContentType;

            req.Timeout = Constants.Timeout;
            req.ReadWriteTimeout = Constants.Timeout;
            req.UserAgent = Constants.UserAgent;
            req.Referer = Constants.Referer;

            // ﾛｸﾞｲﾝ情報を持つｸｯｷｰをｺﾝﾃﾅに追加する。
            req.CookieContainer = new CookieContainer();
            req.CookieContainer.Add(
                GetCookies(LoginModel.Instance.Cookie, req.RequestUri)
            );

            if (!string.IsNullOrWhiteSpace(parameter))
            {
                // ﾊﾟﾗﾒｰﾀが存在する場合、ｽﾄﾘｰﾑにﾊﾟﾗﾒｰﾀを追記する。
                byte[] bytes = Encoding.ASCII.GetBytes(parameter);
                req.ContentLength = bytes.LongLength;
                using (Stream stream = req.GetRequestStream())
                {
                    stream.Write(bytes, 0, (int)bytes.LongLength);
                }
            }

            return req;
        }

        /// <summary>
        /// 指定したﾘｸｴｽﾄでHTTPﾚｽﾎﾟﾝｽを取得します。
        /// </summary>
        /// <param name="req">HTTPﾘｸｴｽﾄ</param>
        /// <param name="isInternetSetCookie">InternetSetCookieを実行するか？</param>
        /// <returns>HTTPﾚｽﾎﾟﾝｽ</returns>
        protected HttpWebResponse GetResponse(HttpWebRequest req, bool isInternetSetCookie = true)
        {
            var res = req.GetResponse() as HttpWebResponse;

            if (isInternetSetCookie)
            {
                HttpUtil.InternetSetCookie(Constants.CookieUrl, GetCookies(req.CookieContainer, req.RequestUri));
            }

            return res;
        }

        /// <summary>
        /// Cookie 固有のURIに関連付けられているｲﾝｽﾀﾝｽを取得します。
        /// </summary>
        /// <param name="container">ｺﾝﾃﾅ</param>
        /// <param name="uri">URI</param>
        /// <returns><code>CookieCollection</code></returns>
        protected CookieCollection GetCookies(CookieContainer container, Uri uri)
        {
            var collection = new CookieCollection();

            // ｺﾝﾃﾅからｸｯｷｰを取得し、加工する。
            container.GetCookies(uri)
                .OfType<Cookie>()
                .Select(c =>
                {
                    if (c.Name == "col")
                    {
                        c.Value = "1";
                    }
                    return c;
                })
                .ToList()
                .ForEach(c => collection.Add(c));

            return collection;
        }

        /// <summary>
        /// 指定したUrlのHtmlﾃｷｽﾄを取得します。
        /// </summary>
        /// <param name="url">Url</param>
        /// <returns>Htmlﾃｷｽﾄ</returns>
        protected string GetSmileVideoHtmlText(string url)
        {
            LoginModel.Instance.Login();

            var req = GetRequest(url);                  // ﾘｸｴｽﾄ作成
            using (var res = GetResponse(req))
            {
                // ﾚｽﾎﾟﾝｽ取得
                var txt = HttpUtil.GetResponseString(res);  // ﾚｽﾎﾟﾝｽからHttpText取得

                // 制御文字を除外する
                Enumerable
                    .Range(0, 31)
                    .Where(i => i != 10)
                    .ToList()
                    .ForEach(i => txt = txt.Replace(((char)i).ToString(), ""));

                // 宣言されていないエンティティを除外する
                txt = txt.Replace("&copy;", "");
                txt = txt.Replace("&nbsp;", " ");
                txt = txt.Replace("&#x20;", " ");

                txt = txt.Replace("&", "&amp;");

                return txt;
            }
        }
    }
}