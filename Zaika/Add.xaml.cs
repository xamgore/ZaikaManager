using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using System.Windows.Input;
using Zaika.Core;

namespace Zaika {
    public partial class AddWindow {
        private static int Amount = 0;

        public AddWindow() {
            InitializeComponent();
            KeyDown += (o, e) => { if (e.Key == Key.Enter) button_Click(o, null); };

            DisplayProducts();
            DisplayProducers();
            DisplayWarehouses();

            Products.SelectionChanged += LoadAvailableProductAmount;
            Warehouses.SelectionChanged += LoadAvailableProductAmount;

            //DB.WarehousesLoaded += Ui(DisplayWarehouses);
            //DB.ProducersLoaded += Ui(DisplayProducers);
            //DB.ProductsLoaded += Ui(DisplayProducts);
        }

        private async void LoadAvailableProductAmount(object sender, SelectionChangedEventArgs e) {
            if (Products.SelectedIndex == -1 || Warehouses.SelectedIndex == -1)
                return;

            var product = (Products.SelectedItem as ProductInfo).Product.Id;
            var warehouse = DB.Warehouses[Warehouses.SelectedItem as string].Id;
            button.IsEnabled = false;

            await DB.Zaika.SingleAsync<Stuff>(MicroLite.Builder.SqlBuilder.Select("Amount").From("Stuffs")
                  .Where("ProductId").IsEqualTo(product).AndWhere("WarehouseId").IsEqualTo(warehouse).ToSqlQuery())
                .ContinueWith(task => Amount = task.Result?.Amount ?? 0);

            maxAmount.Text = $"Доступно {Amount}";
            button.IsEnabled = true;
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

        private async void button_Click(object sender, RoutedEventArgs e) {
            int augment;
            decimal price;

            if (!int.TryParse(Augment.Text, out augment) ||
                !decimal.TryParse(Price.Text, out price))
                return;

            var product = (Products.SelectedItem as ProductInfo).Product.Id;
            var warehouse = DB.Warehouses[Warehouses.SelectedItem as string].Id;

            var op = new Operation {
                Augment = augment,
                Price = price,
                Date = DateTime.Now,
                WarehouseId = DB.Warehouses[Warehouses.SelectedItem as string].Id,
                ProducerId = (Producers.SelectedItem as ProducerInfo).Producer.Id,
                ProductId = (Products.SelectedItem as ProductInfo).Product.Id,
            };

            await DB.Zaika.SingleAsync<Stuff>(MicroLite.Builder.SqlBuilder.Select("Amount").From("Stuffs")
                  .Where("ProductId").IsEqualTo(product).AndWhere("WarehouseId").IsEqualTo(warehouse).ToSqlQuery())
                .ContinueWith(task => Amount = task.Result.Amount);

            if (Amount + augment < 0)
                return;

            await DB.Zaika.InsertAsync(op).ContinueWith(_ => DB.InsertOperation(op));
            Close();
        }
    }
}
