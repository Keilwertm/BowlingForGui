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
        private void UpdateCumulativeScores()
        {
            int score = 0;
            int rollIndex = 0;

            for (int frame = 0; frame < 10; frame++)
            {
                if (rollIndex >= rolls.Count)
                    break;

                // Strike
                if (rolls[rollIndex] == 10)
                {
                    if (rollIndex + 2 < rolls.Count)
                    {
                        score += 10 + rolls[rollIndex + 1] + rolls[rollIndex + 2];
                        if (TotalScore.Children[frame] is TextBox scoreBox)
                            scoreBox.Text = score.ToString();
                    }
                    else
                        break;

                    rollIndex += 1;
                }
                // Spare or open
                else if (rollIndex + 1 < rolls.Count)
                {
                    int frameTotal = rolls[rollIndex] + rolls[rollIndex + 1];

                    if (frameTotal == 10) // Spare
                    {
                        if (rollIndex + 2 < rolls.Count)
                        {
                            score += 10 + rolls[rollIndex + 2];
                            if (TotalScore.Children[frame] is TextBox scoreBox)
                                scoreBox.Text = score.ToString();
                        }
                        else break;
                    }
                    else // Open
                    {
                        score += frameTotal;
                        if (TotalScore.Children[frame] is TextBox scoreBox)
                            scoreBox.Text = score.ToString();
                    }

                    rollIndex += 2;
                }
                else
                {
                    break; 
                }
            }
        }

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
        
        Dictionary<int, int> frameScores = new Dictionary<int, int>();
        public BowlingViewModel ViewModel { get; set; }

        private Random random = new Random();

        private void RoundInputTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Regex.IsMatch(e.Text, "^[0-10]+$"))
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

        private List<int> rolls = new List<int>(); 
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

            if (DelivaryResult.Children[currentBoxIndex] is TextBox deliveryBox)
            {
                string rollText;
                
                if (knockdown == 10 && rollInFrame == 1)
                {
                    rollText = "X";
                }
                // SPARE (only on second roll of frame, and total pins in frame == 10)
                else if (rollInFrame == 2 && rolls.Count >= 2 &&
                         rolls[rolls.Count - 2] + knockdown == 10)
                {
                    rollText = "/";
                }
                else
                {
                    rollText = knockdown.ToString();
                }

                deliveryBox.Text = rollText;
            }

            if (PinsLeftover.Children[currentBoxIndex] is TextBox leftoverBox)
                leftoverBox.Text = (remainingPins - knockdown).ToString();

            ResultTextBox.Text = $"You knocked over {knockdown} pin(s)!";
            currentBoxIndex++; 
            if (rollInFrame == 1 && knockdown == 10)
                currentBoxIndex++; 
            remainingPins -= knockdown;
            
            if (rollInFrame == 1)
            {
                if (knockdown == 10) // Strike
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
            
            Console.WriteLine($"Frame: {frameIndex}, RollInFrame: {rollInFrame}, Rolls: {string.Join(",", rolls)}");
            
            UpdateCumulativeScores();
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
                // Okay I just need to fix the round logic, I'm displaying spares and strikes correctly. Then add in round 10 exceptions. Finally, I can do some simple automation. 