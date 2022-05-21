using System;
using Foundation;

namespace Buform
{
    public static class NsDateExtensions
    {
        private static readonly DateTime BaseDateTime = new(2001, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static DateTime ToDateTime(this NSDate date)
        {
            return BaseDateTime.AddSeconds(date.SecondsSinceReferenceDate);
        }
    }
}