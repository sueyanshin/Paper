using System.Windows;

namespace Paper
{
    /// <summary>
    /// Interaction logic for TestWindow.xaml
    /// </summary>
    public partial class TestWindow : Window
    {
        public TestWindow()
        {
            InitializeComponent();
            //LoadPdf("C:\\Users\\Acer\\Documents\\c-sharp\\Ado.pdf");
        }
        //private async void LoadPdf(string filePath)
        //{
        //    await pdfViewer.EnsureCoreWebView2Async(null);
        //    pdfViewer.CoreWebView2.Navigate(filePath);
        //}

    }
}
