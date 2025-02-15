using QuizGenerator.ViewModel.Commands;
using QuizGenerator.ViewModel.Commands.Bases;
using QuizGenerator.ViewModel.Other;
using QuizGenerator.ViewModel.Other.Interfaces;
using QuizGenerator.ViewModel.ViewModels.Bases;
using System.Windows.Controls;
using System.Windows.Input;

namespace QuizGenerator.ViewModel.ViewModels;

public class NavigationViewModel : ViewModelBase
{
	public NavigationStore NavigationStore { get; }
	
	public ICommand BackNavigateCommand { get; }

	public NavigationViewModel(
		NavigationStore navigationStore,
		INavigationJournal navigationJournal,
		IBackNavigationService backNavigationService)
	{
		NavigationStore = navigationStore;
		BackNavigateCommand = new BackNavigateCommand(backNavigationService, navigationJournal);
	}

}
