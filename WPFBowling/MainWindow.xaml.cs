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
        private void ResetGame()
        {
            rolls.Clear();
            rollInFrame = 1;
            remainingPins = 10;
            frameIndex = 0;
            totalScore = 0;
            currentBoxIndex = 0;
            
            foreach (var child in DelivaryResult.Children)
            {
                if (child is TextBox box)
                    box.Text = "";
            }
            
            foreach (var child in PinsLeftover.Children)
            {
                if (child is TextBox box)
                    box.Text = "";
            }
            
            foreach (var child in TotalScore.Children)
            {
                if (child is TextBox box)
                    box.Text = "";
            }

            ResultTextBox.Text = "Game reset. Ready to roll!";
        }

        
        // Dictionary to store cumulative scores for each frame
        Dictionary<int, int> frameScores = new Dictionary<int, int>();
        public BowlingViewModel ViewModel { get; set; }

        private Random random = new Random();

        private void RoundInputTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Regex.IsMatch(e.Text, "^[0-9]+$"))
            {
                e.Handled = true;
            }
        }
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

        private List<int> rolls = new List<int>(); // store individual rolls
        private int rollInFrame = 1; 
        private int remainingPins = 10;
        private int frameIndex = 0;
        private int totalScore = 0;
        private int currentBoxIndex = 0;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (frameIndex >= 10)
            {
                ResultTextBox.Text = "Game over! Resetting game...";
                ResetGame();
                return;
            }


            int knockdown = random.Next(0, remainingPins + 1);
            rolls.Add(knockdown);
            totalScore += knockdown;

            if (DelivaryResult.Children[currentBoxIndex] is TextBox deliveryBox)
                deliveryBox.Text = knockdown.ToString();

            if (PinsLeftover.Children[currentBoxIndex] is TextBox leftoverBox)
                leftoverBox.Text = (remainingPins - knockdown).ToString();

            if (TotalScore.Children[frameIndex] is TextBox scoreBox)
                scoreBox.Text = totalScore.ToString();

            ResultTextBox.Text = $"You knocked over {knockdown} pin(s)!";

            currentBoxIndex++;
            remainingPins -= knockdown;

            if (rollInFrame == 1)
            {
                if (knockdown == 10)
                {
                    frameIndex++;
                    rollInFrame = 1;
                    remainingPins = 10;
                }
                else
                {
                    rollInFrame = 2;
                }
            }
            else
            {
                frameIndex++;
                rollInFrame = 1;
                remainingPins = 10;
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

// I still have to account for spares, add up total scores, display the first result as the number of pins on the bottom row, restrict int inputs on the box, and add a plus one to the enter limit so it actually displays the score of the tenth round.
    // Once that is done I can throw in some simple automation and test cases to validate at the end.
    // If I have time I want to add the last round to the tenth round, add a Icon to the task bar, improve the look, and add a animation of some sort. 
    // It would be really cool to have the number of pins fall down across the screen corresponding to the amount of pins that you knock down

    // now - change the roll to a button. It just rolls 1-10. You click roll again to hit the spares, it randomly hits the spares. Finally, if there is a strike it skips the spare step. A strike is a ten, not a randomly generated number to make things easier. 
    // I think at the end I can add if they get two strikes I can add another text box to appear for the third round...