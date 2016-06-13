using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace EmlReader
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            if (e.Args.Length != 1)
            {
                MessageBox.Show("File name not specified." + Environment.NewLine + "Usage <executable> <emlfile>");
                Environment.Exit(1);
            }
            var file = e.Args[0];
            if (!File.Exists(file))
            {
                MessageBox.Show("File not found. " + Environment.NewLine + file);
                Environment.Exit(2);
            }

            bool isNewInstance;
            Mutex singleMutex = new Mutex(true, "MyAppMutex", out isNewInstance);
            if (isNewInstance)
            {
                // Start the form with the file name as a parameter
                var mainWindow = new MainWindow(file);
                mainWindow.ShowDialog();
                mainWindow.Close();                
            }
            else
            {
                string windowTitle = "View Emls";
                // Find the window with the name of the main form
                IntPtr ptrWnd = NativeMethods.FindWindow(null, windowTitle);
                if (ptrWnd == IntPtr.Zero)
                {
                    MessageBox.Show("No window found with the title " + windowTitle);
                }
                else
                {
                    IntPtr ptrCopyData = IntPtr.Zero;
                    try
                    {
                        // Create the data structure and fill with data
                        NativeMethods.COPYDATASTRUCT copyData = new NativeMethods.COPYDATASTRUCT();
                        copyData.dwData = new IntPtr(2);    // Just a number to identify the data type
                        copyData.cbData = file.Length + 1;  // One extra byte for the \0 character
                        copyData.lpData = Marshal.StringToHGlobalAnsi(file);

                        // Allocate memory for the data and copy
                        ptrCopyData = Marshal.AllocCoTaskMem(Marshal.SizeOf(copyData));
                        Marshal.StructureToPtr(copyData, ptrCopyData, false);

                        // Send the message
                        NativeMethods.SendMessage(ptrWnd, NativeMethods.WM_COPYDATA, IntPtr.Zero, ptrCopyData);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                    finally
                    {
                        // Free the allocated memory after the contol has been returned
                        if (ptrCopyData != IntPtr.Zero)
                            Marshal.FreeCoTaskMem(ptrCopyData);
                    }
                }
                Environment.Exit(1);
            }
        }

    }
}
