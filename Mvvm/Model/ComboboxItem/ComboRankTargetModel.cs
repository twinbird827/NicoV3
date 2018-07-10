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
    public class ComboRankTargetModel : BindableBase
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
        public static ComboRankTargetModel Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new ComboRankTargetModel();
                }
                return _Instance;
            }
        }
        private static ComboRankTargetModel _Instance;

        private ComboRankTargetModel()
        {
            _Items = new ObservableSynchronizedCollection<ComboboxItemModel>
            {
                new ComboboxItemModel() { Value = "fav", Description = Resources.VM01063 },
                new ComboboxItemModel() { Value = "view", Description = Resources.VM01031 },
                new ComboboxItemModel() { Value = "res", Description = Resources.VM01032 },
                new ComboboxItemModel() { Value = "mylist", Description = Resources.VM01033 },
            };
        }
    }
}
