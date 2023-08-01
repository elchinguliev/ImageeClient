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
using static System.Net.Mime.MediaTypeNames;

namespace ImageeClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public OpenFileDialog openFileDialog = new OpenFileDialog();
        private BitmapImage image;

        public BitmapImage Image
        {
            get { return image; }
            set { image = value; }
        }

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
            var ipAdress = IPAddress.Parse("192.168.1.106");
            var port = 27001;

            Task.Run(() =>
            {
                var ep = new IPEndPoint(ipAdress, port);

                try
                {
                    var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    socket.Connect(ep);

                    if (socket.Connected)
                    {
                        var sendImage = Image;
                        var bytes = getJPGFromImageControl(Image);
                        socket.Send(bytes);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"{ex.Message}");
                }
            });
        }
    }
}
