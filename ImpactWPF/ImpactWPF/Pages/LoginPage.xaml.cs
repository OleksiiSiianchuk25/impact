// <copyright file="LoginPage.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ImpactWPF
{
    using System.Diagnostics;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using EfCore.entity;
    using EFCore.service.impl;
    using ImpactWPF.Pages;
    using NLog;

    /// <summary>
    /// Interaction logic for LoginPage.xaml.
    /// </summary>
    public partial class LoginPage : Page
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginPage"/> class.
        /// </summary>
        public LoginPage()
        {
            this.InitializeComponent();

            Logger.Info("Сторінка входу успішно ініціалізована");
        }

        private void TestVideo_MediaFailed_1(object sender, ExceptionRoutedEventArgs e)
        {
            var details = $"Failed to load media: {e.ErrorException.Message}";

            Debug.WriteLine(details);
            MessageBox.Show(details);
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            this.MyProgressBar.Visibility = Visibility.Visible;

            string email = this.userEmailLogin.tbInput.Text;
            string password = this.userPasswordLogin.pbInput.Password;

            AuthServiceImpl authService = new AuthServiceImpl(new EfCore.context.ImpactDbContext());
            if (await Task.Run(() => authService.AuthenticateUser(email, password)))
            {
                string role = authService.GetUserRoleByEmail(email).RoleName;
                UserSession.Instance.Login(email, role);

                Logger.Info("Користувач успішно авторизувався");

                Logger.Info("Користувач перенаправлений на домашню сторінку");
                this.NavigationService?.Navigate(new HomePage());
            }
            else
            {
                Logger.Error("Неправильна адреса електронної пошти або пароль.");
                MessageBox.Show("Неправильна адреса електронної пошти або пароль.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            this.MyProgressBar.Visibility = Visibility.Collapsed;
        }

        private void CreateAccountButton_Click(object sender, RoutedEventArgs e)
        {
            Logger.Info("Користувач перейшов на сторінку реєстрації");
            this.NavigationService?.Navigate(new RegistrationPage());
        }

        private void ForgotPasswordButton_Click(object sender, RoutedEventArgs e)
        {
            Logger.Info("Користувач перейшов на сторінку для зміни паролю");
            this.NavigationService?.Navigate(new ForgotPasswordPage());
        }
    }
}
