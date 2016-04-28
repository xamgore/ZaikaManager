using System;
using System.Linq;
using Zaika.Core;

namespace Zaika {
    public partial class MainWindow {
        public MainWindow() {
            InitializeComponent();

            //DB.ProductsLoaded += Ui(DisplayProducts);
            //DB.LoadProducts();
            DB.ProducersLoaded += Ui(DisplayProducers);
            DB.LoadProducers();
        }

        private EventHandler Ui(Action action) =>
            (o, e) => Dispatcher.Invoke(action);

        public void DisplayProducers() =>
            Producers.ItemsSource = DB.Producers.Values.Select(
                producer => new ProducerInfo {
                    Title = { Text = producer.Name },
                    Address = { Text = producer.City }
                }).ToList();

        //public void DisplayProducts() =>
        //    Products.ItemsSource = DB.Products.Values.Select(
        //        toy => new ProductInfo(toy.Name)).ToList();
    }
}
