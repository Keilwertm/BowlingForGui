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
        // Dictionary to store cumulative scores for each frame
        Dictionary<int, int> frameScores = new Dictionary<int, int>();
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
                int randomValue = random.Next(1, 11);
                StartingPositionBox.Text = randomValue.ToString();
            }

            if (e.Key == Key.Enter)
            {
                if (int.TryParse(RoundInputTextBox.Text, out int userInput))
                {
                    if (int.TryParse(StartingPositionBox.Text, out int generatedNumber))
                    {
                        int knockedOverPins = 0; // Variable to store the final result

                        if (userInput == generatedNumber)
                        {
                            ResultTextBox.Text = "Strike!";
                            knockedOverPins = 10;
                        }
                        else if (userInput < generatedNumber)
                        {
                            int randomMoreThan = random.Next(6, 11);
                            ResultTextBox.Text = $"You knocked over {randomMoreThan} pins!";
                            knockedOverPins = randomMoreThan;
                        }
                        else if (userInput > generatedNumber)
                        {
                            int randomMoreThan = random.Next(1, 5);
                            ResultTextBox.Text = $"You knocked over {randomMoreThan} pins!";
                            knockedOverPins = randomMoreThan;
                        }
                        else
                        {
                            ResultTextBox.Text = $"You knocked over {userInput} pins.";
                            knockedOverPins = userInput;
                        }

                        // Updates the textbox to match the knocked over pins for each frame
                        if (frameIndex < DelivaryResult.Children.Count)
                        {
                            if (DelivaryResult.Children[frameIndex] is TextBox textBox)
                            {
                                textBox.Text = knockedOverPins.ToString(); // Store the value in the textbox
                                frameIndex++;
                            }
                        }

                        // Update cumulative score in the dictionary
                        if (frameIndex <= TotalScore.Children.Count)
                        {
                            if (TotalScore.Children[frameIndex - 1] is TextBox totalScoreBox)
                            {
                                // Retrieve the existing total from the dictionary
                                if (!frameScores.TryGetValue(frameIndex - 1, out int currentTotal))
                                {
                                    currentTotal = 0; // Default to 0 if not found
                                }

                                // Add knockedOverPins to the running total
                                int newTotal = currentTotal + knockedOverPins;

                                // Update the dictionary with the new total
                                frameScores[frameIndex + 1] = newTotal;

                                // Display the new total in the TextBox
                                totalScoreBox.Text = newTotal.ToString();

                                // Debugging output
                                Console.WriteLine($"Frame {frameIndex - 1} - Total: {newTotal}");
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

            // Reset the game after 10 frames
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

                // Reset the dictionary to clear previous frame scores
                frameScores.Clear();
            }
        }
    }
}

    // I still have to account for spares, add up total scores, display the first result as the number of pins on the bottom row, restrict int inputs on the box, and add a plus one to the enter limit so it actually displays the score of the tenth round.
    // Once that is done I can throw in some simple automation and test cases to validate at the end.
    // If I have time I want to add the last round to the tenth round, add a Icon to the task bar, improve the look, and add a animation of some sort. 
    // It would be really cool to have the number of pins fall down across the screen corresponding to the amount of pins that you knock down
    