using QuizGenerator.ViewModel.Commands.Bases;
using System.Windows.Input;

namespace QuizGenerator.ViewModel.Commands.Interfaces;

public interface IAsyncCommand<TResult> : ICommand
{
	public Task ExecuteAsync(object? parameter);
	public NotifyTaskCompletion<TResult>? Execution { get; }

	public ICommand CancelCommand { get; }
}
