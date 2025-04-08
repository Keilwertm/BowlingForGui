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
        private int currentRound = 1; // Start from round 1
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
            if (RoundDisplay.Children.Count > 0 && RoundDisplay.Children[0] is TextBox roundDisplayTextBox)
            {
                roundDisplayTextBox.Text = $"{currentRound}:"; // Show current round number
            }

            switch (clickState)
            {
                case 0:
                    firstRoll = rng.Next(0, 11);
                    remainingPins = 10 - firstRoll;

                    if (DelivaryResult.Children[currentBoxIndex] is TextBox firstBox)
                        firstBox.Text = firstRoll.ToString();

                    ResultTextBox.Text = $"First roll: {firstRoll}";
                    clickState = 1;
                    break;

                case 1:
                    if (PinsLeftover.Children[currentBoxIndex] is TextBox previewBox)
                        previewBox.Text = remainingPins.ToString();

                    ResultTextBox.Text = $"Pins left: {remainingPins}";
                    clickState = 2;
                    break;

                case 2:
                    secondRoll = rng.Next(0, remainingPins + 1);

                    if (PinsLeftover.Children[currentBoxIndex] is TextBox secondBox)
                        secondBox.Text = secondRoll.ToString();

                    int total = firstRoll + secondRoll;
                    totalScore += total;

                    if (AllTotalScore.Children.Count > 0 && AllTotalScore.Children[0] is TextBox allTotalBox)
                    {
                        allTotalBox.Text = totalScore.ToString();
                    }

                    if (secondRoll == 0)
                    {
                        ResultTextBox.Text = $"Gutter ball! Second roll: 0, Total: {total}";
                    }
                    else
                    {
                        if (TotalScore.Children.Count > currentBoxIndex)
                        {
                            if (TotalScore.Children[currentBoxIndex] is TextBox totalBox)
                            {
                                totalBox.Text = total.ToString();
                            }
                        }

                        ResultTextBox.Text = $"Second roll: {secondRoll}, Total: {total}";
                    }

                    currentBoxIndex++;
                    clickState = 0;
                    firstRoll = 0;
                    secondRoll = 0;
                    remainingPins = 10;

                    currentRound++;

                    // Update the round display
                    if (RoundDisplay.Children.Count > 0 &&
                        RoundDisplay.Children[0] is TextBox roundDisplayTextBoxUpdate)
                    {
                        roundDisplayTextBoxUpdate.Text = $"{currentRound}:";
                    }

                    // Check if the game is over after 10 rounds
                    if (currentRound > 10)
                    {
                        ResultTextBox.Text = "Game over!";
                        NewGameButton.Visibility = Visibility.Visible;
                    }

                    break;
            }
        }

        private void NewGameButton_Click(object sender, RoutedEventArgs e)
        {
            currentBoxIndex = 0;
            clickState = 0;
            firstRoll = 0;
            secondRoll = 0;
            remainingPins = 10;
            totalScore = 0; // Reset total score when starting a new game
            currentRound = 1; // Start again from round 1

            NewGameButton.Visibility = Visibility.Collapsed;

            // Clear all textboxes
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

            // Reset round display to the first round
            if (RoundDisplay.Children.Count > 0 && RoundDisplay.Children[0] is TextBox roundDisplayTextBoxReset)
            {
                roundDisplayTextBoxReset.Text = $"{currentRound}:";
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
    }
}