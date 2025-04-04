using QuizGenerator.ViewModel.Other.Interfaces;
using QuizGenerator.ViewModel.ViewModels.Bases;

namespace QuizGenerator.ViewModel.Other;

public class ParameterNavigationService<TParameter, TViewModel> : IParameterNavigationService<TParameter, TViewModel>
	where TViewModel : ViewModelBase
{
	private readonly NavigationStore _navigationStore;
	private readonly INavigationJournal _navigationJournal;
	private readonly Func<TParameter, TViewModel> _createFunc;

	public ParameterNavigationService(
		NavigationStore navigationStore,
		INavigationJournal navigationJournal,
		Func<TParameter, TViewModel> createFunc)
	{
		_navigationStore = navigationStore;
		_navigationJournal = navigationJournal;
		_createFunc = createFunc;
	}

	public void Navigate(TParameter parameter)
	{
		_navigationJournal.AddPage(_navigationStore.CurrentViewModel);
		_navigationStore.CurrentViewModel = _createFunc(parameter);
	}
}
