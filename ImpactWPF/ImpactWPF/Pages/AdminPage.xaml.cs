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
    public partial class AdminPage : Page
    {
        private readonly ImpactDbContext context;
        RequestServiceImpl requestService;

        List<Request> requests;
        List<RequestT> requestsT;

        public AdminPage()
        {
            InitializeComponent();

            this.context = new ImpactDbContext();
            requestService = new RequestServiceImpl(context);

            this.requests = requestService.GetAllRequests();
            this.requestsT = MapRequestsForTable(requests);

            requestDataGrid.ItemsSource = this.requestsT;
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

        private List<RequestT> MapRequestsForTable(List<Request> requests)
        {
            List<RequestT> returnedRequestsT = new List<RequestT>();
            foreach (var request in requests)
            {
                RequestT newRequestT = new RequestT();
                newRequestT.Name = request.RequestName;
                newRequestT.Email = request.ContactEmail;

                var roleId = request.RequestId;
                var roleRequest = requestService.GetRequestRoleById(roleId).RoleName;

                

                if (roleRequest.Equals("REQUEST_ORDER"))
                {
                    roleRequest = "Замовлення";
                }
                else
                {
                    roleRequest = "Пропозиція";
                }

                newRequestT.Type = roleRequest;
                returnedRequestsT.Add(newRequestT);
            }
            List<RequestT> sortedRequestsT= returnedRequestsT.OrderBy(member => member.Name).ToList();

            return sortedRequestsT;
        }

        public class RequestT
        {
            public string Name { get; set; }
            public string Email { get; set; }
            public string Type { get; set; }
        }
    }
}
