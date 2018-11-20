using NicoV3.Common;
using NicoV3.Mvvm.Model;
using NicoV3.Mvvm.Model.ComboboxItem;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using WpfUtilV1.Mvvm;
using WpfUtilV1.Mvvm.Service;
using WpfUtilV1.Mvvm.ViewModel;

namespace NicoV3.Mvvm.ViewModel
{
    public class VideoViewModel : ViewModelBase, IListViewItem
    {
        public VideoModel Source { get; private set; }

        public VideoViewModel(string id)
            : this(VideoStatusModel.Instance.GetVideo(id))
        {

        }

        public VideoViewModel(VideoModel model)
            : base(model)
        {
            Source = model;

            VideoUrl = Source.VideoUrl;
            Title = Source.Title;
            Description = Source.Description;
            Tags = Source.Tags;
            CategoryTag = Source.CategoryTag;
            ViewCounter = Source.ViewCounter;
            MylistCounter = Source.MylistCounter;
            CommentCounter = Source.CommentCounter;
            StartTime = Source.StartTime;
            LastCommentTime = Source.LastCommentTime;
            LengthSeconds = Source.LengthSeconds;
            ThumbnailUrl = Source.ThumbnailUrl;
            //Thumbnail = Source.Thumbnail; TODO
            CommunityIcon = Source.CommunityIcon;
            LastUpdateTime = Source.LastUpdateTime;
            LastResBody = Source.LastResBody;
        }

        #region Model Properties

        /// <summary>
        /// ｽﾃｰﾀｽ
        /// </summary>
        public string Status
        {
            get { return VideoStatusModel.Instance.GetStatus(VideoId); }
        }

        /// <summary>
        /// ｺﾝﾃﾝﾂId (http://nico.ms/ の後に連結することでコンテンツへのURLになります)
        /// </summary>
        public string VideoUrl
        {
            get { return _VideoUrl; }
            set { SetProperty(ref _VideoUrl, value); }
        }
        private string _VideoUrl = null;

        /// <summary>
        /// 動画ID
        /// </summary>
        public string VideoId
        {
            get { return VideoUrl.Split('/').Last(); }
        }

        /// <summary>
        /// ﾀｲﾄﾙ
        /// </summary>
        public virtual string Title
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
            set { SetProperty(ref _ThumbnailUrl, value); }
        }
        private string _ThumbnailUrl = null;

