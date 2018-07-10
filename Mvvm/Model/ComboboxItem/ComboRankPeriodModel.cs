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
    public class ComboRankPeriodModel : BindableBase
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
        public static ComboRankPeriodModel Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new ComboRankPeriodModel();
                }
                return _Instance;
            }
        }
        private static ComboRankPeriodModel _Instance;

        private ComboRankPeriodModel()
        {
            _Items = new ObservableSynchronizedCollection<ComboboxItemModel>
            {
                new ComboboxItemModel() { Value = "hourly", Description = Resources.VM01058 },
                new ComboboxItemModel() { Value = "daily", Description = Resources.VM01059 },
                new ComboboxItemModel() { Value = "weekly", Description = Resources.VM01060 },
                new ComboboxItemModel() { Value = "monthly", Description = Resources.VM01061 },
                new ComboboxItemModel() { Value = "total", Description = Resources.VM01062 },
            };
        }
    }
}
