using System.Windows;

namespace QuizGenerator.ViewModel.ViewModels.Bases;

public class DialogViewModel : ViewModelBase
{
	private bool _isSuccess;

	public bool IsSuccess
	{
		get => _isSuccess;
		set
		{
			_isSuccess = value;
			OnPropertyChanged();
		}
	}

	private string _title = string.Empty;

	public string Title
	{
		get => _title;
		set
		{
			_title = value;
			OnPropertyChanged();
		}
	}


	protected void CloseWindow() =>
		Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w.IsActive)?.Close();
}
