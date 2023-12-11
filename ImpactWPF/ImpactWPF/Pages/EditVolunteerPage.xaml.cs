using EfCore.context;
using EfCore.entity;
using EfCore.service.impl;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for EditVolunteerPage.xaml
    /// </summary>
    public partial class EditVolunteerPage : Page
    {
        private readonly UserServiceImpl userService;
        private ObservableCollection<String> petCollection = new ObservableCollection<String>();
        private User currentUser;
        private string currentUserRole;

        public EditVolunteerPage(AdminVolPage.UserT user)
        {
            InitializeComponent();
            PetCollection.Add("Волонтер");
            PetCollection.Add("Замовник");

            userService = new UserServiceImpl(new ImpactDbContext());
            currentUser = userService.GetUserByEmail(user.Email);

            currentUserRole = userService.GetUserRoleByEmail(currentUser.Email).RoleName;
            Loaded += EditVolunteerPage_Loaded;
            roleUpdate.SelectionChanged += RoleEditPage_SelectionChanged;
        }

        private void EditVolunteerPage_Loaded(object sender, RoutedEventArgs e)
        {
            emailUpdate.tbInput.Text = currentUser.Email;
            lastnameUpdate.tbInput.Text = currentUser.LastName;
            firstnameUpdate.tbInput.Text = currentUser.FirstName;
            middlenameUpdate.tbInput.Text = currentUser.MiddleName;
            phoneNumberUpdate.tbInput.Text = currentUser.PhoneNumber;

            if (currentUserRole == "ROLE_VOLUNTEER")
            {
                roleUpdate.SelectedItem = "Волонтер";

            }
            else
            {
                roleUpdate.SelectedItem = "Замовник";
            }

        }

        private void RoleEditPage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedRole = roleUpdate.SelectedItem as string;
            UpdatedRoleTextBlock.Text = selectedRole;
            UpdatedRoleTextBlock.Foreground = Brushes.Black;
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

        private void AdminPage_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new AdminPage());
        }

        private void SupportPage_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new SupportPage());
        }

        private void UpdateUserButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!ValidateFields())
                {
                    return;
                }

                string userEmail = emailUpdate.tbInput.Text;
                string userLastName = lastnameUpdate.tbInput.Text;
                string userFirstName = firstnameUpdate.tbInput.Text;
                string userMiddleName = middlenameUpdate.tbInput.Text;
                string userPhoneNumber = phoneNumberUpdate.tbInput.Text;
                string userRole = roleUpdate.SelectedItem as string;

                userService.AdminUpdateUserData(currentUser, userEmail, userLastName, userFirstName, userMiddleName, userPhoneNumber, userRole);

                MessageBox.Show("Ви успішно відредагували волонтера!");
                NavigationService?.Navigate(new AdminVolPage());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Виникла помилка: {ex.Message}");
            }
        }

        private bool IsPhoneNumberValid(string name)
        {
            Regex regex = new Regex(@"^\+?\d{1,4}?[-.\s]?\(?\d{1,}\)?[-.\s]?\d{1,}[-.\s]?\d{1,}$");
            return regex.IsMatch(name);
        }

        private bool IsValidName(string name)
        {
            Regex regex = new Regex(@"^[a-zA-Zа-яА-ЯїЇіІєЄґҐ'`]+$");
            return regex.IsMatch(name);
        }


        private bool IsValidEmail(string email)
        {
            Regex regex = new Regex(@"^[\w-]+(\.[\w-]+)*@([\w-]+\.)+[a-zA-Z]{2,7}$");
            return regex.IsMatch(email);
        }

        private bool ValidateFields()
        {
            if (!IsValidEmail(emailUpdate.tbInput.Text))
            {
                MessageBox.Show("Некоректний формат електронної адреси! Приклад: example@mail.com");
                return false;
            }

            if (!IsValidName(firstnameUpdate.tbInput.Text))
            {
                MessageBox.Show("Ім'я повинно містити тільки кирилицю або латиницю!");
                return false;
            }

            if (!IsValidName(lastnameUpdate.tbInput.Text))
            {
                MessageBox.Show("Прізвище повинно містити тільки кирилицю або латиницю!");
                return false;
            }

            if (!IsValidName(middlenameUpdate.tbInput.Text))
            {
                MessageBox.Show("По-батькові повинно містити тільки кирилицю або латиницю!");
                return false;
            }

            if (!IsPhoneNumberValid(phoneNumberUpdate.tbInput.Text))
            {
                MessageBox.Show("Некоректний формат номера телефону! \n Приклади: +1234567890\r\n+1 (123) 456-7890\r\n123.456.7890\r\n123-456-7890\r\n1234567890");
                return false;
            }

            return true;
        }
    }
}
