using System;
using MicroLite.Builder;
using MicroLite.Configuration;
using ZaikaManager.DB;

namespace ZaikaManager {
    class Program {
        static void Main(string[] args) {
            string[] connection = { "Server = 127.0.0.1", "Database = zaika", "User Id = postgres" };

            var db = Configure.Fluently()
                .ForPostgreSqlConnection("zaika", string.Join(";", connection), "Npgsql")
                .CreateSessionFactory().OpenSession();

            var transaction = db.BeginTransaction();

            var a = new Product { Name = "kek" };
            db.Insert(a);
            db.Delete(a);

            var products = db.Fetch<Product>(
                SqlBuilder.Select("*").From(typeof(Product)).ToSqlQuery());
            transaction.Commit();

            foreach (var p in products)
                Console.WriteLine(p.Name);
        }
    }
}
