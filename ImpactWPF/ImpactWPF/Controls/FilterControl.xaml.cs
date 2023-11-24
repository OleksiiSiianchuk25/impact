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
    /// Interaction logic for FilterControl.xaml
    /// </summary>
    public partial class FilterControl : UserControl
    {
        public FilterControl()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty ButtonImageSourceProperty =
            DependencyProperty.Register("ButtonImageSource", typeof(ImageSource), typeof(FilterControl));

        public ImageSource ButtonImageSource
        {
            get { return (ImageSource)GetValue(ButtonImageSourceProperty); }
            set { SetValue(ButtonImageSourceProperty, value); }
        }

        public static readonly DependencyProperty PlaceholderProperty =
            DependencyProperty.Register("Placeholder", typeof(string), typeof(FilterControl));

        public string Placeholder
        {
            get { return (string)GetValue(PlaceholderProperty); }
            set { SetValue(PlaceholderProperty, value); }
        }



        private void ImageButton_Checked(object sender, RoutedEventArgs e)
    {
        System.Windows.Media.Color newColor = System.Windows.Media.Color.FromRgb(0xFF, 0xE8, 0x83);
        Filter.Background = new SolidColorBrush(newColor);
        Filter.BorderBrush = new SolidColorBrush(newColor);

        double centerX = ButtonImage.ActualWidth / 2;
        double centerY = ButtonImage.ActualHeight / 2;

        // Rotate the image by 45 degrees around its center
        ((RotateTransform)ButtonImage.RenderTransform).CenterX = centerX;
        ((RotateTransform)ButtonImage.RenderTransform).CenterY = centerY;
        ((RotateTransform)ButtonImage.RenderTransform).Angle = 45;
    }


    private void ImageButton_Unchecked(object sender, RoutedEventArgs e)
    {
        System.Windows.Media.Color newColor = System.Windows.Media.Color.FromRgb(0xFF, 0xE8, 0x83);
        Filter.Background = Brushes.White;
        Filter.BorderBrush = new SolidColorBrush(newColor);

        // Reset the rotation when unchecked
        ((RotateTransform)ButtonImage.RenderTransform).Angle = 0;
    }

      
    }


    

}
