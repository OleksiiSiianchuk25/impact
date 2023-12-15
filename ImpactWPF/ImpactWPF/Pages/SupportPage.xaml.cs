// <copyright file="SupportPage.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ImpactWPF.Pages
{
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
    using EfCore.entity;
    using EfCore.service.impl;
    using NLog;

    /// <summary>
    /// Interaction logic for SupportPage.xaml.
    /// </summary>
    public partial class SupportPage : Page
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger();

        public SupportPage()
        {
            this.InitializeComponent();

            Logger.Info("Сторінка техпідтримки успішно ініціалізована");
        }

        private void UserMenu_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (this.UserMenuGrid.Visibility == Visibility.Collapsed)
            {
                this.UserMenuGrid.Visibility = Visibility.Visible;
                Logger.Info("Користувач відкрив спадне навігаційне меню користувача");
            }
            else
            {
                this.UserMenuGrid.Visibility = Visibility.Collapsed;
                Logger.Info("Користувач закрив спадне навігаційне меню користувача");
            }
        }

        private void HomePage_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            Logger.Info("Користувач перейшов на домашню сторінку");
            this.NavigationService?.Navigate(new HomePage());
        }

        private void CreateProposalPage_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            Logger.Info("Користувач перейшов на сторінку для створення нової пропозиції");
            this.NavigationService?.Navigate(new CreateProposalPage());
        }

        private void CreateOrderPage_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            Logger.Info("Користувач перейшов на сторінку для створення нового замовлення");
            this.NavigationService?.Navigate(new CreateOrderPage());
        }

        private void AdminPage_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            Logger.Info("Користувач перейшов на сторінку адміна з таблицею запитів");
            this.NavigationService?.Navigate(new AdminPage());
        }

        private void SupportPage_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            Logger.Info("Користувач перейшов на сторінку техпідтримки");
            this.NavigationService?.Navigate(new SupportPage());
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string subject = this.themeSupport.tbInput.Text;
                string body = this.textSupport.tbInput.Text;

                if (string.IsNullOrEmpty(subject) || string.IsNullOrEmpty(body))
                {
                    Logger.Warn("Користувач не заповнив обидва поля");
                    MessageBox.Show("Будь ласка, заповніть обидва поля.", "Попередження", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                body = body + "    ВІДПРАВНИК: " + UserSession.Instance.UserEmail;

                VerificationCodeManager.SendSupportEmail(subject, body);

                Logger.Info("Повідомлення користувача успішно надіслано");

                this.themeSupport.tbInput.Text = string.Empty;
                this.textSupport.tbInput.Text = string.Empty;
            }
            catch (Exception ex)
            {
                Logger.Error($"Помилка відправки повідомлення: {ex.Message}");
            }
        }
    }
}
