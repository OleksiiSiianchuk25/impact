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
    /// Interaction logic for EnterEmailPage.xaml
    /// </summary>
    public partial class EnterEmailPage : Page
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly string email;

        public EnterEmailPage(string email)
        {
            InitializeComponent();

            Logger.Info("Сторінку для введення коду підтвердження успішно ініціалізована");

            this.email = email;
        }


        private void CloseButtonClick(object sender, RoutedEventArgs e)
        {
            Logger.Info("Користувач повернувся на сторінку зміни паролю");
            NavigationService?.Navigate(new ForgotPasswordPage());
        }

        private void VerifyCodeButton_Click(object sender, RoutedEventArgs e)
        {
            string enteredCode = codeTextBox.tbInput.Text;

            if (VerificationCodeManager.VerifyCode(email, enteredCode))
            {
                Logger.Info("Користувач успішно ввів код підтвердження");

                Logger.Info("Користувач перенаправлений на сторінку для зміни паролю");
                NavigationService?.Navigate(new ResetPasswordPage(email));
            }
            else
            {
                Logger.Warn("Користувач ввів неправильний код підтвердження");
                MessageBox.Show("Неправильний код підтвердження. Спробуйте ще раз.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
