using MahApps.Metro.Controls.Dialogs;
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
using System.Windows.Shapes;

namespace NicoV3.Mvvm.Dialog
{
    /// <summary>
    /// MylistSelectDialog.xaml の相互作用ロジック
    /// </summary>
    public partial class MylistSelectDialog : BaseMetroDialog
    {
        public MylistSelectDialog()
            : this(new MylistSelectDialogViewModel())
        {

        }

        public MylistSelectDialog(MylistSelectDialogViewModel vm)
        {
            InitializeComponent();

            DataContext = vm;
        }
    }
}
