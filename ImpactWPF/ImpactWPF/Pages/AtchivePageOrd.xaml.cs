// <copyright file="AtchivePageOrd.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ImpactWPF.Pages
{
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
    using EfCore.context;
    using EfCore.entity;
    using EfCore.service.impl;
    using ImpactWPF.View;
    using NLog;

    /// <summary>
    /// Interaction logic for AtchivePageOrd.xaml.
    /// </summary>
    public partial class AtchivePageOrd : Page
    {
        private readonly ArchivePageViewModelOrd archiveViewModel;
        private readonly RequestServiceImpl requestService;
        private readonly ImpactDbContext dbContext;
        private static Logger Logger = LogManager.GetCurrentClassLogger();

        public AtchivePageOrd()
        {
            this.InitializeComponent();

            Logger.Info("Сторінка архіву із замовленнями користувача успішно ініціалізована");

            SetButtonStyles(this.ArchiveOrderButton);

            this.dbContext = new ImpactDbContext();
            this.requestService = new RequestServiceImpl(this.dbContext);

            this.archiveViewModel = new ArchivePageViewModelOrd(this);
            this.archiveViewModel.LoadArchiveRequests();
            this.DataContext = this.archiveViewModel;
        }

        private void ArchiveButton_Click(object sender, RoutedEventArgs e)
        {
            // Отримання кнопки, яка була натиснута
            var clickedButton = (Button)sender;

            // Зміна стилів для обраної кнопки
            SetButtonStyles(clickedButton);

            // Зміна стилів для іншої кнопки
            var otherButton = (clickedButton == this.ArchivePropositionButton) ? this.ArchiveOrderButton : this.ArchivePropositionButton;
            ClearButtonStyles(otherButton);

            Logger.Info("Користувач перейшов на сторінку архіву з пропозиціями користувача");
            this.NavigationService?.Navigate(new AtchivePage());
        }

        private void ArchiveButton2_Click(object sender, RoutedEventArgs e)
        {
            // Отримання кнопки, яка була натиснута
            var clickedButton = (Button)sender;

            // Зміна стилів для обраної кнопки
            SetButtonStyles(clickedButton);

            // Зміна стилів для іншої кнопки
            var otherButton = (clickedButton == this.ArchivePropositionButton) ? this.ArchiveOrderButton : this.ArchivePropositionButton;
            ClearButtonStyles(otherButton);

            Logger.Info("Користувач перейшов на сторінку архіву із замовленнями користувача");
            this.NavigationService?.Navigate(new AtchivePageOrd());
        }

        private static void SetButtonStyles(Button button)
        {
            // Use the Color structure from System.Windows.Media namespace
            System.Windows.Media.Color color = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#002861");

            button.Background = new SolidColorBrush(color);
            button.Foreground = Brushes.White;

            Logger.Info("Стиль кнопки успішно змінений");
        }

        private static void ClearButtonStyles(Button button)
        {
            button.ClearValue(Button.BackgroundProperty);
            button.ClearValue(Button.BorderBrushProperty);
            button.ClearValue(Button.BorderThicknessProperty);
            button.Foreground = Brushes.Black; // Повертаємо колір тексту на чорний
            button.ClearValue(Button.EffectProperty);
        }

        private void ArchiveFilterButtonClick(object sender, RoutedEventArgs e)
        {
            if (this.myArchiveFilterGrid.Visibility == Visibility.Collapsed)
            {
                this.myArchiveFilterGrid.Visibility = Visibility.Visible;
                Logger.Info("Користувач відкрив вікно фільтрів");
            }
            else
            {
                this.myArchiveFilterGrid.Visibility = Visibility.Collapsed;
                Logger.Info("Користувач закрив вікно фільтрів");
            }
        }

        private void CloseFilter(object sender, MouseButtonEventArgs e)
        {
            // Toggle the visibility of the Rectangle
            if (this.myArchiveFilterGrid.Visibility == Visibility.Collapsed)
            {
                this.myArchiveFilterGrid.Visibility = Visibility.Visible;
                Logger.Info("Користувач відкрив вікно фільтрів");
            }
            else
            {
                this.myArchiveFilterGrid.Visibility = Visibility.Collapsed;
                Logger.Info("Користувач закрив вікно фільтрів");
            }
        }

        private void ClearDateButtonClick(object sender, MouseButtonEventArgs e)
        {
            Logger.Info("Початок процесу очищення фільтрів");

            this.SetButtonStyles(this.button2, false);
            this.archiveViewModel.IsDeactivatedFilter = false;

            this.SetButtonStyles(this.button1, false);
            this.archiveViewModel.IsActivatedFilter = false;

            this.archiveViewModel.OnSelectedFromDateChanged(DateTime.MinValue);
            this.selectedDateTextBlock.Text = "від: 00/00/0000";
            this.selectedDateTextBlock.Foreground = Brushes.Gray; // Змінити колір тексту на сірий

            this.archiveViewModel.OnSelectedToDateChanged(DateTime.MaxValue);
            this.selectedDateTextBlock2.Text = "до: 00/00/0000";
            this.selectedDateTextBlock2.Foreground = Brushes.Gray; // Змінити колір тексту на сірий

            this.archiveViewModel.LoadArchiveRequests();

            Logger.Info("Значення фільтрів успішно очищенні");
        }

        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            this.SetButtonStyles(this.button1, true);
            this.archiveViewModel.IsActivatedFilter = true;

            Logger.Info("Користувач обрав фільтр \"активовані\"");

            this.SetButtonStyles(this.button2, false);
            this.archiveViewModel.IsDeactivatedFilter = false;
        }

        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            this.SetButtonStyles(this.button2, true);
            this.archiveViewModel.IsDeactivatedFilter = true;

            Logger.Info("Користувач обрав фільтр \"деактивовані\"");

            this.SetButtonStyles(this.button1, false);
            this.archiveViewModel.IsActivatedFilter = false;
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

        private void myImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (this.isCalendar2Visible)
            {
                this.myCalendar2.Visibility = Visibility.Collapsed;
                this.isCalendar2Visible = false;
            }

            this.myCalendar.Visibility = this.myCalendar.Visibility == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed;
            this.isCalendar1Visible = !this.isCalendar1Visible;

            Logger.Info("Користувач відкрив/закрив календар");
        }

        private void myCalendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.myCalendar.SelectedDate.HasValue)
            {
                this.archiveViewModel.OnSelectedFromDateChanged(this.myCalendar.SelectedDate.Value);
                this.selectedDateTextBlock.Text = $"від: {this.myCalendar.SelectedDate.Value.ToString("dd/MM/yyyy")}";
                this.selectedDateTextBlock.Foreground = Brushes.Black; // Змінити колір тексту на чорний
                Logger.Info("Користувач обрав від якої дати фільтрувати");
            }
            else
            {
                this.archiveViewModel.OnSelectedFromDateChanged(DateTime.MinValue);
                this.selectedDateTextBlock.Text = "від: 00/00/0000";
                this.selectedDateTextBlock.Foreground = Brushes.Gray; // Змінити колір тексту на сірий
                Logger.Info("Обрана стандартне значення дати від якої фільтрувати");
            }
        }

        private void myImage_MouseLeftButtonDown2(object sender, MouseButtonEventArgs e)
        {
            if (this.isCalendar1Visible)
            {
                this.myCalendar.Visibility = Visibility.Collapsed;
                this.isCalendar1Visible = false;
            }

            this.myCalendar2.Visibility = this.myCalendar2.Visibility == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed;
            this.isCalendar2Visible = !this.isCalendar2Visible;

            Logger.Info("Користувач відкрив/закрив календар");
        }

        private void myCalendar_SelectedDatesChanged2(object sender, SelectionChangedEventArgs e)
        {
            if (this.myCalendar2.SelectedDate.HasValue)
            {
                this.archiveViewModel.OnSelectedToDateChanged(this.myCalendar2.SelectedDate.Value);
                this.selectedDateTextBlock2.Text = $"до: {this.myCalendar2.SelectedDate.Value:dd/MM/yyyy}";
                this.selectedDateTextBlock2.Foreground = Brushes.Black; // Змінити колір тексту на чорний
                Logger.Info("Користувач обрав до якої дати фільтрувати");
            }
            else
            {
                this.archiveViewModel.OnSelectedToDateChanged(DateTime.MaxValue);
                this.selectedDateTextBlock2.Text = "до: 00/00/0000";
                this.selectedDateTextBlock2.Foreground = Brushes.Gray; // Змінити колір тексту на сірий
                Logger.Info("Обрана стандартне значення дати до якої фільтрувати");
            }
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

        public void ShowDeactivateGrid(Request request)
        {
            this.DeactivateGrid.Visibility = Visibility.Visible;

            Logger.Info("Користувач відкрив випливаюче вікно для деактивації пропозиції");

            this.YesButton.Click += (sender, e) => this.OnYesButtonClick(request);
            this.NoButton.Click += (sender, e) => this.OnNoButtonClick();
        }

        public void EditRequestPage(Request request)
        {
            if (request != null)
            {
                EditRequestArchive editRequest = new EditRequestArchive(request);
                Logger.Info("Користувача перейшов на сторінку для редагування замовлення");
                this.NavigationService.Navigate(editRequest);
            }
        }

        private void OnYesButtonClick(Request request)
        {
            this.requestService.ChangeRequestStatus(request.RequestId, 2);

            this.DeactivateGrid.Visibility = Visibility.Collapsed;
            Logger.Info($"Користувач деактивував замовлення: {request.RequestName}");

            this.NavigationService?.Navigate(new AtchivePage());
        }

        private void OnNoButtonClick()
        {
            Logger.Info("Користувач закрив випливаюче вікно для деактивації замовлення");
            this.DeactivateGrid.Visibility = Visibility.Collapsed;
        }

        private void ApplyFilterButton_Click(object sender, RoutedEventArgs e)
        {
            this.archiveViewModel.FilterData();
            Logger.Info("Користувач застосував фільтри");
            this.myArchiveFilterGrid.Visibility = Visibility.Collapsed;
        }

        public void ShowActivateGrid(Request request)
        {
            this.ActivateGrid.Visibility = Visibility.Visible;

            Logger.Info("Користувач відкрив випливаюче вікно для активації замовлення");

            this.YesButtonA.Click += (sender, e) => this.OnYesButtonAClick(request);
            this.NoButtonA.Click += (sender, e) => this.OnNoButtonAClick();
        }

        private void OnYesButtonAClick(Request request)
        {
            this.requestService.ChangeRequestStatus(request.RequestId, 1);

            this.ActivateGrid.Visibility = Visibility.Collapsed;
            Logger.Info($"Користувач активував пропозицію: {request.RequestName}");

            this.NavigationService?.Navigate(new AtchivePage());
        }

        private void OnNoButtonAClick()
        {
            Logger.Info("Користувач закрив випливаюче вікно для активації пропозиції");
            this.ActivateGrid.Visibility = Visibility.Collapsed;
        }

        private void HomePage_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            Logger.Info("Користувач перейшов на домашню сторінку");
            this.NavigationService?.Navigate(new HomePage());
        }

        private void CreateProposalPage_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            Logger.Info("Користувач перейшов на сторінку для створення нової пропозиції");
            this.NavigationService?.Navigate(new CreateProposalPage());
        }

        private void CreateOrderPage_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            Logger.Info("Користувач перейшов на сторінку для створення нового замовлення");
            this.NavigationService?.Navigate(new CreateOrderPage());
        }

        private void AdminPage_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            Logger.Info("Користувач перейшов на сторінку адміна з таблицею запитів");
            this.NavigationService?.Navigate(new AdminPage());
        }

        private void SupportPage_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            Logger.Info("Користувач перейшов на сторінку техпідтримки");
            this.NavigationService?.Navigate(new SupportPage());
        }
    }
}
