﻿using System;
using System.Configuration;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Paper.Services;

namespace Paper
{
    /// <summary>
    /// Interaction logic for ChatWithAIWindow.xaml
    /// </summary>
    public partial class ChatWithAIWindow : Window
    {
        private const string DefaultInputText = "Type your message...";
        private readonly ChatService chatService;

        public ChatWithAIWindow()
        {
            InitializeComponent();

            // Initialize chat service with API key
            string apiKey = ConfigurationManager.AppSettings["GeminiApiKey"];
            chatService = new ChatService(apiKey);

            // Disable input until initial message is received
            MessageInput.IsEnabled = false;
            InitializeChat();
        }

        private async void InitializeChat()
        {
            try
            {
                string response = await chatService.SendMessageAsync(
                    "You are an AI assistant helping users learn from PDF documents. " +
                    "Please provide a friendly greeting and offer to help."
                );
                AddAIMessage(response);
                MessageInput.IsEnabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error initializing chat: {ex.Message}");
            }
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

        private async void SendMessage_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(MessageInput.Text) && MessageInput.Text != DefaultInputText)
            {
                string userMessage = MessageInput.Text;
                MessageInput.Text = string.Empty;
                MessageInput.IsEnabled = false;

                // Add user message to chat
                AddUserMessage(userMessage);

                try
                {
                    // Get AI response
                    string response = await chatService.SendMessageAsync(userMessage);
                    AddAIMessage(response);
                }
                catch (Exception ex)
                {
                    AddAIMessage($"Error: {ex.Message}");
                }
                finally
                {
                    MessageInput.IsEnabled = true;
                    MessageInput.Focus();
                }
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

        private void MessageInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && !e.KeyboardDevice.IsKeyDown(Key.LeftShift))
            {
                e.Handled = true;
                SendMessage_Click(sender, e);
            }
        }

        // Add loading indicator
        private void ShowLoadingIndicator(bool show)
        {
            LoadingIndicator.Visibility = show ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
