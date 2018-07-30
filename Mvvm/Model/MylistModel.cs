using NicoV3.Common;
using StatefulModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Xml.Linq;

namespace NicoV3.Mvvm.Model
{
    [DataContract]
    public class MylistModel : HttpModel
    {
        public MylistModel()
        {
            this.Method = "GET";
            this.ContentType = "application/x-www-form-urlencoded";
        }

        public MylistModel(string url, string orderBy = "0")
            : this()
        {
            MylistUrl = url;
            OrderBy = orderBy;

            Reload();
        }

        /// <summary>
        /// ﾏｲﾘｽﾄUrl
        /// </summary>
        [DataMember]
        public string MylistUrl
        {
            get { return string.Format(Constants.MylistUrlRss, NicoDataConverter.ToId(_MylistUrl), OrderBy); }
            set { SetProperty(ref _MylistUrl, value); }
        }
        private string _MylistUrl = null;

        /// <summary>
        /// ｿｰﾄ順
        /// </summary>
        [DataMember]
        public string OrderBy
        {
            get { return _OrderBy; }
            set { SetProperty(ref _OrderBy, value); }
        }
        private string _OrderBy = null;

        /// <summary>
        /// ﾏｲﾘｽﾄId
        /// </summary>
        public string MylistId
        {
            get
            {
                var last = NicoDataConverter.ToId(MylistUrl);
                return last.Substring(0, last.IndexOf("?"));
            }
        }

        /// <summary>
        /// ﾀｲﾄﾙ
        /// </summary>
        [DataMember]
        public string MylistTitle
        {
            get { return _MylistTitle; }
            set { SetProperty(ref _MylistTitle, value); }
        }
        private string _MylistTitle = null;

        /// <summary>
        /// 作成者
        /// </summary>
        [DataMember]
        public string MylistCreator
        {
            get { return _MylistCreator; }
            set { SetProperty(ref _MylistCreator, value); }
        }
        private string _MylistCreator = null;

        /// <summary>
        /// ﾏｲﾘｽﾄ詳細
        /// </summary>
        [DataMember]
        public string MylistDescription
        {
            get { return _MylistDescription; }
            set { SetProperty(ref _MylistDescription, value); }
        }
        private string _MylistDescription = null;

        /// <summary>
        /// 作成者のID
        /// </summary>
        [DataMember]
        public string UserId
        {
            get { return _UserId; }
            set { SetProperty(ref _UserId, value); }
        }
        private string _UserId = null;

        /// <summary>
        /// 作成者のｻﾑﾈｲﾙUrl
        /// </summary>
        [DataMember]
        public string UserThumbnailUrl
        {
            get { return _UserThumbnailUrl; }
            set { SetProperty(ref _UserThumbnailUrl, value); }
        }
        private string _UserThumbnailUrl = null;

        ///// <summary>
        ///// 作成者のｻﾑﾈｲﾙ
        ///// </summary>
        //public BitmapImage UserThumbnail
        //{
        //    get
        //    {
        //        if (_UserThumbnail == null)
        //        {
        //            // TODO ｻﾑﾈ取得失敗時にﾃﾞﾌｫﾙﾄURLで再取得
        //            // TODO ｻﾑﾈ中/大を選択時、取得失敗した場合はﾃﾞﾌｫﾙﾄｻﾑﾈを拡大する
        //            NicoDataConverter.ToThumbnail(UserThumbnailUrl).ContinueWith(
        //                t => UserThumbnail = t.Result,
        //                TaskScheduler.FromCurrentSynchronizationContext()
        //            );
        //        }
        //        return _UserThumbnail;
        //    }
        //    set { SetProperty(ref _UserThumbnail, value); }
        //}
        //private BitmapImage _UserThumbnail = null;

        /// <summary>
        /// 更新日時
        /// </summary>
        [DataMember]
        public DateTime MylistDate
        {
            get { return _MylistDate; }
            set { SetProperty(ref _MylistDate, value); }
        }
        private DateTime _MylistDate = default(DateTime);

