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
    /// Логика взаимодействия для EditOrderStatusWindow.xaml
    /// </summary>
    public partial class EditOrderStatusWindow : Window
    {
        private OrderDTO _order;
        AdminService adminService = new AdminService();
        List<StatusDTO> _statuses = new List<StatusDTO>();

        public EditOrderStatusWindow(OrderDTO order)
        {
            InitializeComponent();
            _order = order;
            DataContext = order;
            Loaded += EditOrderStatusWindow_Loaded;
        }

        private async void EditOrderStatusWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _statuses = await adminService.GetStatuses();
            TxtBoxName.ItemsSource = _statuses;
            TxtBoxName.DisplayMemberPath = "Name";
            TxtBoxName.SelectedValuePath = "Id";
            TxtBoxName.SelectedValue = _order.StatusId;
        }

        private async void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (TxtBoxName.SelectedValue == null)
            {
                MessageBox.Show("Выберите статус");
                return;
            }
            int statusId = (int)TxtBoxName.SelectedValue;
            var res = await adminService.UpdateOrderStatus(new OrderStatusUpdateRequest { OrderId = _order.OrderID, StatusId = statusId });
            if (res.Item1)
            {
                MessageBox.Show(string.IsNullOrEmpty(res.Item2) ? "Статус обновлен" : res.Item2);
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
