using System;
using System.Threading;
using MegaMind;

// Step 1: Play the audio greeting (best-effort — won't crash if file is missing)
var audio = new AudioPlayer();
audio.PlayGreeting();

//  Step 2: Print the ASCII logo in magenta 
Console.ForegroundColor = ConsoleColor.Magenta;
Console.WriteLine(@"
============================================================================================
      ___           ___           ___           ___                                       
     /__/\         /  /\         /  /\         /  /\                                      
    |  |::\       /  /:/_       /  /:/_       /  /::\                                     
    |  |:|:\     /  /:/ /\     /  /:/ /\     /  /:/\:\                                    
  __|__|:|\:\   /  /:/ /:/_   /  /:/_/::\   /  /:/~/::\                                   
 /__/::::| \:\ /__/:/ /:/ /\ /__/:/__\/\:\ /__/:/ /:/\:\                                  
 \  \:\~~\__\/ \  \:\/:/ /:/ \  \:\ /~~/:/ \  \:\/:/__\/                                  
  \  \:\        \  \::/ /:/   \  \:\  /:/   \  \::/                                       
   \  \:\        \  \:\/:/     \  \:\/:/     \  \:\                                       
    \  \:\        \  \::/       \  \::/       \  \:\                                      
     \__\/         \__\/         \__\/         \__\/                                      
                                          ___                       ___          _____    
                                         /__/\        ___          /__/\        /  /::\   
                                        |  |::\      /  /\         \  \:\      /  /:/\:\  
                                        |  |:|:\    /  /:/          \  \:\    /  /:/  \:\ 
                                      __|__|:|\:\  /__/::\      _____\__\:\  /__/:/ \__\:|
                                     /__/::::| \:\ \__\/\:\__  /__/::::::::\ \  \:\ /  /:/
                                     \  \:\~~\__\/    \  \:\/\ \  \:\~~\~~\/  \  \:\  /:/ 
                                      \  \:\           \__\::/  \  \:\  ~~~    \  \:\/:/  
                                       \  \:\          /__/:/    \  \:\         \  \::/   
                                        \  \:\         \__\/      \  \:\         \__\/    
                                         \__\/                     \__\/                  
============================================================================================");

// ── Step 3: Print the welcome banner in cyan 
Console.ResetColor();
Console.WriteLine();
Console.ForegroundColor = ConsoleColor.Cyan;
Console.WriteLine("  ||*******************************************||");
Console.WriteLine("  ||         MEGA MIND CYBERSECURITY BOT       ||");
Console.WriteLine("  ||    Your personal online safety companion  ||");
Console.WriteLine("  ||*******************************************||");
Console.ResetColor();
Console.WriteLine();

// ── Step 4: Ask for the user's name and personalise the session 
Console.ForegroundColor = ConsoleColor.White;
Console.Write(" What's your name? ");
Console.ResetColor();

var user = new User();
user.Name = (Console.ReadLine() ?? string.Empty).Trim();

// Fall back to a generic name if the user pressed Enter without typing anything
if (string.IsNullOrWhiteSpace(user.Name))
    user.Name = "Sir/Madam";

// ── Step 5: Greet the user and show usage hint 
var bot = new Chatbot(user);
Console.WriteLine();
Console.ForegroundColor = ConsoleColor.Green;
bot.TypeWrite($"  Hello, {user.Name}! Welcome to MegaMind — your cybersecurity companion.\n", 28);
Console.ResetColor();

// Brief hint so the user knows how to start
Console.ForegroundColor = ConsoleColor.DarkGray;
Console.WriteLine("  💡 Try typing: 'hello', 'cybersecurity', or 'exit'");
Console.ResetColor();

// ── Step 6: Hand control to the chatbot's interactive loop 
bot.Start();