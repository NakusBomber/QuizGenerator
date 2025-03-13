namespace QuizGenerator.ViewModel.ViewModels.Bases;

public class SavingStateViewModel : ViewModelBase
{
	private bool _isNowSaving = false;

	public bool IsNowSaving
	{
		get => _isNowSaving;
		set
		{
			_isNowSaving = value;
			OnPropertyChanged();
		}
	}

	private bool _isNeedSaving = false;

	public bool IsNeedSaving
	{
		get => _isNeedSaving;
		set
		{
			_isNeedSaving = value;
			OnPropertyChanged();
		}
	}


}
