using EfCore.entity;
using ImpactWPF.Pages;
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

namespace ImpactWPF.Controls
{
    /// <summary>
    /// Interaction logic for ArchiveCardControl2.xaml
    /// </summary>
    public partial class ArchiveCardControl2 : UserControl
    {
        public ArchiveCardControl2()
        {
            InitializeComponent();
            activateImage.MouseLeftButtonDown += ActivateImage_MouseLeftButtonDown;
            editImage.MouseLeftButtonDown += EditImage_MouseLeftButtonDown;
        }
        public event EventHandler ActivateButtonClicked;

        protected virtual void OnActivateButtonClicked(EventArgs e)
        {
            ActivateButtonClicked?.Invoke(this, e);
        }

        public static readonly DependencyProperty ArchiveRequestProperty =
        DependencyProperty.Register("ArchiveRequestG", typeof(Request), typeof(ArchiveCardControl2));

        public Request ArchiveRequestG
        {
            get { return (Request)GetValue(ArchiveRequestProperty); }
            set { SetValue(ArchiveRequestProperty, value); }
        }


        private void ActivateImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            FrameworkElement parent = this;
            while (parent != null && !(parent is AtchivePage))
            {
                parent = VisualTreeHelper.GetParent(parent) as FrameworkElement;
            }

            if (parent is AtchivePage archivePage && ArchiveRequestG != null)
            {
                archivePage.ShowActivateGrid(ArchiveRequestG);

            }
        }

        private void EditImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            FrameworkElement parent = this;
            while (parent != null && !(parent is AtchivePage))
            {
                parent = VisualTreeHelper.GetParent(parent) as FrameworkElement;
            }

            if (parent is AtchivePage archivePage && ArchiveRequestG != null)
            {
                archivePage.EditRequestPage(ArchiveRequestG);
            }
        }
    }

}
