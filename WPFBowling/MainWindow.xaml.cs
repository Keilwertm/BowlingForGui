using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WPFBowling.ViewModels;

namespace WPFBowling.Views
{
    public partial class MainWindow : Window
    {
        private Random _random = new Random();
        
        public BowlingViewModel ViewModel { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            ViewModel = new BowlingViewModel
            {
                Total = 0  // Default starting total
            };
            DataContext = ViewModel;

            roundDisplay(); // Initialize textboxes
        }

        private void roundDisplay()
        {
            // Ensure RoundDisplay exists and initialize its TextBoxes
            for (int i = 0; i < RoundDisplay.Children.Count; i++)
            {
                if (RoundDisplay.Children[i] is TextBox textBox)
                {
                    textBox.Text = (i + 1).ToString();
                }
            }
        }

        // Handle Enter key event for RoundInputTextBox
        private void RoundInputTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (int.TryParse(RoundInputTextBox.Text, out int userValue))
                {
                    ViewModel.Total = userValue; // Update total based on user input
                }
                else
                {
                    MessageBox.Show("Please enter a valid number.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }
        
        private void StartingPositionBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (int.TryParse(StartingPositionBox.Text, out int userInput) && userInput >= 1 && userInput <= 35)
                {
                    int randomValue = _random.Next(1, 36); // Generate 1-35
                    int result = MapToRange(randomValue);

                    ResultTextBox.Text = $"Random: {randomValue}, Result: {result}";
                }
                else
                {
                    MessageBox.Show("Enter a number between 1 and 35.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        private int MapToRange(int number)
        {
            return ((number - 1) * 10 / 34) + 1; // Maps 1-35 → 1-10
        }

        private void StartingPositionBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !int.TryParse(e.Text, out _); // Block non-numeric input
        }
    }
}
