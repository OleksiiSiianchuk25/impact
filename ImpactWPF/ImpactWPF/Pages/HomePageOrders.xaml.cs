// <copyright file="HomePageOrders.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ImpactWPF.Pages
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Markup;
    using System.Windows.Media;
    using System.Xml;
    using EfCore.context;
    using EfCore.entity;
    using EfCore.service.impl;
    using ImpactWPF.View;

    /// <summary>
    /// Interaction logic for HomePageOrders.xaml.
    /// </summary>
    public partial class HomePageOrders : Page
    {
        private readonly ImpactDbContext dbContext;
        private readonly RequestServiceImpl requestService;
        private readonly HomePageOrdersViewModel viewModel;

        public HomePageOrders()
        {
            this.InitializeComponent();

            this.dbContext = new ImpactDbContext();
            this.requestService = new RequestServiceImpl(this.dbContext);

            this.SetButtonStyles(this.OrderButton);

            this.viewModel = new HomePageOrdersViewModel(this);
            this.viewModel.LoadInitialRequests();
            this.DataContext = this.viewModel;
        }

        private void LoadInitialRequests()
        {
            List<Request> initialRequests = this.requestService.GetActiveOrders(this.viewModel.PageSize);
            this.viewModel.Requests = new ObservableCollection<Request>(initialRequests);
        }

        private void Button_LoadMore_Click(object sender, RoutedEventArgs e)
        {
            this.viewModel.LoadMoreRequests();
        }

        private void nameInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Отримання тексту з текстового поля
            this.viewModel.SearchTerm = this.nameInput.Text;
        }

        public Button GetLoadMoreButton()
        {
            return this.Button_LoadMore;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Отримання кнопки, яка була натиснута
            var clickedButton = (Button)sender;

            // Зміна стилів для обраної кнопки
            this.SetButtonStyles(clickedButton);

            // Зміна стилів для іншої кнопки
            var otherButton = (clickedButton == this.PropositionButton) ? this.OrderButton : this.PropositionButton;
            this.ClearButtonStyles(otherButton);

            this.NavigationService?.Navigate(new HomePage());
        }

        private void SetButtonStyles(Button button)
        {
            button.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#002861"));
            button.Foreground = Brushes.White;
        }

        private void ClearButtonStyles(Button button)
        {
            button.ClearValue(Button.BackgroundProperty);
            button.ClearValue(Button.BorderBrushProperty);
            button.ClearValue(Button.BorderThicknessProperty);
            button.Foreground = Brushes.Black; // Повертаємо колір тексту на чорний
            button.ClearValue(Button.EffectProperty);
        }

        // Ваш код
        public class YourViewModel
        {
            public ObservableCollection<RectangleData> YourRectangleCollection { get; } = new ObservableCollection<RectangleData>();

            public YourViewModel()
            {
                // Додайте прямокутники з відповідними властивостями до колекції
                for (int i = 0; i < 12; i++)
                {
                    this.YourRectangleCollection.Add(new RectangleData());
                }
            }
        }

        public class RectangleData
        {
            public Thickness RectangleMargin { get; set; } = new Thickness(5); // Задайте відповідні відступи тут
        }

        private void searchInput_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (textBox.Text == "Пошук")
            {
                textBox.Text = string.Empty;
                textBox.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#002861"));
            }
        }

        private void searchInput_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Text = "Пошук";
                textBox.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#928A8A"));
            }
        }

        private void searchImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (this.CategoriesGrid.Visibility == Visibility.Collapsed)
            {
                this.CategoriesGrid.Visibility = Visibility.Visible;
            }
            else
            {
                this.CategoriesGrid.Visibility = Visibility.Collapsed;
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            if (checkBox != null)
            {
                // Clone the original content (copy only text and style)
                string originalContent = (checkBox.Content ?? string.Empty).ToString();
                Style originalStyle = checkBox.Style;

                // Serialize and deserialize to create a deep copy of the CheckBox
                string checkBoxXaml = XamlWriter.Save(checkBox);

                using (StringReader stringReader = new StringReader(checkBoxXaml))
                {
                    using (XmlReader xmlReader = XmlReader.Create(stringReader))
                    {
                        CheckBox newCheckBox = (CheckBox)XamlReader.Load(xmlReader);

                        if (newCheckBox != null)
                        {
                            newCheckBox.IsChecked = true;
                            StackPanel stackPanel = checkBox.Content as StackPanel;

                            TextBlock textBlock = stackPanel.Children.OfType<TextBlock>().FirstOrDefault();

                            string text = textBlock.Text;

                            // Apply the specific style to the new CheckBox
                            newCheckBox.Style = (Style)this.FindResource("CheckBoxStyle1");

                            Border newBorder = new Border();
                            newBorder.CornerRadius = new CornerRadius(15);
                            newBorder.BorderBrush = new SolidColorBrush(System.Windows.Media.Color.FromArgb(0xFF, 0xFE, 0xE8, 0x83));
                            newBorder.BorderThickness = new Thickness(1);
                            newBorder.Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(0xFF, 0xFE, 0xE8, 0x83));
                            newBorder.Height = 45;
                            newBorder.Width = 235;

                            newBorder.Child = newCheckBox;

                            // Отримайте вихідне зображення
                            Image originalImage = this.FindVisualParent<Grid>(checkBox).Children.OfType<Image>().FirstOrDefault();

                            if (originalImage != null)
                            {
                                // Створіть нове зображення з такими ж властивостями, як і вихідне зображення
                                Image newImage = new Image();
                                newImage.Source = originalImage.Source;
                                newImage.Height = originalImage.Height;
                                newImage.Width = originalImage.Width;
                                newImage.Margin = new Thickness(-3, 0, 0, 10);

                                // Отримайте StackPanel нового CheckBox
                                StackPanel newStackPanel = newCheckBox.Content as StackPanel;

                                // Додайте нове зображення до StackPanel нового CheckBox
                                if (newStackPanel != null)
                                {
                                    newStackPanel.Children.Insert(0, newImage); // Додає зображення на початок StackPanel
                                }
                            }

                            ListBoxItem newItem = new ListBoxItem();
                            newItem.Content = newBorder;

                            // Apply the specific style to the new ListBoxItem
                            newItem.Style = (Style)this.FindResource("ListBoxItemStyle1");

                            this.LeftList.Items.Add(newItem);

                            // Remove the existing handler to avoid multiple subscriptions
                            newCheckBox.Unchecked -= this.CheckBox_Unchecked;
                            newCheckBox.Unchecked += this.CheckBox_Unchecked;

                            // Highlight the original item
                            Border border = this.FindVisualParent<Border>(checkBox);
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

                            this.viewModel.SelectedCategories.Add(text);
                        }
                    }

                    this.viewModel.FilterRequestsByCategories();
                }
            }
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            if (checkBox != null)
            {
                // Find the parent ListBoxItem in LeftList
                foreach (ListBoxItem item in this.LeftList.Items.Cast<ListBoxItem>().ToList())
                {
                    CheckBox itemCheckBox = item.Content as CheckBox;
                    if (itemCheckBox != null && itemCheckBox.Content.ToString() == checkBox.Content.ToString())
                    {
                        this.LeftList.Items.Remove(item);
                        break;
                    }
                }

                // Remove the existing handler to avoid multiple subscriptions
                checkBox.Unchecked -= this.CheckBox_Unchecked;
                StackPanel stackPanel = checkBox.Content as StackPanel;

                TextBlock textBlock = stackPanel.Children.OfType<TextBlock>().FirstOrDefault();

                string text = textBlock.Text;

                // Highlight the original item
                Border border = this.FindVisualParent<Border>(checkBox);
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

                this.viewModel.SelectedCategories.Remove(text);
                this.viewModel.FilterRequestsByCategories();
            }
        }

        private T? FindVisualParent<T>(DependencyObject child)
            where T : DependencyObject
        {
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);

            if (parentObject == null)
            {
                return null;
            }

            T parent = parentObject as T;
            if (parent != null)
            {
                return parent;
            }
            else
            {
                return this.FindVisualParent<T>(parentObject);
            }
        }

        private void Image_MouseEnter(object sender, MouseEventArgs e)
        {
            // Handle MouseEnter event if needed
        }

        private void Image_MouseLeave(object sender, MouseEventArgs e)
        {
            // Handle MouseLeave event if needed
        }

        private void UserMenu_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (this.UserMenuGrid.Visibility == Visibility.Collapsed)
            {
                this.UserMenuGrid.Visibility = Visibility.Visible;
            }
            else
            {
                this.UserMenuGrid.Visibility = Visibility.Collapsed;
            }
        }

        private void HomePage_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            this.NavigationService?.Navigate(new HomePage());
        }

        private void CreateProposalPage_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            this.NavigationService?.Navigate(new CreateProposalPage());
        }

        private void CreateOrderPage_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            this.NavigationService?.Navigate(new CreateOrderPage());
        }

        private void AdminPage_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            this.NavigationService?.Navigate(new AdminPage());
        }

        private void SupportPage_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            this.NavigationService?.Navigate(new SupportPage());
        }
    }
}
