# 🧠 MegaMind – Cybersecurity Chatbot

MegaMind is an interactive, cybersecurity awareness chatbot built with C# (.NET). It greets users by name, responds to input, and provides educational information on key cybersecurity topics through a simple menu-driven interface.

---

## 📋 Features

- Personalised greeting using the user's name
- Typewriter-style animated text output
- Colour-coded console responses for readability
- Greeting audio playback on startup (WAV support)
- Interactive cybersecurity topic menu covering:
  1. What is phishing?
  2. How to create a strong password
  3. What is two-factor authentication (2FA)?
  4. How to recognise a secure website
  5. Safe browsing tips and many more.

---

## ⚙️ Requirements

- Windows (required for `System.Media.SoundPlayer` audio playback)
- Visual Studio 2026
- Code clearity
- Comments in each class/file
- A clear video presentation of your code
- github link

---

##  Usage

Once running, you will be prompted to enter your name. After that, you can type any of the following:

| Input | Response |
|---|---|
| `hello` / `hi` | Personalised greeting |
| `how are you` | Friendly response |
| `what is your purpose` | Bot describes its function |
| `cybersecurity` | Opens the topic menu (1–5) |
| `exit` / `quit` | Exits the chatbot |

---

##  Continuous Integration

This project uses **GitHub Actions** to automatically build the solution on every push and pull request.
- Triggers on every push or pull request to main/master
- Restores NuGet packages (including System.Windows.Extensions)
- Builds the project in Release mode
- Publishes the output to a ./publish folder
---
## Part 2 Features (GUI Upgrade)
- WPF Graphical User Interface replacing the console
- Keyword recognition for cybersecurity topics (password, phishing, privacy, scam, malware and more)
- Random responses per topic using lists — answers vary each time
- Sentiment detection: detects worried, curious, or frustrated tone and responds empathetically
- Memory and recall: remembers your name and favourite topic across the conversation
- Follow-up conversation flow: type "tell me more" to continue any topic without restarting
- Fallback error handling for unrecognised input

## How to Run (Part 2)
1. Clone the repository: git clone https://github.com/Sokabo/PROG6221-Part-1.git
2. Open `MegaMind.sln` in Visual Studio 2022
3. Set the startup project to the WPF project
4. Ensure `greetings.wav` is in the project folder and set to **Copy Always** in its properties
5. Press **F5** or click **Run**

## Prerequisites
- Windows 10 or 11
- Visual Studio 2022
- .NET 8.0 SDK
- `greetings.wav` must be placed in the project root folder for voice greeting to work

## Screenshots
<img width="1174" height="871" alt="image" src="https://github.com/user-attachments/assets/608e96a2-b354-4ee0-8350-a7f3447efc87" />

## Youtube link
- https://youtu.be/uU4i-Mvx3JY


## Releases
| Tag   | Description                                               |
|-------|-----------------------------------------------------------|
| v1.0  | Part 1 – Console chatbot with ASCII art and voice greeting |
| v2.0  | Part 2 – WPF GUI, keyword recognition, random responses   |
| v2.1  | Part 2 – Sentiment detection, memory, conversation flow   |




