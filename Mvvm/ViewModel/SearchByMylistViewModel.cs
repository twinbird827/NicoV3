﻿using NicoV3.Common;
using NicoV3.Mvvm.Dialog;
using NicoV3.Mvvm.Model;
using NicoV3.Mvvm.Model.ComboboxItem;
using StatefulModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using WpfUtilV1.Mvvm;

namespace NicoV3.Mvvm.ViewModel
{
    public class SearchByMylistViewModel : WorkSpaceViewModel
    {
        private SearchByMylistModel Source { get; set; }

        public SearchByMylistViewModel()
            : this(new SearchByMylistModel())
        {

        }

        public SearchByMylistViewModel(SearchByMylistModel model)
        {
            Source = model;

            this.MylistTitle = Source.MylistTitle;
            this.MylistCreator = Source.MylistCreator;
            this.MylistDescription = Source.MylistDescription;
            this.UserId = Source.UserId;
            //this.UserThumbnail = Source.UserThumbnail;
            this.MylistDate = Source.MylistDate;

            Items = Source.Videos.ToSyncedSynchronizationContextCollection(
                id => new SearchByMylistItemViewModel(id),
                AnonymousSynchronizationContext.Current
            );

            SortItems = ComboSortMylistModel
                .Instance
                .Items
                .ToSyncedSynchronizationContextCollection(m => m, AnonymousSynchronizationContext.Current);
            SelectedSortItem = SortItems.First();

        }

        /// <summary>
        /// ﾒｲﾝ項目ﾘｽﾄ
        /// </summary>
        public SynchronizationContextCollection<SearchByMylistItemViewModel> Items
        {
            get { return _Items; }
            set { SetProperty(ref _Items, value); }
        }
        private SynchronizationContextCollection<SearchByMylistItemViewModel> _Items;

        /// <summary>
        /// ｿｰﾄﾘｽﾄ
        /// </summary>
        public SynchronizationContextCollection<ComboboxItemModel> SortItems
        {
            get { return _SortItems; }
            set { SetProperty(ref _SortItems, value); }
        }
        private SynchronizationContextCollection<ComboboxItemModel> _SortItems;

        /// <summary>
        /// 選択中のｿｰﾄ項目
        /// </summary>
        public ComboboxItemModel SelectedSortItem
        {
            get { return _SelectedSortItem; }
            set { SetProperty(ref _SelectedSortItem, value); }
        }
        private ComboboxItemModel _SelectedSortItem;

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
                if (_UserThumbnail == null && UserThumbnailUrl != null)
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
        /// 作成者情報を表示するか
        /// </summary>
        public bool IsCreatorVisible
        {
            get { return _IsCreatorVisible; }
            set { SetProperty(ref _IsCreatorVisible, value); }
        }
        private bool _IsCreatorVisible = default(bool);

        protected override void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(sender, e);

            switch (e.PropertyName)
            {
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
                //case nameof(UserThumbnail):
                //    this.UserThumbnail = Source.UserThumbnail;
                //    break;
                case nameof(MylistDate):
                    this.MylistDate = Source.MylistDate;
                    break;
            }

        }

        /// <summary>
        /// 検索処理
        /// </summary>
        public ICommand OnSearch
        {
            get
            {
                return _OnSearch = _OnSearch ?? new RelayCommand(
              _ =>
              {
                  // 入力値をﾓﾃﾞﾙにｾｯﾄ
                  Source.Word = this.Word;
                  Source.OrderBy = this.SelectedSortItem.Value;

                  // 検索実行
                  this.Source.Reload();

                  this.MylistTitle = Source.MylistTitle;
                  this.MylistCreator = Source.MylistCreator;
                  this.MylistDescription = Source.MylistDescription;
                  this.UserId = Source.UserId;
                  //this.UserThumbnail = Source.UserThumbnail;
                  this.MylistDate = Source.MylistDate;

                  // ｵｰﾅｰ情報を表示するかどうか
                  this.IsCreatorVisible = Source.Videos.Any();
              },
              _ => {
                  return !string.IsNullOrWhiteSpace(Word);
              });
            }
        }
        public ICommand _OnSearch;

        /// <summary>
        /// 追加処理
        /// </summary>
        public ICommand OnAdd
        {
            get
            {
                return _OnAdd = _OnAdd ?? new RelayCommand(
                _ =>
                {
                    MenuModel.Instance.Children
                        .First(m => m.Type == MenuItemType.MylistOfOther)
                        .Mylists.Add(Word);
                    //var vm = new MylistSelectDialogViewModel(
                    //    dvm =>
                    //    {
                    //        dvm.MenuItems
                    //            .First(mi => mi.IsSelected)
                    //            .Source.Mylists.Add(NicoDataConverter.ToId(Source.Word));
                    //    });

                    //await MainWindowViewModel.Instance.ShowMetroDialogAsync(vm.Dialog);
                },
                _ => {
                    return IsCreatorVisible;
                });
            }
        }
        public ICommand _OnAdd;

    }
}
