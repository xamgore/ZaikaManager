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
using System.Windows.Shapes;
using FlickrNet;

namespace Zaika
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<string> toys;
        List<StackPanel> toy;

        string databaseLogin;
        string databasePassword;
        
        public MainWindow(string databaseLogin = "kek", string databasePassword = "")
        {
            this.databasePassword = databasePassword;
            this.databaseLogin = databaseLogin;
            InitializeComponent();

            toys = new List<string>(new[] { "fox", "elephant", "owl" });
            toy = new List<StackPanel>();
            products.ItemsSource = toy;
            products.BorderThickness = new Thickness(2, 2, 2, 2);
           

            for (int i = 0; i < toys.Count; i++)
            {
                TextBlock tb = new TextBlock();
                Image im = new Image();
                im.Width = 50;
                im.Height = 50;
                
                StackPanel stp = new StackPanel();
                tb.Text = toys[i];
                try
                {
                    FlickrNet.Flickr fl = new FlickrNet.Flickr("ce28f896e78baffae502ff23e1df8645", "e4bc6d42f6c0b074");
                    var options = new PhotoSearchOptions { Tags = tb.Text + " toy", PerPage = 1, Page = 1 };
                    PhotoCollection photos = fl.PhotosSearch(options);
                    im.Source = new BitmapImage(new Uri(photos.First().SmallUrl));
                }
                catch { }
                stp.Children.Add(im);
                stp.Children.Add(tb);
                stp.Height = 50;
                stp.Orientation = Orientation.Horizontal;
                tb.Margin = new Thickness(10, 17, 0, 10);
                tb.FontSize = 18;
                toy.Add(stp);
            }
        }
    }
}
