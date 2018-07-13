using NicoV3.Mvvm.Model;
using NicoV3.Mvvm.View.Dialog;
using NicoV3.Mvvm.ViewModel.Dialog;
using StatefulModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfUtilV1.Mvvm;

namespace NicoV3.Mvvm.ViewModel
{
    public class MenuItemViewModel : BindableBase
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
        /// ｱｲﾃﾑ名(ﾘﾈｰﾑ後)
        /// </summary>
        public string Input
        {
            get { return _Input; }
            set { SetProperty(ref _Input, value); }
        }
        private string _Input = null;

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
        /// ﾒﾆｭｰの種類
        /// </summary>
        public bool IsSelected
        {
            get { return _IsSelected; }
            set { SetProperty(ref _IsSelected, value); }
        }
        private bool _IsSelected;

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
                async (p1) =>
                {
                    var dc = MainWindowViewModel.Instance.DialogCoordinator;

                    var vm = new SimpleInputDialogViewModel(
                        new RelayCommand(
                        p =>
                        {

                        }));
                    vm.Title = "Title";
                    vm.Description = "Description";
                    vm.Input = "Input";

                    await dc.ShowMetroDialogAsync(MainWindowViewModel.Instance, new SimpleInputDialog(vm))
                        .ContinueWith(
                            task => Source.Name = vm.Input,
                            TaskScheduler.FromCurrentSynchronizationContext()
                        );


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
                    async (p1) =>
                    {
                        SimpleInputDialog dialog = null;
                        var dc = MainWindowViewModel.Instance.DialogCoordinator;

                        var OkCommand = new RelayCommand(
                            async p2 =>
                            {
                                //
                                await dc.HideMetroDialogAsync(MainWindowViewModel.Instance, dialog);
                            });
                        var CancelCommand = new RelayCommand(
                            async p2 =>
                            {
                                //
                                await dc.HideMetroDialogAsync(MainWindowViewModel.Instance, dialog);
                            });
                        var vm = new SimpleInputDialogViewModel(OkCommand, CancelCommand)
                        {
                            Title = "Title",
                            Name = "Name",
                            Description = "Description",
                            Input = "Input"
                        };

                        dialog = new SimpleInputDialog(vm);

                        await dc.ShowMetroDialogAsync(MainWindowViewModel.Instance, dialog)
                            .ContinueWith(
                                task => Source.Name = vm.Input,
                                TaskScheduler.FromCurrentSynchronizationContext()
                            );
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
              _ =>
              {

              },
              _ => {
                  return true;
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
    }
}
