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
    /// Логика взаимодействия для CartPage.xaml
    /// </summary>
    public partial class CartPage : Page
    {
        public CartPage()
        {
            InitializeComponent();
        }

        private void BtnBuyProduct_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            ProductDTO productDTO = (ProductDTO)button.DataContext;

        }

        private void BtnMinusProd_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
        }

        private void BtnPlusProd_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnDeleteProd_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnOrder_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
