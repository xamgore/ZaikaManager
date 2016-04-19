using System;

namespace Zaika.Core {
    public class Operation {
        public int Id { get; set; }

        public int ProductId { get; set; }
        public int ProducerId { get; set; }
        public int WarehouseId { get; set; }

        public decimal Price { get; set; }
        public int Amount { get; set; }
        public OpType Type { get; set; }
        public DateTime Date { get; set; }
    }

    public enum OpType {
        Income = 0, Outcome = 1
    }
}