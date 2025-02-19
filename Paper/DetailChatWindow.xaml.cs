using System.Windows;
using System.Windows.Input;
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
        public DetailChatWindow(string filePath)
        {
            InitializeComponent();
            this.filePath = filePath;
            LoadPdf(filePath);
            Generate();

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

        private async void Generate()
        {
            string pdfText = ExtractTextFromPdf(filePath);
            GeminiService geminiService = new GeminiService();

            string summary = await geminiService.GetSummaryFromGemini(pdfText);
            SummaryContent.Text = summary;
        }

        private void FlashcardPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (AnswerText.Visibility == Visibility.Collapsed)
            {
                AnswerText.Visibility = Visibility.Visible;
                QuestionText.FontSize = 20;
            }
            else
            {
                AnswerText.Visibility = Visibility.Collapsed;
                QuestionText.FontSize = 24;
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();

            //Application.Current.MainWindow = mainWindow;
        }
    }
}
