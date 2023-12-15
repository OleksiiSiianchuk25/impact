// <copyright file="RegistrationPage.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ImpactWPF
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Text.RegularExpressions;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using EfCore.context;
    using EfCore.dto;
    using EfCore.service.impl;
    using NLog;

    /// <summary>
    /// Interaction logic for RegistrationPage.xaml.
    /// </summary>
    public partial class RegistrationPage : Page, INotifyPropertyChanged
    {
        private ObservableCollection<string> petCollection = new ObservableCollection<string>();
        private readonly UserServiceImpl userService;
        private static Logger Logger = LogManager.GetCurrentClassLogger();

        public RegistrationPage()
        {
            this.InitializeComponent();

            Logger.Info("Сторінка реєстрації успішно ініціалізована");

            this.PetCollection.Add("Волонтер");
            this.PetCollection.Add("Замовник");

            this.userService = new UserServiceImpl(new ImpactDbContext());

            this.roleRegistation.SelectionChanged += this.RoleRegistation_SelectionChanged;
        }

        private bool IsPasswordValid(string password)
        {
            Regex regex = new Regex(@"^(?=.*[0-9])(?=.*[A-Z]).{8,}$");
            return regex.IsMatch(password);
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

        private void RoleRegistation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedRole = this.roleRegistation.SelectedItem as string;
            this.RoleTextBlock.Text = selectedRole;
            this.RoleTextBlock.Foreground = Brushes.Black;
            Logger.Info($"Користувач обрав роль \"{selectedRole}\"");
        }

        public ObservableCollection<string> PetCollection
        {
            get
            {
                return this.petCollection;
            }

            set
            {
                this.petCollection = value;
                this.NotifyPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void TurnBackToLoginPage_Click(object sender, RoutedEventArgs e)
        {
            Logger.Info("Користувач повернувся до сторінки входу");
            this.NavigationService?.Navigate(new LoginPage());
        }

        private void Registration_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Logger.Info("Початок процесу реєстрації користувача");

                if (!this.ValidateFields())
                {
                    Logger.Error("Валідація полів реєстрації не пройшла успішно");
                    return;
                }

                string userEmail = this.emailRegistration.tbInput.Text;

                if (this.userService.GetUserByEmail(userEmail) != null)
                {
                    Logger.Warn($"Користувач з такою електронною поштою вже існує: {userEmail}");
                    MessageBox.Show("Користувач з такою електронною поштою вже існує! " + userEmail, "Попередження", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                string selectedRole = this.roleRegistation.SelectedItem as string;

                string userFirstname = this.firstnameRegistration.tbInput.Text;
                string userLastname = this.lastnameRegistration.tbInput.Text;
                string userMiddlename = this.middlenameRegistration.tbInput.Text;
                string userPhonenumber = this.phoneNumberRegistration.tbInput.Text;
                string userPassword = this.passwordRegistration.pbInput.Password;
                string userPasswordConfirm = this.confirmPasswordRegistration.pbInput.Password;
                int userRoleId = this.GetRoleIdFromRoleName(selectedRole);

                UserDTO userDTO = new UserDTO(userFirstname, userLastname, userMiddlename,
                    userEmail, userPhonenumber, userPassword, userPasswordConfirm, userRoleId);

                this.userService.RegisterUser(userDTO);

                Logger.Info("Користувач успішно зареєстрований");

                Logger.Info("Користувач перенаправлений на сторінку входу");
                this.NavigationService?.Navigate(new LoginPage());
            }
            catch (Exception ex)
            {
                Logger.Error($"Виникла помилка при реєстрації користувача: {ex.Message}");
            }
        }

        private bool ValidateFields()
        {
            if (!this.IsValidEmail(this.emailRegistration.tbInput.Text))
            {
                Logger.Warn("Некоректний формат електронної адреси");
                MessageBox.Show("Некоректний формат електронної адреси! Приклад: example@mail.com", "Попередження", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!this.IsValidName(this.firstnameRegistration.tbInput.Text))
            {
                Logger.Warn("Ім'я містить неприпустимі символи");
                MessageBox.Show("Ім'я повинно містити тільки кирилицю або латиницю!", "Попередження", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!this.IsValidName(this.lastnameRegistration.tbInput.Text))
            {
                Logger.Warn("Прізвище містить неприпустимі символи");
                MessageBox.Show("Прізвище повинно містити тільки кирилицю або латиницю!", "Попередження", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!this.IsValidName(this.middlenameRegistration.tbInput.Text))
            {
                Logger.Warn("По-батькові містить неприпустимі символи");
                MessageBox.Show("По-батькові повинно містити тільки кирилицю або латиницю!", "Попередження", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!this.IsPhoneNumberValid(this.phoneNumberRegistration.tbInput.Text))
            {
                Logger.Warn("Некоректний формат номера телефону");
                MessageBox.Show(
                    "Некоректний формат номера телефону! \n Приклади: +1234567890\r\n+1 (123) 456-7890\r\n123.456.7890\r\n123-456-7890\r\n1234567890",
                    "Попередження", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!this.IsPasswordValid(this.passwordRegistration.pbInput.Password))
            {
                Logger.Warn("Некоректний формат паролю");
                MessageBox.Show(
                    "Пароль має складатися мінімум з 8 символів, перший символ у верхньому регістрі, а також пароль повинен містити мінімум 1 цифру!",
                    "Попередження", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (this.roleRegistation.SelectedItem == null)
            {
                Logger.Warn("Користувач не обрав роль");
                MessageBox.Show("Будь ласка, оберіть роль!", "Попередження", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            Logger.Info("Валідація полів реєстрації успішно завершена.");
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
