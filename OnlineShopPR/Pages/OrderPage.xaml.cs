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
        private int? _singleProductId;
        OrderService orderService = new OrderService();

        public OrderPage(OrderDTO order, int? singleProductId)
        {
            InitializeComponent();
            _order = order;
            _singleProductId = singleProductId;
            DataContext = _order;
            LoadDate();
        }

        private async void LoadDate()
        {
            var pvz = await orderService.GetPickupPoints();
            CombpPVZ.ItemsSource = pvz;
            CombpPVZ.DisplayMemberPath = "AddressName";
            if (pvz.Count > 0) CombpPVZ.SelectedIndex = 0;

            var grid = this.Content as Grid;
            if (grid != null)
            {
                foreach (var c in grid.Children)
                {
                    if (c is ItemsControl ic && Grid.GetRow(ic) == 1)
                    {
                        ic.ItemsSource = _order.Items;
                    }
                }
            }

            if (_order.TotalPrice > 600)
            {
                TxtWarning.Visibility = Visibility.Collapsed;
                RadioDelivery.IsEnabled = true;
            }
            else
            {
                TxtWarning.Visibility = Visibility.Visible;
                RadioDelivery.IsEnabled = false;
                RadioDelivery.IsChecked = false;
            }
            RefreshDeliveryPanels();
        }

        private async void BtnConfirm_Click(object sender, RoutedEventArgs e)
        {
            bool isDelivery = RadioDelivery.IsChecked == true;
            string addressDelivery = TxtBoxDeliveryAddress.Text;

            if (isDelivery && _order.TotalPrice <= 600)
            {
                MessageBox.Show("Доставка доступна только при сумме заказа больше 600 ₽");
                return;
            }

            if (isDelivery && string.IsNullOrWhiteSpace(addressDelivery))
            {
                MessageBox.Show("Введите адрес доставки");
                return;
            }

            int? pickupPointId = null;
            if (!isDelivery)
            {
                var sel = CombpPVZ.SelectedItem as PickupPointDTO;
                if (sel == null)
                {
                    MessageBox.Show("Выберите ПВЗ");
                    return;
                }
                pickupPointId = sel.Id;
            }

            var req = new CreateOrderRequest
            {
                ProductId = _singleProductId,
                IsDelivery = isDelivery,
                PickupPointId = pickupPointId,
                AddressDelivery = isDelivery ? addressDelivery : null
            };

            var res = await orderService.CreateOrder(req);
            if (res.Item1)
            {
                MessageBox.Show(string.IsNullOrEmpty(res.Item2) ? "Заказ успешно оформлен!" : res.Item2);
                NavigationService.Navigate(new MainPage());
            }
            else
            {
                MessageBox.Show(string.IsNullOrEmpty(res.Item2) ? "Не удалось оформить заказ" : res.Item2);
            }
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
