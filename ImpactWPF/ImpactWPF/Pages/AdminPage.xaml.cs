// <copyright file="AdminPage.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ImpactWPF.Pages
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using EfCore.context;
    using EfCore.entity;
    using EfCore.service.impl;
    using NLog;

    /// <summary>
    /// Interaction logic for AdminPage.xaml.
    /// </summary>
    public partial class AdminPage : Page
    {
        private readonly ImpactDbContext context;
        private readonly RequestServiceImpl requestService;
        private readonly List<Request> requests;
        private readonly List<RequestT> requestsT;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public AdminPage()
        {
            this.InitializeComponent();

            Logger.Info("Сторінка адміна з таблицею запитів успішно ініціалізована");

            this.context = new ImpactDbContext();
            this.requestService = new RequestServiceImpl(this.context);

            this.requests = this.requestService.GetAllRequests();
            this.requestsT = this.MapRequestsForTable(this.requests);

            this.requestDataGrid.ItemsSource = this.requestsT;
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

        private List<RequestT> MapRequestsForTable(List<Request> requests)
        {
            List<RequestT> returnedRequestsT = new List<RequestT>();
            foreach (var request in requests)
            {
                RequestT newRequestT = new RequestT();
                newRequestT.Name = request.RequestName;
                newRequestT.Email = request.ContactEmail;

                var roleId = request.RequestId;
                var roleRequest = this.requestService.GetRequestRoleById(roleId).RoleName;

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

            List<RequestT> sortedRequestsT = returnedRequestsT.OrderBy(member => member.Name).ToList();

            Logger.Info("Запити успішно додані до таблиці");

            return sortedRequestsT;
        }

        public class RequestT
        {
            public string Name { get; set; }

            public string Email { get; set; }

            public string Type { get; set; }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            RequestT request = (RequestT)this.requestDataGrid.SelectedItem;

            if (request != null)
            {
                EditRequest editRequest = new EditRequest(request);

                Logger.Info("Користувача перейшов на сторінку для редагування запиту");

                this.NavigationService.Navigate(editRequest);
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            RequestT request = (RequestT)this.requestDataGrid.SelectedItem;

            if (request != null)
            {
                this.requestService.DeleteRequest(this.requestService.SearchRequestByName(request.Name).RequestId);

                Logger.Info($"Користувач успішно видалив запит: {request.Name}");

                this.NavigationService?.Navigate(new AdminPage());
            }
        }
    }
}
