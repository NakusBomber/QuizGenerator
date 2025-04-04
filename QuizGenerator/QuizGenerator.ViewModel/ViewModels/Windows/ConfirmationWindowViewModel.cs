using QuizGenerator.ViewModel.Commands.Bases;
using QuizGenerator.ViewModel.ViewModels.Bases;
using System.Windows.Input;

namespace QuizGenerator.ViewModel.ViewModels.Windows;

public class ConfirmationWindowViewModel : DialogViewModel
{
	private string _text;

	public string Text
	{
		get => _text;
		set
		{
			_text = value;
			OnPropertyChanged();
		}
	}
	public ICommand ConfirmCommand { get; }
	public ICommand CancelCommand { get; }

	public ConfirmationWindowViewModel(string title, string text)
	{
		Title = title;
		_text = text;

		ConfirmCommand = new DelegateCommand(Confirm);
		CancelCommand = new DelegateCommand(Cancel);
	}

	public static ConfirmationWindowViewModel DeletePrefab(string? name = null) =>
		new("Are you sure?", 
			string.IsNullOrEmpty(name) ? "Want delete?"
									   : $"Want delete \"{name}\"?");

	private void Confirm(object? parameter)
	{
		IsSuccess = true;
		CloseWindow();
	}

	private void Cancel(object? parameter)
	{
		IsSuccess = false;
		CloseWindow();
	}
}
