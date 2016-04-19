using System;
using System.Windows;
using MicroLite;
using MicroLite.Configuration;

namespace Zaika.Core {
    public class DB {
        public static ISession Zaika;

        public static bool Connect(string user, string pass) {
            var connectionString = $"Server=127.0.0.1;Database=zaika;User Id={user};Password={pass}";

            try {
                Zaika = Configure.Fluently()
                    .ForPostgreSqlConnection("zaika", connectionString, "Npgsql")
                    .CreateSessionFactory().OpenSession();
            } catch (Exception e) {
                MessageBox.Show(e.Message);
                return false;
            }

            return true;
        }
    }
}
