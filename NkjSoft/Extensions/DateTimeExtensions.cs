//--------------------------文档信息----------------------------
//       
//                 文件名: DateTimeExtensions                 
//                 CLR Version: 4.0.30319.1
//                 项目命名空间: NkjSoft.Extensions
//
//                 作  者: 俞如凯 
//                 Q Q: 250820436　 yurukai@vip.qq.com
//                 E-Mail: yurukai@hotmail.com
//                 创建时间 : 2010/8/1 9:29:01
//                 Copyright (c) Yurukai , All rights reserved.
//----------------------------------------------------------------


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NkjSoft.Extensions
{
    /// <summary>
    /// 对 <see cref="System.DateTime"/> 的扩展.
    /// </summary>
    public static class DateTimeExtensions
    {
        #region --- ForDateTime ---

        #region --- Prepare ---
        private static readonly TimeSpan _OneMinute = new TimeSpan(0, 1, 0);
        private static readonly TimeSpan _TwoMinutes = new TimeSpan(0, 2, 0);
        private static readonly TimeSpan _OneHour = new TimeSpan(1, 0, 0);
        private static readonly TimeSpan _TwoHours = new TimeSpan(2, 0, 0);
        private static readonly TimeSpan _OneDay = new TimeSpan(1, 0, 0, 0);
        private static readonly TimeSpan _TwoDays = new TimeSpan(2, 0, 0, 0);
        private static readonly TimeSpan _OneWeek = new TimeSpan(7, 0, 0, 0);
        private static readonly TimeSpan _TwoWeeks = new TimeSpan(14, 0, 0, 0);
        private static readonly TimeSpan _OneMonth = new TimeSpan(31, 0, 0, 0);
        private static readonly TimeSpan _TwoMonths = new TimeSpan(62, 0, 0, 0);
        private static readonly TimeSpan _OneYear = new TimeSpan(365, 0, 0, 0);
        private static readonly TimeSpan _TwoYears = new TimeSpan(730, 0, 0, 0);
        #endregion
        /// <summary>
        /// 返回当前时间实例和指定时间的时间差。
        /// </summary>
        /// <param name="startTime">当前时间</param>
        /// <param name="endTime">指定终点时间</param>
        /// <returns></returns>
        public static TimeSpan GetTimeSpan(this DateTime startTime, DateTime endTime)
        {
            return endTime.Subtract(startTime);
        }
        /// <summary>
        ///返回指定 <see cref="System.DateTime"/> 生日值的准确年龄值。
        /// </summary>
        /// <param name="dateOfBirth">生日值</param>
        /// <returns></returns>
        public static int CalculateAge(this DateTime dateOfBirth)
        {
            return CalculateAge(dateOfBirth, DateTime.Today);
        }
        /// <summary>
        /// 返回生日以指定参照日期为参照的年龄值。
        /// </summary>
        /// <param name="dateOfBirth">生日值</param>
        /// <param name="referenceDate">参照日期</param>
        /// <returns></returns>
        public static int CalculateAge(this DateTime dateOfBirth, DateTime referenceDate)
        {
            int years = referenceDate.Year - dateOfBirth.Year;
            if (referenceDate.Month < dateOfBirth.Month || (referenceDate.Month == dateOfBirth.Month && referenceDate.Day < dateOfBirth.Day)) --years;
            return years;
        }
        /// <summary>
        /// 获取当前日期月份中总天数。
        /// </summary>
        /// <param name="date">当前日期</param>
        /// <returns></returns>
        public static int GetCountDaysOfMonth(this DateTime date)
        {
            var nextMonth = date.AddMonths(1);
            return new DateTime(nextMonth.Year, nextMonth.Month, 1).AddDays(-1).Day;
        }
        /// <summary>
        /// 返回当前日期的第一天的日期值。
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns></returns>
        public static DateTime GetFirstDayOfMonth(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1);
        }
        /// <summary>
        /// 返回指定日期的第一个星期几的日期值。
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="dayOfWeek">星期几.</param>
        /// <returns></returns>
        public static DateTime GetFirstDayOfMonth(this DateTime date, DayOfWeek dayOfWeek)
        {
            var dt = date.GetFirstDayOfMonth();
            while (dt.DayOfWeek != dayOfWeek)
            { dt = dt.AddDays(1); }
            return dt;
        }
        /// <summary>
        /// 返回指定日期的最后一天的日期值。
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns></returns>
        public static DateTime GetLastDayOfMonth(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, GetCountDaysOfMonth(date));
        }
        /// <summary>
        /// 返回指定日期的最后一个星期几的日期值。
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="dayOfWeek">星期几.</param>
        /// <returns></returns>
        public static DateTime GetLastDayOfMonth(this DateTime date, DayOfWeek dayOfWeek)
        {
            var dt = date.GetLastDayOfMonth();
            while (dt.DayOfWeek != dayOfWeek) { dt = dt.AddDays(-1); } return dt;
        }

        /// <summary>
        /// 返回指定日期相对于当前时间的中文距离说明。比如：一分钟前，两小时前...
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns></returns>
        public static string ToAgo(this DateTime date)
        {
            TimeSpan timeSpan = date.GetTimeSpan(DateTime.Now);
            if (timeSpan < TimeSpan.Zero) return "未来";
            if (timeSpan < _OneMinute) return "现在";
            if (timeSpan < _TwoMinutes) return "1 分钟前";
            if (timeSpan < _OneHour) return String.Format("{0} 分钟前", timeSpan.Minutes);
            if (timeSpan < _TwoHours) return "1 小时前";
            if (timeSpan < _OneDay) return String.Format("{0} 小时前", timeSpan.Hours);
            if (timeSpan < _TwoDays) return "昨天";
            if (timeSpan < _OneWeek) return String.Format("{0} 天前", timeSpan.Days);
            if (timeSpan < _TwoWeeks) return "1 周前";
            if (timeSpan < _OneMonth) return String.Format("{0} 周前", timeSpan.Days / 7);
            if (timeSpan < _TwoMonths) return "1 月前";
            if (timeSpan < _OneYear) return String.Format("{0} 月前", timeSpan.Days / 31);
            if (timeSpan < _TwoYears) return "1 年前";
            return String.Format("{0} 年前", timeSpan.Days / 365);
        }

        /// <summary>
        /// 返回指定日期相对于当前时间的中文距离说明的简化版(只提示到月份，之后将显示以指定格式的日期值)。
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="outOfMonthStringFormatter">日期格式,默认: yyyy-MM-dd HH:mm:ss</param>
        /// <returns></returns>
        public static string ToAgo(this DateTime date, string outOfMonthStringFormatter)
        {
            TimeSpan timeSpan = date.GetTimeSpan(DateTime.Now);
            if (timeSpan < TimeSpan.Zero) return "未来";
            if (timeSpan < _OneMinute) return "现在";
            if (timeSpan < _TwoMinutes) return "1 分钟前";
            if (timeSpan < _OneHour) return String.Format("{0} 分钟前", timeSpan.Minutes);
            if (timeSpan < _TwoHours) return "1 小时前";
            if (timeSpan < _OneDay) return String.Format("{0} 小时前", timeSpan.Hours);
            if (timeSpan < _TwoDays) return "昨天";
            if (timeSpan < _OneWeek) return String.Format("{0} 天前", timeSpan.Days);
            if (timeSpan < _TwoWeeks) return "1 周前";
            if (timeSpan < _OneMonth) return String.Format("{0} 周前", timeSpan.Days / 7);
            if (timeSpan < _TwoMonths) return "1 月前";
            return date.ToString(outOfMonthStringFormatter);
        }


        /// <summary>
        ///  返回以" yyyy年MM月dd日 HH点mm分 "格式的时间字符串。
        /// </summary>
        /// <param name="date">当前时间实例</param>
        /// <returns></returns>
        public static string ToChineseFullLocal(this DateTime date)
        {
            return date.ToString(formatter_Chinese_NoneSecond);
        }
        /// <summary>
        /// 返回以" yyyy年MM月dd日 HH点mm分 "格式的时间字符串，该重载接收一个 <see cref="System.Bool"/> ，表示是否在返回结果中包含秒数。
        /// </summary>
        /// <param name="date">当前时间实例</param>
        /// <param name="includeSecond">是否包含秒数</param>
        /// <returns></returns>
        public static string ToChineseFullLocal(this DateTime date, bool includeSecond)
        {
            if (includeSecond)
                return date.ToString(formatter_Chinese_WithSecond);
            else
                return date.ToString(formatter_Chinese_NoneSecond);
        }

        /// <summary>
        /// 返回以当前年为起点的时间间隔表示的  MM/dd 字符串,如果在今年以前,则返回 yyyy/MM/dd 。
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string ToStringInThisYear(this DateTime date)
        {
            return ToStringInThisYear(date, false);
        }


        private static readonly string formatter = "yyyy/M/d";
        private static readonly string formatter2 = "M/d";
        private static readonly string formatter_Chinese = "yyyy年M月d日";
        private static readonly string formatter_Chinese_NoneSecond = "yyyy年M月d日 H点mm分";
        private static readonly string formatter_Chinese_WithSecond = "yyyy年M月d日 H点mm分ss秒";
        private static readonly string formatter_Chinese2 = "M月d日";
        /// <summary>
        /// 返回以当前年为起点的时间间隔表示的  MM/dd 格式的字符串,如果在今年以前,则返回 yyyy/MM-dd ，接收一个值，用于确定是否是中文输出。
        /// </summary>
        /// <param name="date">当前时间.</param>
        /// <param name="useChinese">是否中文输出</param>
        /// <returns></returns>
        public static string ToStringInThisYear(this DateTime date, bool useChinese)
        {
            if (date.Year >= DateTime.Now.Year)
            {
                return date.ToString(useChinese ? formatter_Chinese2 : formatter2);
            }
            else
                return date.ToString(formatter);
        }




        /// <summary>
        ///  返回中文格式的区分上午下午晚上的时间表示。
        /// </summary>
        /// <param name="date"> </param>
        /// <returns></returns>
        public static string ToShortChineseTimeAmPm(this DateTime date)
        {
            int hour = date.Hour;

            string amOrPm = string.Empty;
            if (hour >= 0 && hour < 1)
                amOrPm = "午夜";
            else if (hour >= 1 && hour <= 6)
                amOrPm = "凌晨";
            else if (hour > 6 && hour <= 12)
                amOrPm = "早上";
            else if (hour > 12 && hour < 13)
                amOrPm = "中午";
            else if (hour > 13 && hour <= 18)
                amOrPm = "下午";
            else if (hour > 18 && hour <= 23)
                amOrPm = "晚上";
            Console.WriteLine(hour);
            int hour1 = hour > 12 ? Math.Abs(12 - hour) : hour;
            return string.Format("{0}{1}点{2}", amOrPm, hour1, date.ToString("mm分"));
        }

        /// <summary>
        /// 返回中文表示的星期数.
        /// </summary>
        /// <param name="dayOfWeek">当前星期</param>
        /// <returns></returns>
        public static string ToChineseDayOfWeek(this DayOfWeek dayOfWeek)
        {
            string week = "";
            switch (dayOfWeek)
            {
                case DayOfWeek.Friday:
                    week = "星期五";
                    break;
                case DayOfWeek.Monday:
                    week = "星期一";
                    break;
                case DayOfWeek.Saturday:
                    week = "星期六";
                    break;
                case DayOfWeek.Sunday:
                    week = "星期日";
                    break;
                case DayOfWeek.Thursday:
                    week = "星期四";
                    break;
                case DayOfWeek.Tuesday:
                    week = "星期二";
                    break;
                case DayOfWeek.Wednesday:
                    week = "星期三";
                    break;
                default:
                    break;
            }
            return week;
        }
        #endregion
    }
}
