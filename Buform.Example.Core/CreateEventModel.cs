using System;

namespace Buform.Example.Core
{
    public sealed class CreateEventModel
    {
        public enum RepeatType
        {
            Never,
            EveryDay,
            EveryWeek,
            Every2Weeks,
            EveryMonth,
            EveryYear
        }

        public enum TravelTimeType
        {
            None,
            FiveMinutes,
            FifteenMinutes,
            ThirtyMinutes,
            OneHour,
            OneHourThirtyMinutes,
            TwoHours
        }

        public string? Title { get; set; }
        public string? Location { get; set; }
        public bool IsAllDay { get; set; }
        public DateTime? StartsAt { get; set; } = DateTime.UtcNow;
        public DateTime? EndsAt { get; set; } = DateTime.UtcNow + TimeSpan.FromHours(1);
        public RepeatType Repeat { get; set; }
        public TravelTimeType TravelTime { get; set; }
        public string? Url { get; set; }
        public string? Notes { get; set; }
    }
}