        /// <summary>
        /// ｻﾑﾈｲﾙ
        /// </summary>
        public BitmapImage Thumbnail
        {
            //get { return _Thumbnail; }
            get
            {
                if (_Thumbnail == null)
                {
                    for (int index = ComboThumbSizeModel.Instance.Items.Count() - 1; 0 < index; index--)
                    {
                        try
                        {
                            var size = ComboThumbSizeModel.Instance.Items[index].Value;

                            ServiceFactory.MessageService.Debug(ThumbnailUrl + size);
                            // TODO ｻﾑﾈ取得失敗時にﾃﾞﾌｫﾙﾄURLで再取得
                            // TODO ｻﾑﾈ中/大を選択時、取得失敗した場合はﾃﾞﾌｫﾙﾄｻﾑﾈを拡大する
                            NicoDataConverter.ToThumbnail(_ThumbnailUrl + size)
                                .ContinueWith(
                                    t => Thumbnail = t.Result,
                                    TaskScheduler.FromCurrentSynchronizationContext()
                                );
                            break;
                        }
                        catch (Exception ex)
                        {
                            ServiceFactory.MessageService.Exception(ex);
                        }
                    }
                }
                return _Thumbnail;
            }
            set { SetProperty(ref _Thumbnail, value); }
        }
        private BitmapImage _Thumbnail;

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
            set { SetProperty(ref _LastResBody, value); }
        }
        private string _LastResBody = null;

        /// <summary>
        /// 選択されているかどうか
        /// </summary>
        public bool IsSelected
        {
            get { return _IsSelected; }
            set { SetProperty(ref _IsSelected, value); }
        }
        private bool _IsSelected = false;

        #endregion

        #region Override Methods

        protected override void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(sender, e);

            switch (e.PropertyName)
            {
                case nameof(VideoUrl):
                    this.VideoUrl = Source.VideoUrl;
                    break;
                case nameof(Title):
                    this.Title = Source.Title;
                    break;
                case nameof(Description):
                    this.Description = Source.Description;
                    break;
                case nameof(Tags):
                    this.Tags = Source.Tags;
                    break;
                case nameof(CategoryTag):
                    this.CategoryTag = Source.CategoryTag;
                    break;
                case nameof(ViewCounter):
                    this.ViewCounter = Source.ViewCounter;
                    break;
                case nameof(MylistCounter):
                    this.MylistCounter = Source.MylistCounter;
                    break;
                case nameof(CommentCounter):
                    this.CommentCounter = Source.CommentCounter;
                    break;
                case nameof(StartTime):
                    this.StartTime = Source.StartTime;
                    break;
                case nameof(LastCommentTime):
                    this.LastCommentTime = Source.LastCommentTime;
                    break;
                case nameof(LengthSeconds):
                    this.LengthSeconds = Source.LengthSeconds;
                    break;
                case nameof(ThumbnailUrl):
                    this.ThumbnailUrl = Source.ThumbnailUrl;
                    break;
                //case nameof(Thumbnail): TODO
                //    this.Thumbnail = Source.Thumbnail;
                //    break;
                case nameof(CommunityIcon):
                    this.CommunityIcon = Source.CommunityIcon;
                    break;
                case nameof(LastUpdateTime):
                    this.LastUpdateTime = Source.LastUpdateTime;
                    break;
                case nameof(LastResBody):
                    this.LastResBody = Source.LastResBody;
                    break;
            }
        }

        #endregion

        #region Commands

        /// <summary>
        /// 項目ﾀﾞﾌﾞﾙｸﾘｯｸ時ｲﾍﾞﾝﾄ
        /// </summary>
        public ICommand OnDoubleClick
        {
            get
            {
                return _OnDoubleClick = _OnDoubleClick ?? new RelayCommand(
                _ =>
                {
                    // ﾌﾞﾗｳｻﾞ表示
                    Source.StartBrowser();

                    // ｽﾃｰﾀｽを明示的に更新
                    OnPropertyChanged(nameof(Status));
                },
                _ =>
                {
                    return true;
                });
            }
        }
        public ICommand _OnDoubleClick;

        /// <summary>
        /// 項目ｷｰ入力時ｲﾍﾞﾝﾄ
        /// </summary>
        public ICommand OnKeyDown
        {
            get
            {
                return _OnKeyDown = _OnKeyDown ?? new RelayCommand<KeyEventArgs>(
                e =>
                {
                    // ﾀﾞﾌﾞﾙｸﾘｯｸと同じ処理
                    OnDoubleClick.Execute(null);
                },
                e =>
                {
                    return e.Key == Key.Enter;
                });
            }
        }
        public ICommand _OnKeyDown;

        /// <summary>
        /// 項目をﾃﾝﾎﾟﾗﾘに追加する
        /// </summary>
        public ICommand OnTemporaryAdd
        {
            get
            {
                return _OnTemporaryAdd = _OnTemporaryAdd ?? new RelayCommand(
                e =>
                {
                    // ﾃﾝﾎﾟﾗﾘに追加
                    SearchByTemporaryModel.Instance.AddVideo(Source.VideoId);

                    // ｽﾃｰﾀｽを明示的に更新
                    OnPropertyChanged(nameof(Status));
                },
                e =>
                {
                    return true;
                });
            }
        }
        public ICommand _OnTemporaryAdd;

        /// <summary>
        /// 項目をﾃﾝﾎﾟﾗﾘから削除する
        /// </summary>
        public ICommand OnTemporaryDel
        {
            get
            {
                return _OnTemporaryDel = _OnTemporaryDel ?? new RelayCommand(
                e =>
                {
                    // ﾃﾝﾎﾟﾗﾘから削除
                    SearchByTemporaryModel.Instance.DeleteVideo(Source.VideoId);

                    // ｽﾃｰﾀｽを明示的に更新
                    OnPropertyChanged(nameof(Status));
                },
                e =>
                {
                    return true;
                });
            }
        }
        public ICommand _OnTemporaryDel;

        /// <summary>
        /// 動画をﾀﾞｳﾝﾛｰﾄﾞする。
        /// </summary>
        public ICommand OnDownload
        {
            get
            {
                return _OnDownload = _OnDownload ?? new RelayCommand(
                async _ =>
                {
                    using (var ms = await Source.GetMovieStreamAsync())
                    {
                        File.WriteAllBytes(@"C:\" + DateTime.Now.ToString("yyyyMMddhhmmssfff.bin"), ms.ToArray());
                    }
                },
                _ =>
                {
                    return true;
                });
            }
        }
        public ICommand _OnDownload;

        #endregion
    }
}
