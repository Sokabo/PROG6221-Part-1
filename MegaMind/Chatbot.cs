using System;
using System.Collections.Generic;
using System.Threading;

namespace MegaMind
{
   
    // The core chatbot engine for MegaMind.
    // Handles user input, produces personalised responses,
    // and displays an interactive cybersecurity topic menu.
   
    public class Chatbot
    {
        // The user currently interacting with the bot
        private readonly User _user;

        // Maps each menu option number to its topic label and response colour
        private static readonly Dictionary<int, (string Topic, ConsoleColor Colour)> _menuMeta = new()
        {
            { 1, ("What is phishing?",                   ConsoleColor.Red)     },
            { 2, ("How to create a strong password?",    ConsoleColor.Green)   },
            { 3, ("What is two-factor authentication?",  ConsoleColor.Cyan)    },
            { 4, ("How to recognize a secure website?",  ConsoleColor.Yellow)  },
            { 5, ("Safe browsing tips",                  ConsoleColor.Magenta) },
        };

        public Chatbot(User user)
        {
            _user = user;
        }

        
        // Starts the main chat loop. Reads user input continuously
        // until the user types "exit" or "quit".
        
        public void Start()
        {
            // Keep the conversation going until the user explicitly exits
            while (true)
            {
                Console.Write("\n> ");

                
                // so "Hello", "HELLO", and "hello" all match the same branch
                var input = (Console.ReadLine() ?? string.Empty).ToLower().Trim();

                // Prompt the user if they submitted a blank line
                if (string.IsNullOrWhiteSpace(input))
                {
                    PrintWarning("Please enter something, Sir/Madam.");
                    continue;
                }

                // Exit commands break the loop and end the session
                if (input == "exit" || input == "quit")
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine($"\n  Goodbye, {_user.Name}! Stay safe online.");
                    Console.ResetColor();
                    break;
                }

                // Route the input to the appropriate response
                if (input.Contains("hello") || input.Contains("hi"))
                {
                    Respond(ConsoleColor.Green,
                        $"Hello, {_user.Name}! How can I assist you today?");
                }
                else if (input.Contains("how are you"))
                {
                    Respond(ConsoleColor.Cyan,
                        $"I am amazing now that you are in my presence, {_user.Name}.");
                }
                else if (input.Contains("what is your purpose"))
                {
                    Respond(ConsoleColor.Yellow,
                        $"I can help you with cybersecurity questions, {_user.Name}. Just ask!");
                }
                else if (input.Contains("cybersecurity"))
                {
                    ShowMenu();
                }
                else
                {
                    // Catch-all for unrecognised input — guides the user rather than just failing
                    PrintWarning($"Sorry, I didn't understand that, {_user.Name}. " +
                                  "Try typing 'cybersecurity', 'hello', or 'exit'.");
                }
            }
        }

       
        // Prints a typewriter-animated, colour-coded response and resets colour afterward.
       
        private void Respond(ConsoleColor colour, string message)
        {
            Console.ForegroundColor = colour;
            TypeWrite($"  {message}\n", 28);
            Console.ResetColor();
        }

        
        // Prints a warning/error message in dark yellow to stand out from normal output.
        
