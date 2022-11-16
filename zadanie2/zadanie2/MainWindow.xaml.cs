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
using PCSC;
using PCSC.Utils;

namespace zadanie2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private static SCardContext cardContext;
        private static String[] readerNames;

        private void ConnectWithReader(object sender, RoutedEventArgs e)
        {
            cardContext = new SCardContext();
            cardContext.Establish(SCardScope.System);
            readerNames = cardContext.GetReaders();

            if (readerNames == null)
            {
                statusLabel.Content = "null";
            }

            else
            {
                statusLabel.Content = readerNames[0];
            }
        }

        private void ConnectWithChip(object sender, RoutedEventArgs e)
        {
            if (readerNames == null) return;

            var reader = new SCardReader(cardContext);
            var connection = reader.Connect(readerNames[0], SCardShareMode.Shared, SCardProtocol.Any);

            if (connection != SCardError.Success)
            {
                chipStatusLabel.Content = "Nie udało się połączyć z kartą";
            }

            chipStatusLabel.Content = "Połączono z kartą";
        }
    }


}
