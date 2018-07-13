using NicoV3.Mvvm.Model;
using StatefulModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NicoV3.Mvvm.ViewModel
{
    public class SearchByTemporaryViewModel : WorkSpaceViewModel
    {
        /// <summary>
        /// 本ｲﾝｽﾀﾝｽのﾃﾞｰﾀ実体
        /// </summary>
        private SearchByTemporaryModel Source { get; set; }

        /// <summary>
        /// ｺﾝｽﾄﾗｸﾀ
        /// </summary>
        public SearchByTemporaryViewModel() : this(SearchByTemporaryModel.Instance)
        {

        }

        /// <summary>
        /// ｺﾝｽﾄﾗｸﾀ
        /// </summary>
        public SearchByTemporaryViewModel(SearchByTemporaryModel model)
        {
            Source = model;

            Items = Source.Videos.ToSyncedSynchronizationContextCollection(
                id => new SearchByTemporaryItemViewModel(id), 
                AnonymousSynchronizationContext.Current
            );
        }

        /// <summary>
        /// ﾒｲﾝ項目ﾘｽﾄ
        /// </summary>
        public SynchronizationContextCollection<SearchByTemporaryItemViewModel> Items
        {
            get { return _Items; }
            set { SetProperty(ref _Items, value); }
        }
        private SynchronizationContextCollection<SearchByTemporaryItemViewModel> _Items;

    }
}
