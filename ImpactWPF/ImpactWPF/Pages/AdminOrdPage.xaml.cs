// <copyright file="AdminOrdPage.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ImpactWPF.Pages
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Navigation;
    using EfCore.context;
    using EfCore.entity;
    using EfCore.service.impl;
    using NLog;

    /// <summary>
    /// Interaction logic for AdminPage.xaml.
    /// </summary>
    public partial class AdminOrdPage : Page
    {
        private readonly ImpactDbContext context;
        private readonly UserServiceImpl userService;
        private readonly List<User> users;
        private readonly List<UserT> usersT;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public AdminOrdPage()
        {
            this.InitializeComponent();

            Logger.Info("Сторінка адміна з таблицею замовників успішно ініціалізована");

            this.context = new ImpactDbContext();
            this.userService = new UserServiceImpl(this.context);

            this.users = this.userService.GetOrderers();
            this.usersT = MapUsersForTable(this.users);

            this.userDataGrid.ItemsSource = this.usersT;
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

        private void AdminVolunteersButton_Click(object sender, RoutedEventArgs e)
        {
            Logger.Info("Користувач перейшов на сторінку адміна з таблицею волонтерів");
            this.NavigationService?.Navigate(new AdminVolPage());
        }

        private void AdminButton_Click(object sender, RoutedEventArgs e)
        {
            Logger.Info("Користувач перейшов на сторінку адміна з таблицею запитів");
            this.NavigationService?.Navigate(new AdminPage());
        }

        private void AdminOrderersButton_Click(object sender, RoutedEventArgs e)
        {
            Logger.Info("Користувач перейшов на сторінку адміна з таблицею замовників");
            this.NavigationService?.Navigate(new AdminOrdPage());
        }

        private static List<UserT> MapUsersForTable(List<User> users)
        {
            List<UserT> returnedUsersT = new List<UserT>();
            foreach (var user in users)
            {
                UserT newRequestT = new UserT();
                newRequestT.Email = user.Email;
                newRequestT.PhoneNumber = user.PhoneNumber;
                newRequestT.LastName = user.LastName;

                returnedUsersT.Add(newRequestT);
            }

            List<UserT> sortedUsersT = returnedUsersT.OrderBy(member => member.Email).ToList();

            Logger.Info("Замовники успішно додані до таблиці");

            return sortedUsersT;
        }

        public class UserT
        {
            public string Email { get; set; }

            public string PhoneNumber { get; set; }

            public string LastName { get; set; }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            UserT user = (UserT)this.userDataGrid.SelectedItem;

            if (user != null)
            {
                EditOrdererPage editOrdererPage = new EditOrdererPage(user);

                Logger.Info("Користувача перейшов на сторінку для редагування замовника");

                this.NavigationService.Navigate(editOrdererPage);
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            UserT user = (UserT)this.userDataGrid.SelectedItem;

            if (user != null)
            {
                this.userService.DeleteUserById(this.userService.GetUserByEmail(user.Email).UserId);

                Logger.Info($"Користувач успішно видалив замовника: {user.Email}");

                this.NavigationService?.Navigate(new AdminOrdPage());
            }
        }
    }
}
