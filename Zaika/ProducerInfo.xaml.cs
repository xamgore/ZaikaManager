using System.Windows;
using System.Windows.Controls;

namespace Zaika {
    public partial class ProducerInfo {
        public ProducerInfo() {
            InitializeComponent();
        }

        private bool Empty(TextBlock block) =>
            string.IsNullOrEmpty(block.Text);

        private void Hide(TextBlock element) =>
            ((UIElement) element.Parent).Visibility = Visibility.Hidden;

        public ProducerInfo Optimize() {
            if (Empty(IName)) Hide(IName);
            if (Empty(OName)) Hide(OName);

            if (Empty(IName) && Empty(OName)) {
                Info.Height = 0;
                Info.Visibility = Visibility.Hidden;
                Padding = new Thickness(0, 0, 0, 5);
            }

            return this;
        }
    }
}
