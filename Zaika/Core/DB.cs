using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using MicroLite;
using MicroLite.Builder;
using MicroLite.Configuration;
using static System.Threading.Tasks.TaskContinuationOptions;

namespace Zaika.Core {
    public static class DB {
        public static IAsyncSession Zaika;

        public static IDictionary<int, Product> Products = new Dictionary<int, Product>();
        public static IDictionary<int, Producer> Producers = new Dictionary<int, Producer>();
        public static IDictionary<int, Operation> Operations = new Dictionary<int, Operation>();
        public static IDictionary<string, Warehouse> Warehouses = new Dictionary<string, Warehouse>();

        public static event EventHandler ProductsLoaded;
        public static event EventHandler ProducersLoaded;
        public static event EventHandler WarehousesLoaded;
        public static event EventHandler OperationsLoaded;


        public static bool Connect(string user, string pass) {
            var connectionString = $"Server=127.0.0.1;Database=zaika;User Id={user};Password={pass}";

            try {
                Zaika = Configure.Fluently()
                    .ForPostgreSqlConnection("zaika", connectionString, "Npgsql")
                    .CreateSessionFactory().OpenAsyncSession();
            } catch (Exception e) {
                MessageBox.Show(e.Message);
                return false;
            }

            return true;
        }

        public static void InsertOperation(Operation op) {
            Operations[op.Id] = op;
            OperationsLoaded?.Invoke(null, EventArgs.Empty);
            LoadProducers();
        }

        public static Task LoadProducts() {
            return Zaika.FetchAsync<Product>(
                SqlBuilder.Select("*").From(typeof(Product)).ToSqlQuery())
                .ContinueWith(task => Products = task.Result.ToDictionary(product => product.Id), OnlyOnRanToCompletion)
                .ContinueWith(task => ProductsLoaded?.Invoke(null, EventArgs.Empty), OnlyOnRanToCompletion);
        }

        public static Task LoadProducers() {
            return Zaika.FetchAsync<Producer>(
               SqlBuilder.Select("*").From(typeof(Producer)).ToSqlQuery())
               .ContinueWith(task => {
                   Producers = task.Result.ToDictionary(producer => producer.Id);
                   LoadMoreInfoAboutProducers();
               }, OnlyOnRanToCompletion);
        }

        public static Task LoadMoreInfoAboutProducers() {
            return Zaika.FetchAsync<LastOperation>(
                SqlBuilder.Select("*").From(typeof(LastOperation)).ToSqlQuery())
                .ContinueWith(task => {
                    task.Result.ForEach(op => Producers[op.Id].Last = op);
                    ProducersLoaded?.Invoke(null, EventArgs.Empty);
                }, OnlyOnRanToCompletion);
        }

        public static Task LoadWarehouses() {
            return Zaika.FetchAsync<Warehouse>(
                SqlBuilder.Select("*").From(typeof(Warehouse)).ToSqlQuery())
                .ContinueWith(task => Warehouses = task.Result.ToDictionary(warehouse => warehouse.Name), OnlyOnRanToCompletion)
                .ContinueWith(task => WarehousesLoaded?.Invoke(null, EventArgs.Empty), OnlyOnRanToCompletion);
        }

        public static Task LoadOperations() {
            return Zaika.FetchAsync<Operation>(
                SqlBuilder.Select("*").From(typeof(Operation)).OrderByDescending("Id").ToSqlQuery())
                .ContinueWith(task => Operations = task.Result.ToDictionary(operation => operation.Id), OnlyOnRanToCompletion)
                .ContinueWith(task => OperationsLoaded?.Invoke(null, EventArgs.Empty), OnlyOnRanToCompletion);
        }

        public static void ForEach<T>(this IEnumerable<T> e, Action<T> action) {
            foreach (var i in e) action(i);
        }
    }
}
