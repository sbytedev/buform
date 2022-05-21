using System;
using Foundation;

namespace Buform
{
    public static class DateTimeExtensions
    {
        private static readonly DateTime BaseDateTime = new(2001, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static NSDate ToDate(this DateTime date)
        {
            return NSDate.FromTimeIntervalSinceReferenceDate((date - BaseDateTime).TotalSeconds);
        }
    }
}