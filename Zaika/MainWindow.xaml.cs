using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using FlickrNet;

namespace Zaika {
    public partial class MainWindow {
        private string databaseLogin;
        private string databasePassword;

        private readonly List<StackPanel> toy;
        private readonly List<string> toys;

        public MainWindow(string databaseLogin = "kek", string databasePassword = "") {
            this.databasePassword = databasePassword;
            this.databaseLogin = databaseLogin;
            InitializeComponent();

            toys = new List<string>(new[] { "fox", "elephant", "owl" });
            toy = new List<StackPanel>();
            products.ItemsSource = toy;
            products.BorderThickness = new Thickness(2, 2, 2, 2);


            foreach (var toyName in toys) {
                var tb = new TextBlock();
                var im = new Image {
                    Width = 50,
                    Height = 50
                };

                var stp = new StackPanel();
                tb.Text = toyName;
                try {
                    var fl = new Flickr("ce28f896e78baffae502ff23e1df8645", "e4bc6d42f6c0b074");
                    var options = new PhotoSearchOptions { Tags = tb.Text + " toy", PerPage = 1, Page = 1 };
                    fl.PhotosSearchAsync(options, photos => {
                        im.Source = new BitmapImage(new Uri(photos.Result.First().SmallUrl));
                    });
                } catch { }

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
