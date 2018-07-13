using NicoV3.Mvvm.Model;
using StatefulModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NicoV3.Mvvm.ViewModel
{
    public class SearchByFavMylistViewModel : WorkSpaceViewModel
    {
        private MenuItemModel Source { get; set; }

        public SearchByFavMylistViewModel(MenuItemModel source)
        {
            Source = source;

            Items = Source.Mylists.ToSyncedSynchronizationContextCollection(
                id => new SearchByFavMylistItemViewModel(id),
                AnonymousSynchronizationContext.Current
            );
        }

        /// <summary>
        /// ﾒｲﾝ項目ﾘｽﾄ
        /// </summary>
        public SynchronizationContextCollection<SearchByFavMylistItemViewModel> Items
        {
            get { return _Items; }
            set { SetProperty(ref _Items, value); }
        }
        private SynchronizationContextCollection<SearchByFavMylistItemViewModel> _Items;

    }
}
