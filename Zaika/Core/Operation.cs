using System;

namespace Zaika.Core {
    public class Operation {
        public int Id { get; set; }

        public int ProductId { get; set; }
        public int ProducerId { get; set; }
        public int WarehouseId { get; set; }

        public decimal Price { get; set; }
        public int Amount { get; set; }
        public DateTime Date { get; set; }
    }
}