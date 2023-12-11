using EfCore.entity;
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
    /// Interaction logic for HomeCardControl.xaml
    /// </summary>
    public partial class HomeCardControl : UserControl
    {
        public HomeCardControl()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty RequestProperty =
            DependencyProperty.Register("Request", typeof(Request), typeof(HomeCardControl));

        public Request Request
        {
            get { return (Request)GetValue(RequestProperty); }
            set { SetValue(RequestProperty, value); }
        }

    }
}
