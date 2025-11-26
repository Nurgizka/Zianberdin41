using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Zianberdin41размер
{
    /// <summary>
    /// Логика взаимодействия для ProductPage.xaml
    /// </summary>
    public partial class ProductPage : Page
    {
        public ProductPage(User user)
        {
            InitializeComponent();
            if (user != null)
            {
                FIOTB.Text = "Вы авторизовались как: " + user.UserSurname + " " + user.UserName + " " + user.UserPatronymic;
                switch (user.UserRole)
                {
                    case 1:
                        RoleTB.Text = "Роль: Клиент";
                        break;
                    case 2:
                        RoleTB.Text = "Роль: Менеджер";
                        break;
                    case 3:
                        RoleTB.Text = "Роль: Администратор";
                        break;
                }
            }
            else
            {
                FIOTB.Text = "Вы авторизовались как: Гость";
                RoleTB.Text = "";
            }
            var currentProducts = Zianberdin41Entities.GetContext().Product.ToList();
            ProductListView.ItemsSource = currentProducts;

            ComboType.SelectedIndex = 0;
            TextItemCount.Text = $"{currentProducts.Count} из {Zianberdin41Entities.GetContext().Product.Count()}";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ZianberdinClass.MainFrame.Navigate(new AddEditPage());
        }

        private void TBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateProducts();
        }
        private void ComboType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateProducts();
        }

        private void RButtonUp_Checked(object sender, RoutedEventArgs e)
        {
            UpdateProducts();
        }

        private void RButtonDown_Checked(object sender, RoutedEventArgs e)
        {
            UpdateProducts();
        }
        private void UpdateProducts()
        {
            var currentServices = Zianberdin41Entities.GetContext().Product.ToList();
            if (ComboType.SelectedIndex == 0)
            {
                currentServices = currentServices.Where(p => (Convert.ToInt32(p.ProductDiscountAmount) >= 0 && Convert.ToInt32(p.ProductDiscountAmount) <= 100)).ToList();
            }
            if (ComboType.SelectedIndex == 1)
            {
                currentServices = currentServices.Where(p => (Convert.ToInt32(p.ProductDiscountAmount) >= 0 && Convert.ToInt32(p.ProductDiscountAmount) < 9.99)).ToList();
            }
            if (ComboType.SelectedIndex == 2)
            {
                currentServices = currentServices.Where(p => (Convert.ToInt32(p.ProductDiscountAmount) >= 10 && Convert.ToInt32(p.ProductDiscountAmount) <= 14.99)).ToList();
            }
            if (ComboType.SelectedIndex == 3)
            {
                currentServices = currentServices.Where(p => (Convert.ToInt32(p.ProductDiscountAmount) >= 15 && Convert.ToInt32(p.ProductDiscountAmount) <= 100)).ToList();
            }
            
            currentServices = currentServices.Where(p => p.ProductName.ToLower().Contains(TBoxSearch.Text.ToLower())).ToList();
            

            if (RButtonDown.IsChecked == true)
            {
                currentServices = currentServices.OrderByDescending(p => p.ProductCost).ToList();
            }
            if (RButtonUp.IsChecked == true)
            {
                currentServices = currentServices.OrderBy(p => p.ProductCost).ToList();
            }
            ProductListView.ItemsSource = currentServices;
            TextItemCount.Text = $"{currentServices.Count} из {Zianberdin41Entities.GetContext().Product.Count()}";
        }
    }
}
