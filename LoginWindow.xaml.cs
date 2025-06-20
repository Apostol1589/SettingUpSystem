
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
namespace SettingUpSystem
{


    public partial class LoginWindow : Window
    {
        public bool IsAuthenticated { get; private set; } = false;
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            string password = pbPassword.Password;
            if (password == "Bigshop1478963")
            {
                IsAuthenticated = true;
                this.DialogResult = true;
            }
            else
            {
                tbEnterPassword.Text = "Неправильно введений пароль. Спробуйте ще раз:";
                tbEnterPassword.Foreground = new SolidColorBrush(Colors.Red);
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;

        }
    }
}
