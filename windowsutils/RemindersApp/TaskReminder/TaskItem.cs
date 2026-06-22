using System;
using System.Collections.Generic;

namespace TaskReminder
{
    public enum ReminderOffset
    {
        FiveMinutes,
        FifteenMinutes,
        ThirtyMinutes,
        OneHour,
        TwoHours,
        FourHours,
        OneDay,
        TwoDays,
        OneWeek
    }

    public static class ReminderOffsetExtensions
    {
        public static TimeSpan ToTimeSpan(this ReminderOffset offset) => offset switch
        {
            ReminderOffset.FiveMinutes    => TimeSpan.FromMinutes(5),
            ReminderOffset.FifteenMinutes => TimeSpan.FromMinutes(15),
            ReminderOffset.ThirtyMinutes  => TimeSpan.FromMinutes(30),
            ReminderOffset.OneHour        => TimeSpan.FromHours(1),
            ReminderOffset.TwoHours       => TimeSpan.FromHours(2),
            ReminderOffset.FourHours      => TimeSpan.FromHours(4),
            ReminderOffset.OneDay         => TimeSpan.FromDays(1),
            ReminderOffset.TwoDays        => TimeSpan.FromDays(2),
            ReminderOffset.OneWeek        => TimeSpan.FromDays(7),
            _                             => TimeSpan.Zero
        };

        public static string ToDisplayString(this ReminderOffset offset) => offset switch
        {
            ReminderOffset.FiveMinutes    => "5 minutes before",
            ReminderOffset.FifteenMinutes => "15 minutes before",
            ReminderOffset.ThirtyMinutes  => "30 minutes before",
            ReminderOffset.OneHour        => "1 hour before",
            ReminderOffset.TwoHours       => "2 hours before",
            ReminderOffset.FourHours      => "4 hours before",
            ReminderOffset.OneDay         => "1 day before",
            ReminderOffset.TwoDays        => "2 days before",
            ReminderOffset.OneWeek        => "1 week before",
            _                             => "Unknown"
        };
    }

    public class TaskItem
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public DateTime DueDateTime { get; set; }
        public List<ReminderOffset> Reminders { get; set; } = new();
        public bool IsCompleted { get; set; }
        public DateTime CreatedAt { get; init; } = DateTime.Now;

        /// <summary>Returns all future reminder fire times for this task.</summary>
        public IEnumerable<(ReminderOffset Offset, DateTime FireAt)> GetReminderFireTimes()
        {
            foreach (var offset in Reminders)
            {
                var fireAt = DueDateTime - offset.ToTimeSpan();
                if (fireAt > DateTime.Now)
                    yield return (offset, fireAt);
            }
        }

        public override string ToString() => Title;
    }
}
