using QuizGenerator.ViewModel.ViewModels.Bases;

namespace QuizGenerator.ViewModel.Other.Interfaces;

public interface INavigationJournal
{
	public void AddPage(ViewModelBase viewModel);
	public ViewModelBase? GetPage();
	public bool IsNotEmptyHistory();
	public void Clear();
}
