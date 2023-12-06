using ImpactWPF.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage : Page
    {
        public HomePage()
        {
            InitializeComponent();

            // Встановлення по замовчуванні для кнопки "Пропозиції"
            SetButtonStyles(PropositionButton);
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Отримання кнопки, яка була натиснута
            var clickedButton = (Button)sender;

            // Зміна стилів для обраної кнопки
            SetButtonStyles(clickedButton);

            // Зміна стилів для іншої кнопки
            var otherButton = (clickedButton == PropositionButton) ? OrderButton : PropositionButton;
            ClearButtonStyles(otherButton);
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
            // Your event handler logic goes here
        }





        private void searchImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (myGrid.Visibility == Visibility.Collapsed)
            {
                myGrid.Visibility = Visibility.Visible;
            }
            else
            {
                myGrid.Visibility = Visibility.Collapsed;
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
    }
}
