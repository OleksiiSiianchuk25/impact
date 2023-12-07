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
            NavigationService?.Navigate(new SupportPage());
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






        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            if (checkBox != null)
            {
                Border border = checkBox.Parent as Border;
                if (border != null)
                {
                    // Set the background color to #FFE883 using System.Windows.Media.Color
                    border.Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(0xFF, 0xFE, 0xE8, 0x83));
                }

                StackPanel stackPanel = checkBox.Content as StackPanel;
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

                            // Rotate the image by 45 degrees around its center
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
                Border border = checkBox.Parent as Border;
                if (border != null)
                {
                    border.Background = new SolidColorBrush(Colors.White);
                }

                StackPanel stackPanel = checkBox.Content as StackPanel;
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


    }
}
