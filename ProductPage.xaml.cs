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
        public ProductPage()
        {
            InitializeComponent();
            var currentServices = Zianberdin41Entities.GetContext().Product.ToList();

            ProductListView.ItemsSource = currentServices;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ZianberdinClass.MainFrame.Navigate(new AddEditPage());
        }
    }
}
