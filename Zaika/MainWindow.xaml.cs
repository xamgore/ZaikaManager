using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using FlickrNet;
using MicroLite.Builder;
using Zaika.Core;

namespace Zaika {
    public partial class MainWindow {
        private static readonly Flickr Flickr = new Flickr("ce28f896e78baffae502ff23e1df8645", "e4bc6d42f6c0b074");

        public MainWindow() {
            InitializeComponent();

            //DisplayProducts(
            //    DB.Zaika.Fetch<Product>(
            //        SqlBuilder.Select("*").From(typeof (Product)).ToSqlQuery()));

        }

        public void DisplayProducts(IList<Product> list) {
            var panelItems = new List<StackPanel>();

            foreach (var toy in list) {
                var icon = new Image {
                    Stretch = Stretch.UniformToFill,
                    Width = 50,
                    Height = 50
                };

                Flickr.PhotosSearchAsync(
                    new PhotoSearchOptions { Tags = toy.Name + " toy", PerPage = 1, Page = 1 },
                    photos => {
                        if (!photos.HasError)
                            icon.Source = new BitmapImage(new Uri(photos.Result.First().SmallUrl));
                    });

                var description = new TextBlock {
                    Margin = new Thickness(10, 17, 0, 10),
                    Text = toy.Name,
                    FontSize = 18
                };

                var stp = new StackPanel {
                    Orientation = Orientation.Horizontal,
                    Children = { icon, description },
                    Height = 50
                };

                panelItems.Add(stp);
            }

            products.ItemsSource = panelItems;
        }
    }
}
