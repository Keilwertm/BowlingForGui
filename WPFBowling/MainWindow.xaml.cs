using System.Windows;
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
                Total = 100  // starting total, I need to change this to the user inputed round number
            };
            DataContext = ViewModel;
        }

        // Enter is input, and then the total is decresed from the above total. 
        private void RoundInputTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ViewModel.DecreaseTotal();
            }
        }
    }
}