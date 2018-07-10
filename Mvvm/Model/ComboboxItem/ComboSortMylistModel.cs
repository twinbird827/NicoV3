using NicoV3.Properties;
using StatefulModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfUtilV1.Mvvm;

namespace NicoV3.Mvvm.Model.ComboboxItem
{
    public class ComboSortMylistModel : BindableBase
    {
        /// <summary>
        /// ｿｰﾄﾘｽﾄ構成
        /// </summary>
        public ObservableSynchronizedCollection<ComboboxItemModel> Items
        {
            get { return _Items; }
            set { SetProperty(ref _Items, value); }
        }
        private ObservableSynchronizedCollection<ComboboxItemModel> _Items;

        /// <summary>
        /// ｲﾝｽﾀﾝｽ (ｼﾝｸﾞﾙﾄﾝﾊﾟﾀｰﾝ)
        /// </summary>
        public static ComboSortMylistModel Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new ComboSortMylistModel();
                }
                return _Instance;
            }
        }
        private static ComboSortMylistModel _Instance;

        private ComboSortMylistModel()
        {
            _Items = new ObservableSynchronizedCollection<ComboboxItemModel>
            {
                new ComboboxItemModel() { Value = "0", Description = Resources.VM01013 },
                new ComboboxItemModel() { Value = "1", Description = Resources.VM01014 },
                new ComboboxItemModel() { Value = "2", Description = Resources.VM01015 },
                new ComboboxItemModel() { Value = "3", Description = Resources.VM01016 },
                new ComboboxItemModel() { Value = "4", Description = Resources.VM01017 },
                new ComboboxItemModel() { Value = "5", Description = Resources.VM01018 },
                new ComboboxItemModel() { Value = "6", Description = Resources.VM01019 },
                new ComboboxItemModel() { Value = "7", Description = Resources.VM01020 },
                new ComboboxItemModel() { Value = "8", Description = Resources.VM01021 },
                new ComboboxItemModel() { Value = "9", Description = Resources.VM01022 },
                new ComboboxItemModel() { Value = "10", Description = Resources.VM01023 },
                new ComboboxItemModel() { Value = "11", Description = Resources.VM01024 },
                new ComboboxItemModel() { Value = "12", Description = Resources.VM01025 },
                new ComboboxItemModel() { Value = "13", Description = Resources.VM01026 },
                new ComboboxItemModel() { Value = "14", Description = Resources.VM01027 },
                new ComboboxItemModel() { Value = "15", Description = Resources.VM01028 },
                new ComboboxItemModel() { Value = "16", Description = Resources.VM01029 },
                new ComboboxItemModel() { Value = "17", Description = Resources.VM01030 },
            };
        }
    }
}
