using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WPFBowling.ViewModels;

namespace WPFBowling.Views
{
    public partial class MainWindow : Window
    {
        public BowlingViewModel ViewModel { get; set; }

        private void RoundInputTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Regex.IsMatch(e.Text, "^[0-9]+$"))
            {
                e.Handled = true;       // Allows only numerical inputs 
            }
        }
        
        // Displays Rounder Number and sets the textboxes to zero
        public MainWindow()
        {
            InitializeComponent();

            ViewModel = new BowlingViewModel
            {
                Total = 0
            };
            
            DataContext = ViewModel;

            for (int i = 0; i < RoundDisplay.Children.Count; i++)
            {
                if (RoundDisplay.Children[i] is TextBox textBox)
                {
                    textBox.Text = (i + 1).ToString();
                }
            }
        }
        
        // This generates a random number everytime they hit enter on the RoundInputTextBox
        
        private Random random = new Random(); // Define random globally to avoid duplicate values

        private void RoundInputTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                int randomValue = random.Next(1, 36); // Generate a number between 1 and 35
                StartingPositionBox.Text = randomValue.ToString();
            }
        }
        
    }
}