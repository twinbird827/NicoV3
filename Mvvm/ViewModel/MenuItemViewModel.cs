using NicoV3.Mvvm.Model;
using NicoV3.Properties;
using StatefulModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfUtilV1.Mvvm;
using WpfUtilV1.Mvvm.ViewModel;

namespace NicoV3.Mvvm.ViewModel
{
    public class MenuItemViewModel : BindableBase, IListViewItem
    {
        /// <summary>
        /// 本ﾒﾆｭｰの親ﾒﾆｭｰ
        /// Dispose時に除外する属性をｾｯﾄ
        /// </summary>
        [Exclusion]
        public MenuItemViewModel Parent { get; set; }

        /// <summary>
        /// 自身のﾒﾆｭｰ構成
        /// </summary>
        public MenuItemModel Source { get; set; }

        public MenuItemViewModel(MenuItemViewModel parent, MenuItemModel source)
            : base(source)
        {
            Parent = parent;
            Source = source;

            this.Name = Source.Name;
            this.Type = Source.Type;
            this.IsSelected = false;

            Children = Source.Children.ToSyncedSynchronizationContextCollection(
                model => new MenuItemViewModel(this, model),
                AnonymousSynchronizationContext.Current
            );

        }

        /// <summary>
        /// ｱｲﾃﾑ名
        /// </summary>
        public string Name
        {
            get { return _Name; }
            set { SetProperty(ref _Name, value); }
        }
        private string _Name = null;

        /// <summary>
        /// ﾒﾆｭｰの種類
        /// </summary>
        public MenuItemType Type
        {
            get { return _Type; }
            set { SetProperty(ref _Type, value); }
        }
        private MenuItemType _Type;

        /// <summary>
        /// 選択されているかどうか
        /// </summary>
        public bool IsSelected
        {
            get { return _IsSelected; }
            set { SetProperty(ref _IsSelected, value); }
        }
        private bool _IsSelected = false;

        /// <summary>
        /// ﾒﾆｭｰが紐付くﾜｰｸｽﾍﾟｰｽのｲﾝｽﾀﾝｽ
        /// </summary>
        public WorkSpaceViewModel WorkSpace
        {
            get
            {
                return GetWorkSpace();
            }
        }
        private WorkSpaceViewModel _WorkSpace;

        /// <summary>
        /// 本ﾒﾆｭｰｱｲﾃﾑの子要素
        /// </summary>
        public SynchronizationContextCollection<MenuItemViewModel> Children
        {
            get { return _Children; }
            set { SetProperty(ref _Children, value); }
        }
        private SynchronizationContextCollection<MenuItemViewModel> _Children;

        /// <summary>
        /// 自身のﾜｰｸｽﾍﾟｰｽを取得します。
        /// </summary>
        /// <returns></returns>
        private WorkSpaceViewModel GetWorkSpace()
        {
            switch (Type)
            {
                case MenuItemType.Setting:
                    _WorkSpace = SettingViewModel.Instance;
                    break;
                case MenuItemType.SearchByWord:
                    _WorkSpace = new SearchByWordViewModel();
                    break;
                case MenuItemType.SearchByMylist:
                    _WorkSpace = new SearchByMylistViewModel();
                    break;
                case MenuItemType.Ranking:
                    _WorkSpace = new SearchByRankingViewModel();
                    break;
                case MenuItemType.Temporary:
                    _WorkSpace = new SearchByTemporaryViewModel();
                    break;
                case MenuItemType.MylistOfOther:
                    _WorkSpace = new SearchByFavMylistViewModel(this.Source);
                    break;
                case MenuItemType.MylistOfMe:
                    _WorkSpace = new SearchByFavMylistViewModel(this.Source);
                    break;
                default:
                    return null;
            }

            return _WorkSpace;
        }

        #region Commands

        /// <summary>
        /// 名前変更処理
        /// </summary>
        public ICommand OnRename
        {
            get
            {
                return _OnRename = _OnRename ?? new RelayCommand(
                    async _ =>
                    {
                        var result = await MainWindowViewModel.Instance.ShowInputAsync(
                            Resources.L_RENAME,
                            Resources.M_RENAME_DESCRIPTION);

                        if (!string.IsNullOrWhiteSpace(result))
                        {
                            Source.Name = result;
                        }
                    });
            }
        }
        public ICommand _OnRename;

        /// <summary>
        /// 子ﾒﾆｭｰ追加処理
        /// </summary>
        public ICommand OnAddChildren
        {
            get
            {
                return _OnAddChildren = _OnAddChildren ?? new RelayCommand(
                    async _ =>
                    {
                        var result = await MainWindowViewModel.Instance.ShowInputAsync(
                            Resources.L_ADD_CHILDREN,
                            Resources.M_ADD_CHILDREN_DESCRIPTION);

                        if (!string.IsNullOrWhiteSpace(result) && !Source.Children.Any(c => c.Name == result))
                        {
                            Source.Children.Add(new MenuItemModel(result, MenuItemType.MylistOfOther));
                        }
                    });
            }
        }
        public ICommand _OnAddChildren;

        /// <summary>
        /// 削除処理
        /// </summary>
        public ICommand OnRemove
        {
            get
            {
                return _OnRemove = _OnRemove ?? new RelayCommand(
                    async _ =>
                    {
                        var result = await MainWindowViewModel.Instance.ShowMessageAsync(
                            Resources.L_REMOVE,
                            Resources.M_REMOVE_DESCRIPTION,
                            MahApps.Metro.Controls.Dialogs.MessageDialogStyle.AffirmativeAndNegative);

                        if (result == MahApps.Metro.Controls.Dialogs.MessageDialogResult.Affirmative)
                        {
                            // 親ﾒﾆｭｰから自身を削除
                            Parent.Source.Children.Remove(Source);
                        }
                    });
            }
        }
        public ICommand _OnRemove;

        /// <summary>
        /// ﾏｳｽﾀﾞﾌﾞﾙｸﾘｯｸ時処理
        /// </summary>
        public ICommand OnMouseDoubleClick
        {
            get
            {
                return _OnMouseDoubleClick = _OnMouseDoubleClick ?? new RelayCommand(
              _ =>
              {
                  MainWindowViewModel.Instance.Current = WorkSpace;
                  MainWindowViewModel.Instance.IsOpenFlyout = false;
              },
              _ => {
                  return true;
              });
            }
        }
        public ICommand _OnMouseDoubleClick;

        #endregion

        protected override void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(sender, e);

            switch (e.PropertyName)
            {
                case nameof(Name) :
                    Name = Source.Name;
                    break;
            }
        }
    }
}
