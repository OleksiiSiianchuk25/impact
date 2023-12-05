using EfCore.context;
using EfCore.dto;
using EfCore.entity;
using EfCore.service.impl;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Interaction logic for CreateProposalPage.xaml
    /// </summary>
    public partial class CreateProposalPage : Page
    {
        private readonly ImpactDbContext dbContext;
        private readonly RequestServiceImpl requestService;
        List<RequestCategory> selectedCategories = new List<RequestCategory>();

        public CreateProposalPage()
        {
            InitializeComponent();
            
            dbContext = new ImpactDbContext();
            requestService = new RequestServiceImpl(dbContext);

            LoadCategories();
        }

        private void LoadCategories()
        {
            try
            {
                var categories = dbContext.RequestCategories.ToList();
                comboBox.ItemsSource = categories;
                comboBox.DisplayMemberPath = "CategoryName";
                comboBox.SelectedValuePath = "CategoryId";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка при завантаженні категорій: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Create a list to hold the selected categories

            // Loop through all items in the ComboBox
            foreach (var item in comboBox.Items)
            {
                // Get the ComboBoxItem that represents the current item
                ComboBoxItem comboBoxItem = (ComboBoxItem)comboBox.ItemContainerGenerator.ContainerFromItem(item);

                // Get the CheckBox from the ComboBoxItem
                CheckBox checkBox = FindVisualChildren<CheckBox>(comboBoxItem).FirstOrDefault();

                // If the CheckBox is checked, add the item to the list of selected categories
                if (checkBox != null && checkBox.IsChecked == true)
                {
                    selectedCategories.Add((RequestCategory)item);
                }
            }

            // Now selectedCategories contains all selected categories
        }

        // Helper method to find visual children of a certain type
        private IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
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
            /*NavigationService?.Navigate(new HomePage());*/
        }

        private void CreateProposalButton_Click(object sender, RoutedEventArgs e)
        {
            // Отримайте дані з елементів управління сторінки
            string proposalName = proposalNameRequest.tbInput.Text;
            string description = descriptionRequest.tbInput.Text;
            string contactPhone = contactPhoneRequest.tbInput.Text;
            string contactEmail = contactEmailRequest.tbInput.Text;
            string location = locationRequest.tbInput.Text;

            // Отримайте обрані категорії з ComboBox
            /*var selectedCategories = selectedItems;*/

            // Створіть об'єкт RequestDTO та встановіть його властивості
            RequestDTO requestDTO = new RequestDTO
            {
                RequestName = proposalName,
                Description = description,
                ContactPhone = contactPhone,
                ContactEmail = contactEmail,
                Location = location,
                /*Categories = (ICollection<RequestCategory>)selectedCategories*/
            };


            /*try
            {
                dbContext.Requests.Add(newRequest);
                dbContext.SaveChanges();
                MessageBox.Show("Пропозиція успішно створена.", "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка при створенні пропозиції: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }*/
        }
    }
}
