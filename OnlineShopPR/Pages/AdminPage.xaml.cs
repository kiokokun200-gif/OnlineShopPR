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
    /// Логика взаимодействия для AdminPage.xaml
    /// </summary>
    public partial class AdminPage : Page
    {
        ProductService productService = new ProductService();
        AdminService adminService = new AdminService();
        ReviewService reviewService = new ReviewService();

        public AdminPage()
        {
            InitializeComponent();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (UserData.CurrentUser == null || (UserData.CurrentUser.RoleName != "Админ"))
            {
                MessageBox.Show("Доступ запрещен");
                NavigationService.Navigate(new MainPage());
                return;
            }
            await LoadAll();
        }

        private async Task LoadAll()
        {
            var products = await productService.GetAllProducts();
            DataGridProducts.ItemsSource = products;

            var categories = await adminService.GetCategories();
            ListProductsCategory.ItemsSource = categories;

            var pvz = await adminService.GetPickupPoints();
            ListPVZ.ItemsSource = pvz;

            var orders = await adminService.GetOrders();
            DataGridOrders.ItemsSource = orders;

            var reviews = await reviewService.GetAllReviews();
            DataGridReviews.ItemsSource = reviews;
        }

        private async void BtnAddroduct_Click(object sender, RoutedEventArgs e)
        {
            var wind = new AddProductWindow();
            wind.ShowDialog();
            await LoadAll();
        }

        private async void BtnEditProduct_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            ProductDTO product = (ProductDTO)button.DataContext;
            if (product == null) return;

            var wind = new EditProductWindow(product);
            wind.ShowDialog();
            await LoadAll();
        }

        private async void BtnAddroductCategory_Click(object sender, RoutedEventArgs e)
        {
            var wind = new AddProductCategoryWindow();
            wind.ShowDialog();
            await LoadAll();
        }

        private async void BtnEditCategory_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            var cat = button.DataContext as CategoryDTO;
            if (cat == null) return;

            var wind = new EditProductCategoryWindow(cat);
            wind.ShowDialog();
            await LoadAll();
        }

        private async void BtnAddPVZ_Click(object sender, RoutedEventArgs e)
        {
            var wind = new AddPvzWindow();
            wind.ShowDialog();
            await LoadAll();
        }

        private async void BtnEditPVZ_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            var pvz = button.DataContext as PickupPointDTO;
            if (pvz == null) return;

            var wind = new EditPvzWindow(pvz);
            wind.ShowDialog();
            await LoadAll();
        }

        private async void BtnChangeOrderStatus_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            OrderDTO order = (OrderDTO)button.DataContext;
            if (order == null) return;

            var wind = new EditOrderStatusWindow(order);
            wind.ShowDialog();
            await LoadAll();
        }

        private async void BtnDeleteReview_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            ReviewDTO review = (ReviewDTO)button.DataContext;
            if (review == null) return;

            MessageBoxResult result = MessageBox.Show("Хотите удалить отзыв?", "Предупреждение", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                var res = await reviewService.DeleteReview(review.Id);
                if (res.Item1)
                    await LoadAll();
                else
                    MessageBox.Show(string.IsNullOrEmpty(res.Item2) ? "Ошибка" : res.Item2);
            }
        }
    }
}
