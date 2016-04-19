using System;
using System.Windows;
using System.Windows.Input;

namespace Zaika {
    public partial class LoginWindow {
        public LoginWindow() {
            InitializeComponent();

            KeyDown += (o, e) => {
                if (e.Key == Key.Escape)
                    Close();
                else if (e.Key == Key.Enter)
                    TryToSignIn(o, e);
            };
        }

        private void TryToSignIn(object sender, RoutedEventArgs e) {
            if (Login.Text.Trim().Length == 0)
                Login.Focus();
            else if (Password.Password.Length == 0)
                Password.Focus();
            else {
                // todo: connect to database
                new MainWindow(Login.Text, Password.Password).Show();
                Close();
            }
        }

    }
}
