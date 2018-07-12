using NicoV3.Mvvm.Model;
using NicoV3.Mvvm.Model.ComboboxItem;
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
    public class SearchByRankingViewModel : WorkSpaceViewModel
    {
        /// <summary>
        /// 本ｲﾝｽﾀﾝｽのﾃﾞｰﾀ実体
        /// </summary>
        public SearchByRankingModel Source { get; set; }

        /// <summary>
        /// ｺﾝｽﾄﾗｸﾀ
        /// </summary>
        public SearchByRankingViewModel() : this(SearchByRankingModel.Instance)
        {

        }

        /// <summary>
        /// ｺﾝｽﾄﾗｸﾀ
        /// </summary>
        public SearchByRankingViewModel(SearchByRankingModel model)
        {
            Source = model;

            Items = Source.Videos.ToSyncedSynchronizationContextCollection(
                id => new SearchByRankingItemViewModel(id), 
                AnonymousSynchronizationContext.Current
            );

            ComboTargetItems = ComboRankTargetModel
                .Instance
                .Items
                .ToSyncedSynchronizationContextCollection(m => m, AnonymousSynchronizationContext.Current);
            SelectedComboTargetItem = ComboTargetItems.First();

            ComboPeriodItems = ComboRankPeriodModel
                .Instance
                .Items
                .ToSyncedSynchronizationContextCollection(m => m, AnonymousSynchronizationContext.Current);
            SelectedComboPeriodItem = ComboPeriodItems.First();

            ComboCategoryItems = ComboRankCategoryModel
                .Instance
                .Items
                .ToSyncedSynchronizationContextCollection(m => m, AnonymousSynchronizationContext.Current);
            SelectedComboCategoryItem = ComboCategoryItems.First();
        }

        /// <summary>
        /// 対象ﾘｽﾄ
        /// </summary>
        public SynchronizationContextCollection<ComboboxItemModel> ComboTargetItems
        {
            get { return _ComboTargetItems; }
            set { SetProperty(ref _ComboTargetItems, value); }
        }
        private SynchronizationContextCollection<ComboboxItemModel> _ComboTargetItems;

        /// <summary>
        /// 選択中の対象項目
        /// </summary>
        public ComboboxItemModel SelectedComboTargetItem
        {
            get { return _SelectedComboTargetItem; }
            set { SetProperty(ref _SelectedComboTargetItem, value); }
        }
        private ComboboxItemModel _SelectedComboTargetItem;

        /// <summary>
        /// 期間ﾘｽﾄ
        /// </summary>
        public SynchronizationContextCollection<ComboboxItemModel> ComboPeriodItems
        {
            get { return _ComboPeriodItems; }
            set { SetProperty(ref _ComboPeriodItems, value); }
        }
        private SynchronizationContextCollection<ComboboxItemModel> _ComboPeriodItems;

        /// <summary>
        /// 選択中の期間項目
        /// </summary>
        public ComboboxItemModel SelectedComboPeriodItem
        {
            get { return _SelectedComboPeriodItem; }
            set { SetProperty(ref _SelectedComboPeriodItem, value); }
        }
        private ComboboxItemModel _SelectedComboPeriodItem;

        /// <summary>
        /// ｶﾃｺﾞﾘﾘｽﾄ
        /// </summary>
        public SynchronizationContextCollection<ComboboxItemModel> ComboCategoryItems
        {
            get { return _ComboCategoryItems; }
            set { SetProperty(ref _ComboCategoryItems, value); }
        }
        private SynchronizationContextCollection<ComboboxItemModel> _ComboCategoryItems;

        /// <summary>
        /// 選択中のｶﾃｺﾞﾘ項目
        /// </summary>
        public ComboboxItemModel SelectedComboCategoryItem
        {
            get { return _SelectedComboCategoryItem; }
            set { SetProperty(ref _SelectedComboCategoryItem, value); }
        }
        private ComboboxItemModel _SelectedComboCategoryItem;

        /// <summary>
        /// ﾒｲﾝ項目ﾘｽﾄ
        /// </summary>
        public SynchronizationContextCollection<SearchByRankingItemViewModel> Items
        {
            get { return _Items; }
            set { SetProperty(ref _Items, value); }
        }
        private SynchronizationContextCollection<SearchByRankingItemViewModel> _Items;

        /// <summary>
        /// 検索処理
        /// </summary>
        public ICommand OnSearch
        {
            get
            {
                return _OnSearch = _OnSearch ?? new RelayCommand(
              _ =>
              {
                  // 入力値をﾓﾃﾞﾙにｾｯﾄ
                  Source.Target = this.SelectedComboTargetItem.Value;
                  Source.Period = this.SelectedComboPeriodItem.Value;
                  Source.Category = this.SelectedComboCategoryItem.Value;

                  // 検索実行
                  this.Source.Reload();
              },
              _ => {
                  return true;
              });
            }
        }
        public ICommand _OnSearch;

    }
}
