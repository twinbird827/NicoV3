using NicoV3.Common;
using NicoV3.Mvvm.Model;
using NicoV3.Properties;
using StatefulModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfUtilV1.Mvvm;
using WpfUtilV1.Mvvm.Service;

namespace NicoV3.Mvvm.ViewModel
{
    public class SearchByFavMylistViewModel : WorkSpaceViewModel
    {
        private MenuItemModel Source { get; set; }

        public SearchByFavMylistViewModel(MenuItemModel source)
        {
            Source = source;

            Items = Source.Mylists.ToSyncedSynchronizationContextCollection(
                id => new SearchByFavMylistItemViewModel(Source, id),
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

        /// <summary>
        /// 追加処理
        /// </summary>
        public ICommand OnAdd
        {
            get
            {
                return _OnAdd = _OnAdd ?? new RelayCommand(
                async _ =>
                {
                    string result = await MainWindowViewModel.Instance.ShowInputAsync(
                        Resources.L_ADD_MYLIST,
                        Resources.M_ADD_MYLIST);

                    MylistModel mylist;
                    try
                    {
                        var tmp = MylistStatusModel.Instance.GetMylist(NicoDataConverter.ToId(result));
                        mylist = tmp;
                    }
                    catch
                    {
                        ServiceFactory.MessageService.Error("有効なUrlを指定してください。");
                        return;
                    }

                    Source.Mylists.Add(mylist.MylistId);
                });
            }
        }
        public ICommand _OnAdd;

        /// <summary>
        /// 削除処理
        /// </summary>
        public ICommand OnDelete
        {
            get
            {
                return _OnDelete = _OnDelete ?? new RelayCommand(
                _ =>
                {
                    foreach (var item in Items.Where(i => i.IsSelected))
                    {
                        Source.Mylists.Remove(item.MylistId);
                    }
                },
                _ =>
                {
                    return Items.Any(i => i.IsSelected);
                });
            }
        }
        public ICommand _OnDelete;

    }
}
