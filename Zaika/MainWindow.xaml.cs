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
using FlickrNet;
using MaterialSkin;

namespace Zaika
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //FlickrNet.Flickr fl = new FlickrNet.Flickr("ce28f896e78baffae502ff23e1df8645", "e4bc6d42f6c0b074");
            //var options = new PhotoSearchOptions { Tags = "fox toy", PerPage = 1, Page = 1 };
            //PhotoCollection photos = fl.PhotosSearch(options);
            //image.Source = new BitmapImage(new Uri(photos.First().SmallUrl));

        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
           // if (login.Text == "Foxxyy")
           // {
                Window1 win = new Window1();
                win.Show();
                this.Close();
           // }
        }

        private void login_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                passwordBox.Focus();
        }

        private void passwordBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                button_Click(sender, e);
        }
    }
}
