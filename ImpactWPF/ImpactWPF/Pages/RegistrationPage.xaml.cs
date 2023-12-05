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
using EfCore;
using EfCore.context;
using EfCore.dto;
using EfCore.entity;
using EfCore.service.impl;
using Microsoft.EntityFrameworkCore;

namespace ImpactWPF
{
    /// <summary>
    /// Interaction logic for RegistrationPage.xaml
    /// </summary>
    public partial class RegistrationPage : Page, INotifyPropertyChanged
    {
        private ObservableCollection<String> petCollection = new ObservableCollection<String>();
        private readonly UserServiceImpl userService;

        public RegistrationPage()
        {
            InitializeComponent();

            PetCollection.Add("Волонтер");
            PetCollection.Add("Замовник");

            userService = new UserServiceImpl(new ImpactDbContext());

            // Додайте цей рядок, щоб підписатися на подію SelectionChanged
            roleRegistation.SelectionChanged += RoleRegistation_SelectionChanged;
        }

        // Цей метод буде викликаний, коли користувач змінить вибір в ComboBox
        private void RoleRegistation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedRole = roleRegistation.SelectedItem as string;
            RoleTextBlock.Text = selectedRole;
            RoleTextBlock.Foreground = Brushes.Black;
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

        public void NotifyPropertyChanged([CallerMemberName] string propertyName="")
        {
            if(PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void TurnBackToLoginPage_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new LoginPage());
        }

        private void Registration_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string userEmail = emailRegistration.tbInput.Text; 

                if (userService.GetUserByEmail(userEmail) != null)
                {
                    MessageBox.Show("Користувач з такою електронною поштою вже існує! " + userEmail);
                    return;
                }

                string selectedRole = roleRegistation.SelectedItem as string;

                string userFirstname = firstnameRegistration.tbInput.Text;
                string userLastname = lastnameRegistration.tbInput.Text;
                string userMiddlename = middlenameRegistration.tbInput.Text;
                string userPhonenumber = phoneNumberRegistration.tbInput.Text;
                string userPassword = passwordRegistration.pbInput.Password;
                string userPasswordConfirm = confirmPasswordRegistration.pbInput.Password;
                int userRoleId = GetRoleIdFromRoleName(selectedRole);

                UserDTO userDTO = new UserDTO(userFirstname, userLastname, userMiddlename, 
                    userEmail, userPhonenumber, userPassword, userPasswordConfirm, userRoleId);

                userService.RegisterUser(userDTO);

                MessageBox.Show("Ви успішно зареєструвалися!");
                NavigationService?.Navigate(new LoginPage());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Виникла помилка: {ex.Message}");
            }
        }

        private int GetRoleIdFromRoleName(string roleName)
        {
            if (roleName == "Волонтер")
            {
                return 2;
            }
            return 1;
        }
    }
}
