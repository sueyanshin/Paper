using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using PdfiumViewer;

namespace Paper
{
    /// <summary>
    /// Interaction logic for DetailChatWindow.xaml
    /// </summary>
    public partial class DetailChatWindow : Window
    {
        private PdfViewer pdfViewer;

        public DetailChatWindow()
        {
            InitializeComponent();
            // LoadPdfViewer();
        }

        // private void LoadPdfViewer()
        // {
        //     try
        //     {
        //         // Create PDF viewer
        //         pdfViewer = new PdfViewer();
        //         PdfHost.Child = pdfViewer;

        //         // Load sample PDF from resources or a file
        //         var samplePdfPath = "Resources/sample.pdf"; // Add your sample PDF to the project
        //         if (File.Exists(samplePdfPath))
        //         {
        //             using (var stream = File.OpenRead(samplePdfPath))
        //             {
        //                 pdfViewer.Document = PdfDocument.Load(stream);
        //             }
        //         }
        //         else
        //         {
        //             MessageBox.Show("Sample PDF file not found.");
        //         }
        //     }
        //     catch (Exception ex)
        //     {
        //         MessageBox.Show($"Error loading PDF: {ex.Message}");
        //     }
        // }

        //private void BackButton_Click(object sender, RoutedEventArgs e)
        //{
        //    var mySpaceWindow = new MySpaceWindow();
        //    mySpaceWindow.Show();
        //    this.Close();
        //}

        // protected override void OnClosed(EventArgs e)
        // {
        //     base.OnClosed(e);
        //     if (pdfViewer != null)
        //     {
        //         pdfViewer.Document?.Dispose();
        //         pdfViewer.Dispose();
        //     }
        // }


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
