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
        private List<ProductDTO> _products;
        private List<string> sort = new List<string>();
        private List<string> goodCategories;
        private string search = "";
        private string selectedSort;
        private string selectefFiltr;
        public MainPage()
        {
            InitializeComponent();
        }
        private void LoadGoods()
        {
            //goods = Core.context.Good.ToList();
            //ListBoxGoods.ItemsSource = goods;
        }
        

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            if(UserData.CurrentUser != null)
            {
                NavigationService.Navigate(new AccountPage());
            }
           else  NavigationService.Navigate(new AuthPage());
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (UserData.CurrentUser != null)
            {
                BtnLogin.Content = "Аккаунт";
            }

            _products = await productService.GetAllProducts();
            if (_products != null)
            {
                goodCategories = _products.Select(p => p.CategoriName).Distinct().ToList();
                goodCategories.Insert(0, "Все");
                ComboBoxFiltr.ItemsSource = goodCategories;
                ListBoxGoods.ItemsSource = _products.Where(p=> p.isActive && p.QuantityInStock > 0);
                TxtBoxPriceOt.Text = _products.OrderBy(p => p.Price).First().Price.ToString();
                TxtBoxPriceDo.Text = _products.OrderByDescending(p => p.Price).First().Price.ToString();
            }

        }

        private void BtnFind_Click(object sender, RoutedEventArgs e)
        {
            string search = TxtBoxSearch.Text;
            string categ = ComboBoxFiltr.SelectedItem.ToString();
            int priceDo = int.Parse(TxtBoxPriceDo.Text);
            int priceOt = int.Parse(TxtBoxPriceOt.Text);

            Find(search, categ, priceOt, priceDo);
        }

        private void Find(string search, string categ, int priceOt, int priceDo)
        {
            var prod = _products;
            if(!string.IsNullOrEmpty(search))
            {
                prod = prod.Where(p => p.Name.ToLower().Contains(search.ToLower())).ToList();
            }
            else if(categ != "Все")
            {
                prod = prod.Where(p => p.CategoriName ==  categ).ToList();
            }
            prod = prod.Where(p => p.Price > priceOt && p.Price < priceDo).ToList();
            ListBoxGoods.ItemsSource = prod;

        }

        private void TxtBoxPriceOt_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !char.IsDigit(e.Text[0]);
        }

        private void BtnAddInCart_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            ProductDTO product = (ProductDTO)btn.DataContext;

        }
    }
}
