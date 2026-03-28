using System;
using System.IO;
using System.Media;

namespace MegaMind
{
    
    // Handles audio playback for the MegaMind chatbot.
    // Searches multiple candidate directories for the audio file
    // and plays it on startup. All failures are handled gracefully —
    // audio is a nice-to-have, not a critical feature.
    
    public class AudioPlayer
    {
        
        // Attempts to play a WAV greeting file by name.
        // Searches the executable directory, common asset subfolders,
        // and walks up the directory tree as a fallback.
       
        // <param name="fileName">WAV filename to search for. Defaults to "greetings.wav".</param>
        public void PlayGreeting(string fileName = "greetings.wav")
        {
            // Nothing to play if no filename was given
            if (string.IsNullOrWhiteSpace(fileName))
                return;

            var exeDir = AppContext.BaseDirectory;

            // Check the most likely locations first before doing a full tree walk
            var candidates = new[]
            {
                Path.Combine(exeDir, fileName),
                Path.Combine(exeDir, "sounds",  fileName),
                Path.Combine(exeDir, "Assets",  fileName),
                Path.Combine(Environment.CurrentDirectory, fileName)
            };

            foreach (var path in candidates)
            {
                if (File.Exists(path))
                {
                    TryPlay(path);
                    return;
                }
            }

            // If not found in immediate locations, walk up the directory tree
            // and check common asset folders at each level
            try
            {
                var dir = new DirectoryInfo(exeDir);
                while (dir != null)
                {
                    var tryPaths = new[]
                    {
                        Path.Combine(dir.FullName, fileName),
                        Path.Combine(dir.FullName, "sounds",    fileName),
                        Path.Combine(dir.FullName, "Assets",    fileName),
                        Path.Combine(dir.FullName, "Resources", fileName)
                    };

                    foreach (var p in tryPaths)
                    {
                        if (File.Exists(p))
                        {
                            // Copy the file next to the executable so future runs find it immediately
                            var dest = Path.Combine(exeDir, fileName);
                            TryCopyAndPlay(p, dest);
                            return;
                        }
                    }

                    // Move one level up the directory tree
                    dir = dir.Parent;
                }
            }
            catch
            {
                // Tree walk is best-effort only; any failure here is non-fatal
            }
        }

       
        // Plays a WAV file at the given path. Silently ignores errors.
        
        private static void TryPlay(string path)
        {
            try
            {
                using var player = new SoundPlayer(path);
                player.Play();
            }
            catch
            {
                // Audio errors are non-critical — the bot continues without sound
            }
        }

        
        // Copies a WAV file to the destination path, then plays it.
        // Silently ignores copy and playback errors.
        
        private static void TryCopyAndPlay(string source, string destination)
        {
            try
            {
                File.Copy(source, destination, overwrite: true);
                TryPlay(destination);
            }
            catch
            {
                // Copy or play failure is non-critical
            }
        }
    }
}