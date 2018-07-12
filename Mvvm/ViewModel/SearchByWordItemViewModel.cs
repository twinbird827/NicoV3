﻿using NicoV3.Mvvm.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NicoV3.Mvvm.ViewModel
{
    public class SearchByWordItemViewModel : VideoViewModel
    {
        public SearchByWordItemViewModel(string id)
            : base(id)
        {

        }

        public SearchByWordItemViewModel(VideoModel model)
            : base(model)
        {

        }
    }
}
