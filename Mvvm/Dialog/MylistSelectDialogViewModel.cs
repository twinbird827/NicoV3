using NicoV3.Mvvm.Model;
using NicoV3.Mvvm.ViewModel;
using StatefulModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfUtilV1.Mvvm;
using WpfUtilV1.Mvvm.ViewModel;

namespace NicoV3.Mvvm.Dialog
{
    public class MylistSelectDialogViewModel : ViewModelBase
    {
        public MylistSelectDialogViewModel()
            : this(null)
        {

        }

        public MylistSelectDialogViewModel(Action<MylistSelectDialogViewModel> execute)
        {
            // OKﾎﾞﾀﾝ押下時
            OnAccept = new RelayCommand(
                _ =>
                {
                    execute(this);
                    MainWindowViewModel.Instance.HideMetroDialogAsync(Dialog);
                },
                _ =>
                {
                    return MenuItems.Any(mi => mi.IsSelected);
                });

            // ｷｬﾝｾﾙﾎﾞﾀﾝ押下時
            OnCancel = new RelayCommand(
                _ =>
                {
                    MainWindowViewModel.Instance.HideMetroDialogAsync(Dialog);
                });

            // ﾒﾆｭｰ設定
            MenuItems = MenuModel.Instance.Children
                .First(c => c.Type == MenuItemType.SearchByMylist)
                .Children
                .ToSyncedSynchronizationContextCollection(
                    model => new MenuItemViewModel(null, model),
                    AnonymousSynchronizationContext.Current
            );

        }

        /// <summary>
        /// ﾒﾆｭｰﾂﾘｰﾋﾞｭｰ構成
        /// </summary>
        public SynchronizationContextCollection<MenuItemViewModel> MenuItems
        {
            get { return _MenuItems; }
            set { SetProperty(ref _MenuItems, value); }
        }
        private SynchronizationContextCollection<MenuItemViewModel> _MenuItems;

        /// <summary>
        /// ﾀﾞｲｱﾛｸﾞ
        /// </summary>
        public MylistSelectDialog Dialog
        {
            get { return _Dialog = _Dialog ?? new MylistSelectDialog(this); }
        }
        private MylistSelectDialog _Dialog;

        /// <summary>
        /// OKﾎﾞﾀﾝ押下時処理
        /// </summary>
        public ICommand OnAccept
        {
            get { return _OnAccept; }
            set { SetProperty(ref _OnAccept, value); }
        }
        private ICommand _OnAccept;

        /// <summary>
        /// ｷｬﾝｾﾙﾎﾞﾀﾝ押下時処理
        /// </summary>
        public ICommand OnCancel
        {
            get { return _OnCancel; }
            set { SetProperty(ref _OnCancel, value); }
        }
        private ICommand _OnCancel;
    }
}
