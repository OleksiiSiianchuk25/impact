using EfCore.context;
using EfCore.dto;
using EfCore.entity;
using EfCore.service.impl;
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
    /// Interaction logic for EditRequest.xaml
    /// </summary>
    public partial class EditRequest : Page
    {
        private readonly ImpactDbContext dbContext;
        private readonly RequestServiceImpl requestService;
        private readonly UserServiceImpl userService;
        private Request currentRequest;
        List<string> selectedCategoriesList = new List<string>();

        public EditRequest(AdminPage.RequestT request)
        {
            InitializeComponent();

            dbContext = new ImpactDbContext();
            requestService = new RequestServiceImpl(dbContext);
            userService = new UserServiceImpl(dbContext);

            currentRequest = requestService.SearchRequestByName(request.Name);

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

        private void AdminPage_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new AdminPage());
        }

        private void CreateProposalButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!ValidateFields())
                {
                    return;
                }

                string requestName = nameRequest.tbInput.Text;
                string description = descriptionRequest.tbInput.Text;
                string contactPhone = contactPhoneRequest.tbInput.Text;
                string contactEmail = contactEmailRequest.tbInput.Text;
                string location = locationRequest.tbInput.Text;

                List<int> selectedCategoryIds = GetCategoryIds(selectedCategoriesList);

                requestService.UpdateRequest(currentRequest, requestName, description, contactPhone, contactEmail, location, selectedCategoryIds);

                NavigationService?.Navigate(new AdminPage());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка при редагуванні запиту: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
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
            }
            else
            {
                Categories.Visibility = Visibility.Collapsed;
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
                MessageBox.Show("Некоректний формат електронної адреси! Приклад: example@mail.com");
                return false;
            }

            if (nameRequest.tbInput.Text.Length <= 1)
            {
                MessageBox.Show("Ім'я запиту повинно містити більше 1 символа!");
                return false;
            }

            if (!IsPhoneNumberValid(contactPhoneRequest.tbInput.Text))
            {
                MessageBox.Show("Некоректний формат номера телефону! \n Приклади: +1234567890\r\n+1 (123) 456-7890\r\n123.456.7890\r\n123-456-7890\r\n1234567890");
                return false;
            }

            if (selectedCategoriesList.Count > 3 || selectedCategoriesList.Count < 0)
            {
                MessageBox.Show("Будь ласка, оберіть від 1 до 3 категорій!");
                return false;
            }

            if (descriptionRequest.tbInput.Text.Length < 0 || descriptionRequest.tbInput.Text.Length > 200)
            {
                MessageBox.Show("Опис запиту повинен містити від 0 до 200 символів!");
                return false;
            }

            return true;
        }
    }
}
