using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
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
        private int userId;
        private Content content;
        private readonly GeminiService geminiService;
        private string pdfText;
        private User user;

        public DocumentWindow(User user, int contentId)
        {
            InitializeComponent();
            this.contentId = contentId;
            this.user = user;
            databaseService = new DatabaseService();
            geminiService = new GeminiService();
            flashcards = new List<Flashcard>();
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
    }
}
