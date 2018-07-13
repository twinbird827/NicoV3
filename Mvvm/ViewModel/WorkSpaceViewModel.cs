using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfUtilV1.Mvvm.ViewModel;

namespace NicoV3.Mvvm.ViewModel
{
    public class WorkSpaceViewModel : ViewModelBase
    {
        protected IDialogCoordinator DC { get { return MainWindowViewModel.Instance.DialogCoordinator; } }
    }
}
