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
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        ProductService productService = new ProductService();
        KorzinaService korzinaService = new KorzinaService();
        FavoriteService favoriteService = new FavoriteService();

        private List<ProductDTO> _products = new List<ProductDTO>();
        private List<string> goodCategories = new List<string>();

        public MainPage()
        {
            InitializeComponent();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (UserData.CurrentUser != null)
            {
                if (UserData.CurrentUser.RoleName == "Админ")
                    NavigationService.Navigate(new AdminPage());
                else
                    NavigationService.Navigate(new AccountPage());
            }
            else NavigationService.Navigate(new AuthPage());
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (UserData.CurrentUser != null)
            {
                if (UserData.CurrentUser.RoleName == "Админ")
                    BtnLogin.Content = "Админ";
                else
                    BtnLogin.Content = "Аккаунт";
            }
            else
            {
                BtnLogin.Content = "Войти";
            }

            await LoadProducts();
        }

        private async Task LoadProducts()
        {
            _products = await productService.GetAllProducts();
            if (_products == null) _products = new List<ProductDTO>();

            goodCategories = _products.Select(p => p.CategoriName).Where(x => !string.IsNullOrEmpty(x)).Distinct().ToList();
            goodCategories.Insert(0, "Все");
            ComboBoxFiltr.ItemsSource = goodCategories;
            ComboBoxFiltr.SelectedIndex = 0;

            var viewProducts = _products.Where(p => p.isActive && p.QuantityInStock > 0).ToList();
            ListBoxGoods.ItemsSource = viewProducts;

            if (viewProducts.Count > 0)
            {
                TxtBoxPriceOt.Text = ((int)Math.Floor(viewProducts.Min(p => p.Price))).ToString();
                TxtBoxPriceDo.Text = ((int)Math.Ceiling(viewProducts.Max(p => p.Price))).ToString();
            }
            else
            {
                TxtBoxPriceOt.Text = "0";
                TxtBoxPriceDo.Text = "0";
            }
        }

        private void BtnFind_Click(object sender, RoutedEventArgs e)
        {
            string search = TxtBoxSearch.Text ?? "";
            string categ = ComboBoxFiltr.SelectedItem?.ToString() ?? "Все";
            int.TryParse(TxtBoxPriceOt.Text, out int priceOt);
            int.TryParse(TxtBoxPriceDo.Text, out int priceDo);
            if (priceDo == 0 && priceOt == 0 && _products.Count > 0) priceDo = (int)Math.Ceiling(_products.Max(p => p.Price));

            Find(search, categ, priceOt, priceDo);
        }

        private void Find(string search, string categ, int priceOt, int priceDo)
        {
            var prod = _products.Where(p => p.isActive && p.QuantityInStock > 0).ToList();
            if (!string.IsNullOrEmpty(search))
            {
                prod = prod.Where(p => p.Name != null && p.Name.ToLower().Contains(search.ToLower())).ToList();
            }
            if (categ != "Все" && !string.IsNullOrEmpty(categ))
            {
                prod = prod.Where(p => p.CategoriName == categ).ToList();
            }
            prod = prod.Where(p => p.Price >= priceOt && p.Price <= priceDo).ToList();
            ListBoxGoods.ItemsSource = prod;
        }

        private void TxtBoxPriceOt_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !char.IsDigit(e.Text[0]);
        }

        private async void BtnAddInCart_Click(object sender, RoutedEventArgs e)
        {
            if (UserData.CurrentUser == null)
            {
                MessageBox.Show("Для добавления в корзину необходимо авторизоваться");
                return;
            }
            Button btn = sender as Button;
            ProductDTO product = (ProductDTO)btn.DataContext;
            if (product == null) return;

            var result = await korzinaService.AddProduct(product.ProductID);
            if (result.Item1)
            {
                await LoadProducts();
                BtnFind_Click(null, null);
            }
            else
            {
                MessageBox.Show(string.IsNullOrEmpty(result.Item2) ? "Не удалось добавить товар в корзину" : result.Item2);
            }
        }

        private async void BtnFavorite_Click(object sender, RoutedEventArgs e)
        {
            if (UserData.CurrentUser == null)
            {
                MessageBox.Show("Для работы с избранным необходимо авторизоваться");
                return;
            }
            Button btn = sender as Button;
            ProductDTO product = (ProductDTO)btn.DataContext;
            if (product == null) return;

            if (product.IsFavorite)
            {
                var res = await favoriteService.RemoveFavorite(product.ProductID);
                if (res.Item1)
                {
                    await LoadProducts();
                    BtnFind_Click(null, null);
                }
                else MessageBox.Show(res.Item2);
            }
            else
            {
                var res = await favoriteService.AddFavorite(product.ProductID);
                if (res.Item1)
                {
                    await LoadProducts();
                    BtnFind_Click(null, null);
                }
                else MessageBox.Show(res.Item2);
            }
        }

        private void ListBoxGoods_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var prod = ListBoxGoods.SelectedItem as ProductDTO;
            if (prod != null)
            {
                NavigationService.Navigate(new ProductPage(prod));
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (UserData.CurrentUser == null)
            {
                MessageBox.Show("Для просмотра корзины необходимо авторизоваться");
                return;
            }
            NavigationService.Navigate(new CartPage());
        }
    }
}
