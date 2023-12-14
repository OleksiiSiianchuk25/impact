using EfCore.entity;
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
    /// Interaction logic for SupportPage.xaml
    /// </summary>
    public partial class SupportPage : Page
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger();

        public SupportPage()
        {
            InitializeComponent();

            Logger.Info("Сторінка техпідтримки успішно ініціалізована");
        }

        private void UserMenu_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (UserMenuGrid.Visibility == Visibility.Collapsed)
            {
                UserMenuGrid.Visibility = Visibility.Visible;
                Logger.Info("Користувач відкрив спадне навігаційне меню користувача");
            }
            else
            {
                UserMenuGrid.Visibility = Visibility.Collapsed;
                Logger.Info("Користувач закрив спадне навігаційне меню користувача");
            }
        }

        private void HomePage_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            Logger.Info("Користувач перейшов на домашню сторінку");
            NavigationService?.Navigate(new HomePage());
        }

        private void CreateProposalPage_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            Logger.Info("Користувач перейшов на сторінку для створення нової пропозиції");
            NavigationService?.Navigate(new CreateProposalPage());
        }

        private void CreateOrderPage_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            Logger.Info("Користувач перейшов на сторінку для створення нового замовлення");
            NavigationService?.Navigate(new CreateOrderPage());
        }

        private void AdminPage_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            Logger.Info("Користувач перейшов на сторінку адміна з таблицею запитів");
            NavigationService?.Navigate(new AdminPage());
        }

        private void SupportPage_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            Logger.Info("Користувач перейшов на сторінку техпідтримки");
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
                    Logger.Warn("Користувач не заповнив обидва поля");
                    MessageBox.Show("Будь ласка, заповніть обидва поля.", "Попередження", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                body = body + "    ВІДПРАВНИК: " + UserSession.Instance.UserEmail;

                VerificationCodeManager.SendSupportEmail(subject, body);

                Logger.Info("Повідомлення користувача успішно надіслано");

                themeSupport.tbInput.Text = "";
                textSupport.tbInput.Text = "";

            }
            catch (Exception ex)
            {
                Logger.Error($"Помилка відправки повідомлення: {ex.Message}");
            }
        }
    }
}
