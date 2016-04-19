using System;
using System.Windows;
using System.Windows.Input;
using Zaika.Core;

namespace Zaika {
    public partial class LoginWindow {
        public LoginWindow() {
            InitializeComponent();
            Login.Text = "postgres";
            Password.Password = ".";

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
            else if (DB.Connect(Login.Text, Password.Password)) {
                new MainWindow().Show();
                Close();
            }
        }
    }
}
