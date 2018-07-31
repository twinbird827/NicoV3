using NicoV3.Common;
using NicoV3.Mvvm.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Xml.Linq;
using WpfUtilV1.Common;

namespace NicoV3.Mvvm.Model
{
    public class VideoModel : HttpModel
    {
        public VideoModel()
        {
            this.Method = "GET";
            this.ContentType = "application/x-www-form-urlencoded";
        }

        public VideoModel(string id)
            : this()
        {
            this.VideoUrl = id;

            // ﾋﾞﾃﾞｵ情報読み取り
            this.Reload();
        }

        /// <summary>
        /// ｺﾝﾃﾝﾂId (http://nico.ms/ の後に連結することでコンテンツへのURLになります)
        /// </summary>
        public string VideoUrl
        {
            get
            {
                return "http://nico.ms/" + _VideoUrl;
            }
            set
            {
                SetProperty(ref _VideoUrl, NicoDataConverter.ToId(value));
            }
        }
        private string _VideoUrl = null;

        /// <summary>
        /// 動画ID
        /// </summary>
        public string VideoId
        {
            get { return NicoDataConverter.ToId(VideoUrl); }
        }

        /// <summary>
        /// ﾀｲﾄﾙ
        /// </summary>
        public string Title
        {
            get { return _Title; }
            set { SetProperty(ref _Title, value); }
        }
        private string _Title = null;

        /// <summary>
        /// ｺﾝﾃﾝﾂの説明文
        /// </summary>
        public string Description
        {
            get { return _Description; }
            set { SetProperty(ref _Description, value); }
        }
        private string _Description = null;

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
        /// ｶﾃｺﾞﾘﾀｸﾞ
        /// </summary>
        public string CategoryTag
        {
            get { return _CategoryTag; }
            set { SetProperty(ref _CategoryTag, value); }
        }
        private string _CategoryTag = null;

        /// <summary>
        /// 再生数
        /// </summary>
        public double ViewCounter
        {
            get { return _ViewCounter; }
            set { SetProperty(ref _ViewCounter, value); }
        }
        private double _ViewCounter = default(int);

        /// <summary>
        /// ﾏｲﾘｽﾄ数
        /// </summary>
        public double MylistCounter
        {
            get { return _MylistCounter; }
            set { SetProperty(ref _MylistCounter, value); }
        }
        private double _MylistCounter = default(int);

        /// <summary>
        /// ｺﾒﾝﾄ数
        /// </summary>
        public double CommentCounter
        {
            get { return _CommentCounter; }
            set { SetProperty(ref _CommentCounter, value); }
        }
        private double _CommentCounter = default(int);

        /// <summary>
        /// ｺﾝﾃﾝﾂの投稿時間
        /// </summary>
        public DateTime StartTime
        {
            get { return _StartTime; }
            set { SetProperty(ref _StartTime, value); }
        }
        private DateTime _StartTime = default(DateTime);

        /// <summary>
        /// 最終ｺﾒﾝﾄ時間
        /// </summary>
        public DateTime LastCommentTime
        {
            get { return _LastCommentTime; }
            set { SetProperty(ref _LastCommentTime, value); }
        }
        private DateTime _LastCommentTime = default(DateTime);

        /// <summary>
        /// 再生時間 (秒)
        /// </summary>
        public double LengthSeconds
        {
            get { return _LengthSeconds; }
            set { SetProperty(ref _LengthSeconds, value); }
        }
        private double _LengthSeconds = default(double);

        /// <summary>
        /// ｻﾑﾈｲﾙUrl
        /// </summary>
        public string ThumbnailUrl
        {
            get { return _ThumbnailUrl; }
            set
            {
                if (_ThumbnailUrl != value)
                {
                    SetProperty(ref _ThumbnailUrl, value);
                    //TODO Thumbnail = null;
                }
            }
        }
        private string _ThumbnailUrl = null;

        ///// <summary>
        ///// ｻﾑﾈｲﾙ
        ///// </summary>
        //public BitmapImage Thumbnail
        //{
        //    get
        //    {
        //        //if (_Thumbnail == null)
        //        //{
        //        //    // TODO ｻﾑﾈ取得失敗時にﾃﾞﾌｫﾙﾄURLで再取得
        //        //    // TODO ｻﾑﾈ中/大を選択時、取得失敗した場合はﾃﾞﾌｫﾙﾄｻﾑﾈを拡大する
        //        //    NicoDataConverter.ToThumbnail(_ThumbnailUrl)
        //        //        .ContinueWith(
        //        //            t => Thumbnail = t.Result,
        //        //            TaskScheduler.FromCurrentSynchronizationContext()
        //        //        );
        //        //}
        //        return _Thumbnail;
        //    }
        //    set { SetProperty(ref _Thumbnail, value); }
        //}
        //private BitmapImage _Thumbnail;

        /// <summary>
        /// ｺﾐｭﾆﾃｨｱｲｺﾝのUrl
        /// </summary>
        public string CommunityIcon
        {
            get { return _CommunityIcon; }
            set { SetProperty(ref _CommunityIcon, value); }
        }
        private string _CommunityIcon = null;

        /// <summary>
        /// 最終更新時間
        /// </summary>
        public DateTime LastUpdateTime
        {
            get { return _LastUpdateTime; }
            set { SetProperty(ref _LastUpdateTime, value); }
        }
        private DateTime _LastUpdateTime = DateTime.Now;

