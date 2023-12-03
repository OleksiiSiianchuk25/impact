using EfCore.service.impl;
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

namespace ImpactWPF.Pages
{
    /// <summary>
    /// Interaction logic for ResetPasswordPage.xaml
    /// </summary>
    public partial class ResetPasswordPage : Page
    {
        private readonly string email;

        public ResetPasswordPage(string email)
        {
            InitializeComponent();
            this.email = email;
        }

        private void ResetPasswordButton_Click(object sender, RoutedEventArgs e)
        {
            // Отримати пароль і підтвердження пароля з текстових полів
            string password = passwordTextBox.pbInput.Password;
            string confirmPassword = confirmPasswordTextBox.pbInput.Password;

            if (password == confirmPassword)
            {
                // Пароль і підтвердження пароля співпадають, викликаємо метод для зміни пароля
                try
                {
                    UserServiceImpl userService = new UserServiceImpl(new EfCore.context.ImpactDbContext());
                    userService.ChangePassword(email, password);

                    // Пароль успішно змінено, ви можете виконати додаткові дії або відобразити повідомлення
                    MessageBox.Show("Пароль успішно змінено.", "Інформація", MessageBoxButton.OK, MessageBoxImage.Information);
                    NavigationService?.Navigate(new LoginPage());
                }
                catch (Exception ex)
                {
                    // Обробка помилок, наприклад, виведення повідомлення про помилку
                    MessageBox.Show($"Помилка при зміні пароля: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                // Пароль і підтвердження пароля не співпадають, можна вивести повідомлення
                MessageBox.Show("Пароль і підтвердження пароля не співпадають. Спробуйте ще раз.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void GetBackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new ForgotPasswordPage());
        }
    }
}
