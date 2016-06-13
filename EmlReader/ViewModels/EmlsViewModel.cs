using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EmlReader
{
    public class EmlsViewModel
    {
        public EmlsViewModel()
        {
            Emls = new ObservableCollection<string> { "aa", "bb", "cc" };
            AddEmlCommand = new RelayCommand(x=> Execute(x));
        }

        public ObservableCollection<string> Emls { get; set; }
        
        public RelayCommand AddEmlCommand { get; set; }

        private void Execute(object parameter)
        {
            var openFileDialog = new OpenFileDialog();
            var showDialogRes = openFileDialog.ShowDialog();
            if (showDialogRes.HasValue && showDialogRes.Value)
            {
                Emls.Add(openFileDialog.SafeFileName);
            }
        }
    }
}
