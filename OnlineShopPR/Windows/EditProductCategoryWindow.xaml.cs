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
        //как то получить datacontext
        public EditProductCategoryWindow()
        {
            InitializeComponent();
        }

        
        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            string name = TxtBoxName.Text;
            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Введите название");
                return;
            }
            //изменить
        }
    }
}
