using EfCore.service.impl;
using NLog;
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
        private static Logger Logger = LogManager.GetCurrentClassLogger();

        public ResetPasswordPage(string email)
        {
            InitializeComponent();

            Logger.Info("Сторінку для введення нового паролю успішно ініціалізована");

            this.email = email;
        }

        private void ResetPasswordButton_Click(object sender, RoutedEventArgs e)
        {
            string password = passwordTextBox.pbInput.Password;
            string confirmPassword = confirmPasswordTextBox.pbInput.Password;

            if (password == confirmPassword)
            {
                try
                {
                    UserServiceImpl userService = new UserServiceImpl(new EfCore.context.ImpactDbContext());
                    userService.ChangePassword(email, password);

                    Logger.Info("Користувач успіщно змінив пароль");
                    MessageBox.Show("Пароль успішно змінено.", "Інформація", MessageBoxButton.OK, MessageBoxImage.Information);

                    Logger.Info("Користувач перенаправлений на сторінку входу");
                    NavigationService?.Navigate(new LoginPage());
                }
                catch (Exception ex)
                {
                    Logger.Error($"Помилка при зміні пароля: {ex.Message}");
                    MessageBox.Show($"Помилка при зміні пароля: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                Logger.Error("Пароль і підтвердження пароля не співпадають");
                MessageBox.Show("Пароль і підтвердження пароля не співпадають. Спробуйте ще раз.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void GetBackButton_Click(object sender, RoutedEventArgs e)
        {
            Logger.Info("Користувач повернувся на сторінку зміни паролю");
            NavigationService?.Navigate(new ForgotPasswordPage());
        }
    }
}
