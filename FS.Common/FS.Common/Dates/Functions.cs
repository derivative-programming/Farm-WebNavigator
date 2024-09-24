using System;
using System.Collections.Generic;
using System.Text;

namespace FS.Common.Dates
{
    public class Functions
    {

        public static DateTime ConvertFromUnixTimestamp(double timestamp)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return origin.AddSeconds(timestamp);
        }


        public static double ConvertToUnixTimestamp(DateTime date)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            TimeSpan diff = date - origin;
            return System.Math.Floor(diff.TotalSeconds);
        }
        public static bool XIsEarlierThanY(DateTime earlyDate, DateTime LaterDate)
        {
            if (System.DateTime.Compare(earlyDate, LaterDate) < 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool IsSameDateTime(DateTime date1, DateTime date2)
        {
                if (date1.Year == date2.Year &&
                    date1.Month == date2.Month &&
                    date1.Day == date2.Day &&
                    date1.Hour == date2.Hour &&
                    date1.Minute == date2.Minute &&
                    date1.Second == date2.Second)
                {
                    return true;
                }
                else
                {
                    return false;
                }
        }

        public static bool IsSameMinute(DateTime date1, DateTime date2)
        {
                if (date1.Year == date2.Year &&
                    date1.Month == date2.Month &&
                    date1.Day == date2.Day &&
                    date1.Hour == date2.Hour &&
                    date1.Minute == date2.Minute)
                {
                    return true;
                }
                else
                {
                    return false;
                }
        }

        public static bool IsSameHour(DateTime date1, DateTime date2)
        {
                if (date1.Year == date2.Year &&
                    date1.Month == date2.Month &&
                    date1.Day == date2.Day &&
                    date1.Hour == date2.Hour)
                {
                    return true;
                }
                else
                {
                    return false;
                }
        }

        public static bool IsSameDay(DateTime date1, DateTime date2)
        {
                if (date1.Year == date2.Year &&
                    date1.Month == date2.Month &&
                    date1.Day == date2.Day)
                {
                    return true;
                }
                else
                {
                    return false;
                }
        }

        public static bool IsSameMonth(DateTime date1, DateTime date2)
        {
                if (date1.Year == date2.Year &&
                    date1.Month == date2.Month)
                {
                    return true;
                }
                else
                {
                    return false;
                }
        }

        public static bool IsSameYear(DateTime date1, DateTime date2)
        {
            
                if (date1.Year == date2.Year)
                {
                    return true;
                }
                else
                {
                    return false;
                }
        }

        public static DateTime GetEST(DateTime val)
        {
            TimeZoneInfo easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            DateTime easternTime = TimeZoneInfo.ConvertTimeFromUtc(val, easternZone);
            return easternTime;
        }
    }
}
