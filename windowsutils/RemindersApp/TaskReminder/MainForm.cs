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

        // Ollama settings (session-level; extend to app settings for persistence)
        private string _ollamaUrl   = "http://localhost:11434";
        private string _ollamaModel = "llama3";

        // ── UI controls ───────────────────────────────────────────────
        private readonly ListView             _listView  = new();
        private readonly StatusStrip          _statusBar = new();
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
            Size            = new Size(980, 640);
            MinimumSize     = new Size(760, 480);
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

            Button MkBtn(string text, Color bg, int width = 120)
            {
                var b = new Button
                {
                    Text      = text,
                    Size      = new Size(width, 34),
                    BackColor = bg,
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Font      = new Font("Segoe UI Semibold", 9f),
                    Cursor    = Cursors.Hand,
                    Anchor    = AnchorStyles.Top | AnchorStyles.Right
                };
                b.FlatAppearance.BorderSize = 0;
                return b;
            }

            var btnAI     = MkBtn("✨ AI Assistant", Color.FromArgb(109, 40, 217), 130);
            var btnAdd    = MkBtn("＋  Add Task",    Color.FromArgb(22, 163, 74));
            var btnEdit   = MkBtn("✏  Edit",         Color.FromArgb(71, 85, 105), 100);
            var btnDelete = MkBtn("🗑  Delete",       Color.FromArgb(220, 38, 38), 100);
            var btnSettings = MkBtn("⚙",             Color.FromArgb(55, 65, 81), 36);
            btnSettings.Font    = new Font("Segoe UI", 12f);
            btnSettings.ToolTip(toolbar, "Ollama settings");

            void Reposition()
            {
                int w = ClientSize.Width;
                btnSettings.Location = new Point(w - 50,  10);
                btnDelete.Location   = new Point(w - 160, 10);
                btnEdit.Location     = new Point(w - 270, 10);
                btnAdd.Location      = new Point(w - 400, 10);
                btnAI.Location       = new Point(w - 542, 10);
            }
            Reposition();
            Resize += (_, _) => Reposition();

            btnAI.Click       += (_, _) => OnAiAddTask();
            btnAdd.Click      += (_, _) => OnAddTask();
            btnEdit.Click     += (_, _) => OnEditTask();
            btnDelete.Click   += (_, _) => OnDeleteTask();
            btnSettings.Click += (_, _) => OnOllamaSettings();

            toolbar.Controls.Add(lblAppTitle);
            toolbar.Controls.Add(btnAI);
            toolbar.Controls.Add(btnAdd);
            toolbar.Controls.Add(btnEdit);
            toolbar.Controls.Add(btnDelete);
            toolbar.Controls.Add(btnSettings);

            // ── ListView ──────────────────────────────────────────────
            _listView.Dock          = DockStyle.Fill;
            _listView.View          = View.Details;
            _listView.FullRowSelect = true;
            _listView.GridLines     = true;
            _listView.MultiSelect   = false;
            _listView.Font          = new Font("Segoe UI", 9.5f);
            _listView.BorderStyle   = BorderStyle.None;
            _listView.BackColor     = Color.White;

            _listView.Columns.Add("Title",         200);
            _listView.Columns.Add("Description",   200);
            _listView.Columns.Add("Location",      130);
            _listView.Columns.Add("Due Date/Time", 160);
            _listView.Columns.Add("Reminders",     180);
            _listView.Columns.Add("Status",         80);

            _listView.DoubleClick += (_, _) => OnEditTask();

            // ── Status bar ────────────────────────────────────────────
            _statusBar.BackColor = Color.FromArgb(30, 41, 59);
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
                Text      = "No tasks yet.\nUse  ✨ AI Assistant  or  ＋ Add Task  to get started.",
                Font      = new Font("Segoe UI", 11f),
                ForeColor = Color.FromArgb(148, 163, 184),
                Dock      = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                Name      = "lblEmpty"
            };

            var listPanel = new Panel { Dock = DockStyle.Fill };
            listPanel.Controls.Add(_listView);
            listPanel.Controls.Add(lblEmpty);
            _listView.BringToFront();

            _listView.ItemSelectionChanged += (_, _) =>
                lblEmpty.Visible = _listView.Items.Count == 0;

            Controls.Add(listPanel);
            Controls.Add(_statusBar);
            Controls.Add(toolbar);
            toolbar.BringToFront();

            // ── Right-click context menu ──────────────────────────────
            var ctx = new ContextMenuStrip();
            ctx.Items.Add("✨ Edit with AI",   null, (_, _) => OnAiAddTask());
            ctx.Items.Add("✏  Edit Task",      null, (_, _) => OnEditTask());
            ctx.Items.Add("✔  Mark Done",      null, (_, _) => OnMarkDone());
            ctx.Items.Add(new ToolStripSeparator());
            ctx.Items.Add("🗑  Delete Task",    null, (_, _) => OnDeleteTask());
            _listView.ContextMenuStrip = ctx;

            FormClosing += OnFormClosing;
        }

        // ══════════════════════════════════════════════════════════════
        // Task operations
        // ══════════════════════════════════════════════════════════════

        private void OnAiAddTask()
        {
            using var dlg = new AiTaskDialog(_ollamaUrl, _ollamaModel);
            if (dlg.ShowDialog(this) == DialogResult.OK && dlg.Result != null)
            {
                _tasks.Add(dlg.Result);
                RefreshList();
                _engine.UpdateTasks(_tasks);
                _engine.CheckNow();
                UpdateStatus($"✨ AI created task: "{dlg.Result.Title}"");
            }
        }

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
            if (MessageBox.Show($"Delete "{task.Title}"?", "Confirm Delete",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
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

        private void OnOllamaSettings()
        {
            using var dlg = new OllamaSettingsDialog(_ollamaUrl, _ollamaModel);
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                _ollamaUrl   = dlg.OllamaUrl;
                _ollamaModel = dlg.ModelName;
                UpdateStatus($"Ollama settings saved — model: {_ollamaModel}");
            }
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
                    Tag = t,
                    UseItemStyleForSubItems = false
                };
                item.SubItems.Add(t.Description.Length > 50 ? t.Description[..47] + "…" : t.Description);
                item.SubItems.Add(string.IsNullOrWhiteSpace(t.Location) ? "—" : t.Location);
                item.SubItems.Add(t.DueDateTime.ToString("dd MMM yyyy  HH:mm"));
                item.SubItems.Add(string.Join(", ", t.Reminders.Select(r => r.ToDisplayString())));
                item.SubItems.Add(t.IsCompleted ? "✔ Done" : "Pending");

                if (t.IsCompleted)
                {
                    item.ForeColor = Color.FromArgb(148, 163, 184);
                    item.Font      = new Font(_listView.Font, FontStyle.Strikeout);
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

            var emp = Controls.Find("lblEmpty", true).FirstOrDefault();
            if (emp != null) emp.Visible = _listView.Items.Count == 0;

            _lblStatus.Text = $"{_tasks.Count(t => !t.IsCompleted)} active  ·  {_tasks.Count(t => t.IsCompleted)} completed";
        }

        // ══════════════════════════════════════════════════════════════
        // Reminder popup
        // ══════════════════════════════════════════════════════════════

        private void OnReminderFired(object? sender, ReminderFiredEventArgs e)
        {
            if (InvokeRequired) { Invoke(() => OnReminderFired(sender, e)); return; }

            _tray.BalloonTipTitle = $"⏰ {e.Offset.ToDisplayString().ToUpper()}";
            _tray.BalloonTipText  = $"{e.Task.Title}\n{e.Task.DueDateTime:dd MMM  HH:mm}";
            _tray.ShowBalloonTip(5000);

            var popup = new ReminderPopup(e.Task, e.Offset);
            popup.Show(this);
            popup.BringToFront();

            UpdateStatus($"Reminder: "{e.Task.Title}" — {e.Offset.ToDisplayString()}");
        }

        // ══════════════════════════════════════════════════════════════
        // Helpers
        // ══════════════════════════════════════════════════════════════

        private void UpdateStatus(string msg) => _lblStatus.Text = msg;

        private void UpdateClock()
            => _lblClock.Text = DateTime.Now.ToString("ddd, dd MMM yyyy  •  HH:mm");

        private NotifyIcon BuildTrayIcon()
        {
            var ni = new NotifyIcon { Text = "Task Reminder", Visible = true, Icon = SystemIcons.Application };
            var ctx = new ContextMenuStrip();
            ctx.Items.Add("Open", null, (_, _) => { Show(); WindowState = FormWindowState.Normal; BringToFront(); });
            ctx.Items.Add("Exit", null, (_, _) => { ni.Visible = false; Application.Exit(); });
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
                _tray.ShowBalloonTip(3000, "Still running", "Task Reminder is in the system tray.", ToolTipIcon.Info);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) { _engine.Dispose(); _tray.Dispose(); _clockTimer.Dispose(); }
            base.Dispose(disposing);
        }
    }

    // ── Extension: ToolTip helper ─────────────────────────────────────────
    internal static class ControlExtensions
    {
        public static void ToolTip(this Control c, Control owner, string text)
        {
            var tt = new ToolTip();
            tt.SetToolTip(c, text);
        }
    }
}
