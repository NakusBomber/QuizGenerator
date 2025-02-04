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
	private readonly EventHandler _handler;

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

		_handler = (sender, e) => RaiseViewModelChanged();

		var discriptor = DependencyPropertyDescriptor.FromProperty(DataContextProperty, typeof(FrameContainer));
		discriptor?.AddValueChanged(this, _handler);

		Unloaded += FrameContainer_Unloaded;
	}

	private void FrameContainer_Unloaded(object sender, RoutedEventArgs e)
	{
		var descriptor = DependencyPropertyDescriptor.FromProperty(DataContextProperty, typeof(FrameContainer));
		descriptor?.RemoveValueChanged(this, _handler);

		Unloaded -= FrameContainer_Unloaded;
	}

	private void RaiseViewModelChanged()
	{
		var args = new RoutedEventArgs(ViewModelChangedEvent);
		RaiseEvent(args);
	}

}
