using QuizGenerator.ViewModel.ViewModels.Bases;

namespace QuizGenerator.ViewModel.ViewModels.Models;

public class UserAnswerViewModel : ViewModelBase
{
	private bool _isSelected;

	public bool IsSelected
	{
		get => _isSelected;
		set
		{
			_isSelected = value;
			OnPropertyChanged();
		}
	}

	private string? _text;

	public string? Text
	{
		get => _text;
		set
		{
			_text = value;
			OnPropertyChanged();
		}
	}

	private bool _isReadOnly;

	public bool IsReadOnly
	{
		get => _isReadOnly;
		set
		{
			_isReadOnly = value;
			OnPropertyChanged();
		}
	}

}
