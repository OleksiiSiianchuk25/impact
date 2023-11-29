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
    /// Interaction logic for EnterEmailPage.xaml
    /// </summary>
    public partial class EnterEmailPage : Page
    {
        private readonly string email;

        public EnterEmailPage(string email)
        {
            InitializeComponent();
            this.email = email;
        }


        private void CloseButtonClick(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new ForgotPasswordPage());
        }

        private void VerifyCodeButton_Click(object sender, RoutedEventArgs e)
        {
            string enteredCode = codeTextBox.tbInput.Text;

            if (VerificationCodeManager.VerifyCode(email, enteredCode))
            {
                NavigationService?.Navigate(new ResetPasswordPage(email));
            }
            else
            {
                MessageBox.Show("Неправильний код підтвердження. Спробуйте ще раз.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ResendCodeButton_Click(object sender, RoutedEventArgs e)
        {
            // Згенерувати новий код підтвердження
            string newVerificationCode = VerificationCodeManager.GenerateVerificationCode();

            // Зберегти новий код підтвердження в тимчасовому сховищі
            VerificationCodeManager.StoreVerificationCode(email, newVerificationCode);

            // Відправити новий код на введену електронну адресу
            string subject = "Новий код підтвердження";
            string body = $"Ваш новий код підтвердження: {newVerificationCode}";

            VerificationCodeManager.SendEmail(email, subject, body);

            // Можна вивести повідомлення, що код відправлено знову
        }
    }
}
