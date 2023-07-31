using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Net;
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

namespace ImageeClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public OpenFileDialog openFileDialog = new OpenFileDialog();
        public MainWindow()
        {
            InitializeComponent();
        }

 

        private void ChooseImage_Click_1(object sender, RoutedEventArgs e)
        {
            openFileDialog.Title = "Select a picture";
            openFileDialog.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png";
            if (openFileDialog.ShowDialog() == true)
            {
                Imagee.Source = new BitmapImage(new Uri(openFileDialog.FileName));
            }
        }
        public byte[] getJPGFromImageControl(BitmapImage imageC)
        {
            MemoryStream memStream = new MemoryStream();
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(imageC));
            encoder.Save(memStream);
            return memStream.ToArray();
        }
        private void SendToClick_Click(object sender, RoutedEventArgs e)
        {
            Task.Run(() =>
            {

                if (Imagee.Source != null)
                {
                    var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    var ipAddress = IPAddress.Parse("10.1.18.7");
                    var port = 27001;
                    var ep = new IPEndPoint(ipAddress, port);

                    try
                    {
                        socket.Connect(ep);

                        if (socket.Connected)
                        {
                            serverInfoLbl.Content = "Connected Server . . .";
                            var bytes = getJPGFromImageControl(Imagee.Source as BitmapImage);
                            socket.Send(bytes);
                        }
                    }
                    catch (Exception ex)
                    {
                        serverInfoLbl.Content = ex.Message;
                    }
                }
                else
                {
                    MessageBox.Show("Please select a photo !", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            });
        }
    }
}
