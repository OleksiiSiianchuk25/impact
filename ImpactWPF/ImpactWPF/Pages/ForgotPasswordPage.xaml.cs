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
    /// Interaction logic for ForgotPasswordPage.xaml
    /// </summary>
    public partial class ForgotPasswordPage : Page
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger();

        public ForgotPasswordPage()
        {
            InitializeComponent();

            Logger.Info("Сторінка зміни паролю успішно ініціалізована");
        }

        private void TurnBackButton_Click(object sender, RoutedEventArgs e)
        {
            Logger.Info("Користувач повернувся на сторінку входу");
            NavigationService?.Navigate(new LoginPage());
        }

        private void ForgotPasswordButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string email = emailForgotPassword.tbInput.Text;
                    
                string verificationCode = VerificationCodeManager.GenerateVerificationCode();
                Logger.Info("Код підтвердження успішно згенерований");

                VerificationCodeManager.StoreVerificationCode(email, verificationCode);
                Logger.Info($"Код підтвердження успішно прив'язаний до електронної адреси користувача: {email}");

                string subject = "Код підтвердження";
                string body = $"Ваш код підтвердження: {verificationCode}";

                VerificationCodeManager.SendEmail(email, subject, body);
                Logger.Info("Користувач отримав код підтвердження на свою електронну адресу");

                Logger.Info("Користувач перенаправлений на сторінку для вводу коду підтвердження");
                NavigationService?.Navigate(new EnterEmailPage(email));
            }
            catch (Exception ex)
            {
                Logger.Warn($"Помилка: {ex.Message}");
            }
        }

    }
}
