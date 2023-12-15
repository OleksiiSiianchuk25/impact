// <copyright file="EditVolunteerPage.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ImpactWPF.Pages
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Text.RegularExpressions;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;
    using EfCore.context;
    using EfCore.entity;
    using EfCore.service.impl;
    using NLog;

    /// <summary>
    /// Interaction logic for EditVolunteerPage.xaml.
    /// </summary>
    public partial class EditVolunteerPage : Page
    {
        private readonly UserServiceImpl userService;
        private ObservableCollection<string> petCollection = new ObservableCollection<string>();
        private User currentUser;
        private string currentUserRole;
        private static Logger Logger = LogManager.GetCurrentClassLogger();

        public EditVolunteerPage(AdminVolPage.UserT user)
        {
            this.InitializeComponent();

            Logger.Info("Сторінка для редагування волонтера успішно ініціалізована");

            this.PetCollection.Add("Волонтер");
            this.PetCollection.Add("Замовник");

            this.userService = new UserServiceImpl(new ImpactDbContext());
            this.currentUser = this.userService.GetUserByEmail(user.Email);

            this.currentUserRole = this.userService.GetUserRoleByEmail(this.currentUser.Email).RoleName;
            this.Loaded += this.EditVolunteerPage_Loaded;
            this.roleUpdate.SelectionChanged += this.RoleEditPage_SelectionChanged;
        }

        private void EditVolunteerPage_Loaded(object sender, RoutedEventArgs e)
        {
            this.emailUpdate.tbInput.Text = this.currentUser.Email;
            this.lastnameUpdate.tbInput.Text = this.currentUser.LastName;
            this.firstnameUpdate.tbInput.Text = this.currentUser.FirstName;
            this.middlenameUpdate.tbInput.Text = this.currentUser.MiddleName;
            this.phoneNumberUpdate.tbInput.Text = this.currentUser.PhoneNumber;

            if (this.currentUserRole == "ROLE_VOLUNTEER")
            {
                this.roleUpdate.SelectedItem = "Волонтер";
            }
            else
            {
                this.roleUpdate.SelectedItem = "Замовник";
            }

            Logger.Info($"Дані волонтера: {this.currentUser.Email} успішно завантажені");
        }

        private void RoleEditPage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedRole = this.roleUpdate.SelectedItem as string;
            this.UpdatedRoleTextBlock.Text = selectedRole;
            this.UpdatedRoleTextBlock.Foreground = Brushes.Black;
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

        private void UpdateUserButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Logger.Info("Початок процесу оновлення даних волонтера");

                if (!this.ValidateFields())
                {
                    Logger.Error("Валідація полів вводу не пройшла успішно");
                    return;
                }

                string userEmail = this.emailUpdate.tbInput.Text;
                string userLastName = this.lastnameUpdate.tbInput.Text;
                string userFirstName = this.firstnameUpdate.tbInput.Text;
                string userMiddleName = this.middlenameUpdate.tbInput.Text;
                string userPhoneNumber = this.phoneNumberUpdate.tbInput.Text;
                string userRole = this.roleUpdate.SelectedItem as string;

                this.userService.AdminUpdateUserData(this.currentUser, userEmail, userLastName, userFirstName, userMiddleName, userPhoneNumber, userRole);

                Logger.Info("Дані волонтера успішно оновленні");

                this.NavigationService?.Navigate(new AdminVolPage());
            }
            catch (Exception ex)
            {
                Logger.Error($"Виникла помилка: {ex.Message}");
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
            if (!this.IsValidEmail(this.emailUpdate.tbInput.Text))
            {
                Logger.Warn("Некоректний формат електронної адреси");
                MessageBox.Show("Некоректний формат електронної адреси! Приклад: example@mail.com", "Попередження", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!this.IsValidName(this.firstnameUpdate.tbInput.Text))
            {
                Logger.Warn("Ім'я містить неприпустимі символи");
                MessageBox.Show("Ім'я повинно містити тільки кирилицю або латиницю!", "Попередження", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!this.IsValidName(this.lastnameUpdate.tbInput.Text))
            {
                Logger.Warn("Прізвище містить неприпустимі символи");
                MessageBox.Show("Прізвище повинно містити тільки кирилицю або латиницю!", "Попередження", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!this.IsValidName(this.middlenameUpdate.tbInput.Text))
            {
                Logger.Warn("По-батькові містить неприпустимі символи");
                MessageBox.Show("По-батькові повинно містити тільки кирилицю або латиницю!", "Попередження", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!this.IsPhoneNumberValid(this.phoneNumberUpdate.tbInput.Text))
            {
                Logger.Warn("Некоректний формат номера телефону");
                MessageBox.Show(
                    "Некоректний формат номера телефону! \n Приклади: +1234567890\r\n+1 (123) 456-7890\r\n123.456.7890\r\n123-456-7890\r\n1234567890",
                    "Попередження", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            Logger.Info("Валідація полів вводу успішно завершена.");
            return true;
        }
    }
}
