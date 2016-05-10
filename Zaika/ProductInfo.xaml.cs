using System;
using System.Linq;
using System.Windows.Media.Imaging;
using FlickrNet;
using Zaika.Core;

namespace Zaika {
    public partial class ProductInfo {
        private static readonly Flickr Flickr = new Flickr("ce28f896e78baffae502ff23e1df8645", "e4bc6d42f6c0b074");
        public Product Product { get; set; }

        public ProductInfo(Product prod) {
            InitializeComponent();

            Product = prod;
            Title.Text = prod.Name;
            Flickr.PhotosSearchAsync(
                    new PhotoSearchOptions { Tags = prod.Name + " toy", PerPage = 1, Page = 1 },
                    photos => {
                        if (!photos.HasError && photos.Result.Count > 0)
                            Icon.Source = new BitmapImage(new Uri(photos.Result.First().SmallUrl));
                    });

        }
        
    }
}
