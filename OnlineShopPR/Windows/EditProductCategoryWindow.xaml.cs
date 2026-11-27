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
using System.Windows.Shapes;

namespace OnlineShopPR.Windows
{
    /// <summary>
    /// Логика взаимодействия для EditProductCategoryWindow.xaml
    /// </summary>
    public partial class EditProductCategoryWindow : Window
    {
        AdminService adminService = new AdminService();
        private CategoryDTO _cat;

        public EditProductCategoryWindow(CategoryDTO cat)
        {
            InitializeComponent();
            _cat = cat;
            DataContext = _cat;
        }

        private async void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            string name = TxtBoxName.Text;
            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Введите название");
                return;
            }
            var res = await adminService.SaveCategory(new CategorySaveRequest { CategoryID = _cat.CategoryID, Name = name });
            if (res.Item1)
            {
                MessageBox.Show(string.IsNullOrEmpty(res.Item2) ? "Категория изменена" : res.Item2);
                DialogResult = true;
                Close();
            }
            else
            {
                MessageBox.Show(string.IsNullOrEmpty(res.Item2) ? "Ошибка" : res.Item2);
            }
        }
    }
}
