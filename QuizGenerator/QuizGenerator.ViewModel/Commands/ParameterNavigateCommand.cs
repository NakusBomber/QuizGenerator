using QuizGenerator.ViewModel.Commands.Bases;
using QuizGenerator.ViewModel.Other.Interfaces;

namespace QuizGenerator.ViewModel.Commands;

public class ParameterNavigateCommand<TParameter> : DelegateCommand
{
	public ParameterNavigateCommand(IParameterNavigationService<TParameter> parameterNavigationService)
		: base(o => parameterNavigationService.Navigate(o is TParameter param ? param : default!))
	{
	}
}
