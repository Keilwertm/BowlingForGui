using System.ComponentModel;

namespace WPFBowling.ViewModels
{
    public class BowlingViewModel : INotifyPropertyChanged
    {
        private double _total;
        private double _roundInput;

        // Total property, which will be decremented
        public double Total
        {
            get => _total;
            set
            {
                if (_total != value)
                {
                    _total = value;
                    OnPropertyChanged(nameof(Total));
                }
            }
        }

        // Input property for the round value to decrease the total
        public double RoundInput
        {
            get => _roundInput;
            set
            {
                if (_roundInput != value)
                {
                    _roundInput = value;
                    OnPropertyChanged(nameof(RoundInput));
                }
            }
        }

        // Method to decrease the total by round input
        public void DecreaseTotal()
        {
            Total -= RoundInput;  
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}