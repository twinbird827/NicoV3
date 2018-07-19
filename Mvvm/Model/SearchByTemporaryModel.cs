using Codeplex.Data;
using NicoV3.Common;
using NicoV3.Mvvm.Service;
using StatefulModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NicoV3.Mvvm.Model
{
    public class SearchByTemporaryModel : HttpModel
    {
        public static SearchByTemporaryModel Instance { get; } = new SearchByTemporaryModel();

        private SearchByTemporaryModel()
        {
            this.Method = "GET";
            this.ContentType = "application/x-www-form-urlencoded";

            Reload();
        }

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
            // ﾛｸﾞｲﾝする。
            LoginModel.Instance.Login();

            // ﾛｸﾞｲﾝできなかった場合
            if (!LoginModel.Instance.IsLogin)
            {
                ServiceFactory.MessageService.Error("Login error");
                return;
            }

            Videos.Clear();

            string txt = GetSmileVideoHtmlText(Constants.DeflistList);

            // TODO 入力ﾁｪｯｸ

            var json = DynamicJson.Parse(txt);

            foreach (dynamic data in json["mylistitem"])
            {
                var video = new VideoModel()
                {
                    VideoUrl = data["item_id"],
                    Title = data["item_data"]["title"],
                    Description = data["description"],
                    //Tags = data["tags"],
                    //CategoryTag = data["categoryTags"],
                    ViewCounter = long.Parse(data["item_data"]["view_counter"]),
                    MylistCounter = long.Parse(data["item_data"]["mylist_counter"]),
                    CommentCounter = long.Parse(data["item_data"]["num_res"]),
                    StartTime = NicoDataConverter.FromUnixTime((long)data["create_time"]),
                    //LastCommentTime = Converter.ToDatetime(data["lastCommentTime"]),
                    LengthSeconds = long.Parse(data["item_data"]["length_seconds"]),
                    ThumbnailUrl = data["item_data"]["thumbnail_url"],
                    LastResBody = data["item_data"]["last_res_body"],
                    //CommunityIcon = data["communityIcon"]
                };

                // 状態に追加
                VideoStatusModel.Instance.VideoMerge(video);

                // 自身に追加
                Videos.Add(video.VideoId);
            }
        }

        public void AddVideo(string id, string description = "")
        {
            if (!Videos.Any(v => v == id))
            {
                // とりあえずﾏｲﾘｽﾄに追加
                string url = string.Format(Constants.DeflistAdd, id, description, LoginModel.Instance.Token);
                string txt = GetSmileVideoHtmlText(url);

                // 自身に追加
                Videos.Insert(0, id);

                // NEWﾘｽﾄに追加
                VideoStatusModel.Instance.NewVideos.Add(id);
            }
        }

        public void DeleteVideo(string id)
        {
            if (Videos.Any(v => v == id))
            {
                // とりあえずﾏｲﾘｽﾄから削除
                string url = string.Format(Constants.DeflistDel, id, LoginModel.Instance.Token);
                string txt = GetSmileVideoHtmlText(url);

                // 自身から削除
                Videos.Remove(id);

                // ｽﾃｰﾀｽを削除
                VideoStatusModel.Instance.DeleteStatus(id);
            }
        }
    }
}
