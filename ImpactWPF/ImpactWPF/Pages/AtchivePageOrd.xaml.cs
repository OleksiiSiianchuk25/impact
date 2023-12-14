using EfCore.context;
using EfCore.entity;
using EfCore.service.impl;
using ImpactWPF.View;
using NLog;
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
    /// Interaction logic for AtchivePageOrd.xaml
    /// </summary>
    public partial class AtchivePageOrd : Page
    {
        private readonly ArchivePageViewModelOrd archiveViewModel;
        private readonly RequestServiceImpl requestService;
        private readonly ImpactDbContext dbContext;
        private static Logger Logger = LogManager.GetCurrentClassLogger();

        public AtchivePageOrd()
        {
            InitializeComponent();

            Logger.Info("Сторінка архіву із замовленнями користувача успішно ініціалізована");

            SetButtonStyles(ArchiveOrderButton);

            dbContext = new ImpactDbContext();
            requestService = new RequestServiceImpl(dbContext);

            archiveViewModel = new ArchivePageViewModelOrd(this);
            archiveViewModel.LoadArchiveRequests();
            DataContext = archiveViewModel;

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

            Logger.Info("Користувач перейшов на сторінку архіву з пропозиціями користувача");
            NavigationService?.Navigate(new AtchivePage());
        }

        private void ArchiveButton2_Click(object sender, RoutedEventArgs e)
        {
            // Отримання кнопки, яка була натиснута
            var clickedButton = (Button)sender;

            // Зміна стилів для обраної кнопки
            SetButtonStyles(clickedButton);

            // Зміна стилів для іншої кнопки
            var otherButton = (clickedButton == ArchivePropositionButton) ? ArchiveOrderButton : ArchivePropositionButton;
            ClearButtonStyles(otherButton);

            Logger.Info("Користувач перейшов на сторінку архіву із замовленнями користувача");
            NavigationService?.Navigate(new AtchivePageOrd());
        }

        private void SetButtonStyles(Button button)
        {
            // Use the Color structure from System.Windows.Media namespace
            System.Windows.Media.Color color = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#002861");

            button.Background = new SolidColorBrush(color);
            button.Foreground = Brushes.White;

            Logger.Info("Стиль кнопки успішно змінений");
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
                Logger.Info("Користувач відкрив вікно фільтрів");
            }
            else
            {
                myArchiveFilterGrid.Visibility = Visibility.Collapsed;
                Logger.Info("Користувач закрив вікно фільтрів");
            }
        }

        private void CloseFilter(object sender, MouseButtonEventArgs e)
        {
            // Toggle the visibility of the Rectangle
            if (myArchiveFilterGrid.Visibility == Visibility.Collapsed)
            {
                myArchiveFilterGrid.Visibility = Visibility.Visible;
                Logger.Info("Користувач відкрив вікно фільтрів");
            }
            else
            {
                myArchiveFilterGrid.Visibility = Visibility.Collapsed;
                Logger.Info("Користувач закрив вікно фільтрів");
            }
        }

        private void ClearDateButtonClick(object sender, MouseButtonEventArgs e)
        {
            Logger.Info("Початок процесу очищення фільтрів");

            SetButtonStyles(button2, false);
            archiveViewModel.IsDeactivatedFilter = false;

            SetButtonStyles(button1, false);
            archiveViewModel.IsActivatedFilter = false;

            archiveViewModel.OnSelectedFromDateChanged(DateTime.MinValue);
            selectedDateTextBlock.Text = "від: 00/00/0000";
            selectedDateTextBlock.Foreground = Brushes.Gray; // Змінити колір тексту на сірий

            archiveViewModel.OnSelectedToDateChanged(DateTime.MaxValue);
            selectedDateTextBlock2.Text = "до: 00/00/0000";
            selectedDateTextBlock2.Foreground = Brushes.Gray; // Змінити колір тексту на сірий

            archiveViewModel.LoadArchiveRequests();

            Logger.Info("Значення фільтрів успішно очищенні");
        }


        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            SetButtonStyles(button1, true);
            archiveViewModel.IsActivatedFilter = true;

            Logger.Info("Користувач обрав фільтр \"активовані\"");

            SetButtonStyles(button2, false);
            archiveViewModel.IsDeactivatedFilter = false;
        }

        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            SetButtonStyles(button2, true);
            archiveViewModel.IsDeactivatedFilter = true;

            Logger.Info("Користувач обрав фільтр \"деактивовані\"");

            SetButtonStyles(button1, false);
            archiveViewModel.IsActivatedFilter = false;
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

            Logger.Info("Користувач відкрив/закрив календар");
        }

        private void myCalendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            if (myCalendar.SelectedDate.HasValue)
            {
                archiveViewModel.OnSelectedFromDateChanged(myCalendar.SelectedDate.Value);
                selectedDateTextBlock.Text = $"від: {myCalendar.SelectedDate.Value.ToString("dd/MM/yyyy")}";
                selectedDateTextBlock.Foreground = Brushes.Black; // Змінити колір тексту на чорний
                Logger.Info("Користувач обрав від якої дати фільтрувати");
            }
            else
            {
                archiveViewModel.OnSelectedFromDateChanged(DateTime.MinValue);
                selectedDateTextBlock.Text = "від: 00/00/0000";
                selectedDateTextBlock.Foreground = Brushes.Gray; // Змінити колір тексту на сірий
                Logger.Info("Обрана стандартне значення дати від якої фільтрувати");
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

            Logger.Info("Користувач відкрив/закрив календар");
        }

        private void myCalendar_SelectedDatesChanged2(object sender, SelectionChangedEventArgs e)
        {
            if (myCalendar2.SelectedDate.HasValue)
            {
                archiveViewModel.OnSelectedToDateChanged(myCalendar2.SelectedDate.Value);
                selectedDateTextBlock2.Text = $"до: {myCalendar2.SelectedDate.Value.ToString("dd/MM/yyyy")}";
                selectedDateTextBlock2.Foreground = Brushes.Black; // Змінити колір тексту на чорний
                Logger.Info("Користувач обрав до якої дати фільтрувати");
            }
            else
            {
                archiveViewModel.OnSelectedToDateChanged(DateTime.MaxValue);
                selectedDateTextBlock2.Text = "до: 00/00/0000";
                selectedDateTextBlock2.Foreground = Brushes.Gray; // Змінити колір тексту на сірий
                Logger.Info("Обрана стандартне значення дати до якої фільтрувати");
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

        public void ShowDeactivateGrid(Request request)
        {
            DeactivateGrid.Visibility = Visibility.Visible;

            Logger.Info("Користувач відкрив випливаюче вікно для деактивації пропозиції");

            YesButton.Click += (sender, e) => OnYesButtonClick(request);
            NoButton.Click += (sender, e) => OnNoButtonClick();
        }

        public void EditRequestPage(Request request)
        {
            if (request != null)
            {
                EditRequestArchive editRequest = new EditRequestArchive(request);
                Logger.Info("Користувача перейшов на сторінку для редагування замовлення");
                NavigationService.Navigate(editRequest);
            }
        }

        private void OnYesButtonClick(Request request)
        {
            requestService.ChangeRequestStatus(request.RequestId, 2);

            DeactivateGrid.Visibility = Visibility.Collapsed;
            Logger.Info($"Користувач деактивував замовлення: {request.RequestName}");

            NavigationService?.Navigate(new AtchivePage());
        }

        private void OnNoButtonClick()
        {
            Logger.Info("Користувач закрив випливаюче вікно для деактивації замовлення");
            DeactivateGrid.Visibility = Visibility.Collapsed;
        }

        private void ApplyFilterButton_Click(object sender, RoutedEventArgs e)
        {
            archiveViewModel.FilterData();
            Logger.Info("Користувач застосував фільтри");
            myArchiveFilterGrid.Visibility = Visibility.Collapsed;
        }


        public void ShowActivateGrid(Request request)
        {
            ActivateGrid.Visibility = Visibility.Visible;

            Logger.Info("Користувач відкрив випливаюче вікно для активації замовлення");

            YesButtonA.Click += (sender, e) => OnYesButtonAClick(request);
            NoButtonA.Click += (sender, e) => OnNoButtonAClick();
        }

        private void OnYesButtonAClick(Request request)
        {
            requestService.ChangeRequestStatus(request.RequestId, 1);

            ActivateGrid.Visibility = Visibility.Collapsed;
            Logger.Info($"Користувач активував пропозицію: {request.RequestName}");

            NavigationService?.Navigate(new AtchivePage());
        }

        private void OnNoButtonAClick()
        {
            Logger.Info("Користувач закрив випливаюче вікно для активації пропозиції");
            ActivateGrid.Visibility = Visibility.Collapsed;
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
    }
}
