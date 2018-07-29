using StatefulModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfUtilV1.Mvvm;

namespace NicoV3.Mvvm.Model
{
    public class MylistStatusModel : BindableBase
    {
        public static MylistStatusModel Instance { get; } = new MylistStatusModel();

        private MylistStatusModel()
        {
            // ｼﾝｸﾞﾙﾄﾝﾊﾟﾀｰﾝ
        }

        /// <summary>
        /// ｱｲﾃﾑ構成
        /// </summary>
        public ObservableSynchronizedCollection<MylistModel> Mylists
        {
            get { return _Mylists; }
            set { SetProperty(ref _Mylists, value); }
        }
        private ObservableSynchronizedCollection<MylistModel> _Mylists = new ObservableSynchronizedCollection<MylistModel>();

        /// <summary>
        /// ﾏｲﾘｽﾄ情報をﾏｰｼﾞします。
        /// </summary>
        /// <param name="mylist">ﾏｲﾘｽﾄ情報</param>
        public void MylistMerge(MylistModel mylist)
        {
            if (Mylists.Any(m => m.MylistId == mylist.MylistId))
            {
                var m = Mylists.First(temp => temp.MylistId == mylist.MylistId);

                // 更新要素を再設定
                m.MylistDate = mylist.MylistDate;
                m.Videos.Clear();
                foreach (var video in mylist.Videos)
                {
                    m.Videos.Add(video);
                }
            }
            else
            {
                // 追加
                Mylists.Add(mylist);
            }
        }

        /// <summary>
        /// ﾏｲﾘｽﾄ情報を取得します。
        /// </summary>
        /// <param name="id">取得したいﾏｲﾘｽﾄID</param>
        /// <param name="reload">既存情報をﾘﾛｰﾄﾞするか</param>
        /// <returns>ﾏｲﾘｽﾄ情報</returns>
        public MylistModel GetMylist(string id, bool reload = false)
        {
            var mylist = Mylists.FirstOrDefault(m => m.MylistId == id);

            if (mylist != null)
            {
                if (reload)
                {
                    mylist.Reload();
                }
                return mylist;
            }
            else
            {
                var tmp = new MylistModel(id);

                Mylists.Add(tmp);

                return tmp;
            }
        }
    }
}
