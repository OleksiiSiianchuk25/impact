using EfCore.context;
using EfCore.dto;
using EfCore.entity;
using EfCore.service.impl;
using ImpactWPF.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
    /// Interaction logic for ProfilePage.xaml
    /// </summary>
    public partial class ProfilePage : Page
    {
        private User currentUser;
        private ObservableCollection<String> petCollection = new ObservableCollection<String>();
        private readonly UserServiceImpl userService;

        public ProfilePage()
        {
            InitializeComponent();

            PetCollection.Add("Волонтер");
            PetCollection.Add("Замовник");
            if (UserSession.Instance.UserRole == "ROLE_ADMIN")
            {
                PetCollection.Add("Адмін");
            }

            userService = new UserServiceImpl(new ImpactDbContext());
            Loaded += ProfilePage_Loaded;
        }

        private void ProfilePage_Loaded(object sender, RoutedEventArgs e)
        {
            currentUser = userService.GetUserByEmail(UserSession.Instance.UserEmail);
            string currentUserRole = UserSession.Instance.UserRole;

            emailUpdate.tbInput.Text = currentUser.Email;
            lastnameUpdate.tbInput.Text = currentUser.LastName;
            firstnameUpdate.tbInput.Text = currentUser.FirstName;
            middlenameUpdate.tbInput.Text = currentUser.MiddleName;
            phoneNumberUpdate.tbInput.Text = currentUser.PhoneNumber;
            if (currentUserRole == "ROLE_VOLUNTEER")
            {
                roleUpdate.SelectedItem = "Волонтер";
            }   
            else if (currentUserRole == "ROLE_ADMIN")
            {
                roleUpdate.SelectedItem = "Адмін";
            }
            else
            {
                roleUpdate.SelectedItem = "Замовник";
            }
            
        }

        public ObservableCollection<string> PetCollection
        {
            get { return petCollection; }
            set
            {
                petCollection = value;
                NotifyPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
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

        private void UpdateUserButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string userEmail = emailUpdate.tbInput.Text;
                string userLastName = lastnameUpdate.tbInput.Text;
                string userFirstName = firstnameUpdate.tbInput.Text;
                string userMiddleName = middlenameUpdate.tbInput.Text;
                string userPhoneNumber = phoneNumberUpdate.tbInput.Text;
                string userRole = roleUpdate.SelectedItem as string;

                if (!string.IsNullOrEmpty(passwordUpdate.pbInput.Password))
                {
                    if (passwordUpdate.pbInput.Password != confirmPasswordUpdate.pbInput.Password)
                    {
                        throw new InvalidOperationException("Пароль та його підтвердження не співпадають.");
                    }
                    string userPassword = new string(passwordUpdate.pbInput.Password);
                    userService.UpdateUserPassword(currentUser, userPassword);
                }

                userService.UpdateUserData(currentUser, userEmail, userLastName, userFirstName, userMiddleName, userPhoneNumber, userRole);

                MessageBox.Show("Ви успішно відредагували профіль!");
                NavigationService?.Navigate(new HomePage());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Виникла помилка: {ex.Message}");
            }
        }
    }
}
