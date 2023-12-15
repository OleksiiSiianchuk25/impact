// <copyright file="ArchiveCardControl1Ord.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ImpactWPF.Controls
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;
    using EfCore.entity;
    using ImpactWPF.Pages;

    /// <summary>
    /// Interaction logic for ArchiveCardControl1Ord.xaml
    /// </summary>
    public partial class ArchiveCardControl1Ord : UserControl
    {
        public ArchiveCardControl1Ord()
        {
            this.InitializeComponent();
            this.deactivateImage.MouseLeftButtonDown += this.DeactivateImage_MouseLeftButtonDown;
            this.editImage.MouseLeftButtonDown += this.EditImage_MouseLeftButtonDown;
        }

        public static readonly DependencyProperty ArchiveOrderProperty =
        DependencyProperty.Register("ArchiveOrder", typeof(Request), typeof(ArchiveCardControl1Ord));

        public Request ArchiveOrder
        {
            get { return (Request)this.GetValue(ArchiveOrderProperty); }
            set { this.SetValue(ArchiveOrderProperty, value); }
        }

        public event EventHandler DeactivateButtonClicked;

        protected virtual void OnDeactivateButtonClicked(EventArgs e)
        {
            this.DeactivateButtonClicked?.Invoke(this, e);
        }

        private void DeactivateImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            FrameworkElement parent = this;
            while (parent != null && !(parent is AtchivePageOrd))
            {
                parent = VisualTreeHelper.GetParent(parent) as FrameworkElement;
            }

            if (parent is AtchivePageOrd archivePage && this.ArchiveOrder != null)
            {
                archivePage.ShowDeactivateGrid(this.ArchiveOrder);
            }
        }

        private void EditImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            FrameworkElement parent = this;
            while (parent != null && !(parent is AtchivePageOrd))
            {
                parent = VisualTreeHelper.GetParent(parent) as FrameworkElement;
            }

            if (parent is AtchivePageOrd archivePage && this.ArchiveOrder != null)
            {
                archivePage.EditRequestPage(this.ArchiveOrder);
            }
        }
    }
}
