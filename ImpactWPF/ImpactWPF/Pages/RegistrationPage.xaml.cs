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

            roleRegistation.SelectionChanged += RoleRegistation_SelectionChanged;

        }

        private void PasswordRegistration_LostFocus(object sender, RoutedEventArgs e)
        {
            string userPassword = passwordRegistration.pbInput.Password;

            if (!IsPasswordValid(userPassword))
            {
                MessageBox.Show("Пароль має складатися мінімум з 8 символів, перший символ у верхньому регістрі, а також пароль повинен містити мінімум 1 цифру!");
                emailRegistration.tbInput.Clear();
            }
        }

        private bool IsPasswordValid(string password)
        {
            Regex regex = new Regex(@"^(?=.*[0-9])(?=.*[A-Z]).{8,}$");
            return regex.IsMatch(password);
        }

        private void PhoneNumberRegistration_LostFocus(object sender, RoutedEventArgs e)
        {
            string userPhoneNumber = phoneNumberRegistration.tbInput.Text;

            if (!IsPhoneNumberValid(userPhoneNumber))
            {
                MessageBox.Show("Некоректний формат номера телефону! \n Приклади: +1234567890\r\n+1 (123) 456-7890\r\n123.456.7890\r\n123-456-7890\r\n1234567890");
                emailRegistration.tbInput.Clear();
            }
        }

        private bool IsPhoneNumberValid(string name)
        {
            Regex regex = new Regex(@"^\+?\d{1,4}?[-.\s]?\(?\d{1,}\)?[-.\s]?\d{1,}[-.\s]?\d{1,}$");
            return regex.IsMatch(name);
        }

        private void MiddleNameRegistration_LostFocus(object sender, RoutedEventArgs e)
        {
            string userMiddleName = middlenameRegistration.tbInput.Text;

            if (!IsValidName(userMiddleName))
            {
                MessageBox.Show("По-батькові повинно містити тільки кирилицю або латиницю!");
                emailRegistration.tbInput.Clear();
            }
        }

        private void FirstNameRegistration_LostFocus(object sender, RoutedEventArgs e)
        {
            string userFirstName = firstnameRegistration.tbInput.Text;

            if (!IsValidName(userFirstName))
            {
                MessageBox.Show("Ім'я повинно містити тільки кирилицю або латиницю!");
                emailRegistration.tbInput.Clear();
            }
        }

        private void LastNameRegistration_LostFocus(object sender, RoutedEventArgs e)
        {
            string userLastName = lastnameRegistration.tbInput.Text;

            if (!IsValidName(userLastName))
            {
                MessageBox.Show("Прізвище повинно містити тільки кирилицю або латиницю!");
                emailRegistration.tbInput.Clear();
            }
        }

        private bool IsValidName(string name)
        {
            Regex regex = new Regex(@"^[a-zA-Zа-яА-ЯїЇіІєЄґҐ'`]+$");
            return regex.IsMatch(name);
        }

        private void EmailRegistration_LostFocus(object sender, RoutedEventArgs e)
        {
            string userEmail = emailRegistration.tbInput.Text;

            if (!IsValidEmail(userEmail))
            {
                MessageBox.Show("Некоректний формат електронної адреси! Приклад: example@mail.com");
                emailRegistration.tbInput.Clear();
            }
        }

        private bool IsValidEmail(string email)
        {
            Regex regex = new Regex(@"^[\w-]+(\.[\w-]+)*@([\w-]+\.)+[a-zA-Z]{2,7}$");
            return regex.IsMatch(email);
        }

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
                if (!ValidateFields())
                {
                    return;
                }

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

        private bool ValidateFields()
        {
            if (!IsValidEmail(emailRegistration.tbInput.Text))
            {
                MessageBox.Show("Некоректний формат електронної адреси! Приклад: example@mail.com");
                return false;
            }

            if (!IsValidName(firstnameRegistration.tbInput.Text))
            {
                MessageBox.Show("Ім'я повинно містити тільки кирилицю або латиницю!");
                return false;
            }

            if (!IsValidName(lastnameRegistration.tbInput.Text))
            {
                MessageBox.Show("Прізвище повинно містити тільки кирилицю або латиницю!");
                return false;
            }

            if (!IsValidName(middlenameRegistration.tbInput.Text))
            {
                MessageBox.Show("По-батькові повинно містити тільки кирилицю або латиницю!");
                return false;
            }

            if (!IsPhoneNumberValid(phoneNumberRegistration.tbInput.Text))
            {
                MessageBox.Show("Некоректний формат номера телефону! \n Приклади: +1234567890\r\n+1 (123) 456-7890\r\n123.456.7890\r\n123-456-7890\r\n1234567890");
                return false;
            }

            if (!IsPasswordValid(passwordRegistration.pbInput.Password))
            {
                MessageBox.Show("Пароль має складатися мінімум з 8 символів, перший символ у верхньому регістрі, а також пароль повинен містити мінімум 1 цифру!");
                return false;
            }

            if (roleRegistation.SelectedItem == null)
            {
                MessageBox.Show("Будь ласка, оберіть роль!");
                return false;
            }

            return true;
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
