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
        private List<OrderDTO> _allOrders = new List<OrderDTO>();
        private List<string> historySort;
        OrderService orderService = new OrderService();
        FavoriteService favoriteService = new FavoriteService();

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

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (UserData.CurrentUser == null)
            {
                MessageBox.Show("Необходимо авторизоваться");
                NavigationService.Navigate(new AuthPage());
                return;
            }

            TxtBlockLogin.Text = UserData.CurrentUser.Login;
            await LoadOrders();
            await LoadFavorites();
        }

        private async Task LoadOrders()
        {
            string sort = "new";
            if (ComboBoxSortHistory.SelectedItem != null && ComboBoxSortHistory.SelectedItem.ToString() == "Сначала старые")
                sort = "old";

            _allOrders = await orderService.GetMyOrders(sort);
            var history = _allOrders.Where(o => o.StatusName == "Доставлен" || o.StatusName == "Отменён").ToList();
            var current = _allOrders.Where(o => !(o.StatusName == "Доставлен" || o.StatusName == "Отменён")).ToList();

            ListOrdersHistory.ItemsSource = history;
            ListCurrentOrders.ItemsSource = current;
        }

        private async Task LoadFavorites()
        {
            var favs = await favoriteService.GetFavorites();
            ListBoxGoods.ItemsSource = favs;
        }

        private void BtnShowOrder_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            var order = (OrderDTO)btn.DataContext;
            if (order == null) return;

            var wind = new OrderWindow(order);
            wind.ShowDialog();
        }

        private void BtnShowOrderCurrent_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            var order = (OrderDTO)btn.DataContext;
            if (order == null) return;

            var wind = new OrderWindow(order);
            wind.ShowDialog();
        }

        private async void BtnFavorite_Click(object sender, RoutedEventArgs e)
        {
            if (UserData.CurrentUser == null) return;
            Button btn = sender as Button;
            ProductDTO product = (ProductDTO)btn.DataContext;
            if (product == null) return;

            if (product.IsFavorite)
            {
                MessageBoxResult result = MessageBox.Show("Удалить из избранного?", "Подтверждение", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    var res = await favoriteService.RemoveFavorite(product.ProductID);
                    if (res.Item1)
                        await LoadFavorites();
                    else
                        MessageBox.Show(string.IsNullOrEmpty(res.Item2) ? "Ошибка" : res.Item2);
                }
            }
            else
            {
                var res = await favoriteService.AddFavorite(product.ProductID);
                if (res.Item1)
                    await LoadFavorites();
                else
                    MessageBox.Show(string.IsNullOrEmpty(res.Item2) ? "Ошибка" : res.Item2);
            }
        }

        private async void AddToCart_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            ProductDTO product = button.DataContext as ProductDTO;
            if (product == null) return;

            var res = await favoriteService.MoveToCart(product.ProductID);
            if (res.Item1)
            {
                await LoadFavorites();
            }
            else
            {
                MessageBox.Show(string.IsNullOrEmpty(res.Item2) ? "Не удалось перенести товар в корзину" : res.Item2);
            }
        }

        private async void ComboBoxSortHistory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!IsLoaded) return;
            if (UserData.CurrentUser == null) return;
            await LoadOrders();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
