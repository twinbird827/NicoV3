using StatefulModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfUtilV1.Mvvm;

namespace NicoV3.Mvvm.Model
{
    public class VideoStatusModel : BindableBase
    {
        public static VideoStatusModel Instance { get; } = new VideoStatusModel();

        private VideoStatusModel()
        {
            // ｼﾝｸﾞﾙﾄﾝﾊﾟﾀｰﾝ
        }

        /// <summary>
        /// ｱｲﾃﾑ構成
        /// </summary>
        public ObservableSynchronizedCollection<VideoModel> Videos
        {
            get { return _Videos; }
            set { SetProperty(ref _Videos, value); }
        }
        private ObservableSynchronizedCollection<VideoModel> _Videos = new ObservableSynchronizedCollection<VideoModel>();

        /// <summary>
        /// NEWﾘｽﾄ
        /// </summary>
        public ObservableSynchronizedCollection<string> NewVideos
        {
            get { return _NewVideos; }
            set { SetProperty(ref _NewVideos, value); }
        }
        private ObservableSynchronizedCollection<string> _NewVideos = new ObservableSynchronizedCollection<string>();

        /// <summary>
        /// SEEﾘｽﾄ
        /// </summary>
        public ObservableSynchronizedCollection<string> SeeVideos
        {
            get { return _SeeVideos; }
            set { SetProperty(ref _SeeVideos, value); }
        }
        private ObservableSynchronizedCollection<string> _SeeVideos = new ObservableSynchronizedCollection<string>();

        /// <summary>
        /// ﾋﾞﾃﾞｵ情報をﾏｰｼﾞします。
        /// </summary>
        /// <param name="video">ﾏｰｼﾞするﾋﾞﾃﾞｵ情報</param>
        public void VideoMerge(VideoModel video)
        {
            if (Videos.Any(v => v.VideoId == video.VideoId))
            {
                var v = Videos.First(temp => temp.VideoId == video.VideoId);

                // 更新要素を再設定
                v.ViewCounter = video.ViewCounter;
                v.MylistCounter = video.MylistCounter;
                v.CommentCounter = video.CommentCounter;
                v.LastResBody = video.LastResBody;
            }
            else
            {
                // 追加
                Videos.Add(video);
            }
        }

        /// <summary>
        /// ﾋﾞﾃﾞｵ情報を取得します。
        /// </summary>
        /// <param name="id">取得したいﾋﾞﾃﾞｵID</param>
        /// <param name="reload">既存情報をﾘﾛｰﾄﾞするか</param>
        /// <returns>ﾋﾞﾃﾞｵ情報</returns>
        public VideoModel GetVideo(string id, bool reload = true)
        {
            var video = Videos.FirstOrDefault(v => v.VideoId == id);

            if (video != null)
            {
                if (reload)
                {
                    video.Reload();
                }
                return video;
            }
            else
            {
                var tmp = new VideoModel(id);

                Videos.Add(tmp);

                return tmp;
            }
        }

        public string GetStatus(string id)
        {
            if (SeeVideos.Any(v => v == id))
            {
                return "SEE";
            }
            else if (NewVideos.Any(v => v == id))
            {
                return "NEW";
            }
            else if (SearchByTemporaryModel.Instance.Videos.Any(v => v == id))
            {
                return "FAV";
            }
            else
            {
                return null;
            }
        }

        public void DeleteStatus(string id)
        {
            // NEWﾘｽﾄから削除
            NewVideos.Remove(id);

            // SEEﾘｽﾄから削除
            SeeVideos.Remove(id);
        }
    }
}
