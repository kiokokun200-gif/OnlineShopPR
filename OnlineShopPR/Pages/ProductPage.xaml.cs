using OnlineShop.Core.DTOs.DB;
using OnlineShopPR.Windows;
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
    /// Логика взаимодействия для ProductPage.xaml
    /// </summary>
    public partial class ProductPage : Page
    {
        private ProductDTO _product;
        public ProductPage(ProductDTO product)
        {
            InitializeComponent();
            _product = product;
            DataContext = product;
        }

        private void BtnAddReview_Click(object sender, RoutedEventArgs e)
        {
            if (UserData.CurrentUser == null)
            {
                MessageBoxResult res = MessageBox.Show("Требуется авторизация. Перейти на страницу авторизации?", "Требование", MessageBoxButton.YesNo);
                if (res == MessageBoxResult.Yes)
                {
                    NavigationService.Navigate(new AuthPage());
                }
                else return;
            }

            else
            {
                var wind = new AddReviewWindow();
                wind.ShowDialog();
            }
        }
    }
}
