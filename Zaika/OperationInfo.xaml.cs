using Zaika.Core;

namespace Zaika {
    public partial class OperationInfo {
        public Operation Operation;

        public string Description =>
            $"{DB.Products[Operation.ProductId].Name} ({Operation.Augment}) " +
            $"by {DB.Producers[Operation.ProducerId].Name} " +
            $"at {Operation.Date.ToShortDateString()}";

        public OperationInfo(Operation operation) {
            Operation = operation;
            InitializeComponent();
            DataContext = this;

            FontSize = 22;
        }
    }
}
