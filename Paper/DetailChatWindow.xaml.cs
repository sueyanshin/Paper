using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
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
        //private PdfViewer pdfViewer;

        private string filePath;
        private List<Flashcard> flashcards;
        private int currentFlashcardIndex = 0;
        private readonly GeminiService geminiService;
        private string pdfText;
        private User user;

        public DetailChatWindow(User user, string filePath)
        {
            InitializeComponent();
            this.user = user;
            this.filePath = filePath;
            geminiService = new GeminiService();
            flashcards = new List<Flashcard>();
            LoadPdf(filePath);
            GenerateSummary();
            // Generate flashcards when window loads
            Loaded += async (s, e) => await GenerateFlashcards();


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

        private async void GenerateSummary()
        {
            // Get Text from PDF
            string pdfText = ExtractTextFromPdf(filePath);
            GeminiService geminiService = new GeminiService();

            //Generate summary
            string summary = await geminiService.GetSummaryFromGemini(pdfText);
            SummaryContent.Text = summary;
        }
        private async Task GenerateFlashcards()
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;
                QuestionText.Text = "Generating flashcards...";

                // Get text from pdf
                pdfText = ExtractTextFromPdf(filePath);

                // Generate flashcards
                flashcards = await geminiService.GenerateFlashcardsFromText(pdfText);
                currentFlashcardIndex = 0;

                if (flashcards.Count > 0)
                {
                    UpdateFlashcardDisplay();
                }
                else
                {
                    QuestionText.Text = "No flashcards could be generated";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating flashcards: {ex.Message}");
                QuestionText.Text = "Failed to generate flashcards";
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
    }
}
