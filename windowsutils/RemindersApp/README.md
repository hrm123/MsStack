# Task Reminder — C# WinForms + Local LLM (Ollama)

A Windows desktop app where you describe a task in plain English and a local
Llama model (via Ollama) holds a conversation with you to gather all the details,
then creates a fully structured reminder automatically.

---

## Requirements

- **Windows 10/11**
- **.NET 8 SDK** → https://dotnet.microsoft.com/download/dotnet/8.0
- **Ollama** → https://ollama.com/download  (runs the local LLM)

---

## Setup (2 steps)

### 1 — Install Ollama and pull a model

```powershell
# Install from https://ollama.com/download, then:
ollama pull llama3        # ~4 GB — recommended
# or lighter alternatives:
ollama pull llama3.2      # smaller / faster
ollama pull phi3          # Microsoft Phi-3 mini
ollama pull mistral
```

### 2 — Run the app

```powershell
cd TaskReminder
dotnet run
```

Publish as a single `.exe`:
```powershell
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true
```

---

## How the AI Assistant works

1. Click **✨ AI Assistant** in the toolbar.
2. The app opens a chat window and Llama greets you — just describe your task
   in plain English, e.g.:
   > "Remind me about the project handover meeting next Thursday at 2pm in the
   >  conference room. I want a day before and an hour before reminder."
3. The model asks follow-up questions for anything missing (title, location,
   exact time, which reminders).
4. Once all details are confirmed, Llama outputs a structured JSON block.
   The app parses it and shows a preview panel — click **✔ Add This Task**.
5. The task appears in the list and reminders fire at the scheduled times.

You can still use **＋ Add Task** for the original manual form if you prefer.

---

## Changing the model

- Click **⚙** (top-right) → enter your Ollama URL and model name → **Test Connection** → **Save**.
- Any model installed via `ollama pull <name>` works.

---

## Project Structure

```
TaskReminder/
├── TaskReminder.csproj       — .NET 8 WinForms project
├── Program.cs                — Entry point
├── TaskItem.cs               — Data model + ReminderOffset enum
├── ReminderEngine.cs         — Background timer; fires reminder events
├── OllamaClient.cs           — Async HTTP client for Ollama /api/chat (streaming)
├── AiTaskDialog.cs           — Chat UI + JSON extraction + preview panel
├── OllamaSettingsDialog.cs   — Configure Ollama URL and model
├── MainForm.cs               — Task list, toolbar, tray icon
├── TaskDialog.cs             — Manual Add/Edit form (still available)
└── ReminderPopup.cs          — Reminder popup with Snooze
```

---

## Architecture: How the LLM integration works

```
User types message
      │
      ▼
AiTaskDialog  ──HTTP POST──►  OllamaClient.StreamChatAsync()
      │                              │
      │        streaming tokens ◄────┘  (Ollama /api/chat, stream:true)
      │
      ▼
 TryExtractTask()  — scans reply for ```json block
      │
      ▼
 JsonDocument.Parse()  — deserializes to TaskItem fields
      │
      ▼
 Preview panel shown  →  user clicks "Add This Task"
      │
      ▼
 DialogResult.OK  →  MainForm adds TaskItem to list + ReminderEngine
```

The system prompt includes today's date/time so the model can resolve relative
phrases like "tomorrow morning" or "next Monday at 3pm" into exact datetimes.

---

## Extending

| Idea | Where to change |
|------|----------------|
| Persist tasks to disk | `MainForm` — serialize `_tasks` to JSON on close |
| Remember Ollama URL across sessions | `OllamaSettingsDialog` + `Properties.Settings` |
| Stream into the text box word-by-word | Already done in `AiTaskDialog.StreamAssistantReplyAsync` |
| Add task categories/priorities | `TaskItem.cs` + `TaskDialog.cs` |
| Custom snooze duration | `ReminderPopup.cs` |
