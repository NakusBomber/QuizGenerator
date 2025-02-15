using QuizGenerator.ViewModel.ViewModels.Bases;

namespace QuizGenerator.ViewModel.Other.Interfaces;

public interface IParameterNavigationService<TParameter, TViewModel>
	where TViewModel : ViewModelBase
{
	public void Navigate(TParameter parameter);
}
