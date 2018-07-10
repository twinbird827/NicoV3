using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Xml.Linq;
using WpfUtilV1.Common;

namespace NicoV3.Common
{
    public class NicoDataConverter
    {
        public static DateTime ToDatetime(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return default(DateTime);
            }
            else
            {
                return DateTime.Parse(value);
            }
        }

        public static long ToLengthSeconds(string value)
        {
            var lengthSecondsIndex = 0;
            var lengthSeconds = value
                    .Split(':')
                    .Select(s => long.Parse(s))
                    .Reverse()
                    .Select(l => l * (60 ^ lengthSecondsIndex++))
                    .Sum();
            return lengthSeconds;
        }

        public static long ToLong(string value)
        {
            return long.Parse(value.Replace(",", ""));
        }

        public static DateTime FromUnixTime(long time)
        {
            return DateTimeOffset.FromUnixTimeSeconds(time).LocalDateTime;
        }

        /// <summary>
        /// 指定したUrlのｻﾑﾈｲﾙを取得する
        /// </summary>
        /// <param name="url">Url</param>
        /// <returns>ｻﾑﾈｲﾙ画像</returns>
        public static async Task<BitmapImage> ToThumbnail(string url)
        {
            return await HttpUtil.DownloadImageAsync(url, App.Current.Dispatcher);
        }

        /// <summary>
        /// Xmlｴﾚﾒﾝﾄから指定した名前に紐付くｶｳﾝﾀを取得します。
        /// </summary>
        /// <param name="e">Xmlｴﾚﾒﾝﾄ</param>
        /// <param name="name">ｶｳﾝﾀ名</param>
        /// <returns>ｶｳﾝﾀ</returns>
        public static long ToCounter(XElement e, string name)
        {
            var s = (string)e
                .Descendants("strong")
                .Where(x => (string)x.Attribute("class") == name)
                .FirstOrDefault();
            if (string.IsNullOrEmpty(s))
            {
                return 0;
            }
            else
            {
                return NicoDataConverter.ToLong(s);
            }
        }

    }
}
