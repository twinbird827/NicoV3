using NicoV3.Mvvm.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NicoV3.Mvvm.ViewModel
{
    public class SearchByFavMylistItemViewModel : MylistViewModel
    {
        public SearchByFavMylistItemViewModel(string id)
            : base(id)
        {

        }

        public SearchByFavMylistItemViewModel(MylistModel model)
            : base(model)
        {

        }

    }
}
