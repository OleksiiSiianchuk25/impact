// <copyright file="EnterEmailPage.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ImpactWPF.Pages
{
    using System.Windows;
    using System.Windows.Controls;
    using EfCore.service.impl;
    using NLog;

    /// <summary>
    /// Interaction logic for EnterEmailPage.xaml.
    /// </summary>
    public partial class EnterEmailPage : Page
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly string email;

        public EnterEmailPage(string email)
        {
            this.InitializeComponent();

            Logger.Info("Сторінку для введення коду підтвердження успішно ініціалізована");

            this.email = email;
        }

        private void CloseButtonClick(object sender, RoutedEventArgs e)
        {
            Logger.Info("Користувач повернувся на сторінку зміни паролю");
            this.NavigationService?.Navigate(new ForgotPasswordPage());
        }

        private void VerifyCodeButton_Click(object sender, RoutedEventArgs e)
        {
            string enteredCode = this.codeTextBox.tbInput.Text;

            if (VerificationCodeManager.VerifyCode(this.email, enteredCode))
            {
                Logger.Info("Користувач успішно ввів код підтвердження");

                Logger.Info("Користувач перенаправлений на сторінку для зміни паролю");
                this.NavigationService?.Navigate(new ResetPasswordPage(this.email));
            }
            else
            {
                Logger.Warn("Користувач ввів неправильний код підтвердження");
                MessageBox.Show("Неправильний код підтвердження. Спробуйте ще раз.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
