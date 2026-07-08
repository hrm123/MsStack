using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TaskReminder
{
    /// <summary>
    /// Chat-style dialog that uses a local Llama model (via Ollama) to gather
    /// task details through natural conversation, then extracts a structured
    /// TaskItem from the final JSON block the model produces.
    /// </summary>
    public sealed class AiTaskDialog : Form
    {
        // ── Public result ─────────────────────────────────────────────
        public TaskItem? Result { get; private set; }

        // ── Ollama ────────────────────────────────────────────────────
        private readonly OllamaClient _llm;
        private readonly List<OllamaMessage> _history = new();
        private CancellationTokenSource _cts = new();

        // ── UI ────────────────────────────────────────────────────────
        private readonly RichTextBox _chat    = new();
        private readonly TextBox     _input   = new();
        private readonly Button      _btnSend = new();
        private readonly Button      _btnStop = new();
        private readonly Button      _btnConfirm = new();
        private readonly Panel       _previewPanel = new();
        private readonly Label       _lblPreview   = new();
        private readonly ProgressBar _spinner      = new();
        private readonly ComboBox    _cboModel     = new();

        // Parsed task waiting for user confirmation
        private TaskItem? _pendingTask;

        // ── System prompt ─────────────────────────────────────────────
        private static string SystemPrompt => $"""
            You are a friendly task-reminder assistant. Your job is to gather all the
            information needed to create a reminder task through natural conversation.

            Today is {DateTime.Now:dddd, dd MMMM yyyy}. The current time is {DateTime.Now:HH:mm}.

            You MUST collect (ask follow-up questions until you have ALL of these):
              1. Task title  — short, clear label
              2. Description — more detail about what needs to happen
              3. Location    — where (can be "N/A" or blank if not relevant)
              4. Due date and time — be explicit; resolve relative phrases like
                 "tomorrow at 3pm" or "next Monday morning" to an exact date/time.
              5. Reminders   — one or more from this exact list (use the enum names):
                   FiveMinutes | FifteenMinutes | ThirtyMinutes | OneHour |
                   TwoHours | FourHours | OneDay | TwoDays | OneWeek

            Rules:
            - Ask only ONE question at a time; keep the conversation natural.
            - Once you have everything, say something like "Great, here is your task:"
              then output a JSON block (and NOTHING after it) in this exact format:

            ```json
            {{
              "title":       "...",
              "description": "...",
              "location":    "...",
              "dueDateTime": "YYYY-MM-DDTHH:mm:00",
              "reminders":   ["OneHour", "OneDay"]
            }}
            ```

            - Do NOT output the JSON until you have confirmed all five fields.
            - If the user is vague about reminders, suggest sensible defaults and ask
              them to confirm.
            """;

        // ─────────────────────────────────────────────────────────────

        public AiTaskDialog(string ollamaUrl = "http://localhost:11434", string model = "llama3")
        {
            _llm = new OllamaClient(ollamaUrl, model);
            BuildUI();
            _ = InitConversationAsync();
        }

        // ══════════════════════════════════════════════════════════════
        // UI Construction
        // ══════════════════════════════════════════════════════════════

        private void BuildUI()
        {
            Text            = "✨  AI Task Assistant";
            Size            = new Size(680, 700);
            MinimumSize     = new Size(560, 560);
            StartPosition   = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.Sizable;
            BackColor       = Color.FromArgb(248, 250, 252);
            Font            = new Font("Segoe UI", 9.5f);

            // ── Header ────────────────────────────────────────────────
            var header = new Panel
            {
                Dock      = DockStyle.Top,
                Height    = 52,
                BackColor = Color.FromArgb(109, 40, 217)   // purple-700
            };

            var lblTitle = new Label
            {
                Text      = "✨  AI Task Assistant",
                Font      = new Font("Segoe UI Semibold", 14f),
                ForeColor = Color.White,
                Location  = new Point(14, 12),
                AutoSize  = true
            };

            // Model selector
            var lblModel = new Label
            {
                Text      = "Model:",
                Font      = new Font("Segoe UI", 8.5f),
                ForeColor = Color.FromArgb(221, 214, 254),
                Location  = new Point(380, 16),
                AutoSize  = true
            };

            _cboModel.Location     = new Point(430, 13);
            _cboModel.Width        = 160;
            _cboModel.DropDownStyle= ComboBoxStyle.DropDown;
            _cboModel.Font         = new Font("Segoe UI", 9f);
            _cboModel.BackColor    = Color.FromArgb(139, 92, 246);
            _cboModel.ForeColor    = Color.White;
            _cboModel.Items.AddRange(new object[] { "llama3", "llama3.1", "llama3.2", "llama2",
                                                     "mistral", "phi3", "gemma2", "qwen2.5" });
            _cboModel.Text         = _llm.Model;
            _cboModel.Anchor       = AnchorStyles.Top | AnchorStyles.Right;
            _cboModel.SelectedIndexChanged += (_, _) => _llm.Model = _cboModel.Text;
            _cboModel.Leave                 += (_, _) => _llm.Model = _cboModel.Text;

            header.Controls.Add(lblTitle);
            header.Controls.Add(lblModel);
            header.Controls.Add(_cboModel);

            // ── Chat area ─────────────────────────────────────────────
            _chat.Dock          = DockStyle.Fill;
            _chat.ReadOnly      = true;
            _chat.BackColor     = Color.White;
            _chat.BorderStyle   = BorderStyle.None;
            _chat.Font          = new Font("Segoe UI", 10f);
            _chat.ScrollBars    = RichTextBoxScrollBars.Vertical;
            _chat.Padding       = new Padding(8);

            // ── Spinner (marquee progress bar) ────────────────────────
            _spinner.Style   = ProgressBarStyle.Marquee;
            _spinner.Dock    = DockStyle.Top;
            _spinner.Height  = 3;
            _spinner.Visible = false;
            _spinner.MarqueeAnimationSpeed = 30;

            // ── Preview panel (shown when JSON is ready) ──────────────
            _previewPanel.Dock      = DockStyle.Bottom;
            _previewPanel.Height    = 130;
            _previewPanel.BackColor = Color.FromArgb(240, 253, 244);  // green tint
            _previewPanel.Padding   = new Padding(12, 8, 12, 8);
            _previewPanel.Visible   = false;

            var previewTitle = new Label
            {
                Text      = "📋  Task ready — review and confirm:",
                Font      = new Font("Segoe UI Semibold", 9f),
                ForeColor = Color.FromArgb(22, 163, 74),
                Dock      = DockStyle.Top,
                Height    = 22
            };

            _lblPreview.Dock      = DockStyle.Fill;
            _lblPreview.Font      = new Font("Segoe UI", 9f);
            _lblPreview.ForeColor = Color.FromArgb(30, 41, 59);

            _btnConfirm.Text      = "✔  Add This Task";
            _btnConfirm.Dock      = DockStyle.Bottom;
            _btnConfirm.Height    = 34;
            _btnConfirm.BackColor = Color.FromArgb(22, 163, 74);
            _btnConfirm.ForeColor = Color.White;
            _btnConfirm.FlatStyle = FlatStyle.Flat;
            _btnConfirm.Font      = new Font("Segoe UI Semibold", 9.5f);
            _btnConfirm.FlatAppearance.BorderSize = 0;
            _btnConfirm.Click += OnConfirmTask;

            _previewPanel.Controls.Add(_lblPreview);
            _previewPanel.Controls.Add(previewTitle);
            _previewPanel.Controls.Add(_btnConfirm);

            // ── Input row ─────────────────────────────────────────────
            var inputRow = new Panel
            {
                Dock      = DockStyle.Bottom,
                Height    = 52,
                BackColor = Color.FromArgb(241, 245, 249),
                Padding   = new Padding(10, 8, 10, 8)
            };

            _input.Location    = new Point(10, 10);
            _input.Height      = 32;
            _input.Font        = new Font("Segoe UI", 10f);
            _input.BorderStyle = BorderStyle.FixedSingle;
            _input.PlaceholderText = "Type your message here and press Enter…";
            _input.Anchor      = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
            _input.Width       = inputRow.Width - 170;
            _input.KeyDown    += (_, e) => { if (e.KeyCode == Keys.Enter) { e.SuppressKeyPress = true; OnSendMessage(); } };

            _btnSend.Text      = "Send";
            _btnSend.Size      = new Size(80, 32);
            _btnSend.BackColor = Color.FromArgb(109, 40, 217);
            _btnSend.ForeColor = Color.White;
            _btnSend.FlatStyle = FlatStyle.Flat;
            _btnSend.Font      = new Font("Segoe UI Semibold", 9.5f);
            _btnSend.FlatAppearance.BorderSize = 0;
            _btnSend.Anchor    = AnchorStyles.Right | AnchorStyles.Top;
            _btnSend.Click    += (_, _) => OnSendMessage();

            _btnStop.Text      = "⏹ Stop";
            _btnStop.Size      = new Size(70, 32);
            _btnStop.BackColor = Color.FromArgb(220, 38, 38);
            _btnStop.ForeColor = Color.White;
            _btnStop.FlatStyle = FlatStyle.Flat;
            _btnStop.Font      = new Font("Segoe UI", 9f);
            _btnStop.FlatAppearance.BorderSize = 0;
            _btnStop.Anchor    = AnchorStyles.Right | AnchorStyles.Top;
            _btnStop.Visible   = false;
            _btnStop.Click    += (_, _) => { _cts.Cancel(); };

            inputRow.Controls.Add(_input);
            inputRow.Controls.Add(_btnSend);
            inputRow.Controls.Add(_btnStop);

            // Reposition on resize
            inputRow.Resize += (_, _) =>
            {
                _input.Width       = inputRow.Width - 175;
                _btnSend.Location  = new Point(inputRow.Width - 95,  10);
                _btnStop.Location  = new Point(inputRow.Width - 170, 10);
            };

            // ── Wrap chat in a panel so Fill works inside the form ────
            var chatWrapper = new Panel { Dock = DockStyle.Fill };
            chatWrapper.Controls.Add(_chat);
            chatWrapper.Controls.Add(_spinner);

            Controls.Add(chatWrapper);
            Controls.Add(_previewPanel);
            Controls.Add(inputRow);
            Controls.Add(header);

            header.BringToFront();

            FormClosing += (_, e) => _cts.Cancel();
        }

        // ══════════════════════════════════════════════════════════════
        // Conversation logic
        // ══════════════════════════════════════════════════════════════

        private async Task InitConversationAsync()
        {
            _history.Add(new OllamaMessage("system", SystemPrompt));

            // Check Ollama is running before sending the greeting
            var (ok, err) = await _llm.CheckAsync();
            if (!ok)
            {
                AppendAssistantChunk(
                    $"⚠️  Could not connect to Ollama:\n{err}\n\n" +
                    "Make sure Ollama is running (`ollama serve`) and the model is pulled, " +
                    "then close and reopen this window.");
                SetBusy(false);
                return;
            }

            // Kick off with a greeting prompt (hidden from the UI history)
            _history.Add(new OllamaMessage("user", "Hello, I'd like to add a new task reminder."));
            await StreamAssistantReplyAsync();
        }

        private void OnSendMessage()
        {
            var text = _input.Text.Trim();
            if (string.IsNullOrEmpty(text)) return;

            _input.Clear();
            AppendUserMessage(text);
            _history.Add(new OllamaMessage("user", text));
            _ = StreamAssistantReplyAsync();
        }

        private async Task StreamAssistantReplyAsync()
        {
            SetBusy(true);
            _cts = new CancellationTokenSource();

            // Reserve a position in the RichTextBox for the streaming reply
            AppendAssistantStart();
            var sb = new StringBuilder();

            try
            {
                await foreach (var token in _llm.StreamChatAsync(_history, _cts.Token))
                {
                    sb.Append(token);
                    AppendAssistantChunk(token);   // stream into the chat
                    Application.DoEvents();        // keep UI responsive
                }
            }
            catch (OperationCanceledException) { sb.Append(" [stopped]"); }
            catch (Exception ex)
            {
                AppendAssistantChunk($"\n\n⚠️ Error: {ex.Message}");
            }

            var fullReply = sb.ToString();
            _history.Add(new OllamaMessage("assistant", fullReply));

            // See if the model embedded a JSON task block
            TryExtractTask(fullReply);

            SetBusy(false);
        }

        // ── JSON extraction ───────────────────────────────────────────

        private void TryExtractTask(string reply)
        {
            // Look for a ```json ... ``` fenced block, or a bare { ... } object
            string? json = null;

            var fenceStart = reply.IndexOf("```json", StringComparison.OrdinalIgnoreCase);
            if (fenceStart >= 0)
            {
                var contentStart = reply.IndexOf('\n', fenceStart) + 1;
                var fenceEnd     = reply.IndexOf("```", contentStart, StringComparison.Ordinal);
                if (fenceEnd > contentStart)
                    json = reply[contentStart..fenceEnd].Trim();
            }
            else
            {
                var braceStart = reply.IndexOf('{');
                var braceEnd   = reply.LastIndexOf('}');
                if (braceStart >= 0 && braceEnd > braceStart)
                    json = reply[braceStart..(braceEnd + 1)].Trim();
            }

            if (json == null) return;

            try
            {
                using var doc  = JsonDocument.Parse(json);
                var root       = doc.RootElement;

                var title       = root.GetProperty("title").GetString() ?? "";
                var description = root.TryGetProperty("description", out var d) ? d.GetString() ?? "" : "";
                var location    = root.TryGetProperty("location", out var l)    ? l.GetString() ?? "" : "";
                var dueDt       = root.GetProperty("dueDateTime").GetString()   ?? "";
                var remindersEl = root.GetProperty("reminders");

                if (!DateTime.TryParse(dueDt, out var due))
                    return;   // malformed — wait for a retry

                var reminders = new List<ReminderOffset>();
                foreach (var r in remindersEl.EnumerateArray())
                {
                    if (Enum.TryParse<ReminderOffset>(r.GetString(), out var ro))
                        reminders.Add(ro);
                }

                _pendingTask = new TaskItem
                {
                    Title       = title,
                    Description = description,
                    Location    = location,
                    DueDateTime = due,
                    Reminders   = reminders
                };

                ShowPreview(_pendingTask);
            }
            catch
            {
                // JSON wasn't valid yet — model may still be typing; ignore silently
            }
        }

        // ── Preview panel ─────────────────────────────────────────────

        private void ShowPreview(TaskItem t)
        {
            if (InvokeRequired) { Invoke(() => ShowPreview(t)); return; }

            var remindersText = t.Reminders.Count == 0
                ? "None"
                : string.Join(", ", t.Reminders.Select(r => r.ToDisplayString()));

            _lblPreview.Text =
                $"Title: {t.Title}\n" +
                $"Due:   {t.DueDateTime:dd MMM yyyy  HH:mm}" +
                (string.IsNullOrWhiteSpace(t.Location) ? "" : $"   |   Location: {t.Location}") + "\n" +
                $"Reminders: {remindersText}";

            _previewPanel.Visible = true;
        }

        private void OnConfirmTask(object? sender, EventArgs e)
        {
            if (_pendingTask == null) return;
            Result = _pendingTask;
            DialogResult = DialogResult.OK;
            Close();
        }

        // ══════════════════════════════════════════════════════════════
        // Chat rendering helpers
        // ══════════════════════════════════════════════════════════════

        private bool _assistantTurnOpen = false;

        private void AppendUserMessage(string text)
        {
            if (InvokeRequired) { Invoke(() => AppendUserMessage(text)); return; }
            _assistantTurnOpen = false;

            _chat.SelectionStart  = _chat.TextLength;
            _chat.SelectionLength = 0;

            _chat.SelectionColor = Color.FromArgb(109, 40, 217);
            _chat.SelectionFont  = new Font("Segoe UI Semibold", 10f);
            _chat.AppendText("\n\nYou:  ");

            _chat.SelectionColor = Color.FromArgb(30, 41, 59);
            _chat.SelectionFont  = new Font("Segoe UI", 10f);
            _chat.AppendText(text);

            _chat.ScrollToCaret();
        }

        private void AppendAssistantStart()
        {
            if (InvokeRequired) { Invoke(AppendAssistantStart); return; }
            _assistantTurnOpen = true;

            _chat.SelectionStart  = _chat.TextLength;
            _chat.SelectionLength = 0;

            _chat.SelectionColor = Color.FromArgb(22, 163, 74);
            _chat.SelectionFont  = new Font("Segoe UI Semibold", 10f);
            _chat.AppendText("\n\nAssistant:  ");
            _chat.SelectionColor = Color.FromArgb(30, 41, 59);
            _chat.SelectionFont  = new Font("Segoe UI", 10f);
        }

        private void AppendAssistantChunk(string token)
        {
            if (InvokeRequired) { Invoke(() => AppendAssistantChunk(token)); return; }

            _chat.SelectionStart  = _chat.TextLength;
            _chat.SelectionLength = 0;
            _chat.SelectionColor  = Color.FromArgb(30, 41, 59);
            _chat.SelectionFont   = new Font("Segoe UI", 10f);
            _chat.AppendText(token);
            _chat.ScrollToCaret();
        }

        private void SetBusy(bool busy)
        {
            if (InvokeRequired) { Invoke(() => SetBusy(busy)); return; }
            _spinner.Visible  = busy;
            _btnSend.Enabled  = !busy;
            _btnStop.Visible  = busy;
            _input.Enabled    = !busy;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) { _cts.Cancel(); _llm.Dispose(); }
            base.Dispose(disposing);
        }
    }
}
