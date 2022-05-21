using System;

namespace Buform.Example.MvvmCross.Core
{
    public sealed class CreateEventModel
    {
        public string? Title { get; set; }
        public string? Location { get; set; }
        public bool IsAllDay { get; set; }
        public DateTime? StartsAt { get; set; } = DateTime.UtcNow;
        public DateTime? EndsAt { get; set; } = DateTime.UtcNow + TimeSpan.FromHours(1);
        public string? Url { get; set; }
        public string? Notes { get; set; }
    }
}