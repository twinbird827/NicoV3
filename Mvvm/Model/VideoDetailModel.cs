using NicoV3.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using WpfUtilV1.Common;
using WpfUtilV1.Mvvm;
using WpfUtilV1.Mvvm.Service;

namespace NicoV3.Mvvm.Model
{
    public class VideoDetailModel : HttpModel
    {
        public VideoModel Source { get; set; }

        public VideoDetailModel(string id)
            : this(VideoStatusModel.Instance.GetVideo(id))
        {
            
        }

        public VideoDetailModel(VideoModel source)
        {
            this.Method = "GET";
            this.ContentType = "application/x-www-form-urlencoded";
            Source = source;

            if (string.IsNullOrWhiteSpace(Source.Tags))
            {
                // ﾀｸﾞ情報がない場合はﾘﾛｰﾄﾞして取得する。
                Source.Reload();
            }

            ReadFlvUrl().ConfigureAwait(false);
            Tags = Source.Tags;
        }

        /// <summary>
        /// Flvﾀﾞｳﾝﾛｰﾄﾞ用Url
        /// </summary>
        public string FlvUrl
        {
            get { return _FlvUrl; }
            set { SetProperty(ref _FlvUrl, value); }
        }
        private string _FlvUrl = null;

        /// <summary>
        /// Flvﾌｧｲﾙ
        /// </summary>
        public MemoryStream FlvFile
        {
            get { return _FlvFile; }
            set { SetProperty(ref _FlvFile, value); }
        }
        private MemoryStream _FlvFile = null;

        /// <summary>
        /// ﾀｸﾞ (空白区切り)
        /// </summary>
        public string Tags
        {
            get { return _Tags; }
            set { SetProperty(ref _Tags, value); }
        }
        private string _Tags = null;

        /// <summary>
        /// Flvﾌｧｲﾙ取得用Urlを取得します。
        /// </summary>
        private async Task<object> ReadFlvUrl()
        {
            using (var client = new HttpClient())
            {
                return await Task.Run(async () =>
                {
                    await client.PostAsync(
                        "https://secure.nicovideo.jp/secure/login?site=niconico",
                        new FormUrlEncodedContent(new Dictionary<string, string>()
                        {
                            { "mail", Variables.MailAddress },
                            { "password", Variables.Password },
                        }));

                    await client.GetStringAsync("http://www.nicovideo.jp/watch/" + Source.VideoId);

                    var responseGetFlv = await client.GetStringAsync(new Uri("http://flapi.nicovideo.jp/api/getflv/" + Source.VideoId));
                    string url = Uri.UnescapeDataString(responseGetFlv);

                    FlvUrl = Regex.Match(url, @"&url=.*").Value.Replace("&url=", "");
                    var threadId = Regex.Match(url, @"thread_id=.*&l=").Value.Replace("thread_id=", "").Replace("&l=", "");
                    var msgUrl = Regex.Match(url, @"&ms=.*&ms_sub=").Value.Replace("&ms=", "").Replace("&ms_sub=", "");
                    var request = new HttpRequestMessage(HttpMethod.Get, FlvUrl);
                    var response = await client.SendAsync(request, System.Net.Http.HttpCompletionOption.ResponseHeadersRead);
                    var contentHeaders = response.Content.Headers;

                    var bytes = await client.GetByteArrayAsync(FlvUrl);
                    FlvFile = new MemoryStream(bytes);

                    return default(object);
                });
            }
        }

        /// <summary>
        /// Flvﾌｧｲﾙを取得します。
        /// </summary>
        public MemoryStream GetFlvFile()
        {
            try
            {
                var hwr = (HttpWebRequest)WebRequest.Create(FlvUrl);
                hwr.CookieContainer = LoginModel.Instance.Cookie;
                hwr.Timeout = 30 * 1000;

                WebResponse wres = hwr.GetResponse();

                using (var res = wres.GetResponseStream())
                using (var mstream = new MemoryStream())
                {
                    int intBuffSize = 1024;
                    int intFileSize = 0;
                    int intRet = 0;
                    while (true)
                    {
                        byte[] bytBuff = new byte[intBuffSize];
                        intRet = res.Read(bytBuff, 0, intBuffSize);
                        if (intRet == 0) break;
                        mstream.Write(bytBuff, 0, intRet);
                        intFileSize += intRet;
                    }
                    return mstream;
                }
            }
            catch(Exception ex)
            {
                ServiceFactory.MessageService.Exception(ex);
                throw;
            }
        }
    }
}
