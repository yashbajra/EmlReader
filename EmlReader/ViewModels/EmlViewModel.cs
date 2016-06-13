using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmlReader
{
    public class EmlViewModel : INotifyPropertyChanged
    {
        private MainWindowViewModel _parent;
        private Eml _emlFile;
        private string _name;
        
        private bool _isSelected;

        public event PropertyChangedEventHandler PropertyChanged;

        public EmlViewModel(MainWindowViewModel parent, string fileFullPath)
        {
            _parent = parent;
            _name = fileFullPath;
            PopulateEml(fileFullPath);
        }
        public string Name { get { return _name; } }

        public string From { get { return _emlFile == null ? string.Empty : _emlFile.From; } }
        
        public string To { get { return _emlFile == null ? string.Empty : string.Join(", ", _emlFile.To); } }

        public string CC { get { return  _emlFile == null ? string.Empty : string.Join(", ", _emlFile.CC); } }

        public string Subject { get { return _emlFile == null ? string.Empty : _emlFile.Subject; } }

        public string Body { get { return _emlFile == null ? string.Empty : _emlFile.Body; } }

        public override string ToString()
        {
            return _name;
        }

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    OnPropertyChanged(nameof(IsSelected));
                    if (_isSelected)
                    {
                        _parent.SelectedItem = this;
                    }
                }
            }
        }

        private void PopulateEml(string emlFilePath)
        {
            try
            {
                CDO.Message msg = new CDO.MessageClass();
                ADODB.Stream stream = new ADODB.StreamClass();

                stream.Open(Type.Missing, ADODB.ConnectModeEnum.adModeUnknown, ADODB.StreamOpenOptionsEnum.adOpenStreamUnspecified, String.Empty, String.Empty);
                stream.LoadFromFile(emlFilePath);
                stream.Flush();
                msg.DataSource.OpenObject(stream, "_Stream");
                msg.DataSource.Save();
                stream.Close();

                _emlFile = new Eml
                {
                    From = msg.From,
                    To = msg.To,
                    CC = msg.CC,
                    Body = string.IsNullOrEmpty(msg.HTMLBody) ? msg.TextBody : msg.HTMLBody,
                    Subject = msg.Subject
                };
            }
            catch (Exception ex)
            { }
        }
        
        private void OnPropertyChanged(string propertyName)
        {
            var handler = this.PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
