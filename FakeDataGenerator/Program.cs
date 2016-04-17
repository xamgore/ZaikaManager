using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using FakeO;
using ZaikaManager.DB;
using Address = Faker.Address;
using Company = Faker.Company;
using Phone = Faker.Phone;

namespace FakeDataGenerator {
    internal static class Program {

        public static void Main(string[] args) {
            var producers = Create.New<Producer>(10,
                p => p.Name = Company.Name(),
                p => p.City = Address.City(),
                p => p.Phone = Phone.Number()).ToArray();

            var warehouses = Create.New<Warehouse>(3,
                w => w.Name = Address.StreetName(),
                w => w.City = Address.City()).ToArray();

            var toys = new[] { "bunny", "puppy", "kitten", "octopus", "piggy", "fox" };
            var products = toys.Select(name => new Product { Name = name }).ToArray();

            DateTime from = new DateTime(2016, 1, 1), to = DateTime.Today;
            var rnd = new Random(Environment.TickCount);

            var operations = Create.New<Operation>(80,
                o => o.ProductId = rnd.Next(0, products.Length) + 1,
                o => o.ProducerId = rnd.Next(0, producers.Length) + 1,
                o => o.WarehouseId = rnd.Next(0, warehouses.Length) + 1,
                o => o.Amount = rnd.Next(0, 5000) + 1,
                o => o.Price = rnd.Next(0, 1000) + 1,
                o => o.Type = (OpType) rnd.Next(2),
                o => o.Date = from + new TimeSpan((long) (rnd.NextDouble() * (to - from).Ticks))).ToArray();


            var stream = new StreamWriter(args.Length > 0 ? args[0] : "data.sql");

            producers.WriteQueries(stream);
            products.WriteQueries(stream);
            warehouses.WriteQueries(stream);
            operations.WriteQueries(stream);

            stream.Close();
        }

        private static void WriteQueries<T>(this IEnumerable<T> list, StreamWriter stream) {
            foreach (var e in list) stream.WriteLine(QueryOf(e));
            stream.WriteLine("\n");
        }

        private static string QueryOf<T>(T instance) {
            var fields = typeof(T).GetProperties().Where(f => f.Name != "Id").ToArray();
            var columns = fields.Select(f => $"\"{f.Name}\"").JoinToString(",");
            var values = fields.Select(f => f.GetValue(instance).ToLiteral()).JoinToString(",");

            return $"INSERT INTO \"{typeof(T).Name}s\" ({columns}) VALUES ({values});";
        }

        private static string JoinToString<T>(this IEnumerable<T> list, string separator)
            => string.Join(separator, list);

        private static string ToLiteral(this object obj)
            => obj is int | obj is decimal ? obj.ToString() : $"\'{ToLiteral(obj.ToString())}\'";

        private static string ToLiteral(string input) {
            var literal = new StringBuilder(input.Length + 2);

            foreach (var c in input) {
                switch (c) {
                    case '\'':
                        literal.Append(@"\'");
                        break;
                    case '\"':
                        literal.Append("\\\"");
                        break;
                    case '\\':
                        literal.Append(@"\\");
                        break;
                    case '\0':
                        literal.Append(@"\0");
                        break;
                    case '\a':
                        literal.Append(@"\a");
                        break;
                    case '\b':
                        literal.Append(@"\b");
                        break;
                    case '\f':
                        literal.Append(@"\f");
                        break;
                    case '\n':
                        literal.Append(@"\n");
                        break;
                    case '\r':
                        literal.Append(@"\r");
                        break;
                    case '\t':
                        literal.Append(@"\t");
                        break;
                    case '\v':
                        literal.Append(@"\v");
                        break;
                    default:
                        // ASCII printable character
                        if (c >= 0x20 && c <= 0x7e) {
                            literal.Append(c);
                            // As UTF16 escaped character
                        } else {
                            literal.Append(@"\u");
                            literal.Append(((int) c).ToString("x4"));
                        }
                        break;
                }
            }

            return literal.ToString();
        }

    }
}