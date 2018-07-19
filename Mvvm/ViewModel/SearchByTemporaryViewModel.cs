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
                        var result = await MainWindowViewModel.Instance.ShowInputAsync(
                            Resources.L_ADD,
                            Resources.M_ADD_TEMPORARY);

                        var id = result.Split('/').Last();

                        SearchByTemporaryModel.Instance.AddVideo(id);
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
                        var targets = Items
                            .Where(item => item.IsSelected)
                            .Select(item => item.VideoId);

                        foreach (var id in targets)
                        {
                            SearchByTemporaryModel.Instance.DeleteVideo(id);
                        }
                    });
            }
        }
        public ICommand _OnDelete;

    }
}
