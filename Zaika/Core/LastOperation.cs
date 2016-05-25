using System;

namespace Zaika.Core {
    public class LastOperation {
        public int Id { get; set; }

        public string IncomeProduct { get; set; }
        public decimal? IncomePrice { get; set; }
        public DateTime? IncomeDate { get; set; }

        public string OutcomeProduct { get; set; }
        public decimal? OutcomePrice { get; set; }
        public DateTime? OutcomeDate { get; set; }
    }
}