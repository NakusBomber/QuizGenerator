using QuizGenerator.ViewModel.Commands.Interfaces;
using QuizGenerator.ViewModel.ViewModels.Bases;
using System.Windows.Input;

namespace QuizGenerator.ViewModel.Commands.Bases;

public abstract class AsyncCommandBase<TResult> : ObservableObject, IAsyncCommand<TResult>
{
	public abstract NotifyTaskCompletion<TResult>? Execution { get; protected set; }
	public abstract bool CanExecute(object? parameter);
	public abstract Task ExecuteAsync(object? parameter);
	public abstract ICommand CancelCommand { get; }
	public async void Execute(object? parameter)
	{
		await ExecuteAsync(parameter);
	}
	public event EventHandler? CanExecuteChanged
	{
		add { CommandManager.RequerySuggested += value; }
		remove { CommandManager.RequerySuggested -= value; }
	}
	protected void RaiseCanExecuteChanged()
	{
		CommandManager.InvalidateRequerySuggested();
	}
}
