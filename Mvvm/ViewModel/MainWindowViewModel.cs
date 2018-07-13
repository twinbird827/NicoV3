using MahApps.Metro.Controls.Dialogs;
using NicoV3.Mvvm.Model;
using StatefulModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfUtilV1.Mvvm;
using WpfUtilV1.Mvvm.ViewModel;

namespace NicoV3.Mvvm.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        /// <summary>
        /// ｼﾝｸﾞﾙﾄﾝﾊﾟﾀｰﾝ
        /// </summary>
        public static MainWindowViewModel Instance { get; private set; }

        public MainWindowViewModel()
        {
            if (Instance != null)
            {
                throw new InvalidOperationException("本ViewModelは複数のｲﾝｽﾀﾝｽを作成することができません。");
            }

            Instance = this;

            // ﾃﾞﾌｫﾙﾄ画面
            // TODO 前回起動時の情報を保持する。
            Current = new SearchByRankingViewModel();

            // ﾒﾆｭｰ設定
            MenuItems = MenuModel.Instance.Children.ToSyncedSynchronizationContextCollection(
                model => new MenuItemViewModel(null, model),
                AnonymousSynchronizationContext.Current
            );

        }

        /// <summary>
        /// ﾀﾞｲｱﾛｸﾞ表示用ｲﾝｽﾀﾝｽ
        /// </summary>
        public IDialogCoordinator DialogCoordinator { get; set; }

        /// <summary>
        /// ｶﾚﾝﾄﾜｰｸｽﾍﾟｰｽ
        /// </summary>
        public WorkSpaceViewModel Current
        {
            get { return _Current; }
            set { SetProperty(ref _Current, value); }
        }
        private WorkSpaceViewModel _Current;

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
        /// Flyoutの状態 (true: 表示 / false: 非表示)
        /// </summary>
        public bool IsOpenFlyout
        {
            get { return _IsOpenFlyout; }
            set { SetProperty(ref _IsOpenFlyout, value); }
        }
        private bool _IsOpenFlyout;

        /// <summary>
        /// ﾒﾆｭｰ切替時処理
        /// </summary>
        public ICommand OnClickMenu
        {
            get
            {
                return _OnClickMenu = _OnClickMenu ?? new RelayCommand<MenuItemType>(
                    type =>
                    {
                        Current = MenuItems.Where(vm => vm.Type == type).First().WorkSpace;
                    },
                    type => {
                        return true;
                    });
            }
        }
        public ICommand _OnClickMenu;

        /// <summary>
        /// ﾒﾆｭｰ切替時処理
        /// </summary>
        public ICommand OnOpenFlyout
        {
            get
            {
                return _OnOpenFlyout = _OnOpenFlyout ?? new RelayCommand(
                    _ =>
                    {
                        IsOpenFlyout = true;
                    },
                    _ => {
                        return true;
                    });
            }
        }
        public ICommand _OnOpenFlyout;

    }
}
