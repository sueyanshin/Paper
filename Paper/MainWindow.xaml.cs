using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Paper
{
    public partial class MainWindow : Window
    {
        private string selectedFilePath;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void BrowseFiles_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "PDF files (*.pdf)|*.pdf",
                Title = "Select a PDF File"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                selectedFilePath = openFileDialog.FileName;
                SelectedFileText.Text = "Selected: " + System.IO.Path.GetFileName(selectedFilePath);
                SelectedFileText.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2196F3"));
                StartLearningButton.IsEnabled = true;
            }
        }

        private void StartLearning_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(selectedFilePath))
            {
                var detailWindow = new DetailChatWindow();
                detailWindow.Show();
                this.Close();
            }
        }

        private void TabButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button clickedButton)
            {
                //SetActiveTab(clickedButton);

                switch (clickedButton.Name)
                {
                    case "NewTabButton":
                        MainTabControl.SelectedIndex = 0;
                        break;
                    case "MySpaceBtn":
                        var mySpaceWindow = new MySpaceWindow();
                        mySpaceWindow.Show();
                        this.Close();
                        break;
                    case "ChatButton":
                        // Add handling for Chat tab if needed
                        break;
                    case "LogoutButton":
                        Application.Current.Shutdown();
                        break;
                }
            }
        }

        private void TabButton2(object sender, RoutedEventArgs e)
        {
            MySpaceWindow mySpaceWindow = new MySpaceWindow();
            mySpaceWindow.Show();
            this.Close();
        }

        private void TabButton3(object sender, RoutedEventArgs e)
        {
            ChatWithAIWindow chatWithAIWindow = new ChatWithAIWindow();
            chatWithAIWindow.Show();
            this.Close();
        }
    }
}
