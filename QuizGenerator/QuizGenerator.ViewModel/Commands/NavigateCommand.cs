using QuizGenerator.ViewModel.Commands.Bases;
using QuizGenerator.ViewModel.Other.Interfaces;
using QuizGenerator.ViewModel.ViewModels.Bases;

namespace QuizGenerator.ViewModel.Commands;

public class NavigateCommand<TViewModel> : DelegateCommand
	where TViewModel : ViewModelBase
{
	public NavigateCommand(INavigationService<TViewModel> navigationService) 
		: base((o) => navigationService.Navigate())
	{
	}
}
