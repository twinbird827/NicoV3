using StatefulModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using WpfUtilV1.Mvvm;

namespace NicoV3.Mvvm.Model
{
    [DataContract]
    public class MenuItemModel : BindableBase
    {
        /// <summary>
        /// ｺﾝｽﾄﾗｸﾀ
        /// </summary>
        public MenuItemModel(string name, MenuItemType type)
        {
            Name = name;
            Type = type;
        }

        /// <summary>
        /// ｱｲﾃﾑ名
        /// </summary>
        [DataMember]
        public string Name
        {
            get { return _Name; }
            set { SetProperty(ref _Name, value); }
        }
        private string _Name = null;

        /// <summary>
        /// ﾒﾆｭｰの種類
        /// </summary>
        [DataMember]
        public MenuItemType Type
        {
            get { return _Type; }
            set { SetProperty(ref _Type, value); }
        }
        private MenuItemType _Type;

        /// <summary>
        /// 子ﾒﾆｭｰ
        /// </summary>
        [DataMember]
        public ObservableSynchronizedCollection<MenuItemModel> Children
        {
            get { return _Children; }
            set { SetProperty(ref _Children, value); }
        }
        private ObservableSynchronizedCollection<MenuItemModel> _Children = new ObservableSynchronizedCollection<MenuItemModel>();

        /// <summary>
        /// ﾒﾆｭｰに紐づくﾏｲﾘｽﾄ情報
        /// </summary>
        [DataMember]
        public ObservableSynchronizedCollection<string> Mylists
        {
            get { return _Mylists; }
            set { SetProperty(ref _Mylists, value); }
        }
        private ObservableSynchronizedCollection<string> _Mylists = new ObservableSynchronizedCollection<string>();

    }
}