        /// <summary>
        /// 最新ｺﾒﾝﾄ
        /// </summary>
        public string LastResBody
        {
            get { return _LastResBody; }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    SetProperty(ref _LastResBody, value);
                }
            }
        }
        private string _LastResBody = null;

        /// <summary>
        /// ﾋﾞﾃﾞｵ情報を読み取ります。
        /// </summary>
        public void Reload()
        {
            // ﾋﾞﾃﾞｵ詳細情報取得
            var url = string.Format(Constants.VideoDetailUrl, this.VideoId);
            var txt = GetSmileVideoHtmlText(url);
            var xml = XDocument.Load(new StringReader(txt)).Root.Element("thumb");

            Title = xml.Element("title").Value;
            ViewCounter = long.Parse(xml.Element("view_counter").Value);
            MylistCounter = long.Parse(xml.Element("mylist_counter").Value);
            CommentCounter = long.Parse(xml.Element("comment_num").Value);
            StartTime = DateTime.Parse(xml.Element("first_retrieve").Value);
            ThumbnailUrl = xml.Element("thumbnail_url").Value;
            LengthSeconds = NicoDataConverter.ToLengthSeconds(xml.Element("length").Value);
            LastResBody = xml.Element("last_res_body").Value;
            Tags = string.Join(" ", xml
                .Elements("tags")
                .First(x => (string)x.Attribute("domain") == "jp")
                .Elements("tag")
                .Select(x => (string)x)
            );
        }

        /// <summary>
        /// ｺﾝﾃﾝﾂをﾌﾞﾗｳｻﾞ起動する。
        /// </summary>
        public void StartBrowser()
        {
            // Process.Start(Variables.BrowserPath, VideoUrl);

            MainWindowViewModel.Instance.Current = new VideoDetail2ViewModel(this);

            // SEEﾘｽﾄに追加
            VideoStatusModel.Instance.SeeVideos.Add(VideoId);

            // NEWﾘｽﾄから削除
            VideoStatusModel.Instance.NewVideos.Remove(VideoId);
        }

        public async Task<MemoryStream> GetMovieStreamAsync()
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

                    await client.GetStringAsync("http://www.nicovideo.jp/watch/" + VideoId);

                    var responseGetFlv = await client.GetStringAsync(new Uri("http://flapi.nicovideo.jp/api/getflv/" + VideoId));
                    string url = Uri.UnescapeDataString(responseGetFlv);

                    var FlvUrl = Regex.Match(url, @"&url=.*").Value.Replace("&url=", "");
                    var threadId = Regex.Match(url, @"thread_id=.*&l=").Value.Replace("thread_id=", "").Replace("&l=", "");
                    var msgUrl = Regex.Match(url, @"&ms=.*&ms_sub=").Value.Replace("&ms=", "").Replace("&ms_sub=", "");
                    var request = new HttpRequestMessage(HttpMethod.Get, FlvUrl);
                    var response = await client.SendAsync(request, System.Net.Http.HttpCompletionOption.ResponseHeadersRead);
                    var contentHeaders = response.Content.Headers;

                    var bytes = await client.GetByteArrayAsync(FlvUrl);
                    return new MemoryStream(bytes);
                });
            }
            //return await Task.Run(async () =>
            //{
            //    // 動画Urlに接続
            //    var tmp = GetSmileVideoHtmlText(string.Format(Constants.WatchUrl, VideoId));
            //    // 動画情報を取得
            //    var txt = HttpUtil.FromUrlEncode(GetSmileVideoHtmlText(string.Format(Constants.GetFlvUrl, VideoId)));
            //    //var txt = Uri.UnescapeDataString(GetSmileVideoHtmlText(string.Format(Constants.GetFlvUrl, VideoId)));
            //    // 動画情報から動画ﾀﾞｳﾝﾛｰﾄﾞ用Urlを取得
            //    var url = Regex.Match(txt, @"&url=.*").Value.Replace("&url=", "");
            //    // 動画ﾃﾞｰﾀを取得
            //    var req = GetRequest(url);

            //    using (var res = req.GetResponse())
            //    using (var st = res.GetResponseStream())
            //    using (var ms = new MemoryStream())
            //    {
            //        int read;
            //        byte[] buffer = new byte[1024];
            //        while ((read = st.Read(buffer, 0, buffer.Length)) > 0)
            //        {
            //            ms.Write(buffer, 0, read);
            //        }
            //        return ms;
            //    }
            //});
        }

        public async Task<Uri> GetMovieUriAsync()
        {
            return await Task.Run(async () =>
            {
                // 動画Urlに接続
                var tmp = GetSmileVideoHtmlText(string.Format(Constants.WatchUrl, VideoId));
                // 動画情報を取得
                var txt = HttpUtil.FromUrlEncode(GetSmileVideoHtmlText(string.Format(Constants.GetFlvUrl, VideoId)));
                //var txt = Uri.UnescapeDataString(GetSmileVideoHtmlText(string.Format(Constants.GetFlvUrl, VideoId)));
                // 動画情報から動画ﾀﾞｳﾝﾛｰﾄﾞ用Urlを取得
                var url = Regex.Match(txt, @"&url=.*").Value.Replace("&url=", "");

                return new Uri(url);
            });
        }
    }
}
