using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ImpactWPF.View.UserControls
{
    /// <summary>
    /// Interaction logic for CustomTextBox.xaml
    /// </summary>
    public partial class CustomTextBox : UserControl
    {
        public CustomTextBox()
        {
            InitializeComponent();
            tbInput.TextChanged += TbInput_TextChanged;
        }

        private void TbInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            tbPlaceHolder.Visibility = string.IsNullOrEmpty(tbInput.Text) ? Visibility.Visible : Visibility.Hidden;
        }

        private void OnTextBoxGotFocus(object sender, RoutedEventArgs e)
        {
            tbPlaceHolder.Visibility = Visibility.Collapsed;
        }

        private string placeholder;

        public string Placeholder
        {
            get { return placeholder; }
            set
            {
                placeholder = value;
                tbPlaceHolder.Text = placeholder;
            }
        }

        public ImageSource ImageSource
        {
            get { return tbImage.Source; }
            set { tbImage.Source = value; }
        }

        public double ImageWidth
        {
            get { return tbImage.Width; }
            set { tbImage.Width = value; }
        }

        public double ImageHeight
        {
            get { return tbImage.Height; }
            set { tbImage.Height = value; }
        }

        public HorizontalAlignment ImageHorizontalAlignment
        {
            get { return tbImage.HorizontalAlignment; }
            set { tbImage.HorizontalAlignment = value; }
        }

        public VerticalAlignment ImageVerticalAlignment
        {
            get { return tbImage.VerticalAlignment; }
            set { tbImage.VerticalAlignment = value; }
        }

        public Thickness ImageMargin
        {
            get { return tbImage.Margin; }
            set { tbImage.Margin = value; }
        }

        public Thickness PlaceholderMargin
        {
            get { return tbPlaceHolder.Margin; }
            set { tbPlaceHolder.Margin = value; }
        }

        public Thickness InputMargin
        {
            get { return tbInput.Margin; }
            set { tbInput.Margin = value; }
        }
    }
}
