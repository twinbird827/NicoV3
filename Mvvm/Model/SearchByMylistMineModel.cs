using Codeplex.Data;
using NicoV3.Common;
using WpfUtilV1.Mvvm.Service;
using StatefulModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NicoV3.Mvvm.Model
{
    public class SearchByMylistMineModel : HttpModel
    {
        public static SearchByMylistMineModel Instance { get; } = new SearchByMylistMineModel();

        private SearchByMylistMineModel()
        {
            this.Method = "GET";
            this.ContentType = "application/x-www-form-urlencoded";
        }

        /// <summary>
        /// ｱｲﾃﾑ構成
        /// </summary>
        public ObservableSynchronizedCollection<string> Mylists
        {
            get { return _Mylists; }
            set { SetProperty(ref _Mylists, value); }
        }
        private ObservableSynchronizedCollection<string> _Mylists = new ObservableSynchronizedCollection<string>();

        public void Reload()
        {
            // ﾛｸﾞｲﾝする。
            LoginModel.Instance.Login();

            // ﾛｸﾞｲﾝできなかった場合
            if (!LoginModel.Instance.IsLogin)
            {
                ServiceFactory.MessageService.Error("Login error");
                return;
            }

            Mylists.Clear();

            var url = string.Format(Constants.MylistOfMe);
            var txt = GetSmileVideoHtmlText(url);

            ServiceFactory.MessageService.Debug(txt);

            // TODO ｴﾗｰﾁｪｯｸ

            var json = DynamicJson.Parse(txt);

            foreach (dynamic data in json["mylistgroup"])
            {
                if ((string)data["public"] == "1")
                {
                    // ﾏｲﾘｽﾄ情報を取得する。
                    MylistModel mylist = MylistStatusModel.Instance.GetMylist(data["id"]);

                    // ﾏｲﾘｽﾄ情報を状態に追加
                    MylistStatusModel.Instance.MylistMerge(mylist);

                    // 自身のﾘｽﾄに追加
                    Mylists.Add(mylist.MylistId);
                }
            }
        }
    }
}
