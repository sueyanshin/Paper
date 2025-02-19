using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Paper.Models;
using Paper.Services;

namespace Paper
{
    /// <summary>
    /// Interaction logic for MySpaceWindow.xaml
    /// </summary>
    public partial class MySpaceWindow : Window
    {
        private User user;
        private readonly DatabaseService databaseService;

        public MySpaceWindow(User user)
        {
            InitializeComponent();
            this.user = user;
            databaseService = new DatabaseService();
            Loaded += async (s, e) => await LoadChatHistory();
        }

        private async Task LoadChatHistory()
        {
            try
            {
                var contents = await databaseService.GetContentsByUserId(user.UserId);
                HistoryPanel.Children.Clear();

                foreach (var content in contents)
                {
                    var card = CreateHistoryCard(content);
                    HistoryPanel.Children.Add(card);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading history: {ex.Message}");
            }
        }

        private Border CreateHistoryCard(Content content)
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

            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

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
                Text = content.FileName,
                FontWeight = FontWeights.Medium,
                FontSize = 16,
                Margin = new Thickness(0, 10, 0, 5),
                TextTrimming = TextTrimming.CharacterEllipsis
            };
            Grid.SetRow(fileName, 1);

            // Summary Preview
            var summary = new TextBlock
            {
                Text = content.Summary,
                TextWrapping = TextWrapping.Wrap,
                TextTrimming = TextTrimming.CharacterEllipsis,
                MaxHeight = 40,
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#757575")),
                FontSize = 12
            };
            Grid.SetRow(summary, 2);

            // Created Date
            var createdAt = new TextBlock
            {
                Text = $"Created: {DateTime.Parse(content.CreatedAt):yyyy-MM-dd}",
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#757575")),
                FontSize = 12
            };
            Grid.SetRow(createdAt, 3);

            // Add delete button
            var deleteButton = new Button
            {
                Content = new Path
                {
                    Data = Geometry.Parse("M19,4H15.5L14.5,3H9.5L8.5,4H5V6H19M6,19A2,2 0 0,0 8,21H16A2,2 0 0,0 18,19V7H6V19Z"),
                    Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF5252")),
                    Width = 16,
                    Height = 16,
                    Stretch = Stretch.Uniform
                },
                Width = 24,
                Height = 24,
                Padding = new Thickness(4),
                Background = Brushes.Transparent,
                BorderThickness = new Thickness(0),
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Top,
                Cursor = Cursors.Hand
            };

            deleteButton.Click += async (s, e) =>
            {
                if (MessageBox.Show(
                    "Are you sure you want to delete this content? This action cannot be undone.",
                    "Confirm Delete",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    try
                    {
                        Mouse.OverrideCursor = Cursors.Wait;
                        await databaseService.DeleteContent(content.ContentId);
                        await LoadChatHistory(); // Refresh the list
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error deleting content: {ex.Message}");
                    }
                    finally
                    {
                        Mouse.OverrideCursor = null;
                    }
                }
            };

            // Add delete button to the grid
            Grid.SetRow(deleteButton, 0);
            Grid.SetColumn(deleteButton, 1);
            grid.Children.Add(deleteButton);

            grid.Children.Add(icon);
            grid.Children.Add(fileName);
            grid.Children.Add(summary);
            grid.Children.Add(createdAt);

            card.Child = grid;
            card.MouseLeftButtonDown += (s, e) =>
            {
                var documentWindow = new DocumentWindow(user, content.ContentId);
                documentWindow.Show();
                this.Close();
            };

            return card;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = new MainWindow(user);
            mainWindow.Show();
            this.Close();
        }
    }
}
