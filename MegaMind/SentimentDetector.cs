using System;
using System.Collections.Generic;

namespace MegaMind
{
    /// <summary>
    /// Represents the emotional tone detected in a user's message.
    /// </summary>
    public enum Sentiment
    {
        Neutral,
        Worried,
        Curious,
        Frustrated,
        Happy
    }

    /// <summary>
    /// Detects the sentiment of a user's message and returns an empathetic
    /// opening response that the chatbot prepends before its cybersecurity tip.
    /// </summary>
    public class SentimentDetector
    {
        // Maps each sentiment to the words/phrases that trigger it
        private readonly Dictionary<Sentiment, List<string>> _triggerWords = new()
        {
            [Sentiment.Worried] = new List<string>
            {
                "worried", "scared", "afraid", "anxious", "nervous", "unsafe",
                "concerned", "frightened", "panic", "terrified", "fear", "stress",
                "stressed", "uneasy", "alarmed", "help me", "i don't feel safe"
            },
            [Sentiment.Curious] = new List<string>
            {
                "curious", "wondering", "interested", "want to know", "how does",
                "what is", "how do", "tell me about", "explain", "can you tell",
                "i'd like to know", "i want to learn", "teach me", "what happens"
            },
            [Sentiment.Frustrated] = new List<string>
            {
                "frustrated", "annoyed", "confused", "don't understand", "not working",
                "this is stupid", "angry", "irritated", "fed up", "sick of",
                "doesn't make sense", "no idea", "lost", "stuck", "can't figure"
            },
            [Sentiment.Happy] = new List<string>
            {
                "great", "thanks", "thank you", "helpful", "awesome", "amazing",
                "love it", "perfect", "excellent", "fantastic", "brilliant", "well done",
                "good job", "happy", "glad", "pleased", "appreciate"
            }
        };

        private readonly Dictionary<Sentiment, string> _sentimentResponses = new()
        {
            [Sentiment.Worried] =
                "It's completely understandable to feel that way — cybersecurity threats can be daunting. " +
                "Let me share some practical tips to help you feel more secure. ",
            [Sentiment.Curious] =
                "Great question! Curiosity is your best defence online. " +
                "Here is what you should know: ",
            [Sentiment.Frustrated] =
                "I hear you — this stuff can be confusing at first. Let me break it down clearly for you. ",
            [Sentiment.Happy] =
                "Glad to hear that! Staying positive about security is a great mindset. Here is another useful tip: ",
            [Sentiment.Neutral] = string.Empty
        };

        /// <summary>
        /// Analyses the input string and returns the best-matching Sentiment.
        /// Returns Neutral if no trigger words are found.
        /// </summary>
        public Sentiment Detect(string input)
        {
            var lower = input.ToLowerInvariant();

            foreach (var kvp in _triggerWords)
            {
                foreach (var trigger in kvp.Value)
                {
                    if (lower.Contains(trigger))
                        return kvp.Key;
                }
            }

            return Sentiment.Neutral;
        }

        /// <summary>
        /// Returns the empathetic opener for the given sentiment.
        /// Returns an empty string for Neutral so nothing is prepended.
        /// </summary>
        public string GetSentimentResponse(Sentiment sentiment)
        {
            return _sentimentResponses.TryGetValue(sentiment, out var response)
                ? response
                : string.Empty;
        }
    }
}
