using QuizGenerator.ViewModel.Commands;
using QuizGenerator.ViewModel.Other.Interfaces;
using QuizGenerator.ViewModel.ViewModels.Bases;
using System.Windows.Input;

namespace QuizGenerator.ViewModel.ViewModels;

public class StartViewModel : ViewModelBase
{
	public ICommand SelectNavigateCommand { get; }
	public ICommand QuizNavigateCommand { get; }
	public StartViewModel(
		INavigationService quizNavigationService,
		INavigationService selectNavigationService)
	{
		QuizNavigateCommand = new NavigateCommand(quizNavigationService);
		SelectNavigateCommand = new NavigateCommand(selectNavigationService);
	}
}
