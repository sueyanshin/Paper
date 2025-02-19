using System;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Paper
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private readonly string connectionString = "Data Source=SYS\\SQLEXPRESS;Initial Catalog=paper;Integrated Security=True;";

        public LoginWindow()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string email = EmailTextBox.Text;
            string password = PasswordBox.Password;

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please enter both email and password.", "Login Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            string query = "SELECT COUNT(1) FROM Users WHERE Email = @Email and Password = @Password";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Email", email);
            cmd.Parameters.AddWithValue("@Password", password);
            int count = Convert.ToInt32(cmd.ExecuteScalar());
            conn.Close();

            if (count == 1)
            {
                MessageBox.Show("Login Successful!");
                this.Hide();
                MainWindow mainForm = new MainWindow();
                mainForm.Show();
            }
            else
            {
                MessageBox.Show("Invalid Username or Password.");
            }
        }

        private void RegisterText_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var registerWindow = new RegisterWindow();
            registerWindow.Show();
            this.Close();
        }
    }
}
