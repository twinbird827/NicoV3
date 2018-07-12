using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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


    }
}
