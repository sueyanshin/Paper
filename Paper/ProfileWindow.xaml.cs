using System;
using System.Windows;
using System.Windows.Input;
using Paper.Models;
using Paper.Services;

namespace Paper
{
    /// <summary>
    /// Interaction logic for ProfileWindow.xaml
    /// </summary>
    public partial class ProfileWindow : Window
    {
        private readonly DatabaseService databaseService;
        private readonly User user;

        public ProfileWindow(User user)
        {
            InitializeComponent();
            this.user = user;
            databaseService = new DatabaseService();

            // Set user info
            UserNameText.Text = user.Name;
            UserEmailText.Text = user.Email;
        }

        private async void DeleteAccount_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show(
                "Are you sure you want to delete your account? This will delete all your contents and flashcards. This action cannot be undone.",
                "Confirm Delete Account",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning
            );

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    Mouse.OverrideCursor = Cursors.Wait;
                    await databaseService.DeleteUser(user.UserId);

                    // Show login window
                    var loginWindow = new LoginWindow();
                    loginWindow.Show();

                    // Close all windows
                    foreach (Window window in Application.Current.Windows)
                    {
                        if (window != loginWindow)
                        {
                            window.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting account: {ex.Message}");
                }
                finally
                {
                    Mouse.OverrideCursor = null;
                }
            }
        }
    }
}
