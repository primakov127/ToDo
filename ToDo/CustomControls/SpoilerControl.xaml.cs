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

namespace ToDo.CustomControls
{
    /// <summary>
    /// Логика взаимодействия для SpoilerControl.xaml
    /// </summary>
    public partial class SpoilerControl : UserControl
    {


        public object Secret
        {
            get { return (object)GetValue(SecretProperty); }
            set { SetValue(SecretProperty, value); }
        }

        public static readonly DependencyProperty SecretProperty =
            DependencyProperty.Register("Secret", typeof(object), typeof(SpoilerControl), new PropertyMetadata(0));



        public SpoilerControl()
        {
            InitializeComponent();
        }

        private void Spoiler_Click(object sender, RoutedEventArgs e)
        {
            if (spoilerGrid.Visibility == Visibility.Visible)
            {
                contentGrid.Visibility = Visibility.Visible;
                spoilerGrid.Visibility = Visibility.Collapsed;
            }
            else
            {
                contentGrid.Visibility = Visibility.Collapsed;
                spoilerGrid.Visibility = Visibility.Visible;
            }
        }
    }
}
