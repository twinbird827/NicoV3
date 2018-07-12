using NicoV3.Mvvm.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NicoV3.Mvvm.ViewModel
{
    public class SearchByTemporaryItemViewModel : VideoViewModel
    {
        public SearchByTemporaryItemViewModel(string id)
            : base(id)
        {

        }

        public SearchByTemporaryItemViewModel(VideoModel model)
            : base(model)
        {

        }

    }
}
