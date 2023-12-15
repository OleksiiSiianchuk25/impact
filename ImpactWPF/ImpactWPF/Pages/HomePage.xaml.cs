using EfCore.context;
using EfCore.entity;
using EfCore.service.impl;
using ImpactWPF.Controls;
using ImpactWPF.View;
using NLog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;

namespace ImpactWPF.Pages
{
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage : Page
    {
        private readonly ImpactDbContext dbContext;
        private readonly RequestServiceImpl requestService;
        private readonly HomePageViewModel viewModel;

        public HomePage()
        {
            InitializeComponent();

            dbContext = new ImpactDbContext();
            requestService = new RequestServiceImpl(dbContext);

            SetButtonStyles(PropositionButton);

            viewModel = new HomePageViewModel(this);
            viewModel.LoadInitialRequests();
            DataContext = viewModel;
        }

        private void Button_LoadMore_Click(object sender, RoutedEventArgs e)
        {
            viewModel.LoadMoreRequests();
        }



        public Button GetLoadMoreButton()
        {
            return Button_LoadMore;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var clickedButton = (Button)sender;

            SetButtonStyles(clickedButton);

            var otherButton = (clickedButton == PropositionButton) ? OrderButton : PropositionButton;
            ClearButtonStyles(otherButton);

            NavigationService?.Navigate(new HomePageOrders());
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
                    YourRectangleCollection.Add(new RectangleData());
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

        private void nameInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Отримання тексту з текстового поля
            viewModel.SearchTerm = nameInput.Text;
        }


        private void searchImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (CategoriesGrid.Visibility == Visibility.Collapsed)
            {
                CategoriesGrid.Visibility = Visibility.Visible;
            }
            else
            {
                CategoriesGrid.Visibility = Visibility.Collapsed;
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

        private void AdminPage_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new AdminPage());
        }

        private void SupportPage_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new SupportPage());
        }






        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            if (checkBox != null)
            {
                // Clone the original content (copy only text and style)
                string originalContent = (checkBox.Content ?? "").ToString();
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
                            newCheckBox.Style = (Style)FindResource("CheckBoxStyle1");

                            Border newBorder = new Border();
                            newBorder.CornerRadius = new CornerRadius(15);
                            newBorder.BorderBrush = new SolidColorBrush(System.Windows.Media.Color.FromArgb(0xFF, 0xFE, 0xE8, 0x83));
                            newBorder.BorderThickness = new Thickness(1);
                            newBorder.Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(0xFF, 0xFE, 0xE8, 0x83));
                            newBorder.Height = 45;
                            newBorder.Width = 235;

                            newBorder.Child = newCheckBox;

                            // Отримайте вихідне зображення
                            Image originalImage = FindVisualParent<Grid>(checkBox).Children.OfType<Image>().FirstOrDefault();

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
                            newItem.Style = (Style)FindResource("ListBoxItemStyle1");

                            LeftList.Items.Add(newItem);

                            // Remove the existing handler to avoid multiple subscriptions
                            newCheckBox.Unchecked -= CheckBox_Unchecked;
                            newCheckBox.Unchecked += CheckBox_Unchecked;

                            // Highlight the original item
                            Border border = FindVisualParent<Border>(checkBox);
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
                            viewModel.SelectedCategories.Add(text);
                        }
                    }
                    viewModel.FilterRequestsByCategories();
                }
            }
        }






        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            if (checkBox != null)
            {
                // Find the parent ListBoxItem in LeftList
                foreach (ListBoxItem item in LeftList.Items.Cast<ListBoxItem>().ToList())
                {
                    CheckBox itemCheckBox = item.Content as CheckBox;
                    if (itemCheckBox != null && itemCheckBox.Content.ToString() == checkBox.Content.ToString())
                    {
                        LeftList.Items.Remove(item);
                        break;
                    }
                }

                // Remove the existing handler to avoid multiple subscriptions
                checkBox.Unchecked -= CheckBox_Unchecked;
                StackPanel stackPanel = checkBox.Content as StackPanel;

                TextBlock textBlock = stackPanel.Children.OfType<TextBlock>().FirstOrDefault();

                string text = textBlock.Text;


                // Highlight the original item
                Border border = FindVisualParent<Border>(checkBox);
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

                viewModel.SelectedCategories.Remove(text);
                viewModel.FilterRequestsByCategories();
            }
        }







        private T FindVisualParent<T>(DependencyObject child) where T : DependencyObject
        {
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);

            if (parentObject == null)
                return null;
            T parent = parentObject as T;
            if (parent != null)
            {
                return parent;
            }
            else
            {
                return FindVisualParent<T>(parentObject);
            }
        }


    }

}
