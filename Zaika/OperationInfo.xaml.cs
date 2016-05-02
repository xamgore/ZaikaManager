using System;
using System.Windows.Media;
using Zaika.Core;

namespace Zaika {
    public partial class OperationInfo {
        public Operation Operation { get; set; }

        public string ProductName => Capitalize(DB.Products[Operation.ProductId].Name);

        public string Augment => (Operation.Augment > 0 ? "+" : "–") + Math.Abs(Operation.Augment);
        public Brush AugColor => new SolidColorBrush(Operation.Augment > 0 ? Colors.LimeGreen : Colors.OrangeRed);


        public string Description =>
            $"by {DB.Producers[Operation.ProducerId].Name}";


        public OperationInfo(Operation operation) {
            Operation = operation;
            InitializeComponent();
            DataContext = this;
        }


        private static string Capitalize(string str) {
            if (str == null)
                return null;
            if (str.Length > 1)
                return char.ToUpper(str[0]) + str.Substring(1);
            return str.ToUpper();
        }
    }
}
