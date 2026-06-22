# Task Reminder — C# WinForms App

A Windows desktop app to create tasks with scheduled reminders that pop up at the right time.

---

## Requirements

- **Windows 10/11**
- **.NET 8 SDK** → https://dotnet.microsoft.com/download/dotnet/8.0
- Visual Studio 2022 *or* VS Code with C# Dev Kit

---

## Quick Start (Command Line)

```powershell
cd TaskReminder
dotnet build
dotnet run
```

Or to publish a standalone `.exe`:

```powershell
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true
# Output: bin\Release\net8.0-windows\win-x64\publish\TaskReminder.exe
```

---

## Features

| Feature | Detail |
|---|---|
| **Add / Edit / Delete tasks** | Title, description, location, due date & time |
| **Multiple reminders per task** | 5 min / 15 min / 30 min / 1 hr / 2 hr / 4 hr / 1 day / 2 days / 1 week before |
| **Popup reminder dialog** | Shows full task detail with Dismiss & Snooze (5 min) buttons |
| **System tray balloon** | Quick notification even when window is hidden |
| **Overdue highlighting** | Red rows for past-due tasks, amber for due within 24 h |
| **System tray icon** | App minimizes to tray instead of closing; double-click to restore |
| **Live clock** | Status bar shows current date & time, refreshed every minute |

---

## Project Structure

```
TaskReminder/
├── TaskReminder.csproj   — Project file (.NET 8, WinForms)
├── Program.cs            — Entry point
├── TaskItem.cs           — Data model + ReminderOffset enum
├── ReminderEngine.cs     — Background timer; fires reminder events
├── MainForm.cs           — Main window (task list, toolbar, tray)
├── TaskDialog.cs         — Add / Edit task dialog
└── ReminderPopup.cs      — Reminder pop-up with snooze
```

---

## How It Works

1. `ReminderEngine` runs a `System.Windows.Forms.Timer` every **30 seconds**.
2. Each tick checks every active task's reminder fire times  
   `(DueDateTime − offset)` against the current clock.
3. If `now` falls within a 35-second window past the scheduled fire time  
   and that `(taskId, offset)` pair hasn't already fired, it raises `ReminderFired`.
4. `MainForm` handles the event on the UI thread, shows a `ReminderPopup`  
   and a tray balloon notification simultaneously.

---

## Extending

- **Persistence** — serialize `_tasks` to JSON (`System.Text.Json`) on exit and reload on startup.
- **Custom snooze duration** — add a `NumericUpDown` to `ReminderPopup`.
- **Sound** — call `SystemSounds.Exclamation.Play()` in `OnReminderFired`.
- **Categories / priorities** — add a `Category` property to `TaskItem` and color-code rows.
