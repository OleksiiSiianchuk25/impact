using EfCore.context;
using EfCore.entity;
using EfCore.service.impl;
using Microsoft.Extensions.Logging;
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

namespace ImpactWPF.Pages
{
    /// <summary>
    /// Interaction logic for AdminPage.xaml
    /// </summary>
    public partial class AdminOrdPage : Page
    {
        private readonly ImpactDbContext context;

        List<User> users;
        List<UserT> usersT;

        public AdminOrdPage()
        {
            InitializeComponent();

            this.context = new ImpactDbContext();
            UserServiceImpl userService = new UserServiceImpl(context);

            this.users = userService.GetOrderers();
            this.usersT = MapUsersForTable(users);

            userDataGrid.ItemsSource = this.usersT;
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

        private void AdminPage_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new AdminPage());
        }

        private void SupportPage_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new SupportPage());
        }

        private void AdminVolunteersButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new AdminVolPage());
        }

        private void AdminButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new AdminPage());
        }

        private void AdminOrderersButton_Click(object sender, RoutedEventArgs e)
        {
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

            return sortedUsersT;
        }

        public class UserT
        {
            public string Email { get; set; }
            public string PhoneNumber { get; set; }
            public string LastName { get; set; }
        }
    }
}
