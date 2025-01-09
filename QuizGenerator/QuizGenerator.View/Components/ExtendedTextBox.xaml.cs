using Material.Icons;
using Material.Icons.WPF;
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

namespace QuizGenerator.View.Components
{
	/// <summary>
	/// Interaction logic for ExtendedTextBox.xaml
	/// </summary>
	public partial class ExtendedTextBox : TextBox
    {

        public bool IconSeparator
        {
            get { return (bool)GetValue(IconSeparatorProperty); }
            set { SetValue(IconSeparatorProperty, value); }
        }

        public static readonly DependencyProperty IconSeparatorProperty =
            DependencyProperty.Register("IconSeparator", typeof(bool), typeof(ExtendedTextBox), new PropertyMetadata(false));


        public MaterialIconKind? MaterialIcon
        {
            get { return (MaterialIconKind?)GetValue(MaterialIconProperty); }
            set { SetValue(MaterialIconProperty, value); }
        }

        public static readonly DependencyProperty MaterialIconProperty =
            DependencyProperty.Register("MaterialIcon", typeof(MaterialIconKind?), typeof(ExtendedTextBox), new PropertyMetadata(null));


        public string Placeholder
        {
            get { return (string)GetValue(PlaceholderProperty); }
            set { SetValue(PlaceholderProperty, value); }
        }

        public static readonly DependencyProperty PlaceholderProperty =
            DependencyProperty.Register("Placeholder", typeof(string), typeof(ExtendedTextBox), new PropertyMetadata(string.Empty));


        public ExtendedTextBox()
        {
            InitializeComponent();
        }
    }
}
