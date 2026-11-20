using OnlineShop.Core.DTOs.DB;
using OnlineShopPR.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
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
using System.Windows.Shapes;

namespace OnlineShopPR.Windows
{
    /// <summary>
    /// Логика взаимодействия для AddProductWindow.xaml
    /// </summary>
    public partial class AddProductWindow : Window
    {
        ProductService productService = new ProductService();
        List<CategoryDTO> _categories = new List<CategoryDTO>();

        public AddProductWindow()
        {
            InitializeComponent();
            Loaded += AddProductWindow_Loaded;
        }

        private async void AddProductWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _categories = await productService.GetCategories();
            ComboCategory.ItemsSource = _categories;
            ComboCategory.DisplayMemberPath = "Name";
            if (_categories.Count > 0) ComboCategory.SelectedIndex = 0;
        }

        private async void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            string name = TxtBoxName.Text;
            string description = TxtBoxDescription.Text;
            string inputPrice = TxtBoxPrice.Text;
            var cat = ComboCategory.SelectedItem as CategoryDTO;
            string imagePath = TxtBoxPathImg.Text;
            string quantityInStockstring = TxtBoxQuantityInStock.Text;
            int.TryParse(TxtBoxQuantityInPack.Text, out int quantityInPack);
            bool isActive = CheckIsActive.IsChecked == true;

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(description) || string.IsNullOrEmpty(inputPrice) || string.IsNullOrEmpty(quantityInStockstring) || cat == null)
            {
                MessageBox.Show("Введите все поля");
                return;
            }
            if (!int.TryParse(quantityInStockstring, out int quantityInStock))
            {
                MessageBox.Show("Введите корректное количество на складе");
                return;
            }

            string normalizedInput = inputPrice.Replace(',', '.');

            if (!(decimal.TryParse(normalizedInput, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal price) && price > 0))
            {
                MessageBox.Show("Введите корректную цену больше нуля.");
                return;
            }

            var product = new ProductDTO
            {
                Name = name,
                Description = description,
                Price = price,
                CategoriName = cat.Name,
                CategoriId = cat.CategoryID,
                PathImg = imagePath,
                QuantityInStock = quantityInStock,
                QuantityInPack = quantityInPack,
                isActive = isActive
            };

            var res = await productService.AddProduct(product);
            if (res.Item1)
            {
                MessageBox.Show(string.IsNullOrEmpty(res.Item2) ? "Товар добавлен" : res.Item2);
                DialogResult = true;
                Close();
            }
            else
            {
                MessageBox.Show(string.IsNullOrEmpty(res.Item2) ? "Ошибка" : res.Item2);
            }
        }

        private void TxtBoxQuantityInStock_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !char.IsDigit(e.Text[0]);
        }
    }
}
