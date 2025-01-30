using QuizGenerator.ViewModel.Other.Interfaces;
using QuizGenerator.ViewModel.ViewModels.Bases;

namespace QuizGenerator.ViewModel.Other;

public class NavigationService<TViewModel> : INavigationService where TViewModel : ViewModelBase
{
	private readonly NavigationStore _navigationStore;
	private readonly INavigationJournal _navigationJournal;
	private readonly Func<TViewModel> _createVMFunc;

	public NavigationService(
		NavigationStore navigationStore,
		INavigationJournal navigationJournal,
		Func<TViewModel> createVMFunc)
	{
		_navigationStore = navigationStore;
		_navigationJournal = navigationJournal;
		_createVMFunc = createVMFunc;
	}

	public void Navigate()
	{
		_navigationJournal.AddPage(_navigationStore.CurrentViewModel);
		_navigationStore.CurrentViewModel = _createVMFunc();
	}

	
}
