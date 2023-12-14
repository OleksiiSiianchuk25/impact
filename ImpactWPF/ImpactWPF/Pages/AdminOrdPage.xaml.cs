using EfCore.context;
using EfCore.entity;
using EfCore.service.impl;
using Microsoft.Extensions.Logging;
using NLog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Metrics;
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
using static ImpactWPF.Pages.AdminPage;

namespace ImpactWPF.Pages
{
    /// <summary>
    /// Interaction logic for AdminPage.xaml
    /// </summary>
    public partial class AdminOrdPage : Page
    {
        private readonly ImpactDbContext context;
        UserServiceImpl userService;
        List<User> users;
        List<UserT> usersT;
        private static Logger Logger = LogManager.GetCurrentClassLogger();

        public AdminOrdPage()
        {
            InitializeComponent();

            Logger.Info("Сторінка адміна з таблицею замовників успішно ініціалізована");

            this.context = new ImpactDbContext();
            userService = new UserServiceImpl(context);

            this.users = userService.GetOrderers();
            this.usersT = MapUsersForTable(users);

            userDataGrid.ItemsSource = this.usersT;
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

        private void AdminVolunteersButton_Click(object sender, RoutedEventArgs e)
        {
            Logger.Info("Користувач перейшов на сторінку адміна з таблицею волонтерів");
            NavigationService?.Navigate(new AdminVolPage());
        }

        private void AdminButton_Click(object sender, RoutedEventArgs e)
        {
            Logger.Info("Користувач перейшов на сторінку адміна з таблицею запитів");
            NavigationService?.Navigate(new AdminPage());
        }

        private void AdminOrderersButton_Click(object sender, RoutedEventArgs e)
        {
            Logger.Info("Користувач перейшов на сторінку адміна з таблицею замовників");
            NavigationService?.Navigate(new AdminOrdPage());
        }

        private List<UserT> MapUsersForTable(List<User> users)
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
            UserT user = (UserT)userDataGrid.SelectedItem;

            if (user != null)
            {
                EditOrdererPage editOrdererPage = new EditOrdererPage(user);

                Logger.Info("Користувача перейшов на сторінку для редагування замовника");

                NavigationService.Navigate(editOrdererPage);
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            UserT user = (UserT)userDataGrid.SelectedItem;

            if (user != null)
            {
                userService.DeleteUserById(userService.GetUserByEmail(user.Email).UserId);

                Logger.Info($"Користувач успішно видалив замовника: {user.Email}");

                NavigationService?.Navigate(new AdminOrdPage());
            }
        }
    }
}
