using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Paper.Models;
using Paper.Services;
namespace Paper
{
    /// <summary>
    /// Interaction logic for DocumentWindow.xaml
    /// </summary>
    public partial class DocumentWindow : Window
    {
        private int contentId;
        private List<Flashcard> flashcards;
        private int currentFlashcardIndex = 0;
        private readonly DatabaseService databaseService;
        private Content content;
        private readonly GeminiService geminiService;
        private User user;
        private readonly ChatService chatService;
        string apiKey = System.Configuration.ConfigurationManager.AppSettings["GeminiApiKey"];


        public DocumentWindow(User user, int contentId)
        {
            InitializeComponent();
            this.contentId = contentId;
            this.user = user;
            databaseService = new DatabaseService();
            geminiService = new GeminiService();
            flashcards = new List<Flashcard>();
            chatService = new ChatService(apiKey);

            // Clear example messages
            ChatMessagesPanel.Children.Clear();

            // Set placeholder text and clear on focus
            MessageInput.GotFocus += (s, e) =>
            {
                if (MessageInput.Text == "Type your message here...")
                {
                    MessageInput.Text = string.Empty;
                }
            };

            MessageInput.LostFocus += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(MessageInput.Text))
                {
                    MessageInput.Text = "Type your message here...";
                }
            };

            Loaded += async (s, e) => await LoadStoredContent();
        }

        private async Task LoadStoredContent()
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;

                // Get content from database
                content = await databaseService.GetContentById(contentId);
                if (content != null)
                {
                    // Load PDF
                    string uploadDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Uploads");
                    string pdfPath = Path.Combine(uploadDir, content.FileName);
                    LoadPdf(pdfPath);

                    // Load flashcards
                    flashcards = await databaseService.GetFlashcardsByContentId(contentId);

                    // Update UI
                    SummaryContent.Text = content.Summary;
                    if (flashcards.Count > 0)
                    {
                        UpdateFlashcardDisplay();
                    }
                }
                else
                {
                    MessageBox.Show("Content not found in database.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading content: {ex.Message}");
            }
            finally
            {
                Mouse.OverrideCursor = null;
            }
        }

        private async void LoadPdf(string filePath)
        {
            if (File.Exists(filePath))
            {
                await pdfViewer.EnsureCoreWebView2Async(null);
                pdfViewer.CoreWebView2.Navigate(filePath);
            }
            else
            {
                MessageBox.Show("PDF file not found in the Uploads folder.");
            }
        }

        private void UpdateFlashcardDisplay()
        {
            if (flashcards.Count == 0)
            {
                QuestionText.Text = "No flashcards available";
                CounterText.Text = "0/0";
                PrevButton.IsEnabled = false;
                NextButton.IsEnabled = false;
                return;
            }

            var currentCard = flashcards[currentFlashcardIndex];

            // Update the flashcard text and styling
            QuestionText.Text = currentCard.IsFlipped ? currentCard.Answer : currentCard.Question;
            QuestionText.Foreground = currentCard.IsFlipped ?
                (SolidColorBrush)new BrushConverter().ConvertFrom("#000000") :
                (SolidColorBrush)new BrushConverter().ConvertFrom("#FF0056FF");
            QuestionText.FontSize = currentCard.IsFlipped ? 20 : 24;

            // Update counter and navigation buttons
            CounterText.Text = $"{currentFlashcardIndex + 1}/{flashcards.Count}";
            PrevButton.IsEnabled = currentFlashcardIndex > 0;
            NextButton.IsEnabled = currentFlashcardIndex < flashcards.Count - 1;
        }

        private void FlashcardPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (flashcards.Count > 0)
            {
                // Flip the current card
                flashcards[currentFlashcardIndex].IsFlipped = !flashcards[currentFlashcardIndex].IsFlipped;
                UpdateFlashcardDisplay();
            }
        }

        private void PrevButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentFlashcardIndex > 0)
            {
                currentFlashcardIndex--;
                // Reset flip state for the new card
                flashcards[currentFlashcardIndex].IsFlipped = false;
                UpdateFlashcardDisplay();
            }
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentFlashcardIndex < flashcards.Count - 1)
            {
                currentFlashcardIndex++;
                // Reset flip state for the new card
                flashcards[currentFlashcardIndex].IsFlipped = false;
                UpdateFlashcardDisplay();
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {

            MySpaceWindow window = new MySpaceWindow(user);
            window.Show();
            this.Close();
        }

        private async Task SendMessage(string message)
        {
            try
            {
                // Add user message to UI
                AddMessageToChat(message, true);

                // Clear input
                MessageInput.Text = string.Empty;

                // Show loading indicator
                LoadingIndicator.Visibility = Visibility.Visible;
                MessageInput.IsEnabled = false;

                // Get response from Gemini
                string response = await chatService.SendMessageAsync($"Based on this PDF content: {content.Summary}\n\nUser question: {message}");

                // Add AI response to UI
                AddMessageToChat(response, false);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error sending message: {ex.Message}");
            }
            finally
            {
                // Hide loading indicator
                LoadingIndicator.Visibility = Visibility.Collapsed;
                MessageInput.IsEnabled = true;
                MessageInput.Focus();
            }
        }

        private void AddMessageToChat(string message, bool isUser)
        {
            var messageBorder = new Border
            {
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(
                    isUser ? "#2196F3" : "#FFFFFF")),
                CornerRadius = new CornerRadius(10),
                Padding = new Thickness(15, 10, 15, 10),
                Margin = new Thickness(0, 5, 0, 5),
                HorizontalAlignment = isUser ? HorizontalAlignment.Right : HorizontalAlignment.Left,
                MaxWidth = 400,
                Effect = new System.Windows.Media.Effects.DropShadowEffect
                {
                    BlurRadius = 4,
                    ShadowDepth = 2,
                    Direction = 270,
                    Opacity = 0.2
                }
            };

            var messageText = new TextBlock
            {
                Text = message,
                TextWrapping = TextWrapping.Wrap,
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(
                    isUser ? "#FFFFFF" : "#000000"))
            };

            messageBorder.Child = messageText;
            ChatMessagesPanel.Children.Add(messageBorder);

            // Scroll to bottom
            var scrollViewer = FindVisualChild<ScrollViewer>(ChatMessagesPanel);
            scrollViewer?.ScrollToBottom();
        }

        private T FindVisualChild<T>(DependencyObject parent) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is T found)
                    return found;
                var result = FindVisualChild<T>(child);
                if (result != null)
                    return result;
            }
            return null;
        }

        // Add event handler for send button
        private async void SendButton_Click(object sender, RoutedEventArgs e)
        {
            string message = MessageInput.Text;
            if (!string.IsNullOrWhiteSpace(message) && message != "Type your message here...")
            {
                await SendMessage(message);
            }
        }

        // Add event handler for Enter key
        private async void MessageInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && !e.KeyboardDevice.Modifiers.HasFlag(ModifierKeys.Shift))
            {
                e.Handled = true;
                string message = MessageInput.Text;
                if (!string.IsNullOrWhiteSpace(message) && message != "Type your message here...")
                {
                    await SendMessage(message);
                }
            }
        }
    }
}
