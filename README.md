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

