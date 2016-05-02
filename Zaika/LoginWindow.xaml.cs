using System.Threading;
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
            else {
                var login = (string) Login.Text.Clone();
                var pass = (string) Password.Password.Clone();

                new Thread(() => {
                    if (DB.Connect(login, pass)) {
                        Thread.Sleep(500);
                        Dispatcher.Invoke(() => {
                            new MainWindow().Show();
                            Close();
                        });
                    }
                }).Start();
            }
        }
    }
}
