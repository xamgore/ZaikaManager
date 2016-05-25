using System.Windows;
using Zaika.Core;
using static System.Windows.Visibility;

namespace Zaika {
    public partial class ProducerInfo {
        public ProducerInfo(Producer producer) {
            Producer = producer;
            Last = producer.Last;
            InitializeComponent();
            DataContext = this;
        }

        public Producer Producer { get; set; }
        public LastOperation Last { get; set; }

        public Visibility ShowIncome => string.IsNullOrEmpty(Last.IncomeProduct) ? Hidden : Visible;
        public Visibility ShowOutcome => string.IsNullOrEmpty(Last.OutcomeProduct) ? Hidden : Visible;
        public Visibility ShowInfo => ShowIncome == Hidden && ShowOutcome == Hidden ? Hidden : Visible;
        public double InfoHeight => ShowInfo == Hidden ? 5 : 30;
    }
}
