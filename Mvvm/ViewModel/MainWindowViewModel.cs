using MahApps.Metro.Controls.Dialogs;
using NicoV3.Common;
using NicoV3.Mvvm.Model;
using StatefulModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;
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

        /// <summary>
        /// ｺﾝｽﾄﾗｸﾀ
        /// </summary>
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

            MylistUpdateTimer = new DispatcherTimer(DispatcherPriority.Normal, App.Current.Dispatcher);
            MylistUpdateTimer.Interval = TimeSpan.FromMilliseconds(Variables.MylistUpdateInterval);
            MylistUpdateTimer.Tick += MylistUpdateTimer_Tick;
            MylistUpdateTimer.Start();
        }

        /// <summary>
        /// ﾀﾞｲｱﾛｸﾞ表示用ｲﾝｽﾀﾝｽ
        /// </summary>
        public IDialogCoordinator DialogCoordinator { get; set; }

        /// <summary>
        /// ﾏｲﾘｽﾄ確認用ﾀｲﾏｰ
        /// </summary>
        private DispatcherTimer MylistUpdateTimer { get; set; }

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

        /// <summary>
        /// ﾒｯｾｰｼﾞﾀﾞｲｱﾛｸﾞを表示します。
        /// </summary>
        /// <param name="title">ﾀｲﾄﾙ</param>
        /// <param name="message">ﾒｯｾｰｼﾞ</param>
        /// <param name="style">ﾀﾞｲｱﾛｸﾞｽﾀｲﾙ</param>
        /// <param name="settings">設定情報</param>
        /// <returns><code>MessageDialogResult</code></returns>
        public async Task<MessageDialogResult> ShowMessageAsync(string title, string message, MessageDialogStyle style = MessageDialogStyle.Affirmative, MetroDialogSettings settings = null)
        {
            return await DialogCoordinator.ShowMessageAsync(this, title, message, style, settings);
        }

        /// <summary>
        /// 入力ﾀﾞｲｱﾛｸﾞを表示します。
        /// </summary>
        /// <param name="title">ﾀｲﾄﾙ</param>
        /// <param name="message">ﾒｯｾｰｼﾞ</param>
        /// <param name="settings">設定情報</param>
        /// <returns>入力値</returns>
        public async Task<string> ShowInputAsync(string title, string message, MetroDialogSettings settings = null)
        {
            return await DialogCoordinator.ShowInputAsync(this, title, message, settings);
        }

        /// <summary>
        /// ｶｽﾀﾑﾀﾞｲｱﾛｸﾞを表示します。
        /// </summary>
        /// <param name="dialog">ｶｽﾀﾑﾀﾞｲｱﾛｸﾞのｲﾝｽﾀﾝｽ</param>
        /// <param name="settings">設定情報</param>
        /// <returns><code>Task</code></returns>
        public Task ShowMetroDialogAsync(BaseMetroDialog dialog, MetroDialogSettings settings = null)
        {
            return DialogCoordinator.ShowMetroDialogAsync(this, dialog, settings);
        }

        /// <summary>
        /// ｶｽﾀﾑﾀﾞｲｱﾛｸﾞを非表示にします。
        /// </summary>
        /// <param name="dialog">ｶｽﾀﾑﾀﾞｲｱﾛｸﾞのｲﾝｽﾀﾝｽ</param>
        /// <param name="settings">設定情報</param>
        /// <returns><code>Task</code></returns>
        public Task HideMetroDialogAsync(BaseMetroDialog dialog, MetroDialogSettings settings = null)
        {
            return DialogCoordinator.HideMetroDialogAsync(this, dialog, settings);
        }

        protected override void OnDisposed()
        {
            base.OnDisposed();

            MenuModel.Instance.Dispose();
        }

        private async void MylistUpdateTimer_Tick(object sender, EventArgs e)
        {
            // ﾃﾝﾎﾟﾗﾘに追加するﾋﾞﾃﾞｵのﾘｽﾄを取得
            var videos = await GetVideosByAddTemporaryAsync();

            // ﾃﾝﾎﾟﾗﾘに追加
            await Task.Run(() =>
            {
                foreach (var video in videos)
                {
                    SearchByTemporaryModel.Instance.AddVideo(video.VideoId);
                }
            });
            // UIｽﾚｯﾄﾞでﾃﾝﾎﾟﾗﾘに追加
            //await App.Current.Dispatcher.InvokeAsync(() =>
            //{
            //    foreach (var video in videos)
            //    {
            //        SearchByTemporaryModel.Instance.AddVideo(video.VideoId);
            //    }
            //});
        }

        /// <summary>
        /// ﾃﾝﾎﾟﾗﾘに追加するﾋﾞﾃﾞｵのﾘｽﾄを取得します。
        /// </summary>
        /// <returns>ﾃﾝﾎﾟﾗﾘに追加するﾋﾞﾃﾞｵのﾘｽﾄ</returns>
        private async Task<IEnumerable<VideoModel>> GetVideosByAddTemporaryAsync()
        {
            return await Task.Run(
                () =>
                {
                    var lastConfirmTime = Variables.MylistUpdateDatetime;

                    Variables.MylistUpdateDatetime = DateTime.Now;

                var videos = GetAllMenu(MenuModel.Instance.Children)
                        .SelectMany(m => m.Mylists)
                        .Select(m => { Thread.Sleep(500); return MylistStatusModel.Instance.GetMylist(m); })
                        .SelectMany(m => m.Videos)
                        .Where(v => !SearchByTemporaryModel.Instance.Videos.Any(t => t == v))
                        .Select(v => { Thread.Sleep(500); return VideoStatusModel.Instance.GetVideo(v); })
                        .Where(v => lastConfirmTime < v.LastUpdateTime);

                    return videos;
                });
        }

        /// <summary>
        /// 全てのﾒﾆｭｰを取得します。
        /// </summary>
        /// <param name="menues">親ﾒﾆｭｰ</param>
        /// <returns>全てのﾒﾆｭｰ</returns>
        private IEnumerable<MenuItemModel> GetAllMenu(IEnumerable<MenuItemModel> menues)
        {
            if (menues.Any())
            {
                return menues.Union(GetAllMenu(menues.SelectMany(m => m.Children)));
            }
            else
            {
                return menues;
            }
        }
    }
}
