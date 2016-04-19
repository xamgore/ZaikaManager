using System.Windows;
using System.Windows.Input;

namespace Zaika {
    public partial class LoginWindow {
        public LoginWindow() {
            InitializeComponent();
            Login.Focus();
        }

        private void TryToSignIn(object sender, RoutedEventArgs e) {
            if (Login.Text.Trim().Length == 0)
                Login.Focus();
            else if (Password.Password.Length == 0)
                Password.Focus();
            else {
                // todo: подключение к базе данных
                new MainWindow(Login.Text, Password.Password).Show();
                Close();
            }
        }

        private void LoginKeyDown(object sender, KeyEventArgs e) {
            if (e.Key == Key.Enter && Login.Text.Trim().Length != 0)
                Password.Focus();
        }

        private void PasswordKeyDown(object sender, KeyEventArgs e) {
            if (e.Key != Key.Enter) return;

            if (Login.Text.Trim().Length == 0)
                Login.Focus();
            else TryToSignIn(sender, e);
        }
    }
}
