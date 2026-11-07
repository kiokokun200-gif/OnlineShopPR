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
    /// Логика взаимодействия для AddReviewWindow.xaml
    /// </summary>
    public partial class AddReviewWindow : Window
    {
        public AddReviewWindow()
        {
            InitializeComponent();
            List<int> ratings = new List<int>()
            {
                5, 4, 3, 2 , 1
            };
            ComboBoxRating.ItemsSource = ratings;
        }

        private void BtnAddReview_Click(object sender, RoutedEventArgs e)
        {
            int rating = (int)ComboBoxRating.SelectedItem;
            string reviewText = TxtBoxTextReview.Text;

        }
    }
}
