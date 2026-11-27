using OnlineShop.Core.DTOs.DB;
using OnlineShopPR.Services;
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
        KorzinaService korzinaService = new KorzinaService();
        List<KorzinaProductDTO> _items = new List<KorzinaProductDTO>();

        public CartPage()
        {
            InitializeComponent();
        }

        private async Task LoadCart()
        {
            _items = await korzinaService.allProductInCart();
            ListBoxProductsInCart.ItemsSource = _items;
            TxtBlockCartQuantity.Text = _items.Sum(k => k.Quantity).ToString();
            decimal total = _items.Sum(k => k.Quantity * k.ProductDTO.Price);
            TxtBlcTotalPrice.Text = $"{total} ₽";

            if (_items.Count == 0)
            {
                TxtBlockEmptyCart.Visibility = Visibility.Visible;
                StackTotalPrice.Visibility = Visibility.Collapsed;
                BtnOrder.Visibility = Visibility.Collapsed;
            }
            else
            {
                TxtBlockEmptyCart.Visibility = Visibility.Collapsed;
                StackTotalPrice.Visibility = Visibility.Visible;
                BtnOrder.Visibility = Visibility.Visible;
            }
        }

        private void BtnBuyProduct_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            var item = (KorzinaProductDTO)button.DataContext;
            if (item == null) return;

            var order = new OrderDTO
            {
                TotalPrice = item.Quantity * item.ProductDTO.Price,
                Items = new List<OrderProductDTO>
                {
                    new OrderProductDTO
                    {
                        ProductId = item.ProductDTO.ProductID,
                        Name = item.ProductDTO.Name,
                        Quantity = item.Quantity,
                        PurchasePrice = item.ProductDTO.Price
                    }
                }
            };
            NavigationService.Navigate(new OrderPage(order, item.ProductDTO.ProductID));
        }

        private async void BtnMinusProd_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            var item = (KorzinaProductDTO)button.DataContext;
            if (item == null) return;

            var res = await korzinaService.Remove(item.ProductDTO.ProductID);
            if (!res.Item1 && !string.IsNullOrEmpty(res.Item2)) MessageBox.Show(res.Item2);
            await LoadCart();
        }

        private async void BtnPlusProd_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            var item = (KorzinaProductDTO)button.DataContext;
            if (item == null) return;

            var res = await korzinaService.AddProduct(item.ProductDTO.ProductID);
            if (!res.Item1 && !string.IsNullOrEmpty(res.Item2)) MessageBox.Show(res.Item2);
            await LoadCart();
        }

        private async void BtnDeleteProd_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            var item = (KorzinaProductDTO)button.DataContext;
            if (item == null) return;

            var res = await korzinaService.RemoveProduct(item.ProductDTO.ProductID);
            if (!res.Item1 && !string.IsNullOrEmpty(res.Item2)) MessageBox.Show(res.Item2);
            await LoadCart();
        }

        private void BtnOrder_Click(object sender, RoutedEventArgs e)
        {
            if (_items == null || _items.Count == 0)
            {
                MessageBox.Show("Корзина пуста");
                return;
            }

            var order = new OrderDTO
            {
                TotalPrice = _items.Sum(i => i.Quantity * i.ProductDTO.Price),
                Items = _items.Select(i => new OrderProductDTO
                {
                    ProductId = i.ProductDTO.ProductID,
                    Name = i.ProductDTO.Name,
                    Quantity = i.Quantity,
                    PurchasePrice = i.ProductDTO.Price
                }).ToList()
            };
            NavigationService.Navigate(new OrderPage(order, null));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (UserData.CurrentUser == null)
            {
                MessageBox.Show("Необходимо авторизоваться");
                NavigationService.Navigate(new AuthPage());
                return;
            }
            await LoadCart();
        }
    }
}
