using QuizGenerator.ViewModel.Other.Interfaces;
using QuizGenerator.ViewModel.ViewModels.Bases;
using System.Windows;

namespace QuizGenerator.ViewModel.Other;

public abstract class WindowNavigationServiceBase<TViewModel, TResult> :
	IWindowNavigationService<TViewModel, TResult> where TViewModel : DialogViewModel
{
	protected abstract Type WindowType { get; }
	public abstract TResult Navigate(TViewModel viewModel);

	protected TViewModel ShowWindow(TViewModel viewModel)
	{
		if (Activator.CreateInstance(WindowType) is Window window)
		{
			window.DataContext = viewModel;
			window.ShowDialog();

			return viewModel;
		}

		throw new InvalidOperationException("WindowType must be a subclass of Window");
	}
}
