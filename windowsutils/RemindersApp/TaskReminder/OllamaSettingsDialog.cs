using System;
using System.Drawing;
using System.Windows.Forms;

namespace TaskReminder
{
    /// <summary>
    /// Simple settings dialog for Ollama URL and model name.
    /// Values persist in-memory for the session (extend with app settings for persistence).
    /// </summary>
    public sealed class OllamaSettingsDialog : Form
    {
        private readonly TextBox _txtUrl   = new();
        private readonly TextBox _txtModel = new();
        private readonly Label   _lblStatus= new();

        public string OllamaUrl   { get; private set; }
        public string ModelName   { get; private set; }

        public OllamaSettingsDialog(string currentUrl = "http://localhost:11434", string currentModel = "llama3")
        {
            OllamaUrl  = currentUrl;
            ModelName  = currentModel;
            BuildUI(currentUrl, currentModel);
        }

        private void BuildUI(string url, string model)
        {
            Text            = "Ollama Settings";
            Size            = new Size(440, 310);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition   = FormStartPosition.CenterParent;
            MaximizeBox     = false;
            MinimizeBox     = false;
            BackColor       = Color.FromArgb(248, 250, 252);
            Font            = new Font("Segoe UI", 9.5f);

            int y = 24;

            void Field(string label, TextBox tb, string value, string hint)
            {
                Controls.Add(new Label
                {
                    Text      = label,
                    Font      = new Font("Segoe UI Semibold", 9f),
                    ForeColor = Color.FromArgb(71, 85, 105),
                    Location  = new Point(24, y),
                    AutoSize  = true
                });
                y += 22;

                tb.Location        = new Point(24, y);
                tb.Width           = 376;
                tb.Text            = value;
                tb.BorderStyle     = BorderStyle.FixedSingle;
                tb.Font            = new Font("Segoe UI", 10f);
                tb.PlaceholderText = hint;
                Controls.Add(tb);
                y += 42;
            }

            Field("Ollama Base URL", _txtUrl,   url,   "http://localhost:11434");
            Field("Model Name",       _txtModel, model, "e.g. llama3, mistral, phi3");

            // Hint text
            var hint = new Label
            {
                Text      = "Tip:  ollama pull llama3   (run in terminal to download the model)",
                Font      = new Font("Segoe UI", 8.5f),
                ForeColor = Color.FromArgb(148, 163, 184),
                Location  = new Point(24, y),
                AutoSize  = true
            };
            Controls.Add(hint);
            y += 28;

            // Status label (shown after Test)
            _lblStatus.Location  = new Point(24, y);
            _lblStatus.Size      = new Size(376, 20);
            _lblStatus.Font      = new Font("Segoe UI", 9f);
            _lblStatus.ForeColor = Color.FromArgb(100, 116, 139);
            Controls.Add(_lblStatus);
            y += 28;

            // Buttons
            var btnTest = new Button
            {
                Text      = "Test Connection",
                Location  = new Point(24, y),
                Size      = new Size(140, 34),
                BackColor = Color.FromArgb(71, 85, 105),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font      = new Font("Segoe UI", 9.5f)
            };
            btnTest.FlatAppearance.BorderSize = 0;
            btnTest.Click += async (_, _) =>
            {
                btnTest.Enabled    = false;
                _lblStatus.Text    = "Testing…";
                _lblStatus.ForeColor = Color.FromArgb(100, 116, 139);

                using var client = new OllamaClient(_txtUrl.Text.Trim(), _txtModel.Text.Trim());
                var (ok, err) = await client.CheckAsync();

                _lblStatus.ForeColor = ok ? Color.FromArgb(22, 163, 74) : Color.FromArgb(220, 38, 38);
                _lblStatus.Text      = ok ? "✔  Connected successfully!" : $"✘  {err}";
                btnTest.Enabled = true;
            };

            var btnSave = new Button
            {
                Text         = "Save",
                Location     = new Point(290, y),
                Size         = new Size(110, 34),
                BackColor    = Color.FromArgb(37, 99, 235),
                ForeColor    = Color.White,
                FlatStyle    = FlatStyle.Flat,
                Font         = new Font("Segoe UI Semibold", 9.5f),
                DialogResult = DialogResult.OK
            };
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Click += (_, _) =>
            {
                OllamaUrl  = _txtUrl.Text.Trim();
                ModelName  = _txtModel.Text.Trim();
                DialogResult = DialogResult.OK;
                Close();
            };

            Controls.Add(btnTest);
            Controls.Add(btnSave);

            AcceptButton = btnSave;
        }
    }
}
