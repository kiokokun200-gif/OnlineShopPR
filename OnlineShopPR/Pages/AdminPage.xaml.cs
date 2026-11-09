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
    /// Логика взаимодействия для AdminPage.xaml
    /// </summary>
    public partial class AdminPage : Page
    {
        public AdminPage()
        {
            InitializeComponent();
        }

        private void BtnAddroduct_Click(object sender, RoutedEventArgs e)
        {
            var wind = new AddProductWindow();
            wind.ShowDialog();
        }

        private void BtnEditProduct_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            ProductDTO product = (ProductDTO)button.DataContext;

            var wind = new EditProductWindow(product);
            wind.ShowDialog();
        }

        private void BtnAddroductCategory_Click(object sender, RoutedEventArgs e)
        {
            var wind = new AddProductCategoryWindow();
            wind.ShowDialog();
        }

        private void BtnEditCategory_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            //ProductDTO product = (ProductDTO)button.DataContext;
            //хз что передавать string или что


            //var wind = new EditProductWindow(product);
            //wind.ShowDialog();
        }

        private void BtnChangeOrderStatus_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            OrderDTO order = (OrderDTO)button.DataContext;

            var wind = new EditOrderStatusWindow(order);
            wind.ShowDialog();
        }

        private void BtnDeleteReview_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            ReviewDTO order = (ReviewDTO)button.DataContext;

            MessageBoxResult result = MessageBox.Show("Хотите удалить отзыв?", "Предупреждение", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                //удалить
            }
        }
    }
}
