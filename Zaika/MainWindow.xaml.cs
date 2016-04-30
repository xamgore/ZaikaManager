using System;
using System.Linq;
using Zaika.Core;

namespace Zaika {
    public partial class MainWindow {
        public MainWindow() {
            InitializeComponent();

            //DB.ProductsLoaded += Ui(DisplayProducts);
            //DB.ProducersLoaded += Ui(DisplayProducers);
            DB.OperationsLoaded += Ui(DisplayOperations);

            DB.ProductsLoaded += (o, e) => DB.LoadProducers();
            DB.ProducersLoaded += (o, e) => DB.LoadOperations();

            DB.LoadProducts();
        }

        private EventHandler Ui(Action action) =>
            (o, e) => Dispatcher.Invoke(action);

        public void DisplayOperations() =>
            Operations.ItemsSource = DB.Operations.Values.Select(
                operation => new OperationInfo(operation)).ToList();

        //public void DisplayProducers() =>
        //    Producers.ItemsSource = DB.Producers.Values.Select(
        //        producer => new ProducerInfo(producer)).ToList();

        //public void DisplayProducts() =>
        //    Products.ItemsSource = DB.Products.Values.Select(
        //        toy => new ProductInfo(toy.Name)).ToList();
    }
}
