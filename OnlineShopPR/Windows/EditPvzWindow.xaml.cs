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
    /// Логика взаимодействия для EditPvzWindow.xaml
    /// </summary>
    public partial class EditPvzWindow : Window
    {
        AdminService adminService = new AdminService();
        private PickupPointDTO _pvz;

        public EditPvzWindow(PickupPointDTO pvz)
        {
            InitializeComponent();
            _pvz = pvz;
            DataContext = _pvz;
        }

        private async void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            string name = TxtBoxName.Text;
            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Введите название");
                return;
            }
            var res = await adminService.SavePickupPoint(new PickupPointSaveRequest { Id = _pvz.Id, AddressName = name });
            if (res.Item1)
            {
                MessageBox.Show(string.IsNullOrEmpty(res.Item2) ? "ПВЗ изменен" : res.Item2);
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
