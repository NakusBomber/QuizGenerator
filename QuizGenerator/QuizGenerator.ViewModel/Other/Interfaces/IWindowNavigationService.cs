using QuizGenerator.ViewModel.ViewModels.Bases;

namespace QuizGenerator.ViewModel.Other.Interfaces;

public interface IWindowNavigationService<TViewModel, TResult>
	where TViewModel : ViewModelBase
{
	public TResult Navigate(TViewModel viewModel);
}
