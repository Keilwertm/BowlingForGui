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
        private int currentBoxIndex = 0;
        private int clickState = 0; // 0: first roll, 1: show remaining, 2: do second roll + total
        private int firstRoll = 0;
        private int secondRoll = 0;
        private int remainingPins = 10;
        private Random rng = new Random();
        private int currentRound = 0;
        private int totalScore = 0;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
{
    if (currentBoxIndex >= DelivaryResult.Children.Count)
    {
        ResultTextBox.Text = "Game over!";
        return;
    }

    // Update the RoundDisplay TextBox to show the current round with a ":"
    if (RoundDisplay.Children.Count > 0 && RoundDisplay.Children[0] is TextBox roundDisplayBox)
    {
        roundDisplayBox.Text = $"{currentRound}:";
    }

    switch (clickState)
    {
        case 0:
            // First roll (0–10)
            firstRoll = rng.Next(0, 11);
            remainingPins = 10 - firstRoll;

            if (DelivaryResult.Children[currentBoxIndex] is TextBox firstBox)
                firstBox.Text = firstRoll.ToString();

            ResultTextBox.Text = $"First roll: {firstRoll}";
            clickState = 1;
            break;

        case 1:
            // Show how many pins are left (preview)
            if (PinsLeftover.Children[currentBoxIndex] is TextBox previewBox)
                previewBox.Text = remainingPins.ToString();

            ResultTextBox.Text = $"Pins left: {remainingPins}";
            clickState = 2;
            break;

        case 2:
            // Second roll (random from remaining)
            secondRoll = rng.Next(0, remainingPins + 1); // This can generate a 0 for a gutter ball

            if (PinsLeftover.Children[currentBoxIndex] is TextBox secondBox)
                secondBox.Text = secondRoll.ToString();

            int total = firstRoll + secondRoll;

            // Update the cumulative total score
            totalScore += total;

            // Update the AllTotalScore TextBox with the running total
            if (AllTotalScore.Children.Count > 0 && AllTotalScore.Children[0] is TextBox allTotalBox)
            {
                allTotalBox.Text = totalScore.ToString();
            }

            // If gutter ball, display message and total as 0
            if (secondRoll == 0)
            {
                ResultTextBox.Text = "Gutter ball! Second roll: 0, Total: " + total;
            }
            else
            {
                // Find the appropriate TextBox in the TotalScore StackPanel to update the score
                if (TotalScore.Children.Count > currentBoxIndex)
                {
                    if (TotalScore.Children[currentBoxIndex] is TextBox totalBox)
                    {
                        totalBox.Text = total.ToString(); // Set total for that frame
                    }
                }

                ResultTextBox.Text = $"Second roll: {secondRoll}, Total: {total}";
            }

            // Reset for next frame
            currentBoxIndex++;
            clickState = 0;
            firstRoll = 0;
            secondRoll = 0;
            remainingPins = 10;

            // Increment the round count after each frame
            currentRound++;

            // If we've reached 10 rounds, reset the game or display "Game over!"
            if (currentRound > 10)
            {
                ResultTextBox.Text = "Game over!";
                // Make the "New Game?" button visible
                NewGameButton.Visibility = Visibility.Visible;
            }
            break;
    }
}

        // Handle the New Game button click event
        private void NewGameButton_Click(object sender, RoutedEventArgs e)
        {
            // Reset all the variables for a new game
            currentBoxIndex = 0;
            clickState = 0;
            firstRoll = 0;
            secondRoll = 0;
            remainingPins = 10;
            currentRound = 1;

            // Hide the New Game button
            NewGameButton.Visibility = Visibility.Collapsed;

            // Reset the UI (if necessary)
            // For example, clear the TotalScore and DelivaryResult TextBoxes
            foreach (TextBox textBox in DelivaryResult.Children)
            {
                textBox.Clear();
            }

            foreach (TextBox textBox in PinsLeftover.Children)
            {
                textBox.Clear();
            }

            foreach (TextBox textBox in TotalScore.Children)
            {
                textBox.Clear();
            }

            ResultTextBox.Clear();

            // Reset the round display to show the first round
            if (RoundDisplay.Children.Count > 0 && RoundDisplay.Children[0] is TextBox roundDisplayBox)
            {
                // Only reset the round number in the TextBox, not the TextBox itself
                roundDisplayBox.Text = $"{currentRound}:";
            }
        }


        // Close button logic
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        // Drag window logic
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        // Minimize window logic
        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
    }
}


// Okay I just need to fix the round logic, I'm displaying spares and strikes correctly. Then add in round 10 exceptions. Finally, I can do some simple automation. 