        /// <summary>
        /// ﾏｲﾘｽﾄ内のﾋﾞﾃﾞｵ情報(詳細情報はﾋﾞﾃﾞｵｽﾃｰﾀｽで持つので、本ﾘｽﾄはidのみ保持する
        /// </summary>
        public ObservableSynchronizedCollection<string> Videos
        {
            get { return _Videos; }
            set { SetProperty(ref _Videos, value); }
        }
        private ObservableSynchronizedCollection<string> _Videos = new ObservableSynchronizedCollection<string>();

        /// <summary>
        /// ﾏｲﾘｽﾄ情報を再取得します。
        /// </summary>
        public void Reload()
        {
            var result = XDocument.Load(new StringReader(GetSmileVideoHtmlText(MylistUrl))).Root;
            var channel = result.Descendants("channel").First();

            // ﾏｲﾘｽﾄ情報を本ｲﾝｽﾀﾝｽのﾌﾟﾛﾊﾟﾃｨに転記
            MylistTitle = channel.Element("title").Value;
            MylistCreator = channel.Element(XName.Get("creator", "http://purl.org/dc/elements/1.1/")).Value;
            MylistDate = DateTime.Parse(channel.Element("lastBuildDate").Value);
            MylistDescription = channel.Element("description").Value;

            UserId = GetUserId();
            UserThumbnailUrl = GetThumbnailUrl();

            Videos.Clear();
            foreach (var item in channel.Descendants("item"))
            {
                var desc = XDocument.Load(new StringReader("<root>" + item.Element("description").Value + "</root>")).Root;
                var lengthSecondsStr = (string)desc
                        .Descendants("strong")
                        .Where(x => (string)x.Attribute("class") == "nico-info-length")
                        .First();

                var video = new VideoModel()
                {
                    VideoUrl = item.Element("link").Value,
                    Title = item.Element("title").Value,
                    ViewCounter = NicoDataConverter.ToCounter(desc, "nico-numbers-view"),
                    MylistCounter = NicoDataConverter.ToCounter(desc, "nico-numbers-mylist"),
                    CommentCounter = NicoDataConverter.ToCounter(desc, "nico-numbers-res"),
                    StartTime = DateTime.Parse(channel.Element("pubDate").Value),
                    ThumbnailUrl = (string)desc.Descendants("img").First().Attribute("src"),
                    LengthSeconds = NicoDataConverter.ToLengthSeconds(lengthSecondsStr),
                };

                // ﾋﾞﾃﾞｵ情報をｽﾃｰﾀｽﾓﾃﾞﾙに追加
                VideoStatusModel.Instance.VideoMerge(video);

                // idを追加
                Videos.Add(video.VideoId);
            }

            OnPropertyChanged(nameof(Videos));
        }

        /// <summary>
        /// ﾏｲﾘｽﾄからﾕｰｻﾞIDを取得します。
        /// </summary>
        /// <returns>ﾕｰｻﾞID</returns>
        private string GetUserId()
        {
            var url = string.Format(Constants.MylistUrl, MylistId); // 生のURL 
            var txt = GetSmileVideoHtmlText(url);                   // HTMLﾃｷｽﾄを取得 & user id を取得
            var id = Regex.Match(txt, "user_id: (?<user_id>[\\d]+)").Groups["user_id"].Value;
            return id;
        }

        /// <summary>
        /// ﾕｰｻﾞIDからｻﾑﾈｲﾙUrlを取得します。
        /// </summary>
        /// <returns>ｻﾑﾈｲﾙUrl</returns>
        private string GetThumbnailUrl()
        {
            var url = string.Format(Constants.UserIframe, UserId);
            var txt = GetSmileVideoHtmlText(url);
            var thumbnail = Regex.Match(txt, Constants.UserThumbnailUrl + "(?<url>[^\"]+)").Groups["url"].Value;
            return Constants.UserThumbnailUrl + thumbnail;
        }
    }
}
