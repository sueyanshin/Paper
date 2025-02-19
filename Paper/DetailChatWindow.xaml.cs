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
using UglyToad.PdfPig;

namespace Paper
{
    /// <summary>
    /// Interaction logic for DetailChatWindow.xaml
    /// </summary>
    public partial class DetailChatWindow : Window
    {

        private string filePath;
        private List<Flashcard> flashcards;
        private int currentFlashcardIndex = 0;
        private readonly GeminiService geminiService;
        private string pdfText;
        private User user;
        private readonly DatabaseService databaseService;
        private int userId; // Set this when user logs in
        private readonly ChatService chatService;

        public DetailChatWindow(User user, string filePath)
        {
            InitializeComponent();
            this.filePath = filePath;
            this.user = user;
            this.userId = user.UserId;
            geminiService = new GeminiService();
            databaseService = new DatabaseService();
            flashcards = new List<Flashcard>();
            string apiKey = System.Configuration.ConfigurationManager.AppSettings["GeminiApiKey"];
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

            LoadPdf(filePath);
            Loaded += async (s, e) => await GenerateAndSaveContent();
        }
        private async void LoadPdf(string filePath)
        {
            await pdfViewer.EnsureCoreWebView2Async(null);
            pdfViewer.CoreWebView2.Navigate(filePath);
        }
        private string ExtractTextFromPdf(string filePath)
        {
            using (PdfDocument document = PdfDocument.Open(filePath))
            {
                string text = "";
                foreach (var page in document.GetPages())
                {
                    text += page.Text;
                }
                return text;
            }
        }

        private async Task GenerateAndSaveContent()
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;

                // Extract PDF text
                pdfText = ExtractTextFromPdf(filePath);

                // Generate summary and flashcards
                var summary = await geminiService.GetSummaryFromGemini(pdfText);
                flashcards = await geminiService.GenerateFlashcardsFromText(pdfText);

                // Create content object
                var content = new Content
                {
                    UserId = userId,
                    FileName = Path.GetFileName(filePath),
                    Summary = summary,
                    Flashcards = flashcards
                };

                // Save to database
                int contentId = await databaseService.SaveContent(content, filePath);
                await databaseService.SaveFlashcards(contentId, flashcards);

                // Update UI
                SummaryContent.Text = summary;
                if (flashcards.Count > 0)
                {
                    UpdateFlashcardDisplay();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating content: {ex.Message}");
            }
            finally
            {
                Mouse.OverrideCursor = null;
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
            //if (AnswerText.Visibility == Visibility.Collapsed)
            //{
            //    AnswerText.Visibility = Visibility.Visible;
            //    QuestionText.FontSize = 20;
            //}
            //else
            //{
            //    AnswerText.Visibility = Visibility.Collapsed;
            //    QuestionText.FontSize = 24;
            //}
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
            MainWindow mainWindow = new MainWindow(user);
            mainWindow.Show();
            this.Close();

            //Application.Current.MainWindow = mainWindow;
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
                string response = await chatService.SendMessageAsync($"Based on this PDF content: {pdfText}\n\nUser question: {message}");

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

        private async void SendButton_Click(object sender, RoutedEventArgs e)
        {
            string message = MessageInput.Text;
            if (!string.IsNullOrWhiteSpace(message) && message != "Type your message here...")
            {
                await SendMessage(message);
            }
        }

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
