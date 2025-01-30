using QuizGenerator.ViewModel.Commands.Bases;
using QuizGenerator.ViewModel.Other;
using QuizGenerator.ViewModel.Other.Interfaces;

namespace QuizGenerator.ViewModel.Commands;

public class BackNavigateCommand : DelegateCommand
{
	public BackNavigateCommand(INavigationService backNavigationService, INavigationJournal navigationJournal) 
		: base((o) => backNavigationService.Navigate(), (o) => navigationJournal.IsNotEmptyHistory())
	{
	}
}
