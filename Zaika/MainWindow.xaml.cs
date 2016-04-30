using System;
using System.Linq;
using Zaika.Core;

namespace Zaika {
    public partial class MainWindow {
        public MainWindow() {
            InitializeComponent();

            //DB.ProductsLoaded += Ui(DisplayProducts);
            //DB.ProducersLoaded += Ui(DisplayProducers);

            DB.ProductsLoaded += (o, e) => DB.LoadProducers();
            DB.ProducersLoaded += (o, e) => DB.LoadOperations();
            DB.OperationsLoaded += Ui(DisplayOperations);

            DB.LoadProducts();
        }

        private EventHandler Ui(Action action) =>
            (o, e) => Dispatcher.Invoke(action);

        public void DisplayOperations() =>
            Operations.ItemsSource = DB.Operations.Values.Select(
                op => new OperationInfo {
                    Title = { Text = $"{DB.Products[op.ProductId].Name} ({op.Augment}) by {DB.Producers[op.ProducerId].Name} at {op.Date.ToShortDateString()}" },
                    FontSize = 22
                }).ToList();

        //public void DisplayProducers() =>
        //    Producers.ItemsSource = DB.Producers.Values.Select(
        //        producer => new ProducerInfo {
        //            Title = { Text = producer.Name },
        //            Address = { Text = producer.City },
        //            IName = { Text = producer.Last.IncomeProduct },
        //            ICost = { Text = producer.Last.IncomePrice.ToString() },
        //            IDate = { Text = producer.Last.IncomeDate?.ToShortDateString() },
        //            OName = { Text = producer.Last.OutcomeProduct },
        //            OCost = { Text = producer.Last.OutcomePrice.ToString() },
        //            ODate = { Text = producer.Last.OutcomeDate?.ToShortDateString() },
        //        }.Optimize()).ToList();

        //public void DisplayProducts() =>
        //    Products.ItemsSource = DB.Products.Values.Select(
        //        toy => new ProductInfo(toy.Name)).ToList();
    }
}
