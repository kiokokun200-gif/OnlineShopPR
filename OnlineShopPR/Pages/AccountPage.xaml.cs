using OnlineShop.Core.DTOs.DB;
using OnlineShopPR.Services;
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
    /// Логика взаимодействия для AccountPage.xaml
    /// </summary>
    public partial class AccountPage : Page
    {
        //private List<OrderDTO> _currentOrders;
        //private List<OrderDTO> _historyOrders;
        private List<string> historySort;
        ProductService productService = new ProductService();
        KorzinaService korzinaService = new KorzinaService();
        public AccountPage()
        {
            InitializeComponent();
            historySort =
            [
                "Сначала новые",
                "Сначала старые"
            ];

            ComboBoxSortHistory.ItemsSource = historySort;
            ComboBoxSortHistory.SelectedIndex = 0;
        }

        private void BtnShowOrder_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            var order = (OrderDTO)btn.DataContext;

            var wind = new OrderWindow(order);
            wind.Show();
        }

        private void BtnShowOrderCurrent_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            var order = (OrderDTO)btn.DataContext;

            var wind = new OrderWindow(order);
            wind.Show();
        }

        private async void BtnFavorite_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            ProductDTO product = (ProductDTO)btn.DataContext;
            bool res;
            if (product != null)

            {
                if (!product.IsFavorite)
                {
                    res = await productService.AddFavorite(product.ProductID);

                }
                else
                {
                    MessageBoxResult result = MessageBox.Show("Удалить из избранного?", "Подтверждение", MessageBoxButton.YesNo);
                    if (result == MessageBoxResult.Yes)
                    {
                        res = await productService.RemoveFavorite(product.ProductID);
                    }
                    else return;
                }

                if (res)
                {
                    product.IsFavorite = !product.IsFavorite;
                    ListBoxGoods.Items.Refresh();
                }
                else
                {
                    MessageBox.Show("Ошибка");
                }
            }
        }

        private async void BtnAddInCart_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            ProductDTO product = button.DataContext as ProductDTO;

            bool result = await korzinaService.AddProduct(product.ProductID);

            if (result)
            {
                //product.isInCart = true;
                //product.quantityInCart = 1;

                ListBoxGoods.Items.Refresh();
            }
        }
        private async void Minus_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            ProductDTO product = button.DataContext as ProductDTO;

            bool result = await korzinaService.Remove(product.ProductID);

            if (result)
            {
                //product.quantityInCart--;

                //if (product.quantityInCart <= 0)
                //{
                //    product.quantityInCart = 0;
                //    product.isInCart = false;
                //}

                ListBoxGoods.Items.Refresh();
            }
        }

        private async void Plus_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            ProductDTO product = button.DataContext as ProductDTO;

            bool result = await korzinaService.AddProduct(product.ProductID);

            if (result)
            {
                //product.quantityInCart++;
                ListBoxGoods.Items.Refresh();
            }

        }

        private void ComboBoxSortHistory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboBoxSortHistory.SelectedItem != null)
            {
                if(ComboBoxSortHistory.SelectedItem.ToString() == "Сначала новые")
                {
                    //сорт
                }
                else
                {
                    //
                }
            }
        }

        private void AddToCart_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
