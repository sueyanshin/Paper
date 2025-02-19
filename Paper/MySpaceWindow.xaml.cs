using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Paper.Models;

namespace Paper
{
    /// <summary>
    /// Interaction logic for MySpaceWindow.xaml
    /// </summary>
    public partial class MySpaceWindow : Window
    {
        private User user;
        public MySpaceWindow(User user)
        {
            InitializeComponent();
            this.user = user;
            LoadChatHistory();

        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = new MainWindow(user);
            mainWindow.Show();
            this.Close();
        }

        private void LoadChatHistory()
        {
            var histories = new List<ChatHistory>
            {
                new ChatHistory
                {
                    FileName = "AI_Research_Paper.pdf",
                    FilePath = "path/to/file.pdf",
                    LastAccessed = DateTime.Now
                },
                // Add more sample items as needed
            };

            foreach (var history in histories)
            {
                var card = CreateHistoryCard(history);
                HistoryPanel.Children.Add(card);
            }
        }

        private Border CreateHistoryCard(ChatHistory history)
        {
            var card = new Border
            {
                Width = 280,
                Height = 160,
                Margin = new Thickness(0, 0, 20, 20),
                Background = Brushes.White,
                CornerRadius = new CornerRadius(10),
                BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E0E0E0")),
                BorderThickness = new Thickness(1),
                Cursor = Cursors.Hand,
                Effect = new System.Windows.Media.Effects.DropShadowEffect
                {
                    BlurRadius = 10,
                    ShadowDepth = 1,
                    Direction = 270,
                    Opacity = 0.2
                }
            };

            var grid = new Grid();
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.Margin = new Thickness(20);

            // PDF Icon
            var icon = new Path
            {
                Data = Geometry.Parse("M14,2L20,8V20A2,2 0 0,1 18,22H6A2,2 0 0,1 4,20V4A2,2 0 0,1 6,2H14M18,20V9H13V4H6V20H18M16,11H8V13H16V11M16,15H8V17H16V15Z"),
                Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2196F3")),
                Width = 24,
                Height = 24,
                Stretch = Stretch.Uniform,
                HorizontalAlignment = HorizontalAlignment.Left
            };
            Grid.SetRow(icon, 0);

            // File Name
            var fileName = new TextBlock
            {
                Text = history.FileName,
                FontWeight = FontWeights.Medium,
                FontSize = 16,
                Margin = new Thickness(0, 10, 0, 5),
                TextTrimming = TextTrimming.CharacterEllipsis
            };
            Grid.SetRow(fileName, 1);

            // Last Accessed Date
            var lastAccessed = new TextBlock
            {
                Text = $"Last accessed: {history.LastAccessed:yyyy-MM-dd}",
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#757575")),
                FontSize = 12
            };
            Grid.SetRow(lastAccessed, 3);

            grid.Children.Add(icon);
            grid.Children.Add(fileName);
            grid.Children.Add(lastAccessed);

            card.Child = grid;
            card.MouseLeftButtonDown += (s, e) =>
            {
                var detailWindow = new DetailChatWindow(user, "");
                detailWindow.Show();
                this.Close();
            };

            return card;
        }
    }

    public class ChatHistory
    {
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public DateTime LastAccessed { get; set; }
    }
}
