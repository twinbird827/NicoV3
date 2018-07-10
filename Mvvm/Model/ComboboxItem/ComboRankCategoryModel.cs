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
    public class ComboRankCategoryModel : BindableBase
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
        public static ComboRankCategoryModel Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new ComboRankCategoryModel();
                }
                return _Instance;
            }
        }
        private static ComboRankCategoryModel _Instance;

        private ComboRankCategoryModel()
        {
            _Items = new ObservableSynchronizedCollection<ComboboxItemModel>
            {
                new ComboboxItemModel() { Value = "all", Description = Resources.VM01034 },
                new ComboboxItemModel() { Value = "music", Description = Resources.VM01035 },
                new ComboboxItemModel() { Value = "ent", Description = Resources.VM01036 },
                new ComboboxItemModel() { Value = "anime", Description = Resources.VM01037 },
                new ComboboxItemModel() { Value = "game", Description = Resources.VM01038 },
                new ComboboxItemModel() { Value = "animal", Description = Resources.VM01039 },
                new ComboboxItemModel() { Value = "que", Description = Resources.VM01040 },
                new ComboboxItemModel() { Value = "radio", Description = Resources.VM01041 },
                new ComboboxItemModel() { Value = "sport", Description = Resources.VM01042 },
                new ComboboxItemModel() { Value = "politics", Description = Resources.VM01043 },
                new ComboboxItemModel() { Value = "chat", Description = Resources.VM01044 },
                new ComboboxItemModel() { Value = "science", Description = Resources.VM01045 },
                new ComboboxItemModel() { Value = "history", Description = Resources.VM01046 },
                new ComboboxItemModel() { Value = "cooking", Description = Resources.VM01047 },
                new ComboboxItemModel() { Value = "nature", Description = Resources.VM01048 },
                new ComboboxItemModel() { Value = "diary", Description = Resources.VM01049 },
                new ComboboxItemModel() { Value = "dance", Description = Resources.VM01050 },
                new ComboboxItemModel() { Value = "sing", Description = Resources.VM01051 },
                new ComboboxItemModel() { Value = "play", Description = Resources.VM01052 },
                new ComboboxItemModel() { Value = "lecture", Description = Resources.VM01053 },
                new ComboboxItemModel() { Value = "tw", Description = Resources.VM01054 },
                new ComboboxItemModel() { Value = "other", Description = Resources.VM01055 },
                new ComboboxItemModel() { Value = "test", Description = Resources.VM01056 },
                new ComboboxItemModel() { Value = "r18", Description = Resources.VM01057 },
            };
        }
    }
}
