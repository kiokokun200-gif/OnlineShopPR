using OnlineShop.Core.DTOs.DB;
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
        public EditProductWindow(ProductDTO product)
        {
            InitializeComponent();
            _product = product;
            DataContext = _product;
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            string name = TxtBoxName.Text;
            string description = TxtBoxDescription.Text;
            string inputPrice = TxtBoxPrice.Text;
            string category = ComboCategory.SelectedItem.ToString();
            string imagePath = TxtBoxPathImg.Text;
            string quantityInStockstring = TxtBoxQuantityInStock.Text;

            int quantityInPack = int.Parse(TxtBoxQuantityInPack.Text);
            bool isActive = CheckIsActive.IsChecked == true;

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(description) || string.IsNullOrEmpty(inputPrice) || string.IsNullOrEmpty(quantityInStockstring))
            {
                MessageBox.Show("Введите все поля");
                return;
            }
            int quantityInStock = int.Parse(quantityInStockstring);

            string normalizedInput = inputPrice.Replace(',', '.');

            if (!(decimal.TryParse(normalizedInput, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal price) && price > 0))
            {
                MessageBox.Show("Введите корректную цену больше нуля.");
            }
            //изменить продукт
        }

        private void TxtBoxQuantityInStock_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !char.IsDigit(e.Text[0]);
        }
    }
}
