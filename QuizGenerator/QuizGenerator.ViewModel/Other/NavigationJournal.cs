using QuizGenerator.ViewModel.Other.Interfaces;
using QuizGenerator.ViewModel.ViewModels.Bases;

namespace QuizGenerator.ViewModel.Other;

public class NavigationJournal : INavigationJournal
{
	private readonly Stack<ViewModelBase> _historyPages = new Stack<ViewModelBase>();

	public ViewModelBase? GetPage()
	{
		if (IsNotEmptyHistory())
		{
			return _historyPages.Pop();
		}

		return null;
	}

	public bool IsNotEmptyHistory() => _historyPages.Count > 0;

	public void AddPage(ViewModelBase viewModel)
	{
		_historyPages.Push(viewModel);
	}

	public void Clear() => _historyPages.Clear();
}
