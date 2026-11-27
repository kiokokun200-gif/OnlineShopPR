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
    /// Логика взаимодействия для ProductPage.xaml
    /// </summary>
    public partial class ProductPage : Page
    {
        private ProductDTO _product;
        ReviewService reviewService = new ReviewService();
        ProductService productService = new ProductService();

        public ProductPage(ProductDTO product)
        {
            InitializeComponent();
            _product = product;
            DataContext = _product;
            Loaded += ProductPage_Loaded;
        }

        private async void ProductPage_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadReviews();
            var fresh = await productService.GetProduct(_product.ProductID);
            if (fresh != null)
            {
                _product = fresh;
                DataContext = _product;
            }
        }

        private async Task LoadReviews()
        {
            var reviews = await reviewService.GetReviews(_product.ProductID);
            ListReviews.ItemsSource = reviews;
        }

        private async void BtnAddReview_Click(object sender, RoutedEventArgs e)
        {
            if (UserData.CurrentUser == null)
            {
                MessageBoxResult res = MessageBox.Show("Требуется авторизация. Перейти на страницу авторизации?", "Требование", MessageBoxButton.YesNo);
                if (res == MessageBoxResult.Yes)
                {
                    NavigationService.Navigate(new AuthPage());
                }
                return;
            }

            var wind = new AddReviewWindow(_product.ProductID);
            wind.ShowDialog();
            await LoadReviews();
            var fresh = await productService.GetProduct(_product.ProductID);
            if (fresh != null)
            {
                _product = fresh;
                DataContext = _product;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
