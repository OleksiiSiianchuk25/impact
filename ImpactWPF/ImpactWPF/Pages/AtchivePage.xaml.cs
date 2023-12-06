using ImpactWPF.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
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
    /// Interaction logic for AtchivePage.xaml
    /// </summary>
    public partial class AtchivePage : Page
    {
        public AtchivePage()
        {
            InitializeComponent();
           
        }

        private void ArchiveButton_Click(object sender, RoutedEventArgs e)
        {
            // Отримання кнопки, яка була натиснута
            var clickedButton = (Button)sender;

            // Зміна стилів для обраної кнопки
            SetButtonStyles(clickedButton);

            // Зміна стилів для іншої кнопки
            var otherButton = (clickedButton == ArchivePropositionButton) ? ArchiveOrderButton : ArchivePropositionButton;
            ClearButtonStyles(otherButton);
        }
        private void SetButtonStyles(Button button)
        {
            // Use the Color structure from System.Windows.Media namespace
            System.Windows.Media.Color color = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#002861");

            button.Background = new SolidColorBrush(color);
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


        private void ArchiveFilterButtonClick(object sender, RoutedEventArgs e)
        {
            if (myArchiveFilterGrid.Visibility == Visibility.Collapsed)
            {
                myArchiveFilterGrid.Visibility = Visibility.Visible;
            }
            else
            {
                myArchiveFilterGrid.Visibility = Visibility.Collapsed;
            }
          }

        private void CloseFilter(object sender, MouseButtonEventArgs e)
        {
            // Toggle the visibility of the Rectangle
            if (myArchiveFilterGrid.Visibility == Visibility.Collapsed)
            {
                myArchiveFilterGrid.Visibility = Visibility.Visible;
            }
            else
            {
                myArchiveFilterGrid.Visibility = Visibility.Collapsed;
            }
        }


        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            // Змінюємо вигляд кнопки 1
            SetButtonStyles(button1, true);

            // Повертаємо вигляд кнопки 2 до початкового стану
            SetButtonStyles(button2, false);
        }

        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            // Змінюємо вигляд кнопки 2
            SetButtonStyles(button2, true);

            // Повертаємо вигляд кнопки 1 до початкового стану
            SetButtonStyles(button1, false);
        }

        private void SetButtonStyles(Button button, bool isSelected)
        {
            if (isSelected)
            {
                System.Windows.Media.Color color = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FFE883");

                button.Background = new SolidColorBrush(color);
                button.Foreground = Brushes.Black;

            }
            else
            {
                System.Windows.Media.Color color = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FFE883");

                button.Background = Brushes.Transparent;
                button.BorderBrush = new SolidColorBrush(color);
            }

            // Додавання заокруглення
           
        }


        private bool isCalendar1Visible = false;
        private bool isCalendar2Visible = false;

        private void HideAllCalendars()
        {
            myCalendar.Visibility = Visibility.Collapsed;
            myCalendar2.Visibility = Visibility.Collapsed;
            isCalendar1Visible = false;
            isCalendar2Visible = false;
        }

        private void myImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (isCalendar2Visible)
            {
                myCalendar2.Visibility = Visibility.Collapsed;
                isCalendar2Visible = false;
            }

            myCalendar.Visibility = myCalendar.Visibility == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed;
            isCalendar1Visible = !isCalendar1Visible;
        }

        private void myCalendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            if (myCalendar.SelectedDate.HasValue)
            {
                selectedDateTextBlock.Text = $"від: {myCalendar.SelectedDate.Value.ToString("dd/MM/yyyy")}";
                selectedDateTextBlock.Foreground = Brushes.Black; // Змінити колір тексту на чорний
            }
            else
            {
                selectedDateTextBlock.Text = "від: 00/00/0000";
                selectedDateTextBlock.Foreground = Brushes.Gray; // Змінити колір тексту на сірий
            }
        }

        private void myImage_MouseLeftButtonDown2(object sender, MouseButtonEventArgs e)
        {
            if (isCalendar1Visible)
            {
                myCalendar.Visibility = Visibility.Collapsed;
                isCalendar1Visible = false;
            }

            myCalendar2.Visibility = myCalendar2.Visibility == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed;
            isCalendar2Visible = !isCalendar2Visible;
        }

        private void myCalendar_SelectedDatesChanged2(object sender, SelectionChangedEventArgs e)
        {
            if (myCalendar2.SelectedDate.HasValue)
            {
                selectedDateTextBlock2.Text = $"до: {myCalendar2.SelectedDate.Value.ToString("dd/MM/yyyy")}";
                selectedDateTextBlock2.Foreground = Brushes.Black; // Змінити колір тексту на чорний
            }
            else
            {
                selectedDateTextBlock2.Text = "до: 00/00/0000";
                selectedDateTextBlock2.Foreground = Brushes.Gray; // Змінити колір тексту на сірий
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

        public void ShowDeactivateGrid()
        {
            DeactivateGrid.Visibility = Visibility.Visible;
           
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
