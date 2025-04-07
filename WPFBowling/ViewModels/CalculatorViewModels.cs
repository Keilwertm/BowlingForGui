using System.ComponentModel;

// This section updates information and notifies the app when textboxes have been updated

namespace WPFBowling.ViewModels
{
    public class BowlingViewModel : INotifyPropertyChanged
    { 
        private double _total;
        private double _roundInput;
        
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
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }  
        
        
    }  
}