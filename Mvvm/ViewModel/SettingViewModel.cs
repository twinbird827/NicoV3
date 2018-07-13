using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NicoV3.Mvvm.ViewModel
{
    public class SettingViewModel : WorkSpaceViewModel
    {
        public static SettingViewModel Instance { get; private set; } = new SettingViewModel();

        private SettingViewModel()
        {

        }
    }
}
