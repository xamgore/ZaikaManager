using MicroLite.Builder;
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

            // Register event listeners
            MouseMove += ChangeFloatingButtonOpacity;
            DB.OperationsLoaded += Ui(DisplayOperations);
            DB.WarehousesLoaded += Ui(() => Floating.Visibility = Visibility.Visible);

            // Run async data loads from database
            DB.ProductsLoaded += (o, e) => DB.LoadProducers();
            DB.ProducersLoaded += (o, e) => DB.LoadWarehouses();
            DB.WarehousesLoaded += (o, e) => DB.LoadOperations();
            DB.LoadProducts();
            
            // Update operations data every 5 seconds
            var timer = new DispatcherTimer { Interval = new TimeSpan(0, 0, 5) };
            timer.Tick += (o, e) => DB.LoadOperations();
            timer.Start();

            Closed += (o, e) => DB.RefreshLastOperations();
        }

        private EventHandler Ui(Action action) =>
            (o, e) => Dispatcher.Invoke(action);

        public void DisplayOperations() {
            var index = Operations.Items.Count - Operations.SelectedIndex;
            Operations.ItemsSource = DB.Operations.Values.Select(
                operation => new OperationInfo(operation)).ToList();
            Operations.SelectedIndex = Operations.Items.Count - index;
        }

        private void ChangeFloatingButtonOpacity(object o, MouseEventArgs e) {
            var p = e.GetPosition(Floating);
            p.Offset(-Floating.Width / 2, -Floating.Height / 2);

            var dist = 200;
            Floating.Opacity = 1 - Math.Min(0.8, Math.Min(Math.Sqrt(p.X * p.X + p.Y * p.Y), dist) / dist);
        }

        private void Floating_Click(object sender, RoutedEventArgs e) {
            new AddWindow().ShowDialog();
        }
    }
}
