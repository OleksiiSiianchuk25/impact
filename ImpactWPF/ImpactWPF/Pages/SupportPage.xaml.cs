using EfCore.entity;
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
    /// Interaction logic for SupportPage.xaml
    /// </summary>
    public partial class SupportPage : Page
    {
        public SupportPage()
        {
            InitializeComponent();
        }

        private void UserMenu_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (UserMenuGrid.Visibility == Visibility.Collapsed)
            {
                UserMenuGrid.Visibility = Visibility.Visible;
            }
            else
            {
                UserMenuGrid.Visibility = Visibility.Collapsed;
            }
        }
        private void HomePage_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new HomePage());
        }

        private void CreateProposalPage_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new CreateProposalPage());
        }

        private void CreateOrderPage_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new CreateOrderPage());
        }

        private void SupportPage_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new SupportPage());
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string subject = themeSupport.tbInput.Text;
                string body = textSupport.tbInput.Text;

                if (string.IsNullOrEmpty(subject) || string.IsNullOrEmpty(body))
                {
                    MessageBox.Show("Будь ласка, заповніть обидва поля.");
                    return;
                }

                body = body + "ВІДПРАВНИК: " + UserSession.Instance.UserEmail;

                VerificationCodeManager.SendSupportEmail(subject, body);


                MessageBox.Show("Ваше повідомлення успішно надіслано!");

                themeSupport.tbInput.Text = "";
                textSupport.tbInput.Text = "";

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка відправки повідомлення: {ex.Message}");
            }
        }
    }
}
