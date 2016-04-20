using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using MicroLite;
using MicroLite.Builder;
using MicroLite.Configuration;

namespace Zaika.Core {
    public class DB {
        public static IAsyncSession Zaika;
        public static IDictionary<int, Product> Products;

        public static event EventHandler ProductsLoaded;

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

        public static void LoadProducts() {
            Zaika.FetchAsync<Product>(
                SqlBuilder.Select("*").From(typeof(Product)).ToSqlQuery())
                .ContinueWith(task => Products = task.Result.ToDictionary(product => product.Id))
                .ContinueWith(task => ProductsLoaded?.Invoke(null, EventArgs.Empty));
        }
    }
}
