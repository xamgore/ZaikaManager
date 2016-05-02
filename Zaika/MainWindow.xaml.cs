using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Zaika.Core;

namespace Zaika {
    public partial class MainWindow {
        public MainWindow() {
            InitializeComponent();

            MouseMove += ChangeFloatingButtonOpacity;

            //DB.ProductsLoaded += Ui(DisplayProducts);
            //DB.ProducersLoaded += Ui(DisplayProducers);
            DB.OperationsLoaded += Ui(DisplayOperations);

            DB.ProductsLoaded += (o, e) => DB.LoadProducers();
            DB.ProducersLoaded += (o, e) => DB.LoadOperations();

            var timer = new DispatcherTimer {Interval = new TimeSpan(0, 0, 5)};
            timer.Tick += (o, e) => DB.LoadOperations();
            timer.Start();

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

        private void ChangeFloatingButtonOpacity(object o, MouseEventArgs e) {
            var p = e.GetPosition(Floating);
            p.Offset(-Floating.Width / 2, -Floating.Height / 2);

            var dist = 200;
            Floating.Opacity = 1 - Math.Min(0.8, Math.Min(Math.Sqrt(p.X * p.X + p.Y * p.Y), dist) / dist);
        }
    }
}
