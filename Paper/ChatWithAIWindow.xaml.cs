using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Paper
{
    /// <summary>
    /// Interaction logic for ChatWithAIWindow.xaml
    /// </summary>
    public partial class ChatWithAIWindow : Window
    {
        private const string DefaultInputText = "Type your message...";

        public ChatWithAIWindow()
        {
            InitializeComponent();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void MessageInput_GotFocus(object sender, RoutedEventArgs e)
        {
            if (MessageInput.Text == DefaultInputText)
            {
                MessageInput.Text = string.Empty;
            }
        }

        private void MessageInput_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(MessageInput.Text))
            {
                MessageInput.Text = DefaultInputText;
            }
        }

        private void SendMessage_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(MessageInput.Text) && MessageInput.Text != DefaultInputText)
            {
                AddUserMessage(MessageInput.Text);
                // TODO: Add AI response logic
                AddAIMessage("I received your message: " + MessageInput.Text);
                MessageInput.Text = string.Empty;
            }
        }

        private void AddUserMessage(string message)
        {
            var messageBorder = new Border
            {
                Style = (Style)FindResource("UserMessage")
            };

            var messageText = new TextBlock
            {
                Text = message,
                TextWrapping = TextWrapping.Wrap,
                Foreground = Brushes.White
            };

            messageBorder.Child = messageText;
            ChatMessages.Children.Add(messageBorder);
            MessageScroller.ScrollToBottom();
        }

        private void AddAIMessage(string message)
        {
            var messageBorder = new Border
            {
                Style = (Style)FindResource("AIMessage")
            };

            var stackPanel = new StackPanel();

            var nameText = new TextBlock
            {
                Text = "AI Assistant",
                FontSize = 12,
                Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#757575"),
                Margin = new Thickness(0, 0, 0, 5)
            };

            var messageText = new TextBlock
            {
                Text = message,
                TextWrapping = TextWrapping.Wrap,
                Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#424242")
            };

            stackPanel.Children.Add(nameText);
            stackPanel.Children.Add(messageText);
            messageBorder.Child = stackPanel;
            ChatMessages.Children.Add(messageBorder);
            MessageScroller.ScrollToBottom();
        }
    }
}
