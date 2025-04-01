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
        
        private int frameIndex = 0; 
        
        private void RoundInputTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Regex.IsMatch(e.Text, "^[0-9]+$"))
            {
                e.Handled = true; // Allows only numerical inputs 
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

        private Random random = new Random(); 

        private void RoundInputTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                int randomValue = random.Next(1, 11); // Generate a number between 1 and 10
                StartingPositionBox.Text = randomValue.ToString();
            }

            if (e.Key == Key.Enter)
            {
                if (int.TryParse(RoundInputTextBox.Text, out int userInput))
                {
                    if (int.TryParse(StartingPositionBox.Text, out int generatedNumber))
                    {
                        if (userInput == generatedNumber)
                        {
                            ResultTextBox.Text = "Strike!";
                            userInput = 10; // Strike always counts as 10
                        }
                        else
                        {
                            ResultTextBox.Text = $"You knocked over {userInput} pins.";
                        }

                        // Update the next TextBox in TotalScore
                        if (frameIndex < TotalScore.Children.Count)
                        {
                            if (TotalScore.Children[frameIndex] is TextBox textBox)
                            {
                                textBox.Text = userInput.ToString(); 
                                frameIndex++; 
                            }
                        }
                    }
                    else
                    {
                        ResultTextBox.Text = "Error reading generated number!";
                    }
                }
                else
                {
                    ResultTextBox.Text = "Invalid input! ⛔";
                }
            }
            
            if (frameIndex >= TotalScore.Children.Count)
            {
                frameIndex = 0; // Reset to start over after all ten boxes have a value
                foreach (var child in TotalScore.Children)
                {
                    if (child is TextBox textBox)
                    {
                        textBox.Text = ""; 
                    }
                }
            }


        }
    }
}