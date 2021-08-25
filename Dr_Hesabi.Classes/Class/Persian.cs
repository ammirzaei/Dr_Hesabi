using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Dr_Hesabi.Classes.Class
{
    public static class Persian
    {
        public static DateTime DateNow()
        {
            PersianCalendar PC = new PersianCalendar();
            return DateTime.Parse(PC.GetYear(DateTime.Now) + "/" + PC.GetMonth(DateTime.Now).ToString("00") + "/" +
                   PC.GetDayOfMonth(DateTime.Now).ToString("00"));
        }

        public static string ToDate(this DateTime date)
        {
            PersianCalendar PC = new PersianCalendar();
            return PC.GetYear(date) + "/" + PC.GetMonth(date).ToString("00") + "/" +
                   PC.GetDayOfMonth(date).ToString("00");
        }

        public static string ToTime(this DateTime time)
        {
            PersianCalendar PC = new PersianCalendar();
            return PC.GetHour(time).ToString("00") + ":" + PC.GetMinute(time).ToString("00") + ":" + PC.GetSecond(time).ToString("00");
        }

        public static string GetSocond(this DateTime time)
        {
            var second = (time.Minute * 60) + (time.Hour * 3600) + time.Second;
            return second.ToString();
        }

        public static string ToDateTime(this DateTime date)
        {
            PersianCalendar Pc = new PersianCalendar();
            return Pc.GetHour(date).ToString("00") + ":" + Pc.GetMinute(date).ToString("00") + ":" +
                   Pc.GetSecond(date).ToString("00") + " " + Pc.GetYear(date) + "/" + Pc.GetMonth(date).ToString("00") + "/" +
                Pc.GetDayOfMonth(date).ToString("00");
        }
        public static DateTime ToDateTimeM(this DateTime date)
        {
            PersianCalendar Pc = new PersianCalendar();
            DateTime Date = Pc.ToDateTime(date.Year, date.Month, date.Day, 0, 0, 0, 0);
            return Date;
        }
        public static DateTime ToDateTimeM2(this DateTime date)
        {
            PersianCalendar Pc = new PersianCalendar();
            DateTime Date = Pc.ToDateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second, date.Millisecond);
            return Date;
        }
    }
}
