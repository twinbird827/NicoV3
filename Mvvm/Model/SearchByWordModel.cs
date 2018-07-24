using Codeplex.Data;
using NicoV3.Common;
using WpfUtilV1.Mvvm.Service;
using StatefulModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfUtilV1.Common;

namespace NicoV3.Mvvm.Model
{
    public class SearchByWordModel : HttpModel
    {
        public SearchByWordModel()
        {
            this.Method = "GET";
            this.ContentType = "application/x-www-form-urlencoded";
        }

        /// <summary>
        /// 検索ﾜｰﾄﾞ
        /// </summary>
        public string Word
        {
            get { return _Word; }
            set { SetProperty(ref _Word, value); }
        }
        private string _Word = null;

        /// <summary>
        /// ﾀｸﾞ検索 or ｷｰﾜｰﾄﾞ検索
        /// </summary>
        public bool IsTag
        {
            get { return _IsTag; }
            set { SetProperty(ref _IsTag, value); }
        }
        private bool _IsTag = true;

        /// <summary>
        /// ﾘﾐｯﾄ (何件取得するか)
        /// </summary>
        public int Limit
        {
            get { return _Limit; }
            set { SetProperty(ref _Limit, value); }
        }
        private int _Limit = 100;

        /// <summary>
        /// ｵﾌｾｯﾄ (取得する開始位置)
        /// </summary>
        public int Offset
        {
            get { return _Offset; }
            set { SetProperty(ref _Offset, value); }
        }
        private int _Offset = 0;

        /// <summary>
        /// ｿｰﾄ順
        /// </summary>
        public string OrderBy
        {
            get { return _OrderBy; }
            set { SetProperty(ref _OrderBy, value); }
        }
        private string _OrderBy = null;

        /// <summary>
        /// ﾃﾞｰﾀ件数
        /// </summary>
        public double DataLength
        {
            get { return _DataLength; }
            set { SetProperty(ref _DataLength, value); }
        }
        private double _DataLength = 0;

        /// <summary>
        /// ｻﾑﾈｻｲｽﾞ
        /// </summary>
        public string ThumbSize
        {
            get { return _ThumbSize; }
            set { SetProperty(ref _ThumbSize, value); }
        }
        private string _ThumbSize = null;

        /// <summary>
        /// ｱｲﾃﾑ構成
        /// </summary>
        public ObservableSynchronizedCollection<string> Videos
        {
            get { return _Videos; }
            set { SetProperty(ref _Videos, value); }
        }
        private ObservableSynchronizedCollection<string> _Videos = new ObservableSynchronizedCollection<string>();

        public void Reload()
        {
            if (string.IsNullOrWhiteSpace(Word))
            {
                ServiceFactory.MessageService.Error("検索ワードが入力されていません。");
                return;
            }

            Videos.Clear();

            string targets = IsTag ? Constants.TargetTag : Constants.TargetKeyword;
            string q = HttpUtil.ToUrlEncode(Word);
            string fields = Constants.Fields;
            string offset = Offset.ToString();
            string limit = Limit.ToString();
            string context = Constants.Context;
            string sort = OrderBy;
            string url = String.Format(Constants.SearchByWordUrl, q, targets, fields, sort, offset, limit, context);
            string txt = GetSmileVideoHtmlText(url);

            // TODO 入力ﾁｪｯｸ

            var json = DynamicJson.Parse(txt);

            foreach (dynamic data in json["data"])
            {
                var video = new VideoModel()
                {
                    VideoUrl = data["contentId"],
                    Title = data["title"],
                    Description = data["description"],
                    Tags = data["tags"],
                    CategoryTag = data["categoryTags"],
                    ViewCounter = data["viewCounter"],
                    MylistCounter = data["mylistCounter"],
                    CommentCounter = data["commentCounter"],
                    StartTime = NicoDataConverter.ToDatetime(data["startTime"]),
                    LastCommentTime = NicoDataConverter.ToDatetime(data["lastCommentTime"]),
                    LengthSeconds = data["lengthSeconds"],
                    ThumbnailUrl = data["thumbnailUrl"] + ThumbSize,
                    //CommunityIcon = data["communityIcon"]
                };

                // 状態に追加
                VideoStatusModel.Instance.VideoMerge(video);

                // 自身に追加
                Videos.Add(video.VideoId);
            }

            DataLength = json["meta"]["totalCount"];

            ServiceFactory.MessageService.Debug(url);

        }
    }
}
