using EfCore.entity;
using ImpactWPF.Pages;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ImpactWPF.Controls
{
    /// <summary>
    /// Interaction logic for ArchiveCardControl1Ord.xaml
    /// </summary>
    public partial class ArchiveCardControl1Ord : UserControl
    {
        public ArchiveCardControl1Ord()
        {
            InitializeComponent();
            deactivateImage.MouseLeftButtonDown += DeactivateImage_MouseLeftButtonDown;
            editImage.MouseLeftButtonDown += EditImage_MouseLeftButtonDown;
        }

        public static readonly DependencyProperty ArchiveOrderProperty =
        DependencyProperty.Register("ArchiveOrder", typeof(Request), typeof(ArchiveCardControl1Ord));

        public Request ArchiveOrder
        {
            get { return (Request)GetValue(ArchiveOrderProperty); }
            set { SetValue(ArchiveOrderProperty, value); }
        }

        public event EventHandler DeactivateButtonClicked;

        protected virtual void OnDeactivateButtonClicked(EventArgs e)
        {
            DeactivateButtonClicked?.Invoke(this, e);
        }

        private void DeactivateImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            FrameworkElement parent = this;
            while (parent != null && !(parent is AtchivePageOrd))
            {
                parent = VisualTreeHelper.GetParent(parent) as FrameworkElement;
            }

            if (parent is AtchivePageOrd archivePage && ArchiveOrder != null)
            {
                archivePage.ShowDeactivateGrid(ArchiveOrder);
            }
        }

        private void EditImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            FrameworkElement parent = this;
            while (parent != null && !(parent is AtchivePageOrd))
            {
                parent = VisualTreeHelper.GetParent(parent) as FrameworkElement;
            }

            if (parent is AtchivePageOrd archivePage && ArchiveOrder != null)
            {
                archivePage.EditRequestPage(ArchiveOrder);
            }
        }
    }
}
