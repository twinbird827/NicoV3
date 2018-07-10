using NicoV3.Common;
using NicoV3.Mvvm.Service;
using StatefulModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace NicoV3.Mvvm.Model
{
    public class SearchByRankingModel : HttpModel
    {
        public static SearchByRankingModel Instance { get; } = new SearchByRankingModel();

        private SearchByRankingModel()
        {
            this.Method = "GET";
            this.ContentType = "application/x-www-form-urlencoded";
        }

        /// <summary>
        /// 対象
        /// </summary>
        public string Target
        {
            get { return _Target; }
            set { SetProperty(ref _Target, value); }
        }
        private string _Target = null;

        /// <summary>
        /// 期間
        /// </summary>
        public string Period
        {
            get { return _Period; }
            set { SetProperty(ref _Period, value); }
        }
        private string _Period = null;

        /// <summary>
        /// ｶﾃｺﾞﾘ
        /// </summary>
        public string Category
        {
            get { return _Category; }
            set { SetProperty(ref _Category, value); }
        }
        private string _Category = null;

        /// <summary>
        /// ｱｲﾃﾑ構成
        /// </summary>
        public ObservableSynchronizedCollection<string> Videos
        {
            get { return _Videos; }
            set { SetProperty(ref _Videos, value); }
        }
        private ObservableSynchronizedCollection<string> _Videos = new ObservableSynchronizedCollection<string>();

        /// <summary>
        /// ﾗﾝｷﾝｸﾞ情報を再取得します。
        /// </summary>
        public void Reload()
        {
            if (string.IsNullOrWhiteSpace(Target) || string.IsNullOrWhiteSpace(Period) || string.IsNullOrWhiteSpace(Category))
            {
                ServiceFactory.MessageService.Error("検索ワードが入力されていません。");
                return;
            }

            Videos.Clear();

            var url = string.Format(Constants.RankingUrl, Target, Period, Category);
            var txt = GetSmileVideoHtmlText(url);

            // TODO 入力ﾁｪｯｸ

            // ﾗﾝｷﾝｸﾞを検索した。
            var result = XDocument.Load(new StringReader(txt)).Root;
            var channel = result.Descendants("channel").First();

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
                    ViewCounter = NicoDataConverter.ToCounter(desc, "nico-info-total-view"),
                    MylistCounter = NicoDataConverter.ToCounter(desc, "nico-info-total-res"),
                    CommentCounter = NicoDataConverter.ToCounter(desc, "nico-info-total-mylist"),
                    StartTime = DateTime.Parse(channel.Element("pubDate").Value),
                    ThumbnailUrl = (string)desc.Descendants("img").First().Attribute("src"),
                    LengthSeconds = NicoDataConverter.ToLengthSeconds(lengthSecondsStr),
                };

                // 状態に追加
                VideoStatusModel.Instance.VideoMerge(video);

                // 自身に追加
                Videos.Add(video.VideoId);
            }

            ServiceFactory.MessageService.Debug(url);
        }

    }
}
