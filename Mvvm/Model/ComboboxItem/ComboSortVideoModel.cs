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
    public class ComboSortVideoModel : BindableBase
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
        public static ComboSortVideoModel Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new ComboSortVideoModel();
                }
                return _Instance;
            }
        }
        private static ComboSortVideoModel _Instance;

        private ComboSortVideoModel()
        {
            _Items = new ObservableSynchronizedCollection<ComboboxItemModel>
            {
                //new SortItemModel() { Keyword = "-title", Description = "-title" },
                //new SortItemModel() { Keyword = "%2btitle", Description = "+title" },
                //new SortItemModel() { Keyword = "-description", Description = "-description" },
                //new SortItemModel() { Keyword = "%2bdescription", Description = "+description" },
                //new SortItemModel() { Keyword = "-tags", Description = "-tags" },
                //new SortItemModel() { Keyword = "%2btags", Description = "+tags" },
                //new SortItemModel() { Keyword = "-categoryTags", Description = "-categoryTags" },
                //new SortItemModel() { Keyword = "%2bcategoryTags", Description = "+categoryTags" },
                new ComboboxItemModel() { Value = "-viewCounter", Description = Resources.VM01001 },
                new ComboboxItemModel() { Value = "%2bviewCounter", Description = Resources.VM01002 },
                new ComboboxItemModel() { Value = "-mylistCounter", Description = Resources.VM01003 },
                new ComboboxItemModel() { Value = "%2bmylistCounter", Description = Resources.VM01004 },
                new ComboboxItemModel() { Value = "-commentCounter", Description = Resources.VM01005 },
                new ComboboxItemModel() { Value = "%2bcommentCounter", Description = Resources.VM01006 },
                new ComboboxItemModel() { Value = "-startTime", Description = Resources.VM01007 },
                new ComboboxItemModel() { Value = "%2bstartTime", Description = Resources.VM01008 },
                new ComboboxItemModel() { Value = "-lastCommentTime", Description = Resources.VM01009 },
                new ComboboxItemModel() { Value = "%2blastCommentTime", Description = Resources.VM01010 },
                new ComboboxItemModel() { Value = "-lengthSeconds", Description = Resources.VM01011 },
                new ComboboxItemModel() { Value = "%2blengthSeconds", Description = Resources.VM01012 }
            };
        }
    }
}
