using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using Zaika.Core;

namespace Zaika {
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class AddWindow {
        public AddWindow() {
            InitializeComponent();

            DB.ProductsLoaded += Ui(DisplayProducts);
            DB.ProducersLoaded += Ui(DisplayProducers);
            DB.WarehousesLoaded += Ui(DisplayWarehouses);

            DB.ProductsLoaded += (o, e) => DB.LoadProducers();
            DB.ProducersLoaded += (o, e) => DB.LoadOperations();
            DB.OperationsLoaded += (o, e) => DB.LoadWarehouses();

            //var timer = new DispatcherTimer { Interval = new TimeSpan(0, 0, 5) };
            //timer.Tick += (o, e) => DB.LoadOperations();
            //timer.Start();

            DB.LoadProducts();
            //DB.LoadProducers();
            //DB.LoadWarehouses();
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
           Warehouses.ItemsSource = DB.Warehouses.Values.Select(
               warehouse => warehouse.Name);

        private void button_Click(object sender, RoutedEventArgs e) {
            //var producer = ((ProducerInfo)Producers.SelectedItem).Producer.Id;
            //var product = (ProducerInfo)Products.SelectedItem;
            var warehouse = Warehouses.SelectedItem;
            
            int aug = 0;
            int pr = 0;
            Int32.TryParse(Price.Text, out pr);
            Int32.TryParse(Augment.Text, out aug);
            var op = new Operation() {
                Augment = aug,
                Date = new DateTime().Date,
                Price = pr,
                ProducerId = ((ProducerInfo)Producers.SelectedItem).Producer.Id,
                ProductId = ((ProductInfo)Products.SelectedItem).Product.Id,
                WarehouseId = 1
            };
            DB.Zaika.InsertAsync(op);
            Close();
        }
    }
}
