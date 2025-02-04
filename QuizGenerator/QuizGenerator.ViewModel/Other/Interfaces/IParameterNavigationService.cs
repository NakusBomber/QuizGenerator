using QuizGenerator.ViewModel.ViewModels.Bases;

namespace QuizGenerator.ViewModel.Other.Interfaces;

public interface IParameterNavigationService<TParameter>
{
	public void Navigate(TParameter parameter);
}
