using QuizGenerator.ViewModel.Commands.Bases;
using QuizGenerator.ViewModel.Other.Interfaces;

namespace QuizGenerator.ViewModel.Commands;

public class NavigateCommand : DelegateCommand
{
	public NavigateCommand(INavigationService navigationService) 
		: base((o) => navigationService.Navigate())
	{
	}
}
