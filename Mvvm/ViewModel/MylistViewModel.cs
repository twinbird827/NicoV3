using NicoV3.Common;
using NicoV3.Mvvm.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using WpfUtilV1.Mvvm;
using WpfUtilV1.Mvvm.ViewModel;

namespace NicoV3.Mvvm.ViewModel
{
    public class MylistViewModel : ViewModelBase, IListViewItem
    {
        public MenuItemModel Parent { get; private set; }

        public MylistModel Source { get; private set; }

        public MylistViewModel(MenuItemModel parent, string id)
            : this(parent, MylistStatusModel.Instance.GetMylist(id))
        {

        }

        public MylistViewModel(MenuItemModel parent, MylistModel model)
            : base(model)
        {
            Parent = parent;
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
            get { return NicoDataConverter.ToId(MylistUrl); }
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
            get
            {
                if (_UserThumbnail == null)
                {
                    // TODO ｻﾑﾈ取得失敗時にﾃﾞﾌｫﾙﾄURLで再取得
                    // TODO ｻﾑﾈ中/大を選択時、取得失敗した場合はﾃﾞﾌｫﾙﾄｻﾑﾈを拡大する
                    NicoDataConverter.ToThumbnail(UserThumbnailUrl).ContinueWith(
                        t => UserThumbnail = t.Result,
                        TaskScheduler.FromCurrentSynchronizationContext()
                    );
                }
                return _UserThumbnail;
            }
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
                //case nameof(UserThumbnail):
                //    this.UserThumbnail = Source.UserThumbnail;
                //    break;
                case nameof(MylistDate):
                    this.MylistDate = Source.MylistDate;
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
                        // ﾏｲﾘｽﾄ検索画面へ遷移
                        MainWindowViewModel.Instance.Current = 
                            new SearchByMylistViewModel(new SearchByMylistModel(Source.MylistUrl, OrderBy));
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
        /// URLｺﾋﾟｰ
        /// </summary>
        public ICommand OnCopyUrl
        {
            get
            {
                return _OnCopyUrl = _OnCopyUrl ?? new RelayCommand(
                _ =>
                {
                    // ﾏｲﾘｽﾄ検索画面へ遷移
                    MainWindowViewModel.Instance.Current =
                        new SearchByMylistViewModel(new SearchByMylistModel(Source.MylistUrl));
                },
                _ =>
                {
                    return true;
                });
            }
        }
        public ICommand _OnCopyUrl;

        /// <summary>
        /// 削除処理
        /// </summary>
        public ICommand OnDelete
        {
            get
            {
                return _OnDelete = _OnDelete ?? new RelayCommand(
                _ =>
                {
                    // ﾒﾆｭｰから自身を削除
                    Parent.Mylists.Remove(Source.MylistId);
                },
                _ =>
                {
                    return true;
                });
            }
        }
        public ICommand _OnDelete;

        #endregion

    }
}
