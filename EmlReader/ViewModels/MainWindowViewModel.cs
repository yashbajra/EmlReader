using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmlReader
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private EmlViewModel _selectedItem;

        private MainWindowViewModel()
        {
            EmlFiles = new ObservableCollection<EmlViewModel>();
        }

        public MainWindowViewModel(string filename) : this()
        {
            AddEmlFile(filename);
        }

        public ObservableCollection<EmlViewModel> EmlFiles { get; set; }

        public EmlViewModel SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                if (_selectedItem != value)
                {
                    _selectedItem = value;
                    OnPropertyChanged(nameof(SelectedItem));
                }
            }
        }
        
        public event PropertyChangedEventHandler PropertyChanged;
        
        public void AddEmlFile(string filename)
        {
            var evm = new EmlViewModel(this, filename);
            EmlFiles.Add(evm);
            evm.IsSelected = true;
        }

        private void OnPropertyChanged(string propertyName)
        {
            var handler = this.PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
