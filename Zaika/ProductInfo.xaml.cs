using System;
using System.Linq;
using System.Windows.Media.Imaging;
using FlickrNet;

namespace Zaika {
    public partial class ProductInfo {
        private static readonly Flickr Flickr = new Flickr("ce28f896e78baffae502ff23e1df8645", "e4bc6d42f6c0b074");

        public ProductInfo(string name) {
            InitializeComponent();

            Title.Text = name;
            Flickr.PhotosSearchAsync(
                    new PhotoSearchOptions { Tags = name + " toy", PerPage = 1, Page = 1 },
                    photos => {
                        if (!photos.HasError && photos.Result.Count > 0)
                            Icon.Source = new BitmapImage(new Uri(photos.Result.First().SmallUrl));
                    });

        }
    }
}
