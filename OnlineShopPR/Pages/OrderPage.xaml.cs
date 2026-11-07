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
    /// Логика взаимодействия для OrderPage.xaml
    /// </summary>
    public partial class OrderPage : Page
    {
        private OrderDTO _order;
        //получить пвз

        public OrderPage(OrderDTO order)
        {
            InitializeComponent();
            _order = order;
            DataContext = _order;
            LoadDate();
        }

        private void LoadDate()
        {
            if(_order.TotalPrice < 600)
            {
                TxtWarning.Visibility = Visibility.Visible;
                RadioDelivery.IsEnabled = false;

            }
            else
            {
                TxtWarning.Visibility = Visibility.Collapsed;
                RadioDelivery.IsEnabled = true;

            }
            RefreshDeliveryPanels();
        }

        private void BtnConfirm_Click(object sender, RoutedEventArgs e)
        {
            string addressDelivery = TxtBoxDeliveryAddress.Text;
            if (RadioDelivery.IsChecked == true && string.IsNullOrWhiteSpace(addressDelivery))
            {
                MessageBox.Show("Введите адрес доставки");
                return;
            }
            //отправить заказ
            MessageBox.Show("Заказ успешно оформлен!");
        }
        private void RefreshDeliveryPanels()
        {
            if (RadioDelivery.IsChecked == true)
            {
                StackDelivery.Visibility = Visibility.Visible;
                StackPVZ.Visibility = Visibility.Collapsed;
            }
            else
            {
                StackDelivery.Visibility = Visibility.Collapsed;
                StackPVZ.Visibility = Visibility.Visible;
            }
        }
        private void RadioDelivery_Checked(object sender, RoutedEventArgs e)
        {
            if (!IsLoaded) return;
            RefreshDeliveryPanels();
        }
    }
}
