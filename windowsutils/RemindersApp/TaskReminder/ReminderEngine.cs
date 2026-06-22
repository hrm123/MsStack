using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace TaskReminder
{
    /// <summary>
    /// Monitors tasks and fires reminders at the correct times.
    /// Uses a single System.Windows.Forms.Timer ticking every 30 seconds.
    /// </summary>
    public sealed class ReminderEngine : IDisposable
    {
        // Tracks which (taskId, offset) pairs have already fired
        private readonly HashSet<string> _firedKeys = new();
        private readonly System.Windows.Forms.Timer _timer;
        private IReadOnlyList<TaskItem> _tasks = Array.Empty<TaskItem>();

        public event EventHandler<ReminderFiredEventArgs>? ReminderFired;

        public ReminderEngine()
        {
            _timer = new System.Windows.Forms.Timer { Interval = 30_000 }; // 30 s
            _timer.Tick += OnTick;
        }

        public void Start() => _timer.Start();
        public void Stop()  => _timer.Stop();

        public void UpdateTasks(IReadOnlyList<TaskItem> tasks)
        {
            _tasks = tasks;

            // Purge fired keys for tasks that no longer exist
            var validIds = tasks.Select(t => t.Id.ToString()).ToHashSet();
            _firedKeys.RemoveWhere(k => !validIds.Any(id => k.StartsWith(id)));
        }

        /// <summary>Force an immediate check (call after adding a new task).</summary>
        public void CheckNow() => OnTick(this, EventArgs.Empty);

        private void OnTick(object? sender, EventArgs e)
        {
            var now = DateTime.Now;
            foreach (var task in _tasks)
            {
                if (task.IsCompleted) continue;

                foreach (var (offset, fireAt) in task.GetReminderFireTimes())
                {
                    // Fire if we're within a 35-second window past the scheduled time
                    if (now >= fireAt && now <= fireAt.AddSeconds(35))
                    {
                        var key = $"{task.Id}|{offset}";
                        if (_firedKeys.Add(key))          // returns false if already present
                        {
                            ReminderFired?.Invoke(this, new ReminderFiredEventArgs(task, offset, fireAt));
                        }
                    }
                }
            }
        }

        public void Dispose()
        {
            _timer.Stop();
            _timer.Dispose();
        }
    }

    public sealed class ReminderFiredEventArgs : EventArgs
    {
        public TaskItem Task      { get; }
        public ReminderOffset Offset { get; }
        public DateTime ScheduledAt  { get; }

        public ReminderFiredEventArgs(TaskItem task, ReminderOffset offset, DateTime scheduledAt)
        {
            Task        = task;
            Offset      = offset;
            ScheduledAt = scheduledAt;
        }
    }
}
