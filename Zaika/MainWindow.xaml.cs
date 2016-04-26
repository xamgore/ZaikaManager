using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using FlickrNet;
using Zaika.Core;
using Color = System.Windows.Media.Color;
using Image = System.Windows.Controls.Image;

namespace Zaika {
    public partial class MainWindow {
        private static readonly Flickr Flickr = new Flickr("ce28f896e78baffae502ff23e1df8645", "e4bc6d42f6c0b074");

        public MainWindow() {
            InitializeComponent();

            //DB.ProductsLoaded += Ui(DisplayProducts);
            //DB.LoadProducts();
            DB.ProductsLoaded += Ui(DisplayProducers);
            DB.LoadProducers();
        }

        private EventHandler Ui(Action action) =>
            (o, e) => Dispatcher.Invoke(action);

        public void DisplayProducers() =>
            Producers.ItemsSource = DB.Producers.Values.Select(
                producer => new ProducerInfo {
                    Title = { Text = producer.Name },
                    ProducerPhone = { Text = producer.Phone },
                    Address = { Text = producer.City }
                }).ToList();

        public void DisplayProducts() {
            var panelItems = new List<StackPanel>();

            foreach (var toy in DB.Products.Values) {
                var icon = new Image {
                    Stretch = Stretch.UniformToFill,
                    Width = 50,
                    Height = 50,
                };

                Flickr.PhotosSearchAsync(
                    new PhotoSearchOptions { Tags = toy.Name + " toy", PerPage = 1, Page = 1 },
                    photos => {
                        if (!photos.HasError && photos.Result.Count > 0)
                            icon.Source = new BitmapImage(new Uri(photos.Result.First().SmallUrl));
                    });

                var placeholder = new Border {
                    Background = new SolidColorBrush(Color.FromArgb(190, 0x21, 0x96, 0xf3)),
                    Height = 50,
                    Width = 50,
                    Child = icon
                };

                var description = new TextBlock {
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(10, 0, 0, 0),
                    Text = toy.Name,
                    FontSize = 18
                };

                var stp = new StackPanel {
                    Orientation = Orientation.Horizontal,
                    Children = { placeholder, description },
                    Height = 50
                };

                panelItems.Add(stp);
            }

            //Products.ItemsSource = panelItems;
        }
    }
}
