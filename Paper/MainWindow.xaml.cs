using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Paper.Models;

namespace Paper
{
    public partial class MainWindow : Window
    {
        private string selectedFilePath;
        private User user;

        public MainWindow(User user)
        {
            InitializeComponent();
            this.user = user;
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
                var detailWindow = new DetailChatWindow(user.UserId, selectedFilePath);
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
                        var mySpaceWindow = new MySpaceWindow(user);
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
            MySpaceWindow mySpaceWindow = new MySpaceWindow(user);
            mySpaceWindow.Show();
            this.Close();
        }

        private void TabButton3(object sender, RoutedEventArgs e)
        {
            ChatWithAIWindow chatWithAIWindow = new ChatWithAIWindow(user);
            chatWithAIWindow.Show();
            this.Close();
        }

        private void TabButton4(object sender, RoutedEventArgs e)
        {
            LoginWindow window = new LoginWindow();
            window.Show();
            this.Close();
        }


    }
}
