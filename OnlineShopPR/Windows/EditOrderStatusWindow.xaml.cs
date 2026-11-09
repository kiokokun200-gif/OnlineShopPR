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
    /// Логика взаимодействия для EditOrderStatusWindow.xaml
    /// </summary>
    public partial class EditOrderStatusWindow : Window
    {
        //получить статусы заказа
        private OrderDTO _order;
        public EditOrderStatusWindow(OrderDTO order)
        {
            InitializeComponent();
            _order = order;
            DataContext = order;
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            //изменить
        }
    }
}
