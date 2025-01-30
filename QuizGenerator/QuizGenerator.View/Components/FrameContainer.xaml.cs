using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace QuizGenerator.View.Components;

/// <summary>
/// Interaction logic for FrameContainer.xaml
/// </summary>
public partial class FrameContainer : UserControl
{
	public static readonly RoutedEvent ViewModelChangedEvent = EventManager.RegisterRoutedEvent(
			"ViewModelChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(FrameContainer));

	public event RoutedEventHandler ViewModelChanged
	{
		add { AddHandler(ViewModelChangedEvent, value); }
		remove { RemoveHandler(ViewModelChangedEvent, value); }
	}


	public FrameContainer()
	{
		InitializeComponent();

		var discriptor = DependencyPropertyDescriptor.FromProperty(DataContextProperty, typeof(FrameContainer));
		if (discriptor != null)
		{
			discriptor.AddValueChanged(this, (sender, e) => RaiseViewModelChanged());
		}
	}

	private void RaiseViewModelChanged()
	{
		var args = new RoutedEventArgs(ViewModelChangedEvent);
		RaiseEvent(args);
	}

}
