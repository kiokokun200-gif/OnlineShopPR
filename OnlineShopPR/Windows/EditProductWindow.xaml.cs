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
    /// Логика взаимодействия для EditProductWindow.xaml
    /// </summary>
    public partial class EditProductWindow : Window
    {
        private ProductDTO _product;
        ProductService productService = new ProductService();
        List<CategoryDTO> _categories = new List<CategoryDTO>();

        public EditProductWindow(ProductDTO product)
        {
            InitializeComponent();
            _product = product;
            DataContext = _product;
            Loaded += EditProductWindow_Loaded;
        }

        private async void EditProductWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _categories = await productService.GetCategories();
            ComboCategory.ItemsSource = _categories;
            ComboCategory.DisplayMemberPath = "Name";
            var sel = _categories.FirstOrDefault(c => c.Name == _product.CategoriName || c.CategoryID == _product.CategoriId);
            if (sel != null) ComboCategory.SelectedItem = sel;
            else if (_categories.Count > 0) ComboCategory.SelectedIndex = 0;
        }

        private async void BtnEdit_Click(object sender, RoutedEventArgs e)
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

            _product.Name = name;
            _product.Description = description;
            _product.Price = price;
            _product.CategoriName = cat.Name;
            _product.CategoriId = cat.CategoryID;
            _product.PathImg = imagePath;
            _product.QuantityInStock = quantityInStock;
            _product.QuantityInPack = quantityInPack;
            _product.isActive = isActive;

            var res = await productService.ChangeProduct(_product);
            if (res.Item1)
            {
                MessageBox.Show(string.IsNullOrEmpty(res.Item2) ? "Товар изменен" : res.Item2);
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
