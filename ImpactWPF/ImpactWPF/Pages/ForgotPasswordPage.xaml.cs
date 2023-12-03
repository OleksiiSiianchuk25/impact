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
    /// Interaction logic for ForgotPasswordPage.xaml
    /// </summary>
    public partial class ForgotPasswordPage : Page
    {
        public ForgotPasswordPage()
        {
            InitializeComponent();
        }

        private void TurnBackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new LoginPage());
        }

        private void ForgotPasswordButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string email = emailForgotPassword.tbInput.Text;
                    
                string verificationCode = VerificationCodeManager.GenerateVerificationCode();

                VerificationCodeManager.StoreVerificationCode(email, verificationCode);

                string subject = "Код підтвердження";
                string body = $"Ваш код підтвердження: {verificationCode}";

                VerificationCodeManager.SendEmail(email, subject, body);

                NavigationService?.Navigate(new EnterEmailPage(email));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка: {ex.Message}");
            }
        }

    }
}
