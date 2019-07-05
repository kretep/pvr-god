using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PVR_God
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private ProcessManager pm;

        public MainWindow()
        {
            InitializeComponent();
            Console.SetOut(new ControlWriter(txtOutput));
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Display this machine's local IP address
            txtLocalIP.Text = GetLocalIPAddresses();

            // Log version
            var linkTimeLocal = Assembly.GetExecutingAssembly().GetLinkerTime();
            Console.WriteLine("Application version: " + linkTimeLocal);

            // Get things started
            pm = new ProcessManager();
            pm.RunAll();
        }

        public static string GetLocalIPAddresses()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            return host
                .AddressList
                .Where(ip => ip.AddressFamily == AddressFamily.InterNetwork)
                .Select(ip => ip.ToString())
                .Aggregate((a, b) => a + ", " + b);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            pm.EndAll();
        }

        private void btnRestartDatabase_Click(object sender, RoutedEventArgs e)
        {
            pm.RestartDatabase();
        }

        private void btnRestartAPI_Click(object sender, RoutedEventArgs e)
        {
            pm.RestartAPI();
        }

        private void btnRestartServer_Click(object sender, RoutedEventArgs e)
        {
            pm.RestartServer();
        }

        private void btnLaunchClient_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("http://localhost");
        }

        private void btnRestartAll_Click(object sender, RoutedEventArgs e)
        {
            pm.RestartAll();
        }

        private void txtOutput_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtOutput.ScrollToEnd();
        }
    }
}
