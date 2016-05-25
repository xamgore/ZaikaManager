using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using System.Windows.Input;
using Zaika.Core;

namespace Zaika {
    public partial class AddWindow {
        public AddWindow() {
            InitializeComponent();
            KeyDown += (o, e) => { if (e.Key == Key.Enter) button_Click(o, null); };

            DisplayProducts();
            DisplayProducers();
            DisplayWarehouses();

            //DB.WarehousesLoaded += Ui(DisplayWarehouses);
            //DB.ProducersLoaded += Ui(DisplayProducers);
            //DB.ProductsLoaded += Ui(DisplayProducts);
        }

        private EventHandler Ui(Action action) =>
            (o, e) => Dispatcher.Invoke(action);

        public void DisplayProducers() =>
            Producers.ItemsSource = DB.Producers.Values.Select(
                producer => new ProducerInfo(producer)).ToList();

        public void DisplayProducts() =>
            Products.ItemsSource = DB.Products.Values.Select(
                toy => new ProductInfo(toy)).ToList();

        public void DisplayWarehouses() =>
           Warehouses.ItemsSource = DB.Warehouses.Keys.ToList();

        private void button_Click(object sender, RoutedEventArgs e) {
            int augment;
            decimal price;

            if (!int.TryParse(Augment.Text, out augment) ||
                !decimal.TryParse(Price.Text, out price))
                return;

            var op = new Operation {
                Augment = augment,
                Price = price,
                Date = DateTime.Now,
                WarehouseId = DB.Warehouses[Warehouses.SelectedItem as string].Id,
                ProducerId = (Producers.SelectedItem as ProducerInfo).Producer.Id,
                ProductId = (Products.SelectedItem as ProductInfo).Product.Id,
            };

            DB.Zaika.InsertAsync(op).ContinueWith(_ => DB.InsertOperation(op));
            Close();
        }
    }
}
