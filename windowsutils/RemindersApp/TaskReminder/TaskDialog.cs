using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace TaskReminder
{
    /// <summary>Dialog for creating or editing a TaskItem.</summary>
    public sealed class TaskDialog : Form
    {
        private readonly TextBox         _txtTitle       = new();
        private readonly TextBox         _txtDescription = new();
        private readonly TextBox         _txtLocation    = new();
        private readonly DateTimePicker  _dtpDate        = new();
        private readonly DateTimePicker  _dtpTime        = new();
        private readonly CheckedListBox  _clbReminders   = new();
        private readonly ErrorProvider   _error          = new();

        public TaskItem? Result { get; private set; }

        private readonly TaskItem? _existing;

        public TaskDialog(TaskItem? existing = null)
        {
            _existing = existing;
            BuildUI();
            if (existing != null) Populate(existing);
        }

        private void BuildUI()
        {
            Text            = _existing == null ? "Add New Task" : "Edit Task";
            Size            = new Size(520, 560);
            MinimumSize     = new Size(460, 500);
            StartPosition   = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox     = false;
            MinimizeBox     = false;
            BackColor       = Color.FromArgb(248, 250, 252);
            Font            = new Font("Segoe UI", 9.5f);

            var scroll = new Panel
            {
                AutoScroll = true,
                Dock       = DockStyle.Fill,
                Padding    = new Padding(24, 20, 24, 0)
            };

            int y = 20;

            void AddField(string label, Control ctrl, int height = 32)
            {
                var lbl = new Label
                {
                    Text      = label,
                    Font      = new Font("Segoe UI Semibold", 9f),
                    ForeColor = Color.FromArgb(71, 85, 105),
                    Location  = new Point(24, y),
                    AutoSize  = true
                };
                scroll.Controls.Add(lbl);
                y += 22;

                ctrl.Location = new Point(24, y);
                ctrl.Width    = 450;
                ctrl.Height   = height;
                scroll.Controls.Add(ctrl);
                y += height + 14;
            }

            // Title
            _txtTitle.BorderStyle = BorderStyle.FixedSingle;
            _txtTitle.Font        = new Font("Segoe UI", 10f);
            AddField("Task Title *", _txtTitle);

            // Description
            _txtDescription.Multiline  = true;
            _txtDescription.ScrollBars = ScrollBars.Vertical;
            _txtDescription.BorderStyle= BorderStyle.FixedSingle;
            AddField("Description", _txtDescription, 60);

            // Location
            _txtLocation.BorderStyle = BorderStyle.FixedSingle;
            AddField("Location", _txtLocation);

            // Date & Time on one row
            var lbl2 = new Label
            {
                Text      = "Due Date & Time *",
                Font      = new Font("Segoe UI Semibold", 9f),
                ForeColor = Color.FromArgb(71, 85, 105),
                Location  = new Point(24, y),
                AutoSize  = true
            };
            scroll.Controls.Add(lbl2);
            y += 22;

            _dtpDate.Format   = DateTimePickerFormat.Short;
            _dtpDate.Value    = DateTime.Now.AddHours(1);
            _dtpDate.Location = new Point(24, y);
            _dtpDate.Width    = 150;
            scroll.Controls.Add(_dtpDate);

            _dtpTime.Format       = DateTimePickerFormat.Time;
            _dtpTime.ShowUpDown   = true;
            _dtpTime.Value        = DateTime.Now.AddHours(1);
            _dtpTime.Location     = new Point(184, y);
            _dtpTime.Width        = 120;
            scroll.Controls.Add(_dtpTime);
            y += 36 + 14;

            // Reminders
            var lblR = new Label
            {
                Text      = "Remind me…",
                Font      = new Font("Segoe UI Semibold", 9f),
                ForeColor = Color.FromArgb(71, 85, 105),
                Location  = new Point(24, y),
                AutoSize  = true
            };
            scroll.Controls.Add(lblR);
            y += 22;

            _clbReminders.BorderStyle       = BorderStyle.FixedSingle;
            _clbReminders.CheckOnClick       = true;
            _clbReminders.Location           = new Point(24, y);
            _clbReminders.Width              = 450;
            _clbReminders.Height             = 160;
            _clbReminders.IntegralHeight     = false;

            foreach (ReminderOffset ro in Enum.GetValues<ReminderOffset>())
                _clbReminders.Items.Add(ro.ToDisplayString());

            // Default: 1-hour reminder pre-checked
            _clbReminders.SetItemChecked(3, true);  // OneHour index
            scroll.Controls.Add(_clbReminders);
            y += 174;

            Controls.Add(scroll);

            // ── Footer buttons ────────────────────────────────────────
            var footer = new Panel
            {
                Dock      = DockStyle.Bottom,
                Height    = 56,
                BackColor = Color.White
            };

            var btnSave = new Button
            {
                Text      = _existing == null ? "Add Task" : "Save Changes",
                Size      = new Size(130, 36),
                Location  = new Point(350, 10),
                BackColor = Color.FromArgb(37, 99, 235),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font      = new Font("Segoe UI Semibold", 9.5f)
            };
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Click += OnSave;

            var btnCancel = new Button
            {
                Text      = "Cancel",
                Size      = new Size(90, 36),
                Location  = new Point(252, 10),
                BackColor = Color.White,
                ForeColor = Color.FromArgb(100, 116, 139),
                FlatStyle = FlatStyle.Flat,
                Font      = new Font("Segoe UI", 9.5f),
                DialogResult = DialogResult.Cancel
            };
            btnCancel.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225);
            btnCancel.FlatAppearance.BorderSize  = 1;

            footer.Controls.Add(btnSave);
            footer.Controls.Add(btnCancel);
            Controls.Add(footer);

            AcceptButton = btnSave;
            CancelButton = btnCancel;
        }

        private void Populate(TaskItem t)
        {
            _txtTitle.Text       = t.Title;
            _txtDescription.Text = t.Description;
            _txtLocation.Text    = t.Location;
            _dtpDate.Value       = t.DueDateTime;
            _dtpTime.Value       = t.DueDateTime;

            var allOffsets = Enum.GetValues<ReminderOffset>().ToList();
            for (int i = 0; i < allOffsets.Count; i++)
                _clbReminders.SetItemChecked(i, t.Reminders.Contains(allOffsets[i]));
        }

        private void OnSave(object? sender, EventArgs e)
        {
            _error.Clear();

            if (string.IsNullOrWhiteSpace(_txtTitle.Text))
            {
                _error.SetError(_txtTitle, "Title is required.");
                return;
            }

            var due = _dtpDate.Value.Date + _dtpTime.Value.TimeOfDay;
            if (due <= DateTime.Now)
            {
                MessageBox.Show("Due date/time must be in the future.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var allOffsets = Enum.GetValues<ReminderOffset>().ToList();
            var selected   = _clbReminders.CheckedIndices
                                          .Cast<int>()
                                          .Select(i => allOffsets[i])
                                          .ToList();

            if (selected.Count == 0)
            {
                MessageBox.Show("Please select at least one reminder.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Result = _existing ?? new TaskItem();
            Result.Title       = _txtTitle.Text.Trim();
            Result.Description = _txtDescription.Text.Trim();
            Result.Location    = _txtLocation.Text.Trim();
            Result.DueDateTime = due;
            Result.Reminders   = selected;

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
