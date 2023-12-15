// <copyright file="ForgotPasswordPage.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ImpactWPF.Pages
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using EfCore.service.impl;
    using NLog;

    /// <summary>
    /// Interaction logic for ForgotPasswordPage.xaml.
    /// </summary>
    public partial class ForgotPasswordPage : Page
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger();

        public ForgotPasswordPage()
        {
            this.InitializeComponent();

            Logger.Info("Сторінка зміни паролю успішно ініціалізована");
        }

        private void TurnBackButton_Click(object sender, RoutedEventArgs e)
        {
            Logger.Info("Користувач повернувся на сторінку входу");
            this.NavigationService?.Navigate(new LoginPage());
        }

        private void ForgotPasswordButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string email = this.emailForgotPassword.tbInput.Text;
                string verificationCode = VerificationCodeManager.GenerateVerificationCode();
                Logger.Info("Код підтвердження успішно згенерований");

                VerificationCodeManager.StoreVerificationCode(email, verificationCode);
                Logger.Info($"Код підтвердження успішно прив'язаний до електронної адреси користувача: {email}");

                string subject = "Код підтвердження";
                string body = $"Ваш код підтвердження: {verificationCode}";

                VerificationCodeManager.SendEmail(email, subject, body);
                Logger.Info("Користувач отримав код підтвердження на свою електронну адресу");

                Logger.Info("Користувач перенаправлений на сторінку для вводу коду підтвердження");
                this.NavigationService?.Navigate(new EnterEmailPage(email));
            }
            catch (Exception ex)
            {
                Logger.Warn($"Помилка: {ex.Message}");
            }
        }
    }
}
