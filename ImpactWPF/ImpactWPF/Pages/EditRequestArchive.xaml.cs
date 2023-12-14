using EfCore.context;
using EfCore.entity;
using EfCore.service.impl;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Interaction logic for EditRequestArchive.xaml
    /// </summary>
    public partial class EditRequestArchive : Page
    {
        private readonly ImpactDbContext dbContext;
        private readonly RequestServiceImpl requestService;
        private Request currentRequest;
        List<string> selectedCategoriesList = new List<string>();
        private static Logger Logger = LogManager.GetCurrentClassLogger();

        public EditRequestArchive(Request request)
        {
            InitializeComponent();

            Logger.Info("Сторінка для редагування запиту успішно ініціалізована");

            dbContext = new ImpactDbContext();
            requestService = new RequestServiceImpl(dbContext);

            currentRequest = requestService.GetRequestById(request.RequestId);

            Loaded += EditRequest_Loaded;
        }

        private void EditRequest_Loaded(object sender, RoutedEventArgs e)
        {
            nameRequest.tbInput.Text = currentRequest.RequestName;
            descriptionRequest.tbInput.Text = currentRequest.Description;
            locationRequest.tbInput.Text = currentRequest.Location;
            contactEmailRequest.tbInput.Text = currentRequest.ContactEmail;
            contactPhoneRequest.tbInput.Text = currentRequest.ContactPhone;

            List<string> associatedCategories = requestService.GetRequestCategoriesNames(currentRequest.RequestId);

            Logger.Info($"Дані запиту: {currentRequest.RequestName} успішно завантажені");
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

        private void UpdateRequestButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Logger.Info("Початок процесу оновлення даних запиту");

                if (!ValidateFields())
                {
                    Logger.Error("Валідація полів вводу не пройшла успішно");
                    return;
                }

                string requestName = nameRequest.tbInput.Text;
                string description = descriptionRequest.tbInput.Text;
                string contactPhone = contactPhoneRequest.tbInput.Text;
                string contactEmail = contactEmailRequest.tbInput.Text;
                string location = locationRequest.tbInput.Text;

                List<int> selectedCategoryIds = GetCategoryIds(selectedCategoriesList);

                requestService.UpdateRequest(currentRequest, requestName, description, contactPhone, contactEmail, location, selectedCategoryIds);
                Logger.Info("Запит успішно оновлений");

                NavigationService?.Navigate(new AtchivePage());
            }
            catch (Exception ex)
            {
                Logger.Error($"Помилка при редагуванні запиту: {ex.Message}");
            }

        }

        private List<int> GetCategoryIds(List<string> categoryNames)
        {
            List<int> categoryIds = new List<int>();

            foreach (var categoryName in categoryNames)
            {
                var category = dbContext.RequestCategories.FirstOrDefault(c => c.CategoryName == categoryName);

                if (category != null)
                {
                    categoryIds.Add(category.CategoryId);
                }
            }
            Logger.Info("Успішно отримано id категорій за їх іменами");
            return categoryIds;
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            if (checkBox != null)
            {
                StackPanel stackPanel = checkBox.Content as StackPanel;

                TextBlock textBlock = stackPanel?.Children.OfType<TextBlock>().FirstOrDefault();

                if (textBlock != null)
                {
                    string categoryName = textBlock.Text;

                    if (!selectedCategoriesList.Contains(categoryName))
                    {
                        selectedCategoriesList.Add(categoryName);
                        Logger.Info($"Користувач обрав категорію: {categoryName}");
                    }
                }

                Border border = checkBox.Parent as Border;
                if (border != null)
                {
                    border.Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(0xFF, 0xFE, 0xE8, 0x83));
                }

                if (stackPanel != null)
                {
                    Image image = stackPanel.Children.OfType<Image>().FirstOrDefault();
                    if (image != null)
                    {
                        RotateTransform rotateTransform = image.RenderTransform as RotateTransform;
                        if (rotateTransform != null)
                        {
                            double centerX = image.ActualWidth / 2;
                            double centerY = image.ActualHeight / 2;

                            rotateTransform.CenterX = centerX;
                            rotateTransform.CenterY = centerY;
                            rotateTransform.Angle = 45;
                        }
                    }
                }
            }
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            if (checkBox != null)
            {
                StackPanel stackPanel = checkBox.Content as StackPanel;

                TextBlock textBlock = stackPanel?.Children.OfType<TextBlock>().FirstOrDefault();

                if (textBlock != null)
                {
                    string categoryName = textBlock.Text;

                    if (selectedCategoriesList.Contains(categoryName))
                    {
                        selectedCategoriesList.Remove(categoryName);
                        Logger.Info($"Користувач вилучив категорію з обраних: {categoryName}");
                    }
                }

                Border border = checkBox.Parent as Border;
                if (border != null)
                {
                    border.Background = new SolidColorBrush(Colors.White);
                }

                if (stackPanel != null)
                {
                    Image image = stackPanel.Children.OfType<Image>().FirstOrDefault();
                    if (image != null)
                    {
                        RotateTransform rotateTransform = image.RenderTransform as RotateTransform;
                        if (rotateTransform != null)
                        {
                            rotateTransform.Angle = 0;
                        }
                    }
                }
            }
        }

        private void Categories_Click(object sender, RoutedEventArgs e)
        {
            if (Categories.Visibility == Visibility.Collapsed)
            {
                Categories.Visibility = Visibility.Visible;
                Logger.Info("Користувач відкрив спадний список з категоріями");
            }
            else
            {
                Categories.Visibility = Visibility.Collapsed;
                Logger.Info("Користувач закрив спадний список з категоріями");
            }
        }

        private bool IsValidEmail(string email)
        {
            Regex regex = new Regex(@"^[\w-]+(\.[\w-]+)*@([\w-]+\.)+[a-zA-Z]{2,7}$");
            return regex.IsMatch(email);
        }

        private bool IsPhoneNumberValid(string name)
        {
            Regex regex = new Regex(@"^\+?\d{1,4}?[-.\s]?\(?\d{1,}\)?[-.\s]?\d{1,}[-.\s]?\d{1,}$");
            return regex.IsMatch(name);
        }

        private bool ValidateFields()
        {
            if (!IsValidEmail(contactEmailRequest.tbInput.Text))
            {
                Logger.Warn("Некоректний формат електронної адреси");
                MessageBox.Show("Некоректний формат електронної адреси! Приклад: example@mail.com", "Попередження", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (nameRequest.tbInput.Text.Length <= 1)
            {
                Logger.Warn("Назва запиту містить менше 1 символа");
                MessageBox.Show("Назва замовлення повинно містити більше 1 символа!", "Попередження", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!IsPhoneNumberValid(contactPhoneRequest.tbInput.Text))
            {
                Logger.Warn("Некоректний формат номера телефону");
                MessageBox.Show("Некоректний формат номера телефону! \n Приклади: +1234567890\r\n+1 (123) 456-7890\r\n123.456.7890\r\n123-456-7890\r\n1234567890",
                    "Попередження", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (selectedCategoriesList.Count > 3 || selectedCategoriesList.Count < 0)
            {
                Logger.Warn("Користувач не обрав від 1 до 3 категорій");
                MessageBox.Show("Будь ласка, оберіть від 1 до 3 категорій!", "Попередження", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (descriptionRequest.tbInput.Text.Length < 0 || descriptionRequest.tbInput.Text.Length > 200)
            {
                Logger.Warn("Опис запиту не містить від 0 до 200 символів");
                MessageBox.Show("Опис запиту повинен містити від 0 до 200 символів!", "Попередження", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            Logger.Info("Валідація полів вводу успішно завершена.");
            return true;
        }
    }
}
