using System;
using System.Collections.Generic;
using System.Linq;

namespace MegaMind
{
    /// <summary>
    /// Handles keyword recognition for cybersecurity topics.
    /// Maps keywords to a list of responses; a random one is selected each time
    /// to keep the conversation varied and engaging.
    /// </summary>
    public class KeywordResponder
    {
        private readonly Dictionary<string, List<string>> _responses;
        private readonly Random _random = new();

        public KeywordResponder()
        {
            _responses = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase)
            {
                ["password"] = new List<string>
                {
                    "Use a mix of uppercase, lowercase, numbers, and symbols — at least 12 characters long. Never reuse passwords across sites.",
                    "Consider a passphrase like 'Coffee!Sunrise#42' — long, memorable, and hard to crack.",
                    "Avoid using personal details like your name or birthday. A strong password is random and unique to each account.",
                    "Change your passwords immediately if you suspect a breach, and never share them — not even with IT support."
                },

                ["phishing"] = new List<string>
                {
                    "Be cautious of emails asking for personal information. Scammers often disguise themselves as trusted organisations like banks or government agencies.",
                    "Check the sender's actual email address, not just the display name. Hover over links before clicking to see the real destination.",
                    "Phishing messages often create false urgency — 'Your account will be suspended!' Pause and verify before you act.",
                    "If an email asks you to log in via a link, go directly to the website yourself instead of clicking the link."
                },

                ["privacy"] = new List<string>
                {
                    "Review your app permissions regularly. Many apps request far more access than they actually need.",
                    "Use privacy-focused browsers and search engines. Enable 'Do Not Track' and clear cookies periodically.",
                    "Minimise what you share on social media — your date of birth, home city, and phone number can be used to steal your identity.",
                    "Read privacy policies (or at least the summary) before using a new service. Know what data is collected and how it is used."
                },

                ["scam"] = new List<string>
                {
                    "If an offer seems too good to be true, it almost certainly is. Legitimate companies don't ask for upfront payment to release a prize.",
                    "Scammers often impersonate banks, SARS, or SAPS. Hang up and call the official number directly to verify.",
                    "Never send money or cryptocurrency to someone you have only met online, no matter how convincing their story.",
                    "Verify URLs carefully — scam sites often look identical to real ones but use slightly misspelled domains."
                },

                ["malware"] = new List<string>
                {
                    "Malware is an umbrella term for malicious software. Viruses, ransomware, spyware, and trojans are all types of malware.",
                    "Keep your antivirus updated and run regular scans. Enable real-time protection so threats are caught before they cause damage.",
                    "Avoid downloading software from unofficial sources. Stick to official app stores and vendor websites.",
                    "If your device suddenly slows down, shows unexpected pop-ups, or crashes frequently, malware could be the cause."
                },

                ["vpn"] = new List<string>
                {
                    "A VPN encrypts your internet connection, making it much harder for attackers on the same network to intercept your data.",
                    "Always use a reputable VPN on public Wi-Fi — coffee shops, airports, and hotels are prime hunting grounds for attackers.",
                    "A VPN hides your traffic from your ISP and helps bypass geographic restrictions, but it is not a complete anonymity solution.",
                    "Choose a VPN provider with a strict no-logs policy and an independent audit to back that claim up."
                },

                ["2fa"] = new List<string>
                {
                    "Two-factor authentication adds a second verification step — something you know plus something you have. Enable it everywhere you can.",
                    "Authenticator apps like Google Authenticator or Authy are more secure than SMS codes for 2FA.",
                    "Even if a hacker steals your password, 2FA stops them from accessing your account without your second factor.",
                    "Hardware keys like YubiKey provide the strongest form of 2FA and are nearly impossible to phish."
                },

                ["encryption"] = new List<string>
                {
                    "Encryption scrambles your data so only someone with the correct key can read it. Look for HTTPS and a padlock in your browser.",
                    "Enable full-disk encryption on your laptop and phone. On Windows use BitLocker; on Mac use FileVault.",
                    "End-to-end encryption in messaging apps like Signal means only you and the recipient can read your messages.",
                    "Never send sensitive data like passwords or banking details over unencrypted channels such as plain email or SMS."
                },

                ["firewall"] = new List<string>
                {
                    "A firewall monitors incoming and outgoing network traffic and blocks suspicious connections based on security rules.",
                    "Make sure your operating system's built-in firewall is enabled. It is your first line of defence against network attacks.",
                    "Network firewalls protect an entire organisation, while host-based firewalls protect individual devices.",
                    "Advanced firewalls can detect and block intrusions, perform deep packet inspection, and log suspicious activity."
                },

                ["backup"] = new List<string>
                {
                    "Follow the 3-2-1 rule: 3 copies of your data, on 2 different media types, with 1 copy stored offsite or in the cloud.",
                    "Test your backups regularly — a backup you have never restored from might not actually work when you need it.",
                    "Ransomware encrypts your files and demands payment. Offline backups are your best defence — they cannot be encrypted remotely.",
                    "Automate your backups so they happen without you having to remember. Set it and verify it monthly."
                },

                ["social engineering"] = new List<string>
                {
                    "Social engineering manipulates people rather than systems. Attackers build trust before making their move.",
                    "Be sceptical of unsolicited contact — phone calls, emails, or messages asking for sensitive information or urgent action.",
                    "Pretexting is when an attacker invents a scenario to extract information. Always verify the caller's identity through official channels.",
                    "Train yourself to pause before acting on any urgent request. Pressure and urgency are classic social engineering tactics."
                }
            };
        }

        /// <summary>
        /// Searches the user's input for a known keyword and returns a randomly
        /// selected response from that keyword's list. Returns null if no match.
        /// Also outputs the matched keyword so the caller can store it as the last topic.
        /// </summary>
        public string? GetResponse(string input, out string? matchedKeyword)
        {
            matchedKeyword = null;

            foreach (var kvp in _responses)
            {
                if (input.Contains(kvp.Key, StringComparison.OrdinalIgnoreCase))
                {
                    matchedKeyword = kvp.Key;
                    var list = kvp.Value;
                    return list[_random.Next(list.Count)];
                }
            }

            return null;
        }

        /// <summary>
        /// Returns a response for a specific keyword by name (used for follow-up requests).
        /// </summary>
        public string? GetResponseForTopic(string topic)
        {
            if (_responses.TryGetValue(topic, out var list))
                return list[_random.Next(list.Count)];

            return null;
        }

        /// <summary>Returns all recognised keyword names (used to answer "what can you do").</summary>
        public IEnumerable<string> GetAllKeywords() => _responses.Keys;
    }
}
