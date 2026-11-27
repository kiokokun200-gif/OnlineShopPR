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
    /// Логика взаимодействия для AddReviewWindow.xaml
    /// </summary>
    public partial class AddReviewWindow : Window
    {
        private int _productId;
        ReviewService reviewService = new ReviewService();

        public AddReviewWindow(int productId)
        {
            InitializeComponent();
            _productId = productId;
            List<int> ratings = new List<int>()
            {
                5, 4, 3, 2 , 1
            };
            ComboBoxRating.ItemsSource = ratings;
            ComboBoxRating.SelectedIndex = 0;
        }

        private async void BtnAddReview_Click(object sender, RoutedEventArgs e)
        {
            if (ComboBoxRating.SelectedItem == null)
            {
                MessageBox.Show("Выберите оценку");
                return;
            }
            int rating = (int)ComboBoxRating.SelectedItem;
            string reviewText = TxtBoxTextReview.Text;
            if (string.IsNullOrWhiteSpace(reviewText))
            {
                MessageBox.Show("Введите текст отзыва");
                return;
            }

            var res = await reviewService.AddReview(new ReviewCreateRequest
            {
                ProductId = _productId,
                Rating = rating,
                ReviewText = reviewText
            });

            if (res.Item1)
            {
                MessageBox.Show(string.IsNullOrEmpty(res.Item2) ? "Отзыв добавлен" : res.Item2);
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
