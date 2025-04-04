﻿using Material.Icons;
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
    /// Interaction logic for IconButton.xaml
    /// </summary>
    public partial class IconButton : Button
    {


        public MaterialIconKind MaterialIcon
        {
            get { return (MaterialIconKind)GetValue(MaterialIconProperty); }
            set { SetValue(MaterialIconProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MaterialIcon.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaterialIconProperty =
            DependencyProperty.Register("MaterialIcon", typeof(MaterialIconKind), typeof(IconButton), new PropertyMetadata(null));




        public IconButton()
        {
            InitializeComponent();
        }
    }
}
