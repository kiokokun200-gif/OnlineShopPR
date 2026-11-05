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
        private void LoadData()
        {

            sort.Add("Сначала лучшие");
            sort.Add("Сначала дешевле");
            sort.Add("Сначала дороже");
            ComboBoxSort.ItemsSource = sort;
            //goodCategories = Core.context.GoodCategory.Select(u => u.Name).ToList();
            goodCategories = [];
            goodCategories.Insert(0, "Все");
            ComboBoxFiltr.ItemsSource = goodCategories;


        }

        private void TxtBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void ComboBoxSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ComboBoxFiltr_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            if(UserData.CurrentUser != null)
            {
                //
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
            goodCategories = _products.Select(p => p.CategoriName).Distinct().ToList();
            ComboBoxFiltr.ItemsSource = goodCategories;
            ListBoxGoods.ItemsSource = _products;

        }
    }
}
