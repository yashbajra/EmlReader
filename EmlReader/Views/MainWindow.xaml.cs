using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace EmlReader
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {        
        public MainWindow(string filename)
        {
            InitializeComponent();
            this.DataContext = new MainWindowViewModel(filename);
        }

        // http://stackoverflow.com/questions/624367/how-to-handle-wndproc-messages-in-wpf
        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            HwndSource source = PresentationSource.FromVisual(this) as HwndSource;
            source.AddHook(WndProc);
        }

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == NativeMethods.WM_COPYDATA)
            {
                NativeMethods.COPYDATASTRUCT copyData = (NativeMethods.COPYDATASTRUCT)Marshal.PtrToStructure(lParam, typeof(NativeMethods.COPYDATASTRUCT));
                int dataType = (int)copyData.dwData;
                if (dataType == 2)
                {
                    string filename = Marshal.PtrToStringAnsi(copyData.lpData);

                    // Add the file name to the user control
                    AddEmlFile(filename);
                }
                else
                {
                    MessageBox.Show("Unrecognized data type = " + dataType);
                }
            }
            return IntPtr.Zero;   
        }

        private void AddEmlFile(string file)
        {
            ((MainWindowViewModel)this.DataContext).AddEmlFile(file);
        }
    }
}
