using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Sockets;
using InTheHand.Net;

namespace zadanie1
{
    public partial class MainWindow : Window
    {
        BluetoothDeviceInfo[] btDevices;
        BluetoothClient btClient = new BluetoothClient();

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            await Task.Run(() => btDevices = btClient.DiscoverDevices().ToArray());
            devicesComboBox.Items.Clear();
            foreach (BluetoothDeviceInfo device in btDevices)
            {
                devicesComboBox.Items.Add(device.DeviceName);
            }
            
            
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
