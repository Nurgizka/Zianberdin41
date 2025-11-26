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


namespace Zianberdin41размер
{
    /// <summary>
    /// Логика взаимодействия для AuthPage.xaml
    /// </summary>
    public partial class AuthPage : Page
    {
        // Добавляем поля для капчи
        private string currentCaptcha = "";
        private int failedAttempts = 0;
        private DateTime? blockUntil = null;

        public AuthPage()
        {
            InitializeComponent();
        }

        private void TBVhod_Changed_Click(object sender, RoutedEventArgs e)
        {
            // Проверяем блокировку
            if (blockUntil.HasValue && DateTime.Now < blockUntil.Value)
            {
                MessageBox.Show($"Кнопка заблокирована до {blockUntil.Value:HH:mm:ss}");
                return;
            }

            string login = LoginTB.Text;
            string password = PassTB.Text;
            if (login == "" || password == "")
            {
                MessageBox.Show("Есть пустые поля");
                return;
            }

            // Если капча отображается, проверяем её сначала
            if (CaptchaPanel.Visibility == Visibility.Visible)
            {
                if (string.IsNullOrEmpty(CaptchaInput.Text))
                {
                    MessageBox.Show("Введите капчу!");
                    return;
                }

                if (CaptchaInput.Text.ToUpper() != currentCaptcha)
                {
                    MessageBox.Show("Неверная капча! Кнопка заблокирована на 10 секунд.");
                    BlockLoginButton(10);
                    return;
                }

                // Капча верная, скрываем панель капчи
                CaptchaPanel.Visibility = Visibility.Collapsed;
                CaptchaInput.Text = "";
            }

            User user = Zianberdin41Entities.GetContext().User.ToList().Find(p => p.UserLogin == login && p.UserPassword == password);
            if (user != null)
            {
                ZianberdinClass.MainFrame.Navigate(new ProductPage(user));
                LoginTB.Text = "";
                PassTB.Text = "";
                failedAttempts = 0; // Сбрасываем счетчик при успешном входе
                currentCaptcha = "";
            }
            else
            {
                failedAttempts++;

                // После первой ошибки показываем капчу
                if (failedAttempts >= 1)
                {
                    currentCaptcha = GenerateCaptcha();
                    ShowCaptcha();
                    MessageBox.Show("Неверные данные! Введите капчу.");
                    return; // Прерываем выполнение, ждем ввод капчи
                }

                MessageBox.Show("Введены неверные данные!");
            }
        }

        // Метод для генерации капчи
        private string GenerateCaptcha()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, 4)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        // Метод для показа капчи
        private void ShowCaptcha()
        {
            if (captchaOneWord != null && !string.IsNullOrEmpty(currentCaptcha))
            {
                captchaOneWord.Text = currentCaptcha[0].ToString();
                captchaTwoWord.Text = currentCaptcha[1].ToString();
                captchaThreeWord.Text = currentCaptcha[2].ToString();
                captchaFourWord.Text = currentCaptcha[3].ToString();

                // Показываем панель с капчей
                CaptchaPanel.Visibility = Visibility.Visible;
                CaptchaInput.Focus(); // Ставим фокус на поле ввода
            }
        }

        // Метод для блокировки кнопки
        private async void BlockLoginButton(int seconds)
        {
            TBVhod_Changed.IsEnabled = false;
            blockUntil = DateTime.Now.AddSeconds(seconds);

            string originalContent = TBVhod_Changed.Content.ToString();

            for (int i = seconds; i > 0; i--)
            {
                TBVhod_Changed.Content = $"Заблокировано ({i}с)";
                await Task.Delay(1000);
            }

            TBVhod_Changed.Content = originalContent;
            TBVhod_Changed.IsEnabled = true;
            blockUntil = null;
            currentCaptcha = "";
            failedAttempts = 0;

            // Скрываем капчу и очищаем поле
            CaptchaPanel.Visibility = Visibility.Collapsed;
            CaptchaInput.Text = "";
        }


        private void TBGost_Changed_Click(object sender, RoutedEventArgs e)
        {
            ZianberdinClass.MainFrame.Navigate(new ProductPage(null));
        }
    }
}
