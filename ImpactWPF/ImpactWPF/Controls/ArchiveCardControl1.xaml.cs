using ImpactWPF.Pages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Interaction logic for ArchiveCardControl1.xaml
    /// </summary>
    public partial class ArchiveCardControl1 : UserControl
    {
        public ArchiveCardControl1()
        {
            InitializeComponent();
            deactivateImage.MouseLeftButtonDown += DeactivateImage_MouseLeftButtonDown;     
        }


        public event EventHandler DeactivateButtonClicked;

        protected virtual void OnDeactivateButtonClicked(EventArgs e)
        {
            DeactivateButtonClicked?.Invoke(this, e);
        }


        private void DeactivateImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            FrameworkElement parent = this;
            while (parent != null && !(parent is AtchivePage))
            {
                parent = VisualTreeHelper.GetParent(parent) as FrameworkElement;
            }

            if (parent is AtchivePage archivePage)
            {
                archivePage.ShowDeactivateGrid();

            }
        }

        public void SetDeactivatedState()
        {
            rectangle.Fill = new SolidColorBrush(Colors.Gray);
            deactivateImage.Source = new BitmapImage(new Uri("/images/activate.png", UriKind.Relative));
        }



    }
}