        private static void PrintWarning(string message)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine($"!!!  {message}");
            Console.ResetColor();
        }

        
        // Prints text one character at a time with a small delay,
        // creating a typewriter animation effect.
       
        // <param name="text">The text to animate.</param>
        // <param name="delayMs">Milliseconds between each character (default 50ms).</param>
        public void TypeWrite(string text, int delayMs = 50)
        {
            foreach (var c in text)
            {
                Console.Write(c);
                Thread.Sleep(delayMs); // Pause briefly between each character
            }
        }

        
        // Displays the cybersecurity topic menu and handles the user's selection.
        // Uses colour-coded output per topic to make each answer visually distinct.
        
        private void ShowMenu()
        {
            // Menu header
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("\n  ||==============================================||");
            Console.WriteLine($"  ||  Sure, {_user.Name,-38}||");
            Console.WriteLine("  ||  Please choose a cybersecurity topic below:  ||");
            Console.WriteLine("  ||==============================================||");

            // Print each numbered option using the metadata dictionary
            foreach (var kvp in _menuMeta)
            {
                Console.ForegroundColor = kvp.Value.Colour;
                Console.WriteLine($"  || [{kvp.Key}] {kvp.Value.Topic,-41}||");
            }

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("  ||==============================================||");
            Console.ResetColor();

            // Input & validation 
            Console.Write("\n  Enter choice (1–5): ");
            var selection = (Console.ReadLine() ?? string.Empty).Trim();

            // int.TryParse safely converts the text to a number without throwing an exception
            if (!int.TryParse(selection, out int userInput) || userInput < 1 || userInput > 5)
            {
                PrintWarning("Invalid option. Please enter a number from 1 to 5.");
                return;
            }

            // Responses
            // Look up this topic's colour from the metadata dictionary
            var colour = _menuMeta[userInput].Colour;

            Console.ForegroundColor = colour;
            Console.WriteLine();
            PrintDivider();

            switch (userInput)
            {
                case 1:
                    Console.ForegroundColor = colour;
                    TypeWrite(
                        "  PHISHING\n\n" +
                        "  Phishing is a cyber attack that uses disguised emails as a weapon.\n" +
                        "  The goal is to trick the recipient into believing the message is\n" +
                        "  something they want or need — such as a request from their bank —\n" +
                        "  and to click a link or download a malicious attachment.\n", 18);
                    break;

                case 2:
                    Console.ForegroundColor = colour;
                    TypeWrite(
                        "  STRONG PASSWORDS\n\n" +
                        "  Use a mix of uppercase, lowercase, numbers, and special characters.\n" +
                        "  Avoid guessable info like your name or birthdate.\n" +
                        "  Aim for at least 12 characters — the longer, the better.\n" +
                        "  Consider using a passphrase: e.g. 'Coffee!Sunrise#42'\n", 18);
                    break;

                case 3:
                    Console.ForegroundColor = colour;
                    TypeWrite(
                        "  TWO-FACTOR AUTHENTICATION (2FA)\n\n" +
                        "  2FA adds a second layer of security beyond your password.\n" +
                        "  After entering your credentials, you must also provide:\n" +
                        "    • Something you KNOW  — a PIN or security question\n" +
                        "    • Something you HAVE  — a smartphone or hardware token\n" +
                        "    • Something you ARE   — fingerprint or face recognition\n", 18);
                    break;

                case 4:
                    Console.ForegroundColor = colour;
                    TypeWrite(
                        "  RECOGNISING A SECURE WEBSITE\n\n" +
                        "  Look for these three indicators before entering any data:\n" +
                        "    1) The URL starts with 'https://' — the 's' means encrypted.\n" +
                        "    2) A padlock icon appears in the address bar.\n" +
                        "    3) A valid SSL certificate — click the padlock to verify.\n", 18);
                    break;

                case 5:
                    Console.ForegroundColor = colour;
                    TypeWrite(
                        "   SAFE BROWSING TIPS\n\n" +
                        "    1) Keep your browser and operating system up to date.\n" +
                        "    2) Use strong, unique passwords for every account.\n" +
                        "    3) Be cautious with links or attachments from unknown senders.\n" +
                        "    4) Use a reputable, updated antivirus program.\n" +
                        "    5) Avoid sensitive transactions over public Wi-Fi.\n", 18);
                    break;
            }

            PrintDivider();
            Console.ResetColor();

            // Follow-up prompt 
            // Invite the user to explore more rather than leaving them at a dead end
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("\n  Type 'cybersecurity' to explore another topic, or 'exit' to quit.");
            Console.ResetColor();
        }

        
        /// Prints a simple horizontal divider line for visual separation between sections.
        
        private static void PrintDivider()
        {
            Console.WriteLine("  " + new string('─', 52));
        }
    }
}