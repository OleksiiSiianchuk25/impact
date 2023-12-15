// <copyright file="ArchiveCardControl1.xaml.cs" company="PlaceholderCompany">
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
    /// Interaction logic for ArchiveCardControl1.xaml.
    /// </summary>
    public partial class ArchiveCardControl1 : UserControl
    {
        public ArchiveCardControl1()
        {
            this.InitializeComponent();
            this.deactivateImage.MouseLeftButtonDown += this.DeactivateImage_MouseLeftButtonDown;
            this.editImage.MouseLeftButtonDown += this.EditImage_MouseLeftButtonDown;
        }

        /// <summary>
        /// Gets or sets the archive request associated with the ArchiveCardControl1.
        /// </summary>
        public Request ArchiveRequest
        {
            get { return (Request)this.GetValue(ArchiveCardControl1Helpers.ArchiveRequestProperty); }
            set { this.SetValue(ArchiveCardControl1Helpers.ArchiveRequestProperty, value); }
        }

        public event EventHandler DeactivateButtonClicked = (sender, e) => { };

        protected virtual void OnDeactivateButtonClicked(EventArgs e)
        {
            this.DeactivateButtonClicked?.Invoke(this, e);
        }

        private void DeactivateImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            FrameworkElement parent = this;
            while (parent != null && !(parent is AtchivePage))
            {
                parent = VisualTreeHelper.GetParent(parent) as FrameworkElement;
            }

            if (parent is AtchivePage archivePage && this.ArchiveRequest != null)
            {
                archivePage.ShowDeactivateGrid(this.ArchiveRequest);
            }
        }

        private void EditImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            FrameworkElement parent = this;
            while (parent != null && !(parent is AtchivePage))
            {
                parent = VisualTreeHelper.GetParent(parent) as FrameworkElement;
            }

            if (parent is AtchivePage archivePage && this.ArchiveRequest != null)
            {
                archivePage.EditRequestPage(this.ArchiveRequest);
            }
        }
    }
}
