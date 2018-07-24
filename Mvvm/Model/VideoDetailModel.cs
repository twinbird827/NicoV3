using NicoV3.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WpfUtilV1.Common;
using WpfUtilV1.Mvvm;

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
            Source = source;

            if (string.IsNullOrWhiteSpace(Source.Tags))
            {
                // ﾀｸﾞ情報がない場合はﾘﾛｰﾄﾞして取得する。
                Source.Reload();
            }

            FlvUrl = GetFlvUrl();
            FlvFile = GetFlvFile(); // TODO 非同期
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
        private string GetFlvUrl()
        {
            var url = string.Format(Constants.VideoDetailUrl, Source.VideoId);
            var txt = GetSmileVideoHtmlText(url);

            var json = JsonUtil.Parse(txt);

            return json["url"];
        }

        /// <summary>
        /// Flvﾌｧｲﾙを取得します。
        /// </summary>
        public MemoryStream GetFlvFile()
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
    }
}
