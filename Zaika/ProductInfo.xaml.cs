using System;
using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using FlickrNet;
using Zaika.Core;
using System.Collections.Generic;

namespace Zaika {
    public partial class ProductInfo {
        private static readonly Flickr Flickr = new Flickr("ce28f896e78baffae502ff23e1df8645", "e4bc6d42f6c0b074");
        private static IDictionary<int, ImageSource> icons = new Dictionary<int, ImageSource>();

        public Product Product { get; set; }

        public ProductInfo(Product prod) {
            InitializeComponent();

            Product = prod;
            Title.Text = prod.Name;

            if (icons.ContainsKey(prod.Id))
                Icon.Source = icons[prod.Id];
            else Flickr.PhotosSearchAsync(
                new PhotoSearchOptions { Tags = prod.Name + " toy", PerPage = 1, Page = 1 },
                photos => {
                    if (!photos.HasError && photos.Result.Count > 0)
                        icons[prod.Id] = Icon.Source = new BitmapImage(new Uri(photos.Result.First().SmallUrl));
            });

        }
        
    }
}
