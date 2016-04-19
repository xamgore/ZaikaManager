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
        private static readonly Flickr Flickr = new Flickr("ce28f896e78baffae502ff23e1df8645", "e4bc6d42f6c0b074");

        private string databaseLogin;
        private string databasePassword;

        public MainWindow(string databaseLogin = "kek", string databasePassword = "") {
            InitializeComponent();
            DisplayProducts();

            this.databasePassword = databasePassword;
            this.databaseLogin = databaseLogin;
        }

        public void DisplayProducts() {
            var toyNames = new[] { "fox", "elephant", "owl" };

            var toyList = new List<StackPanel>();
            products.ItemsSource = toyList;

            foreach (var name in toyNames) {
                var icon = new Image {
                    Stretch = Stretch.UniformToFill,
                    Width = 50,
                    Height = 50
                };

                Flickr.PhotosSearchAsync(
                    new PhotoSearchOptions { Tags = name + " toy", PerPage = 1, Page = 1 },
                    photos => {
                        if (!photos.HasError)
                            icon.Source = new BitmapImage(new Uri(photos.Result.First().SmallUrl));
                    });

                var description = new TextBlock {
                    Margin = new Thickness(10, 17, 0, 10),
                    Text = name,
                    FontSize = 18
                };

                var stp = new StackPanel {
                    Orientation = Orientation.Horizontal,
                    Children = { icon, description },
                    Height = 50
                };

                toyList.Add(stp);
            }
        }
    }
}
