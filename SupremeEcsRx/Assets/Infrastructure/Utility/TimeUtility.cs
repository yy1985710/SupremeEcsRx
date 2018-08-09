using System;
using UnityEngine;
using System.Collections;

namespace EcsRx.Utility
{
    public static class TimeUtility
    {

        public static string TimestampToDateTimeString(long timestamp, string format)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 当地时区
            TimeSpan toNow = new TimeSpan(timestamp * 10000);
            var dt = startTime.Add(toNow);
            return dt.ToString(format);
        }
    }

}

