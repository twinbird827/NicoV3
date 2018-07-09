using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NicoV3.Mvvm.Service
{
    public interface IMessageService
    {
        /// <summary>
        /// ｴﾗｰをﾒｯｾｰｼﾞ処理します。
        /// </summary>
        /// <param name="message">ﾒｯｾｰｼﾞ</param>
        void Error(string message);

        /// <summary>
        /// 情報をﾒｯｾｰｼﾞ処理します。
        /// </summary>
        /// <param name="message">ﾒｯｾｰｼﾞ</param>
        void Info(string message);

        /// <summary>
        /// ﾃﾞﾊﾞｯｸﾞﾒｯｾｰｼﾞ処理します。
        /// </summary>
        /// <param name="message">ﾒｯｾｰｼﾞ</param>
        void Debug(string message);

    }
}
