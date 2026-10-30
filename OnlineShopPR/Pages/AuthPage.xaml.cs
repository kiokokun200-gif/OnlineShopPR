using OnlineShop.Core.DTOs.DB;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public AuthPage()
        {
            InitializeComponent();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(TxtBoxLogLogin.Text) || string.IsNullOrEmpty(TxtBoxLogPassword.Password))
            {
                MessageBox.Show("Заполните все поля!");
                return;
            }
            
            
                //if (Login(TxtBoxLogLogin.Text, TxtBoxLogPassword.Password))
                //{
                //    MessageBox.Show($"Вход успешный!");
                    

                //}
                //else
                //{
                //    MessageBox.Show("Пользователь с такими данными не найден");
                //}

            
        }

        private void BtnRegistr_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(TxtBoxRegLogin.Text) || string.IsNullOrEmpty(TxtBoxRegPassword.Password) || string.IsNullOrEmpty(TxtBoxRegConfirmPassword.Password))
            {
                MessageBox.Show("Заполните все поля!");
                return;
            }
            
            
            //if (Registration(TxtBoxRegLogin.Text, TxtBoxRegPassword.Password))
            //{
            //    MessageBox.Show("Успешный вход!");

            //}
            //else MessageBox.Show("Ошибка регистрации");

            
        }

        //private bool Registration(string login, string password, out string message)
        //{
        //    //try
        //    //{
        //    //    var reguser = new UserDTO()
        //    //    {
                    

        //    //    };
        //    //    Core.Context.User.Add(reguser);
        //    //    Core.Context.SaveChanges();
        //    //    Login(number, password);
        //    //    //UserData.CurrentUser = ;
        //    //    message = string.Empty;
        //    //    return true;
        //    //}

        //    //catch(Exception ex) {
        //    //    message = ex.Message;
        //    //    return false;
        //    //}
        //}
    }
}
