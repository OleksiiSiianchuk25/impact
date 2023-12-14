using EfCore.context;
using EfCore.entity;
using EfCore.service.impl;
using NLog;
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
    /// Interaction logic for EditOrdererPage.xaml
    /// </summary>
    public partial class EditOrdererPage : Page
    {
        private readonly UserServiceImpl userService;
        private ObservableCollection<String> petCollection = new ObservableCollection<String>();
        private User currentUser;
        private string currentUserRole;
        private static Logger Logger = LogManager.GetCurrentClassLogger();

        public EditOrdererPage(AdminOrdPage.UserT user)
        {
            InitializeComponent();

            Logger.Info("Сторінка для редагування замовника успішно ініціалізована");

            PetCollection.Add("Волонтер");
            PetCollection.Add("Замовник");

            userService = new UserServiceImpl(new ImpactDbContext());
            currentUser = userService.GetUserByEmail(user.Email);

            currentUserRole = userService.GetUserRoleByEmail(currentUser.Email).RoleName;
            Loaded += EditOrdererPage_Loaded;
            roleUpdate.SelectionChanged += RoleEditPage_SelectionChanged;
        }

        private void EditOrdererPage_Loaded(object sender, RoutedEventArgs e)
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

            Logger.Info($"Дані замовника: {currentUser.Email} успішно завантажені");
        }

        private void RoleEditPage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedRole = roleUpdate.SelectedItem as string;
            UpdatedRoleTextBlock.Text = selectedRole;
            UpdatedRoleTextBlock.Foreground = Brushes.Black;
            Logger.Info($"Користувач обрав роль \"{selectedRole}\"");
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
                Logger.Info("Користувач відкрив спадне навігаційне меню користувача");
            }
            else
            {
                UserMenuGrid.Visibility = Visibility.Collapsed;
                Logger.Info("Користувач закрив спадне навігаційне меню користувача");
            }
        }

        private void HomePage_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            Logger.Info("Користувач перейшов на домашню сторінку");
            NavigationService?.Navigate(new HomePage());
        }

        private void CreateProposalPage_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            Logger.Info("Користувач перейшов на сторінку для створення нової пропозиції");
            NavigationService?.Navigate(new CreateProposalPage());
        }

        private void CreateOrderPage_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            Logger.Info("Користувач перейшов на сторінку для створення нового замовлення");
            NavigationService?.Navigate(new CreateOrderPage());
        }

        private void AdminPage_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            Logger.Info("Користувач перейшов на сторінку адміна з таблицею запитів");
            NavigationService?.Navigate(new AdminPage());
        }

        private void SupportPage_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            Logger.Info("Користувач перейшов на сторінку техпідтримки");
            NavigationService?.Navigate(new SupportPage());
        }

        private void UpdateUserButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Logger.Info("Початок процесу оновлення даних замовника");

                if (!ValidateFields())
                {
                    Logger.Error("Валідація полів вводу не пройшла успішно");
                    return;
                }

                string userEmail = emailUpdate.tbInput.Text;
                string userLastName = lastnameUpdate.tbInput.Text;
                string userFirstName = firstnameUpdate.tbInput.Text;
                string userMiddleName = middlenameUpdate.tbInput.Text;
                string userPhoneNumber = phoneNumberUpdate.tbInput.Text;
                string userRole = roleUpdate.SelectedItem as string;

                userService.AdminUpdateUserData(currentUser, userEmail, userLastName, userFirstName, userMiddleName, userPhoneNumber, userRole);

                Logger.Info("Дані замовника успішно оновленні");

                NavigationService?.Navigate(new AdminOrdPage());
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
            if (!IsValidEmail(emailUpdate.tbInput.Text))
            {
                Logger.Warn("Некоректний формат електронної адреси");
                MessageBox.Show("Некоректний формат електронної адреси! Приклад: example@mail.com", "Попередження", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!IsValidName(firstnameUpdate.tbInput.Text))
            {
                Logger.Warn("Ім'я містить неприпустимі символи");
                MessageBox.Show("Ім'я повинно містити тільки кирилицю або латиницю!", "Попередження", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!IsValidName(lastnameUpdate.tbInput.Text))
            {
                Logger.Warn("Прізвище містить неприпустимі символи");
                MessageBox.Show("Прізвище повинно містити тільки кирилицю або латиницю!", "Попередження", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!IsValidName(middlenameUpdate.tbInput.Text))
            {
                Logger.Warn("По-батькові містить неприпустимі символи");
                MessageBox.Show("По-батькові повинно містити тільки кирилицю або латиницю!", "Попередження", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!IsPhoneNumberValid(phoneNumberUpdate.tbInput.Text))
            {
                Logger.Warn("Некоректний формат номера телефону");
                MessageBox.Show("Некоректний формат номера телефону! \n Приклади: +1234567890\r\n+1 (123) 456-7890\r\n123.456.7890\r\n123-456-7890\r\n1234567890",
                    "Попередження", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            Logger.Info("Валідація полів вводу успішно завершена.");
            return true;
        }
    }
}
