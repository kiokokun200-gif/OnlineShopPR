using OnlineShop.Core.DTOs.DB;
using OnlineShopPR.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OnlineShopPR.Pages
{
    /// <summary>
    /// Логика взаимодействия для AuthPage.xaml
    /// </summary>
    public partial class AuthPage : Page
    {
        AuthService authService = new AuthService();
        public AuthPage()
        {
            InitializeComponent();
        }

        private async void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            string login = TxtBoxLogLogin.Text;
            string password = TxtBoxLogPassword.Password;
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Заполните все поля!");
                return;
            }
            var res = await authService.LoginAsync(login, password);
            if(res.Item1)
            {
                MessageBox.Show(res.Item2);
                NavigationService.Navigate(new MainPage());

            }
            else
            {
                MessageBox.Show(res.Item2);
            }
            






        }

        private async void BtnRegistr_Click(object sender, RoutedEventArgs e)
        {
            string login = TxtBoxRegLogin.Text;
            string password = TxtBoxRegPassword.Password;
            string confirmPassword = TxtBoxRegConfirmPassword.Password;
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword))
            {
                MessageBox.Show("Заполните все поля!");
                return;
            }

            var res = await authService.RegistrAsync(login, password, confirmPassword);

            if (res.Item1)
            {
                MessageBox.Show(res.Item2);
                TabAuth.SelectedIndex = 0;
                TxtBoxLogLogin.Text = login;
                TxtBoxLogPassword.Password = password;
            }
            else
            {
                MessageBox.Show(res.Item2);
            }
            
            
        }

    }
}
