using QuizGenerator.ViewModel.Commands.Bases;
using System.Windows.Input;

namespace QuizGenerator.ViewModel.Commands.Interfaces;

public interface IAsyncCommand<TResult> : ICommand
{
	Task ExecuteAsync(object? parameter);
	NotifyTaskCompletion<TResult>? Execution { get; }
}
