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
    /// Логика взаимодействия для AddPvzWindow.xaml
    /// </summary>
    public partial class AddPvzWindow : Window
    {
        public AddPvzWindow()
        {
            InitializeComponent();
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            string name = TxtBoxName.Text;
            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Заполните название");
                return;
            }

            //создать пвз
        }
    }
}
