using NicoV3.Mvvm.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using WpfUtilV1.Mvvm.ViewModel;

namespace NicoV3.Mvvm.ViewModel
{
    public class MylistViewModel : ViewModelBase
    {
        public MylistModel Source { get; private set; }

        public MylistViewModel(string id)
            : this(MylistStatusModel.Instance.GetMylist(id))
        {

        }

        public MylistViewModel(MylistModel model)
            : base(model)
        {
            Source = model;
        }

        #region Model Properties

        /// <summary>
        /// ﾏｲﾘｽﾄUrl
        /// </summary>
        public string MylistUrl
        {
            get { return _MylistUrl; }
            set { SetProperty(ref _MylistUrl, value); }
        }
        private string _MylistUrl = null;

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
        /// ﾏｲﾘｽﾄId
        /// </summary>
        public string MylistId
        {
            get { return MylistUrl?.Split('/').Last(); }
        }

        /// <summary>
        /// ﾀｲﾄﾙ
        /// </summary>
        public string MylistTitle
        {
            get { return _MylistTitle; }
            set { SetProperty(ref _MylistTitle, value); }
        }
        private string _MylistTitle = null;

        /// <summary>
        /// 作成者
        /// </summary>
        public string MylistCreator
        {
            get { return _MylistCreator; }
            set { SetProperty(ref _MylistCreator, value); }
        }
        private string _MylistCreator = null;

        /// <summary>
        /// ﾏｲﾘｽﾄ詳細
        /// </summary>
        public string MylistDescription
        {
            get { return _MylistDescription; }
            set { SetProperty(ref _MylistDescription, value); }
        }
        private string _MylistDescription = null;

        /// <summary>
        /// 作成者のID
        /// </summary>
        public string UserId
        {
            get { return _UserId; }
            set { SetProperty(ref _UserId, value); }
        }
        private string _UserId = null;

        /// <summary>
        /// 作成者のｻﾑﾈｲﾙUrl
        /// </summary>
        public string UserThumbnailUrl
        {
            get { return _UserThumbnailUrl; }
            set { SetProperty(ref _UserThumbnailUrl, value); }
        }
        private string _UserThumbnailUrl = null;

        /// <summary>
        /// 作成者のｻﾑﾈｲﾙ
        /// </summary>
        public BitmapImage UserThumbnail
        {
            get { return _UserThumbnail; }
            set { SetProperty(ref _UserThumbnail, value); }
        }
        private BitmapImage _UserThumbnail = null;

        /// <summary>
        /// 更新日時
        /// </summary>
        public DateTime MylistDate
        {
            get { return _MylistDate; }
            set { SetProperty(ref _MylistDate, value); }
        }
        private DateTime _MylistDate = default(DateTime);

        #endregion

        #region Override Methods

        protected override void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(sender, e);

            switch (e.PropertyName)
            {
                case nameof(MylistUrl):
                    this.MylistUrl = Source.MylistUrl;
                    break;
                case nameof(OrderBy):
                    this.OrderBy = Source.OrderBy;
                    break;
                case nameof(MylistTitle):
                    this.MylistTitle = Source.MylistTitle;
                    break;
                case nameof(MylistCreator):
                    this.MylistCreator = Source.MylistCreator;
                    break;
                case nameof(MylistDescription):
                    this.MylistDescription = Source.MylistDescription;
                    break;
                case nameof(UserId):
                    this.UserId = Source.UserId;
                    break;
                case nameof(UserThumbnailUrl):
                    this.UserThumbnailUrl = Source.UserThumbnailUrl;
                    break;
                case nameof(UserThumbnail):
                    this.UserThumbnail = Source.UserThumbnail;
                    break;
                case nameof(MylistDate):
                    this.MylistDate = Source.MylistDate;
                    break;
            }
        }

        #endregion

    }
}
