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
    /// Interaction logic for UnloadablePage.xaml
    /// </summary>
    public partial class UnloadableContainer : UserControl
    {
		private object? _savedDataContext;
		private bool _isHandlingDataContextChanged = true;

		public static readonly RoutedEvent DataContextClearingEvent = EventManager.RegisterRoutedEvent(
			"DataContextClearing", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(UnloadableContainer));

		public event RoutedEventHandler DataContextClearing
		{
			add { AddHandler(DataContextClearingEvent, value); }
			remove { RemoveHandler(DataContextClearingEvent, value); }
		}

		public UnloadableContainer()
		{
			InitializeComponent();
		}

		~UnloadableContainer()
		{
			Dispatcher.Invoke(() =>
			{
				Loaded -= UnloadablePage_Loaded;
				DataContextChanged -= UnloadablePage_DataContextChanged;
			});
		}

		protected override void OnInitialized(EventArgs e)
		{
			base.OnInitialized(e);

			Loaded += UnloadablePage_Loaded;
			DataContextChanged += UnloadablePage_DataContextChanged;
		}

		private void UnloadablePage_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			if (e.NewValue == null && 
				_savedDataContext != null &&
				_isHandlingDataContextChanged)
			{
				_isHandlingDataContextChanged = false;

				DataContext = _savedDataContext;
				var args = new RoutedEventArgs(DataContextClearingEvent);
				RaiseEvent(args);
				DataContext = null;

				_savedDataContext = null;
				_isHandlingDataContextChanged = true;
			}
		}

		private void UnloadablePage_Loaded(object sender, RoutedEventArgs e)
		{
			_savedDataContext = DataContext;
		}
	}
}
