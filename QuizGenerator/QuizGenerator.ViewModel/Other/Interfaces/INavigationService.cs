using QuizGenerator.ViewModel.ViewModels.Bases;

namespace QuizGenerator.ViewModel.Other.Interfaces;

public interface INavigationService<TViewModel>
	where TViewModel : ViewModelBase
{
	public void Navigate();
}
