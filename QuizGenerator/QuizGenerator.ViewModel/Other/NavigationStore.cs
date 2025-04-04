using QuizGenerator.ViewModel.ViewModels.Bases;

namespace QuizGenerator.ViewModel.Other;

public class NavigationStore : ObservableObject
{
	private ViewModelBase _currentViewModel;
	public ViewModelBase CurrentViewModel
	{
		get => _currentViewModel;
		set
		{
			_currentViewModel = value;
			OnPropertyChanged();
		}
	}

}
