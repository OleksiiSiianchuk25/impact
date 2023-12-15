// <copyright file="MainWindow.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ImpactWPF
{
    using System.Windows;
    using System.Windows.Controls;
    using ImpactWPF.Pages;
    using NLog;

    /// <summary>
    /// Interaction logic for MainWindow.xaml.
    /// </summary>
    public partial class MainWindow : Window
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger();

        public MainWindow()
        {
            this.InitializeComponent();
            this.mainFrame.Navigate(new Animation());
            this.WindowState = WindowState.Maximized;
            this.anianimation.Play();

            // Встановлюємо обробник події завершення анімації
            this.anianimation.MediaEnded += this.MediaElement_MediaEnded;

            Logger.Info("Застосунок успішно запустився");
        }

        private void MediaElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            // Обробка події завершення анімації
            this.anianimation.Visibility = Visibility.Collapsed;

            this.mainFrame.Navigate(new LoginPage());
        }

        public void NavigateToPage(Page page)
        {
            this.mainFrame.Navigate(page);
        }
    }
}