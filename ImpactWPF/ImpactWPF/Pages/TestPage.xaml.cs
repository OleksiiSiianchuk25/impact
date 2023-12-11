using EfCore.context;
using EfCore.entity;
using EfCore.service.impl;
using ImpactWPF.Controls;
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
    /// Interaction logic for TestPage.xaml
    /// </summary>
    public partial class TestPage : Page
    {
        private RequestServiceImpl requestService; // Assuming you have an instance of RequestServiceImpl

        public TestPage()
        {
            InitializeComponent();
            requestService = new RequestServiceImpl(new ImpactDbContext());

            // Assuming you have a mechanism to get the currently logged-in user's email
            string userEmail = UserSession.Instance.UserEmail;

            // Retrieve user's requests with role 1
            List<Request> userRequests = requestService.GetOrderRequestsByEmailAndRole(userEmail);

            // Create instances of ArchiveCardData for each request
            List<ArchiveCardData> archiveCardDataList = new List<ArchiveCardData>();
            foreach (var userRequest in userRequests)
            {
                ArchiveCardData archiveCardData = new ArchiveCardData
                {
                    RequestName = userRequest.RequestName,
                    Description = userRequest.Description,
                    Location = userRequest.Location,
                    ContactEmail = userRequest.ContactEmail,
                    ContactPhone = userRequest.ContactPhone,
                    CreatedAt = (DateTime)userRequest.CreatedAt
                };

                archiveCardDataList.Add(archiveCardData);
            }

            archiveCardsItemsControl.ItemsSource = archiveCardDataList;
        }
    }
}
