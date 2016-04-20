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


            DB.Zaika.FetchAsync<Product>(
                SqlBuilder.Select("*").From(typeof(Product)).ToSqlQuery())
                .ContinueWith(task => Dispatcher.Invoke(() => DisplayProducts(task.Result))
            );

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
                        if (!photos.HasError && photos.Result.Count > 0)
                            icon.Source = new BitmapImage(new Uri(photos.Result.First().SmallUrl));
                    });

                var description = new TextBlock {
                    Margin = new Thickness(10, 0, 0, 0),
                    VerticalAlignment = VerticalAlignment.Center,
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

            Products.ItemsSource = panelItems;
        }
    }
}
