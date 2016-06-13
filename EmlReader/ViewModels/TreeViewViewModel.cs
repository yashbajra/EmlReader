using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmlReader
{
    public class TreeViewViewModel : INotifyPropertyChanged
    {
        public TreeViewViewModel()
        {
            TreeViewItems = new ObservableCollection<string>();
        }

        public ObservableCollection<string> TreeViewItems { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
