using System;
using System.Collections.Generic;

namespace MegaMind
{
    /// <summary>
    /// Stores user-specific information across the conversation so the chatbot
    /// can personalise its responses and reference earlier statements.
    /// </summary>
    public class MemoryStore
    {
        // First-class properties for the most commonly recalled items
        public string UserName { get; set; } = "there";
        public string FavouriteTopic { get; set; } = string.Empty;

        // General-purpose key-value store for any other remembered facts
        private readonly Dictionary<string, string> _store = new(StringComparer.OrdinalIgnoreCase);

        /// <summary>Saves an arbitrary key-value pair for later recall.</summary>
        public void Store(string key, string value)
        {
            if (!string.IsNullOrWhiteSpace(key))
                _store[key] = value;
        }

        /// <summary>
        /// Retrieves a stored value by key.
        /// Returns null if the key has not been stored.
        /// </summary>
        public string? Recall(string key)
        {
            return _store.TryGetValue(key, out var val) ? val : null;
        }

        /// <summary>
        /// Builds a personalised opening sentence using the stored user name and
        /// favourite topic (if any). Used to enrich chatbot responses.
        /// </summary>
        public string GetPersonalisedOpener()
        {
            if (!string.IsNullOrWhiteSpace(FavouriteTopic))
                return $"As someone interested in {FavouriteTopic}, {UserName}, you may find this especially useful — ";

            return $"{UserName}, here is something worth knowing: ";
        }

        /// <summary>
        /// Returns true if the user has mentioned an interest in a topic.
        /// Stores it and updates FavouriteTopic when found.
        /// </summary>
        public bool TryDetectAndStoreFavouriteTopic(string input)
        {
            // Patterns: "I am interested in X", "I like X", "I care about X"
            var patterns = new[]
            {
                "i am interested in ",
                "i'm interested in ",
                "i like ",
                "i care about ",
                "i want to learn about ",
                "i love "
            };

            var lower = input.ToLowerInvariant();
            foreach (var pattern in patterns)
            {
                var idx = lower.IndexOf(pattern, StringComparison.Ordinal);
                if (idx >= 0)
                {
                    var topic = input[(idx + pattern.Length)..].Trim().TrimEnd('.', '!', '?');
                    if (!string.IsNullOrWhiteSpace(topic))
                    {
                        FavouriteTopic = topic;
                        Store("favourite_topic", topic);
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
