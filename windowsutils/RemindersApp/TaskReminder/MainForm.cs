using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace TaskReminder
{
    public sealed class MainForm : Form
    {
        // ── State ──────────────────────────────────────────────────────
        private readonly List<TaskItem>  _tasks  = new();
        private readonly ReminderEngine  _engine = new();
        private readonly NotifyIcon      _tray;
        private readonly System.Windows.Forms.Timer _clockTimer = new() { Interval = 60_000 };

        // ── UI controls ───────────────────────────────────────────────
        private readonly ListView        _listView    = new();
        private readonly StatusStrip     _statusBar   = new();
        private readonly ToolStripStatusLabel _lblStatus = new();
        private readonly ToolStripStatusLabel _lblClock  = new();

        public MainForm()
        {
            _tray = BuildTrayIcon();
            BuildUI();

            _engine.ReminderFired += OnReminderFired;
            _engine.Start();

            _clockTimer.Tick += (_, _) => UpdateClock();
            _clockTimer.Start();
            UpdateClock();
        }

        // ══════════════════════════════════════════════════════════════
        // UI Construction
        // ══════════════════════════════════════════════════════════════

        private void BuildUI()
        {
            Text            = "Task Reminder";
            Size            = new Size(900, 620);
            MinimumSize     = new Size(700, 450);
            StartPosition   = FormStartPosition.CenterScreen;
            BackColor       = Color.FromArgb(248, 250, 252);
            Font            = new Font("Segoe UI", 9.5f);

            // ── Top toolbar ───────────────────────────────────────────
            var toolbar = new Panel
            {
                Dock      = DockStyle.Top,
                Height    = 54,
                BackColor = Color.FromArgb(37, 99, 235),
                Padding   = new Padding(14, 8, 14, 8)
            };

            var lblAppTitle = new Label
            {
                Text      = "📋  Task Reminder",
                Font      = new Font("Segoe UI Semibold", 14f),
                ForeColor = Color.White,
                Location  = new Point(14, 13),
                AutoSize  = true
            };

            Button MkBtn(string text, int x, Color bg)
            {
                var b = new Button
                {
                    Text      = text,
                    Location  = new Point(x, 10),
                    Size      = new Size(110, 34),
                    BackColor = bg,
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Font      = new Font("Segoe UI Semibold", 9f),
                    Cursor    = Cursors.Hand
                };
                b.FlatAppearance.BorderSize  = 0;
                return b;
            }

            var btnAdd    = MkBtn("＋  Add Task",  650, Color.FromArgb(22, 163, 74));
            var btnEdit   = MkBtn("✏  Edit",       770, Color.FromArgb(71, 85, 105));
            var btnDelete = MkBtn("🗑  Delete",     890, Color.FromArgb(220, 38, 38));

            // Reposition to right edge
            btnAdd.Anchor    = AnchorStyles.Top | AnchorStyles.Right;
            btnEdit.Anchor   = AnchorStyles.Top | AnchorStyles.Right;
            btnDelete.Anchor = AnchorStyles.Top | AnchorStyles.Right;

            int W = ClientSize.Width;
            btnDelete.Location = new Point(W - 128, 10);
            btnEdit.Location   = new Point(W - 248, 10);
            btnAdd.Location    = new Point(W - 368, 10);

            btnAdd.Click    += (_, _) => OnAddTask();
            btnEdit.Click   += (_, _) => OnEditTask();
            btnDelete.Click += (_, _) => OnDeleteTask();

            toolbar.Controls.Add(lblAppTitle);
            toolbar.Controls.Add(btnAdd);
            toolbar.Controls.Add(btnEdit);
            toolbar.Controls.Add(btnDelete);

            // Reposition on resize
            Resize += (_, _) =>
            {
                int w2 = ClientSize.Width;
                btnDelete.Location = new Point(w2 - 128, 10);
                btnEdit.Location   = new Point(w2 - 248, 10);
                btnAdd.Location    = new Point(w2 - 368, 10);
            };

            // ── ListView ──────────────────────────────────────────────
            _listView.Dock          = DockStyle.Fill;
            _listView.View          = View.Details;
            _listView.FullRowSelect = true;
            _listView.GridLines     = true;
            _listView.MultiSelect   = false;
            _listView.Font          = new Font("Segoe UI", 9.5f);
            _listView.BorderStyle   = BorderStyle.None;
            _listView.BackColor     = Color.White;
            _listView.OwnerDraw     = false;

            _listView.Columns.Add("Title",       200);
            _listView.Columns.Add("Description", 200);
            _listView.Columns.Add("Location",    130);
            _listView.Columns.Add("Due Date/Time", 160);
            _listView.Columns.Add("Reminders",   180);
            _listView.Columns.Add("Status",       80);

            _listView.DoubleClick += (_, _) => OnEditTask();

            // ── Status bar ────────────────────────────────────────────
            _statusBar.BackColor = Color.FromArgb(30, 41, 59);
            _statusBar.ForeColor = Color.White;
            _lblStatus.ForeColor = Color.FromArgb(148, 163, 184);
            _lblStatus.Text      = "Ready";
            _lblClock.ForeColor  = Color.FromArgb(148, 163, 184);
            _lblClock.Spring     = true;
            _lblClock.TextAlign  = ContentAlignment.MiddleRight;
            _statusBar.Items.Add(_lblStatus);
            _statusBar.Items.Add(_lblClock);

            // ── Empty-state label ─────────────────────────────────────
            var lblEmpty = new Label
            {
                Text      = "No tasks yet.\nClick  ＋ Add Task  to get started.",
                Font      = new Font("Segoe UI", 11f),
                ForeColor = Color.FromArgb(148, 163, 184),
                Dock      = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                Name      = "lblEmpty"
            };

            // Wrap list + empty label in a panel
            var listPanel = new Panel { Dock = DockStyle.Fill };
            listPanel.Controls.Add(_listView);
            listPanel.Controls.Add(lblEmpty);
            _listView.BringToFront();

            _listView.ItemSelectionChanged += (_, _) =>
                lblEmpty.Visible = _listView.Items.Count == 0;

            Controls.Add(listPanel);
            Controls.Add(_statusBar);
            Controls.Add(toolbar);

            // Keep toolbar on top
            toolbar.BringToFront();

            // Context menu
            var ctxMenu = new ContextMenuStrip();
            ctxMenu.Items.Add("✏  Edit Task",   null, (_, _) => OnEditTask());
            ctxMenu.Items.Add("✔  Mark Done",   null, (_, _) => OnMarkDone());
            ctxMenu.Items.Add(new ToolStripSeparator());
            ctxMenu.Items.Add("🗑  Delete Task", null, (_, _) => OnDeleteTask());
            _listView.ContextMenuStrip = ctxMenu;

            FormClosing += OnFormClosing;
        }

        // ══════════════════════════════════════════════════════════════
        // Task operations
        // ══════════════════════════════════════════════════════════════

        private void OnAddTask()
        {
            using var dlg = new TaskDialog();
            if (dlg.ShowDialog(this) == DialogResult.OK && dlg.Result != null)
            {
                _tasks.Add(dlg.Result);
                RefreshList();
                _engine.UpdateTasks(_tasks);
                _engine.CheckNow();
                UpdateStatus($"Task "{dlg.Result.Title}" added.");
            }
        }

        private void OnEditTask()
        {
            if (_listView.SelectedItems.Count == 0) return;

            var task = (TaskItem)_listView.SelectedItems[0].Tag!;
            using var dlg = new TaskDialog(task);
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                RefreshList();
                _engine.UpdateTasks(_tasks);
                _engine.CheckNow();
                UpdateStatus($"Task "{task.Title}" updated.");
            }
        }

        private void OnDeleteTask()
        {
            if (_listView.SelectedItems.Count == 0) return;

            var task = (TaskItem)_listView.SelectedItems[0].Tag!;
            var confirm = MessageBox.Show(
                $"Delete "{task.Title}"?", "Confirm Delete",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm == DialogResult.Yes)
            {
                _tasks.Remove(task);
                RefreshList();
                _engine.UpdateTasks(_tasks);
                UpdateStatus($"Task "{task.Title}" deleted.");
            }
        }

        private void OnMarkDone()
        {
            if (_listView.SelectedItems.Count == 0) return;
            var task = (TaskItem)_listView.SelectedItems[0].Tag!;
            task.IsCompleted = !task.IsCompleted;
            RefreshList();
            _engine.UpdateTasks(_tasks);
            UpdateStatus(task.IsCompleted ? $""{task.Title}" marked complete." : $""{task.Title}" reopened.");
        }

        // ══════════════════════════════════════════════════════════════
        // ListView refresh
        // ══════════════════════════════════════════════════════════════

        private void RefreshList()
        {
            _listView.BeginUpdate();
            _listView.Items.Clear();

            foreach (var t in _tasks.OrderBy(t => t.DueDateTime))
            {
                var item = new ListViewItem(t.Title)
                {
                    Tag       = t,
                    UseItemStyleForSubItems = false
                };

                item.SubItems.Add(t.Description.Length > 50
                    ? t.Description[..47] + "…"
                    : t.Description);
                item.SubItems.Add(string.IsNullOrWhiteSpace(t.Location) ? "—" : t.Location);
                item.SubItems.Add(t.DueDateTime.ToString("dd MMM yyyy  HH:mm"));
                item.SubItems.Add(string.Join(", ", t.Reminders.Select(r => r.ToDisplayString())));
                item.SubItems.Add(t.IsCompleted ? "✔ Done" : "Pending");

                if (t.IsCompleted)
                {
                    item.ForeColor       = Color.FromArgb(148, 163, 184);
                    item.Font            = new Font(_listView.Font, FontStyle.Strikeout);
                }
                else if (t.DueDateTime < DateTime.Now)
                {
                    item.BackColor = Color.FromArgb(255, 241, 242);
                    item.ForeColor = Color.FromArgb(190, 18, 60);
                }
                else if (t.DueDateTime < DateTime.Now.AddHours(24))
                {
                    item.BackColor = Color.FromArgb(255, 251, 235);
                }

                _listView.Items.Add(item);
            }

            _listView.EndUpdate();

            // Show/hide empty state
            var emp = Controls.Find("lblEmpty", true).FirstOrDefault();
            if (emp != null) emp.Visible = _listView.Items.Count == 0;

            _lblStatus.Text = $"{_tasks.Count(t => !t.IsCompleted)} active task(s)  ·  {_tasks.Count(t => t.IsCompleted)} completed";
        }

        // ══════════════════════════════════════════════════════════════
        // Reminder fired
        // ══════════════════════════════════════════════════════════════

        private void OnReminderFired(object? sender, ReminderFiredEventArgs e)
        {
            // Must marshal to the UI thread
            if (InvokeRequired) { Invoke(() => OnReminderFired(sender, e)); return; }

            _tray.BalloonTipTitle = $"⏰ {e.Offset.ToDisplayString().ToUpper()}";
            _tray.BalloonTipText  = $"{e.Task.Title}\n{e.Task.DueDateTime:dd MMM  HH:mm}";
            _tray.ShowBalloonTip(5000);

            var popup = new ReminderPopup(e.Task, e.Offset);
            popup.Show(this);
            popup.BringToFront();

            UpdateStatus($"Reminder fired: "{e.Task.Title}" ({e.Offset.ToDisplayString()})");
        }

        // ══════════════════════════════════════════════════════════════
        // Helpers
        // ══════════════════════════════════════════════════════════════

        private void UpdateStatus(string msg) => _lblStatus.Text = msg;

        private void UpdateClock()
            => _lblClock.Text = DateTime.Now.ToString("ddd, dd MMM yyyy  •  HH:mm");

        private NotifyIcon BuildTrayIcon()
        {
            var ni = new NotifyIcon
            {
                Text    = "Task Reminder",
                Visible = true,
                Icon    = SystemIcons.Application
            };

            var ctx = new ContextMenuStrip();
            ctx.Items.Add("Open",  null, (_, _) => { Show(); WindowState = FormWindowState.Normal; BringToFront(); });
            ctx.Items.Add("Exit",  null, (_, _) => { ni.Visible = false; Application.Exit(); });
            ni.ContextMenuStrip = ctx;
            ni.DoubleClick     += (_, _) => { Show(); WindowState = FormWindowState.Normal; BringToFront(); };

            return ni;
        }

        private void OnFormClosing(object? sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
                _tray.ShowBalloonTip(3000,
                    "Still running",
                    "Task Reminder is minimized to the system tray.",
                    ToolTipIcon.Info);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _engine.Dispose();
                _tray.Dispose();
                _clockTimer.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
