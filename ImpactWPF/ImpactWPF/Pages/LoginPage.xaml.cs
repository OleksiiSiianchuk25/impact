using EfCore.entity;
using EfCore.service.impl;
using EFCore.service.impl;
using ImpactWPF.Pages;
using Microsoft.Extensions.Logging;
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

namespace ImpactWPF
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger();

        public LoginPage()
        {
            InitializeComponent();

            Logger.Info("Сторінка входу успішно ініціалізована");
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            MyProgressBar.Visibility = Visibility.Visible;

            string email = userEmailLogin.tbInput.Text;
            string password = userPasswordLogin.pbInput.Password;

            AuthServiceImpl authService = new AuthServiceImpl(new EfCore.context.ImpactDbContext());
            if (await Task.Run(() => authService.AuthenticateUser(email, password)))
            {
                string role = authService.GetUserRoleByEmail(email).RoleName;
                UserSession.Instance.Login(email, role);

                Logger.Info("Користувач успішно авторизувався");

                Logger.Info("Користувач перенаправлений на домашню сторінку");
                NavigationService?.Navigate(new HomePage());
            }
            else
            {
                Logger.Error("Неправильна адреса електронної пошти або пароль.");
                MessageBox.Show("Неправильна адреса електронної пошти або пароль.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            MyProgressBar.Visibility = Visibility.Collapsed;
        }


        private void CreateAccountButton_Click(object sender, RoutedEventArgs e)
        {
            Logger.Info("Користувач перейшов на сторінку реєстрації");
            NavigationService?.Navigate(new RegistrationPage());
        }

        private void ForgotPasswordButton_Click(object sender, RoutedEventArgs e)
        {
            Logger.Info("Користувач перейшов на сторінку для зміни паролю");
            NavigationService?.Navigate(new ForgotPasswordPage());
        }
    }
}
