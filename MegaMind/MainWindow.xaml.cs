using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace MegaMind
{
    /// <summary>
    /// Code-behind for MainWindow. Stays thin: UI events only.
    /// All chatbot logic lives in ChatBot, KeywordResponder, SentimentDetector, and MemoryStore.
    /// </summary>
    public partial class MainWindow : Window
    {
        // ── Fields ─────────────────────────────────────────────────────
        private readonly ChatBot _chatBot;
        private readonly AudioPlayer _audio;
        private readonly DispatcherTimer _clockTimer;

        // Colour palette (mirrors XAML resource keys)
        private static readonly SolidColorBrush BotColour   = new(Color.FromRgb(0, 229, 255));    // cyan
        private static readonly SolidColorBrush UserColour  = new(Color.FromRgb(255, 0, 200));    // magenta
        private static readonly SolidColorBrush SystemColour= new(Color.FromRgb(80, 160, 80));    // green
        private static readonly SolidColorBrush DimColour   = new(Color.FromRgb(42, 64, 96));

        // ASCII art for the GUI header (smaller version that fits the window)
        private const string AsciiArt =
            "  ███╗   ███╗███████╗ ██████╗  █████╗ ███╗   ███╗██╗███╗   ██╗██████╗ \n" +
            "  ████╗ ████║██╔════╝██╔════╝ ██╔══██╗████╗ ████║██║████╗  ██║██╔══██╗\n" +
            "  ██╔████╔██║█████╗  ██║  ███╗███████║██╔████╔██║██║██╔██╗ ██║██║  ██║\n" +
            "  ██║╚██╔╝██║██╔══╝  ██║   ██║██╔══██║██║╚██╔╝██║██║██║╚██╗██║██║  ██║\n" +
            "  ██║ ╚═╝ ██║███████╗╚██████╔╝██║  ██║██║ ╚═╝ ██║██║██║ ╚████║██████╔╝\n" +
            "  ╚═╝     ╚═╝╚══════╝ ╚═════╝ ╚═╝  ╚═╝╚═╝     ╚═╝╚═╝╚═╝  ╚═══╝╚═════╝ ";

        // ── Constructor ────────────────────────────────────────────────
        public MainWindow()
        {
            InitializeComponent();

            _chatBot = new ChatBot();
            _audio   = new AudioPlayer();

            // Display ASCII art in header
            LoadAsciiArt();

            // Play voice greeting (best-effort; won't crash if .wav is missing)
            PlayVoiceGreeting();

            // Live clock in header
            _clockTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            _clockTimer.Tick += (_, _) => ClockBlock.Text = DateTime.Now.ToString("yyyy-MM-dd  HH:mm:ss");
            _clockTimer.Start();

            // Show opening bot message
            AppendBotMessage(_chatBot.GetGreeting());
        }

        // ── Private helpers ─────────────────────────────────────────────

        private void LoadAsciiArt()
        {
            AsciiArtBlock.Text = AsciiArt;
        }

        private void PlayVoiceGreeting()
        {
            _audio.PlayGreeting("greetings.wav");
        }

        private void SendMessage()
        {
            var text = UserInputBox.Text.Trim();
            if (string.IsNullOrWhiteSpace(text)) return;

            // Show user message
            AppendUserMessage(text);
            UserInputBox.Clear();
            PlaceholderText.Visibility = Visibility.Visible;

            // Get and display bot response
            var response = _chatBot.ProcessInput(text);
            AppendBotMessage(response);

            ScrollToBottom();
        }

        /// Appends a bot message bubble to the chat panel.</summary>
        private void AppendBotMessage(string message)
        {
            var panel = new StackPanel { Margin = new Thickness(0, 6, 0, 0) };

            // Label
            var label = new TextBlock
            {
                Text = "  MEGAMIND  //",
                Foreground = DimColour,
                FontSize = 10,
                FontFamily = new FontFamily("Consolas"),
                Margin = new Thickness(0, 0, 0, 2)
            };

            // Message bubble
            var border = new Border
            {
                Background = new SolidColorBrush(Color.FromRgb(13, 20, 38)),
                BorderBrush = BotColour,
                BorderThickness = new Thickness(1, 0, 0, 0),
                CornerRadius = new CornerRadius(0, 6, 6, 6),
                Padding = new Thickness(14, 10, 14, 10),
                MaxWidth = 700,
                HorizontalAlignment = HorizontalAlignment.Left
            };

            var text = new TextBlock
            {
                Text = message,
                Foreground = BotColour,
                FontFamily = new FontFamily("Consolas"),
                FontSize = 13,
                TextWrapping = TextWrapping.Wrap,
                LineHeight = 20
            };

            border.Child = text;
            panel.Children.Add(label);
            panel.Children.Add(border);
            ChatPanel.Children.Add(panel);
        }

        /// Appends a user message bubble to the chat panel (right-aligned).</summary>
        private void AppendUserMessage(string message)
        {
            var panel = new StackPanel
            {
                HorizontalAlignment = HorizontalAlignment.Right,
                Margin = new Thickness(0, 6, 0, 0)
            };

            var label = new TextBlock
            {
                Text = "  YOU  //",
                Foreground = DimColour,
                FontSize = 10,
                FontFamily = new FontFamily("Consolas"),
                HorizontalAlignment = HorizontalAlignment.Right,
                Margin = new Thickness(0, 0, 0, 2)
            };

            var border = new Border
            {
                Background = new SolidColorBrush(Color.FromRgb(20, 0, 30)),
                BorderBrush = UserColour,
                BorderThickness = new Thickness(0, 0, 1, 0),
                CornerRadius = new CornerRadius(6, 0, 6, 6),
                Padding = new Thickness(14, 10, 14, 10),
                MaxWidth = 600,
                HorizontalAlignment = HorizontalAlignment.Right
            };

            var text = new TextBlock
            {
                Text = message,
                Foreground = UserColour,
                FontFamily = new FontFamily("Consolas"),
                FontSize = 13,
                TextWrapping = TextWrapping.Wrap,
                LineHeight = 20
            };

            border.Child = text;
            panel.Children.Add(label);
            panel.Children.Add(border);
            ChatPanel.Children.Add(panel);
        }

        private void ScrollToBottom()
        {
            ChatScrollViewer.UpdateLayout();
            ChatScrollViewer.ScrollToBottom();
        }

        // ── UI Event handlers ──────────────────────────────────────────

        private void SendButton_Click(object sender, RoutedEventArgs e) => SendMessage();

        private void UserInputBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SendMessage();
                e.Handled = true;
            }
        }

        private void UserInputBox_GotFocus(object sender, RoutedEventArgs e)
        {
            PlaceholderText.Visibility = Visibility.Collapsed;
        }

        private void UserInputBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(UserInputBox.Text))
                PlaceholderText.Visibility = Visibility.Visible;
        }
    }
}
