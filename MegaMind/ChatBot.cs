using System;
using System.Collections.Generic;
using System.Linq;

namespace MegaMind
{
    
    /// Central chatbot logic class.
    /// MainWindow.xaml.cs calls only <see cref="ProcessInput"/> (and
    /// <see cref="GetGreeting"/> once on startup). All routing, memory,
    /// sentiment, keyword handling, and conversation flow live here.
    
    public class ChatBot
    {
        private readonly KeywordResponder _keywords;
        private readonly SentimentDetector _sentiment;
        private readonly MemoryStore _memory;

        // True until the user has supplied their name
        private bool _awaitingName = true;

        // The last cybersecurity keyword topic discussed (used for follow-up)
        private string? _lastTopic;

        // Follow-up phrases that ask for more on the current topic
        private static readonly HashSet<string> FollowUpPhrases = new(StringComparer.OrdinalIgnoreCase)
        {
            "tell me more", "explain more", "more", "give me another tip",
            "another tip", "more tips", "keep going", "go on", "elaborate",
            "what else", "anything else", "continue"
        };

        // Fallback responses for completely unrecognised input
        private static readonly List<string> Fallbacks = new()
        {
            "I'm not sure I understood that. Try asking about a topic like 'phishing', 'passwords', or 'privacy'.",
            "Hmm, I didn't catch that. You can type a cybersecurity keyword or ask 'what can you do?'",
            "I couldn't find anything helpful for that. Try a topic like 'malware', 'VPN', or '2FA'.",
            "That one's a bit outside my area! Ask me about cybersecurity topics and I'll do my best to help."
        };

        private readonly Random _random = new();

        public ChatBot()
        {
            _keywords = new KeywordResponder();
            _sentiment = new SentimentDetector();
            _memory = new MemoryStore();
        }

        // ─────────────────────────────────────────────────────────────
        // Public API
        // ─────────────────────────────────────────────────────────────

        /// <summary>Returns the opening prompt shown when the app first launches.</summary>
        public string GetGreeting()
        {
            return "Welcome to MegaMind — your cybersecurity companion!\n" +
                   "Before we begin, what is your name?";
        }

        /// <summary>
        /// Main routing method. Accepts raw user input and returns the chatbot's
        /// full response string. Handles name capture, follow-ups, sentiment,
        /// keyword recognition, special phrases, and fallback in that order.
        /// </summary>
        public string ProcessInput(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return "Please type something — I'm here to help!";

            var trimmed = input.Trim();

            // ── 1. Capture the user's name on first message ──────────────
            if (_awaitingName)
            {
                _memory.UserName = trimmed;
                _awaitingName = false;
                return $"Great to meet you, {_memory.UserName}! 🛡️\n" +
                       "I can help you stay safe online. Ask me about topics like " +
                       "'phishing', 'passwords', 'malware', 'VPN', '2FA', 'privacy', or 'scams'.\n" +
                       "You can also say 'what can you do?' to see everything I know.";
            }

            // ── 2. Check if the user declared a favourite topic ──────────
            if (_memory.TryDetectAndStoreFavouriteTopic(trimmed))
            {
                return $"Great! I'll remember that you're interested in {_memory.FavouriteTopic}, " +
                       $"{_memory.UserName}. It's a crucial part of staying safe online.\n" +
                       $"Ask me anything about {_memory.FavouriteTopic} and I'll share what I know.";
            }

            // ── 3. Handle follow-up requests ─────────────────────────────
            if (IsFollowUp(trimmed))
            {
                if (_lastTopic != null)
                {
                    var followUpResponse = _keywords.GetResponseForTopic(_lastTopic);
                    return followUpResponse != null
                        ? $"Here's another tip on {_lastTopic}, {_memory.UserName}:\n{followUpResponse}"
                        : $"I've shared all I have on {_lastTopic} for now, {_memory.UserName}. " +
                          "Try asking about a different topic!";
                }

                return $"What topic would you like to explore, {_memory.UserName}? " +
                       "Try asking about 'phishing', 'passwords', or 'malware'.";
            }

            // ── 4. Detect sentiment ───────────────────────────────────────
            var detectedSentiment = _sentiment.Detect(trimmed);
            var sentimentOpener = _sentiment.GetSentimentResponse(detectedSentiment);

            // ── 5. Keyword recognition ────────────────────────────────────
            var keywordResponse = _keywords.GetResponse(trimmed, out var matchedKeyword);
            if (keywordResponse != null)
            {
                _lastTopic = matchedKeyword;

                // If the user has a stored favourite topic, personalise slightly
                var personalised = _memory.FavouriteTopic.Equals(matchedKeyword ?? string.Empty,
                    StringComparison.OrdinalIgnoreCase)
                    ? $"Since {matchedKeyword} is your favourite topic, {_memory.UserName}, here's a tip: "
                    : string.Empty;

                var suffix = $"\n\nType 'tell me more' if you'd like another tip on {matchedKeyword}.";
                return $"{sentimentOpener}{personalised}{keywordResponse}{suffix}";
            }

            // ── 6. Special phrases ────────────────────────────────────────
            var lower = trimmed.ToLowerInvariant();

            if (lower.Contains("how are you"))
                return $"I'm running at full capacity and ready to help, {_memory.UserName}! What would you like to know?";

            if (lower.Contains("what can you do") || lower.Contains("what can i ask"))
            {
                var topicList = string.Join(", ", _keywords.GetAllKeywords());
                return $"I can help you with the following cybersecurity topics, {_memory.UserName}:\n{topicList}\n\n" +
                       "Just mention any of these in your message and I'll respond with advice!";
            }

            if (lower.Contains("hello") || lower.Contains("hi ") || lower == "hi")
                return $"Hello again, {_memory.UserName}! How can I assist you with cybersecurity today?";

            if (lower.Contains("thank") || lower.Contains("thanks"))
                return $"You're very welcome, {_memory.UserName}! Staying informed is the best defence online.";

            if (lower.Contains("bye") || lower.Contains("goodbye"))
                return $"Stay safe online, {_memory.UserName}! Come back any time.";

            if (lower.Contains("my name is"))
            {
                var name = trimmed[(lower.IndexOf("my name is") + 10)..].Trim();
                if (!string.IsNullOrWhiteSpace(name))
                {
                    _memory.UserName = name;
                    return $"Got it! I'll call you {_memory.UserName} from now on.";
                }
            }

            // ── 7. Fallback ───────────────────────────────────────────────
            return Fallbacks[_random.Next(Fallbacks.Count)];
        }

        // ─────────────────────────────────────────────────────────────
        // Private helpers
        // ─────────────────────────────────────────────────────────────

        private static bool IsFollowUp(string input)
        {
            var lower = input.ToLowerInvariant().Trim();
            return FollowUpPhrases.Any(p => lower == p || lower.StartsWith(p + " ") || lower.EndsWith(" " + p));
        }
    }
}
