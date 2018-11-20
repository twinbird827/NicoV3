using NicoV3.Common;
using StatefulModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfUtilV1.Common;
using WpfUtilV1.Mvvm;

namespace NicoV3.Mvvm.Model
{
    public class MenuModel : BindableBase
    {
        public static MenuModel Instance { get; } = CreateInstance();

        private MenuModel()
        {

        }

        /// <summary>
        /// 子ﾒﾆｭｰ
        /// </summary>
        public ObservableSynchronizedCollection<MenuItemModel> Children
        {
            get { return _Children; }
            set { SetProperty(ref _Children, value); }
        }
        private ObservableSynchronizedCollection<MenuItemModel> _Children = new ObservableSynchronizedCollection<MenuItemModel>();

        private static MenuModel CreateInstance()
        {
            var instance = JsonUtil.Deserialize<MenuModel>(Constants.MenuModelPath);

            if (instance != null)
            {
                return instance;
            }
            else
            {
                // 初期ﾒﾆｭｰ作成
                // TODO 名前をﾘｿｰｽにする。
                return new MenuModel()
                {
                    Children = new ObservableSynchronizedCollection<MenuItemModel>()
                    {
                        new MenuItemModel("SearchByWord", MenuItemType.SearchByWord),
                        new MenuItemModel("Ranking", MenuItemType.Ranking),
                        new MenuItemModel("Temporary", MenuItemType.Temporary),
                        new MenuItemModel("SearchByMylist", MenuItemType.SearchByMylist),
                        new MenuItemModel("MyListOfMe", MenuItemType.MylistOfMe),
                        new MenuItemModel("MylistOfOther", MenuItemType.MylistOfOther),
                        new MenuItemModel("Setting", MenuItemType.Setting)
                    }
                };
            }
        }

        protected override void OnDisposing()
        {
            base.OnDisposing();

            // ｲﾝｽﾀﾝｽのﾃﾞｰﾀを保存する。
            JsonUtil.Serialize(Constants.MenuModelPath, this);
        }

        protected override void OnDisposed()
        {
            base.OnDisposed();

            if (Children != null)
            {
                Children.Clear();
                Children = null;
            }
        }

    }
}
