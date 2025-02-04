using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QuizGenerator.View.Components
{
    /// <summary>
    /// Interaction logic for SmoothProgressBar.xaml
    /// </summary>
    public partial class SmoothProgressBar : ProgressBar
    {
		public bool ShowValue
		{
			get { return (bool)GetValue(ShowValueProperty); }
			set { SetValue(ShowValueProperty, value); }
		}

		public static readonly DependencyProperty ShowValueProperty =
			DependencyProperty.Register("ShowValue", typeof(bool), typeof(SmoothProgressBar), new PropertyMetadata(false));


		public CornerRadius CornerRadius
		{
			get { return (CornerRadius)GetValue(CornerRadiusProperty); }
			set { SetValue(CornerRadiusProperty, value); }
		}

		public static readonly DependencyProperty CornerRadiusProperty =
			DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(SmoothProgressBar), new PropertyMetadata(new CornerRadius(0)));


		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();

			this.ValueChanged += SmoothProgressBar_ValueChanged;

			var descriptorWidth = DependencyPropertyDescriptor.FromProperty(ActualWidthProperty, typeof(Border));
			if (descriptorWidth != null)
			{
				var border = Template.FindName("_border", this) as Border;
				if (border != null)
				{
					descriptorWidth.AddValueChanged(border, ActualWidth_ValueChanged);
				}
			}
			var descriptorMaximum = DependencyPropertyDescriptor.FromProperty(MaximumProperty, typeof(ProgressBar));
			if (descriptorMaximum != null)
			{
				descriptorMaximum.AddValueChanged(this, Maximum_ValueChanged);
			}
		}

		private void Maximum_ValueChanged(object? sender, EventArgs e)
		{
			var border = Template.FindName("_progress", this) as Border;

			if (border != null)
			{
				Animate(0, GetWidthProgress(Value));
			}
		}

		private void SmoothProgressBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			var border = Template.FindName("_progress", this) as Border;

			if (border != null)
			{
				Animate(border.Width, GetWidthProgress(Value));
			}

			e.Handled = true;
		}

		private void ActualWidth_ValueChanged(object? sender, EventArgs e)
		{
			var border = Template.FindName("_progress", this) as Border;

			if (border != null)
			{
				Animate(0, GetWidthProgress(Value));
			}
		}

		private double GetWidthProgress(double progress)
		{
			return ActualWidth * (progress / Maximum);
		}

		private void Animate(double oldValue, double newValue)
		{
			var border = Template.FindName("_progress", this) as Border;

			if (border != null)
			{
				DoubleAnimation animation = new DoubleAnimation(
					oldValue, newValue, new Duration(TimeSpan.FromSeconds(0.7)));
				animation.EasingFunction = new CubicEase
				{
					EasingMode = EasingMode.EaseInOut
				};
				
				border.BeginAnimation(Border.WidthProperty, animation, HandoffBehavior.Compose);
			}
		}

		public SmoothProgressBar()
        {
            InitializeComponent();

			Unloaded += SmoothProgressBar_Unloaded;
        }

		private void SmoothProgressBar_Unloaded(object sender, RoutedEventArgs e)
		{
			this.ValueChanged -= SmoothProgressBar_ValueChanged;

			var descriptorWidth = DependencyPropertyDescriptor.FromProperty(ActualWidthProperty, typeof(Border));
			var border = Template.FindName("_border", this) as Border;
			if (border != null)
			{
				descriptorWidth?.RemoveValueChanged(border, ActualWidth_ValueChanged);
			}

			var descriptorMaximum = DependencyPropertyDescriptor.FromProperty(MaximumProperty, typeof(ProgressBar));
			descriptorMaximum?.RemoveValueChanged(this, Maximum_ValueChanged);

			Unloaded -= SmoothProgressBar_Unloaded;
		}
	}
}
