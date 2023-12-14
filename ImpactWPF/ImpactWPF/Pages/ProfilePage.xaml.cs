using EfCore.context;
using EfCore.dto;
using EfCore.entity;
using EfCore.service.impl;
using ImpactWPF.Controls;
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
    /// Interaction logic for ProfilePage.xaml
    /// </summary>
    public partial class ProfilePage : Page
    {
        private User currentUser;
        private ObservableCollection<String> petCollection = new ObservableCollection<String>();
        private readonly UserServiceImpl userService;
        private static Logger Logger = LogManager.GetCurrentClassLogger();

        public ProfilePage()
        {
            InitializeComponent();

            Logger.Info("Сторінка профілю успішно ініціалізована");

            PetCollection.Add("Волонтер");
            PetCollection.Add("Замовник");
            if (UserSession.Instance.UserRole == "ROLE_ADMIN")
            {
                PetCollection.Add("Адмін");
            }

            userService = new UserServiceImpl(new ImpactDbContext());
            Loaded += ProfilePage_Loaded;
            roleUpdate.SelectionChanged += RoleUpdate_SelectionChanged;
        }

        private void RoleUpdate_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedRole = roleUpdate.SelectedItem as string;
            UpdatedRoleTextBlock.Text = selectedRole;
            UpdatedRoleTextBlock.Foreground = Brushes.Black;
            Logger.Info($"Користувач обрав роль \"{selectedRole}\"");
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

            Logger.Info($"Дані користувача: {currentUser.Email} успішно завантажені");
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
                Logger.Info("Початок процесу оновлення даних користувача");

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

                if (!string.IsNullOrEmpty(passwordUpdate.pbInput.Password))
                {
                    if (passwordUpdate.pbInput.Password != confirmPasswordUpdate.pbInput.Password)
                    {
                        throw new InvalidOperationException("Пароль та його підтвердження не співпадають.");
                    }

                    if (!IsPasswordValid(passwordUpdate.pbInput.Password))
                    {
                        Logger.Warn("Некоректний формат паролю");
                        MessageBox.Show("Пароль має складатися мінімум з 8 символів, перший символ у верхньому регістрі, а також пароль повинен містити мінімум 1 цифру!",
                            "Попередження", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    string userPassword = new string(passwordUpdate.pbInput.Password);
                    userService.UpdateUserPassword(currentUser, userPassword);
                }

                userService.UpdateUserData(currentUser, userEmail, userLastName, userFirstName, userMiddleName, userPhoneNumber, userRole);
                Logger.Info("Дані користувача успішно оновленні");

                NavigationService?.Navigate(new HomePage());
            }
            catch (Exception ex)
            {
                Logger.Error($"Виникла помилка: {ex.Message}");
            }
        }

        private bool IsPasswordValid(string password)
        {
            Regex regex = new Regex(@"^(?=.*[0-9])(?=.*[A-Z]).{8,}$
");
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
