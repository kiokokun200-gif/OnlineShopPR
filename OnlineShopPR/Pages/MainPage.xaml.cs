using OnlineShop.Core.DTOs.DB;
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
            goodCategories = Core.context.GoodCategory.Select(u => u.Name).ToList();
            goodCategories.Insert(0, "Все");
            ComboBoxFiltr.ItemsSource = goodCategories;


        }
    }
}
