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
using System.Windows.Forms;


namespace zadanie1
{
    public partial class MainWindow : Window
    {
        BluetoothDeviceInfo[] btDevices;
        BluetoothDeviceInfo pairedDevice = null;
        BluetoothClient btClient = new BluetoothClient();
        String selectedDevice = null;

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            scanBtn.IsEnabled = false;
            scanBtn.Content = "Skanowanie...";
            await Task.Run(() =>
            {
                btDevices = btClient.DiscoverDevices().ToArray();
            }
            );
            devicesComboBox.Items.Clear();
            foreach (BluetoothDeviceInfo device in btDevices)
            {
                devicesComboBox.Items.Add(device.DeviceName);
            }
            scanBtn.IsEnabled = true;
            scanBtn.Content = "Skanuj";
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedDevice = devicesComboBox.SelectedItem.ToString();
            selectedDeviceLabel.Content = (selectedDevice);
        }

        private void Pair_Click(object sender, RoutedEventArgs e)
        {
            if (selectedDevice == null) return;

            foreach (BluetoothDeviceInfo device in btDevices)
            {
                if (device.DeviceName.Equals(selectedDevice)) {
                    pairedDevice = device;

                    pairedDevice.Refresh();
                    pairedDevice.SetServiceState(BluetoothService.ObexObjectPush, true);
                    
                    break;
                }
            }

            if (pairedDevice == null)
            {
                pairDeviceBtn.Content = "Null";
                return;
            }


            if (!pairedDevice.Authenticated && BluetoothSecurity.PairRequest(pairedDevice.DeviceAddress, "12345"))
            {
                pairDeviceBtn.Content = "Połączono";
            }
            else
            {
                pairDeviceBtn.Content = "Nie udało się połączyć";
            }
        }
        
       
        private void Choose_File_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                filenameLabel.Content = openFileDialog.FileName;
            }
        }

        private void Send_File_Click(object sender, RoutedEventArgs e)
        {
            String filename = filenameLabel.Content.ToString();
            if (filename == "" || filename == null) return;

            var uri = new Uri("obex://" +  pairedDevice.DeviceAddress + "/" + filename);
            var request = new ObexWebRequest(uri);
            request.ReadFile(filename);
            var response = (ObexWebResponse) request.GetResponse();
            // check response.StatusCode
            response.Close();
        }
    }
}
