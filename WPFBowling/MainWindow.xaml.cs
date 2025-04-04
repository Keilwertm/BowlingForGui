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
        private int knockedDownThisFrame = 0;
        private int currentIndex = 0;
        private int remainingKnockedDown = 0;
        private int remainingPins = 10;
        private Random random = new Random();

        private void RoundInputTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Regex.IsMatch(e.Text, "^[0-9]+$"))
            {
                e.Handled = true; 
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
                    textBox.Text = $"R:{i + 1}"; 
                }
            }

        }
       private void Button_Click(object sender, RoutedEventArgs e)
{
    if (currentIndex < 10)
    {
        // If we are starting a new frame (or after a strike), reset the remaining pins
        if (remainingPins == 0 || knockedDownThisFrame == 0)
        {
            remainingPins = 10; 
        }
        
        int knockdown = random.Next(1, remainingPins + 1);
        
        knockedDownThisFrame = knockdown;
        remainingPins -= knockdown;
        
        if (DelivaryResult.Children[currentIndex] is TextBox textBox)
        {
            textBox.Text = knockdown.ToString();
        }

        // Update the PinsLeftover TextBox with the remaining pins
        if (PinsLeftover.Children[currentIndex] is TextBox leftoverTextBox)
        {
            leftoverTextBox.Text = remainingPins.ToString(); 
        }
        
        int totalScore = remainingPins + knockedDownThisFrame;

        // Update the TotalScore TextBox with the calculated total score
        if (TotalScore.Children[currentIndex] is TextBox totalScoreTextBox)
        {
            totalScoreTextBox.Text = totalScore.ToString(); 
        }
        
        if (totalScore == 10)
        {
            ResultTextBox.Text = $"You knocked over {totalScore} pins!";
        }
        if (remainingPins == 0)
        {
            currentIndex++; 
            knockedDownThisFrame = 0; 
        }
    }
}
private void ResetTextBoxes()
{
    foreach (var child in DelivaryResult.Children)
    {
        if (child is TextBox textBox)
        {
            textBox.Clear(); 
        }
    }
    foreach (var child in PinsLeftover.Children)
    {
        if (child is TextBox textBox)
        {
            textBox.Clear(); // The clearing does not work right now
        }
    }

    foreach (var child in TotalScore.Children)
    {
        if (child is TextBox textBox)
        {
            textBox.Clear(); 
        }
    }
}

    }
}

    // I still have to account for spares, add up total scores, display the first result as the number of pins on the bottom row, restrict int inputs on the box, and add a plus one to the enter limit so it actually displays the score of the tenth round.
    // Once that is done I can throw in some simple automation and test cases to validate at the end.
    // If I have time I want to add the last round to the tenth round, add a Icon to the task bar, improve the look, and add a animation of some sort. 
    // It would be really cool to have the number of pins fall down across the screen corresponding to the amount of pins that you knock down



    // now - change the roll to a button. It just rolls 1-10. You click roll again to hit the spares, it randomly hits the spares. Finally, if there is a strike it skips the spare step. A strike is a ten, not a randomly generated number to make things easier. 
    // I think at the end I can add if they get two strikes I can add another text box to appear for the third round...