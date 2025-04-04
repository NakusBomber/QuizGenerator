using QuizGenerator.ViewModel.Other.Interfaces;

namespace QuizGenerator.ViewModel.Other;

public class BackNavigationService : IBackNavigationService
{
	private readonly NavigationStore _navigationStore;
	private readonly INavigationJournal _navigationJournal;

	public BackNavigationService(NavigationStore navigationStore, INavigationJournal navigationJournal)
	{
		_navigationStore = navigationStore;
		_navigationJournal = navigationJournal;
	}

	public void Navigate()
	{
		var prevPage = _navigationJournal.GetPage();
		if (prevPage != null)
		{
			_navigationStore.CurrentViewModel = prevPage;
		}
	}
}
