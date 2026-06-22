using System;
using System.Drawing;
using System.Windows.Forms;

namespace TaskReminder
{
    /// <summary>
    /// Full-detail popup shown when a reminder fires.
    /// </summary>
    public sealed class ReminderPopup : Form
    {
        private readonly TaskItem _task;
        private readonly ReminderOffset _offset;
        private readonly Label _headerTitleLabel = new();

        public ReminderPopup(TaskItem task, ReminderOffset offset)
        {
            _task   = task;
            _offset = offset;
            BuildUI();
        }

        private void BuildUI()
        {
            Text            = "⏰  Task Reminder";
            Size            = new Size(480, 360);
            MinimumSize     = new Size(400, 320);
            StartPosition   = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox     = false;
            MinimizeBox     = false;
            BackColor       = Color.FromArgb(245, 247, 250);
            TopMost         = true;
            Font            = new Font("Segoe UI", 9.5f);

            // ── Header bar ────────────────────────────────────────────
            var header = new Panel
            {
                Dock      = DockStyle.Top,
                Height    = 56,
                BackColor = Color.FromArgb(37, 99, 235)
            };

            var bell = new Label
            {
                Text      = "🔔",
                Font      = new Font("Segoe UI Emoji", 22f),
                ForeColor = Color.White,
                Location  = new Point(14, 10),
                AutoSize  = true
            };

            _headerTitleLabel.Text     = "Reminder";
            _headerTitleLabel.Font     = new Font("Segoe UI Semibold", 15f);
            _headerTitleLabel.ForeColor= Color.White;
            _headerTitleLabel.Location = new Point(58, 14);
            _headerTitleLabel.AutoSize = true;

            header.Controls.Add(bell);
            header.Controls.Add(_headerTitleLabel);

            // ── Body ──────────────────────────────────────────────────
            var body = new Panel
            {
                Dock    = DockStyle.Fill,
                Padding = new Padding(20)
            };

            int y = 72;

            void Row(string labelText, string value, bool bold = false)
            {
                if (string.IsNullOrWhiteSpace(value)) return;

                var lbl = new Label
                {
                    Text      = labelText,
                    Font      = new Font("Segoe UI", 8.5f),
                    ForeColor = Color.FromArgb(100, 116, 139),
                    Location  = new Point(20, y),
                    AutoSize  = true
                };
                body.Controls.Add(lbl);

                var val = new Label
                {
                    Text        = value,
                    Font        = bold ? new Font("Segoe UI Semibold", 11f) : new Font("Segoe UI", 10f),
                    ForeColor   = Color.FromArgb(30, 41, 59),
                    Location    = new Point(20, y + 18),
                    MaximumSize = new Size(420, 0),
                    AutoSize    = true
                };
                body.Controls.Add(val);

                y += val.PreferredHeight + 30;
            }

            Row("TASK",        _task.Title, bold: true);
            Row("DESCRIPTION", _task.Description);
            Row("LOCATION",    string.IsNullOrWhiteSpace(_task.Location) ? "—" : _task.Location);
            Row("DUE",         _task.DueDateTime.ToString("dddd, dd MMMM yyyy  •  HH:mm"));
            Row("REMINDER",    _offset.ToDisplayString());

            // ── Buttons ───────────────────────────────────────────────
            var btnOk = new Button
            {
                Text         = "Dismiss",
                Size         = new Size(110, 36),
                BackColor    = Color.FromArgb(37, 99, 235),
                ForeColor    = Color.White,
                FlatStyle    = FlatStyle.Flat,
                Font         = new Font("Segoe UI Semibold", 9.5f),
                DialogResult = DialogResult.OK
            };
            btnOk.FlatAppearance.BorderSize = 0;

            var btnSnooze = new Button
            {
                Text      = "Snooze 5 min",
                Size      = new Size(120, 36),
                BackColor = Color.White,
                ForeColor = Color.FromArgb(37, 99, 235),
                FlatStyle = FlatStyle.Flat,
                Font      = new Font("Segoe UI", 9.5f)
            };
            btnSnooze.FlatAppearance.BorderColor = Color.FromArgb(37, 99, 235);
            btnSnooze.FlatAppearance.BorderSize  = 1;
            btnSnooze.Click += (_, _) =>
            {
                var snoozeTimer = new System.Windows.Forms.Timer { Interval = 5 * 60 * 1000 };
                snoozeTimer.Tick += (_, _) =>
                {
                    snoozeTimer.Stop();
                    snoozeTimer.Dispose();
                    var popup = new ReminderPopup(_task, _offset);
                    popup._headerTitleLabel.Text = "Snoozed Reminder";
                    popup.Show();
                };
                snoozeTimer.Start();
                Close();
            };

            var buttonPanel = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.RightToLeft,
                Dock          = DockStyle.Bottom,
                Height        = 56,
                Padding       = new Padding(12, 8, 12, 8),
                BackColor     = Color.FromArgb(245, 247, 250)
            };
            buttonPanel.Controls.Add(btnOk);
            buttonPanel.Controls.Add(btnSnooze);

            Controls.Add(body);
            Controls.Add(buttonPanel);
            Controls.Add(header);

            AcceptButton = btnOk;
        }
    }
}
