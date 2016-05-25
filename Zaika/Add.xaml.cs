using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using System.Windows.Input;
using Zaika.Core;
using MicroLite;
using MicroLite.Builder;
using MicroLite.Configuration;
using System.Text.RegularExpressions;

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


        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e) {
            Regex isNumber = new Regex("-?[0-9]*\\.?[0-9]*");
            e.Handled = !isNumber.IsMatch(e.Text);
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

            var op = new Operation {
                Augment = int.Parse(Augment.Text),
                Price = int.Parse("0" + Price.Text),
                Date = DateTime.Now,
                WarehouseId = DB.Warehouses[Warehouses.SelectedItem as string].Id,
                ProducerId = (Producers.SelectedItem as ProducerInfo).Producer.Id,
                ProductId = (Products.SelectedItem as ProductInfo).Product.Id,
            };
            DB.Zaika.InsertAsync(op).ContinueWith(_ => DB.InsertOperation(op));
            Close();
        }

        void checkBalance() {
            //int count = 0;
            try {
                //DB.Zaika.FetchAsync<Stuff>(
                //SqlBuilder.Select("*").From(typeof(Stuff)).ToSqlQuery())
                //.ContinueWith(task => count = task.Result.Count);
                //balance.Content = "Available: " + count;
                var product = Products.SelectedItem as ProductInfo;
                var warehouse = DB.Warehouses[Warehouses.SelectedItem as string].Id;
                DB.Zaika.SingleAsync<Stuff>(SqlBuilder.Select("*").From("Stuffs").Where("ProductId")
                    .IsEqualTo(product).AndWhere("WarehouseId").IsEqualTo(warehouse).ToSqlQuery())
                    .ContinueWith(task => Dispatcher.Invoke(() => balance.Content = task.Result.Amount));
            } catch { }
        }

        private void Products_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e) {
            checkBalance();
        }

        private void Warehouses_SelectionChanged_1(object sender, System.Windows.Controls.SelectionChangedEventArgs e) {
           checkBalance();
        }
    }
}
