using QuizGenerator.ViewModel.Commands.Bases;
using QuizGenerator.ViewModel.Other.Interfaces;
using QuizGenerator.ViewModel.ViewModels.Bases;

namespace QuizGenerator.ViewModel.Commands;

public class ParameterNavigateCommand<TParameter, TViewModel> : DelegateCommand
	where TViewModel : ViewModelBase
{
	public ParameterNavigateCommand(IParameterNavigationService<TParameter, TViewModel> parameterNavigationService)
		: base(o => parameterNavigationService.Navigate(o is TParameter param ? param : default!))
	{
	}
}
