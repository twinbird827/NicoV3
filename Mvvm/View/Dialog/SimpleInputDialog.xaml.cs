using MahApps.Metro.Controls.Dialogs;
using NicoV3.Mvvm.ViewModel.Dialog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NicoV3.Mvvm.View.Dialog
{
    /// <summary>
    /// SimpleInputDialog.xaml の相互作用ロジック
    /// </summary>
    public partial class SimpleInputDialog : BaseMetroDialog
    {
        public SimpleInputDialog(SimpleInputDialogViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}
