using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Xml.Linq;
using WpfUtilV1.Common;

namespace NicoV3.Common
{
    public static class NicoDataConverter
    {
        private static Stopwatch LastGetThumnail { get; set; }

        /// <summary>
        /// 静的ｺﾝｽﾄﾗｸﾀ
        /// </summary>
        static NicoDataConverter()
        {
            LastGetThumnail = new Stopwatch();
            LastGetThumnail.Start();
        }

        /// <summary>
        /// 文字をDateTimeに変換します。
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
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

        /// <summary>
        /// HH:mm:ss形式の文字を合算した秒に変換します。
        /// </summary>
        /// <param name="value">HH:mm:ss形式の文字</param>
        /// <returns>合算した秒</returns>
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

        /// <summary>
        /// 指定した文字をlong値に変換します。
        /// </summary>
        /// <param name="value">文字</param>
        /// <returns>long値</returns>
        public static long ToLong(string value)
        {
            return long.Parse(value.Replace(",", ""));
        }

        /// <summary>
        /// Unix時間(long)からDateTimeに変換します。
        /// </summary>
        /// <param name="time">Unix時間(long)</param>
        /// <returns><code>DateTime</code></returns>
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
            
            return 
                await Task.Run(async () => 
                {
                    if (LastGetThumnail.IsRunning)
                    {
                        while (LastGetThumnail.ElapsedMilliseconds < 500) { }
                    }
                    LastGetThumnail.Restart();

                    return await HttpUtil.DownloadImageAsync(url, App.Current.Dispatcher);
                });
            
            //return await HttpUtil.DownloadImageAsync(url, App.Current.Dispatcher);
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

        /// <summary>
        /// 指定したﾜｰﾄﾞをIDに変換します。
        /// </summary>
        /// <param name="word">ﾜｰﾄﾞ</param>
        /// <returns>ID</returns>
        public static string ToId(string word)
        {
            return word?.Split('/').Last();
        }
    }
}
