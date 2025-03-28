using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WPFBowling.ViewModels;

namespace WPFBowling.Views
{
    public partial class MainWindow : Window
    {
        public BowlingViewModel ViewModel { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            ViewModel = new BowlingViewModel
            {
                Total = 0  // Default starting total
            };
            DataContext = ViewModel;

            // Initialize TextBoxes with 1-10
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
            }
        }
    }
}