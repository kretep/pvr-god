using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
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

        private ProcessManager pm = new ProcessManager();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtLocalIP.Text = GetLocalIPAddresses();
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
    }
}
