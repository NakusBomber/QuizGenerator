using QuizGenerator.ViewModel.ViewModels.Bases;
using System.ComponentModel;

namespace QuizGenerator.ViewModel.Commands.Bases;

// Reference https://learn.microsoft.com/ru-ru/archive/msdn-magazine/2014/april/async-programming-patterns-for-asynchronous-mvvm-applications-commands
public sealed class NotifyTaskCompletion<TResult> : ObservableObject
{
	public NotifyTaskCompletion(Task<TResult> task)
	{
		Task = task;
		TaskCompletion = WatchTaskAsync(task);
	}
	private async Task WatchTaskAsync(Task task)
	{
		try
		{
			await task;
		}
		catch
		{
		}
		OnPropertyChanged(nameof(Status));
		OnPropertyChanged(nameof(IsCompleted));
		OnPropertyChanged(nameof(IsNotCompleted));
		if (task.IsCanceled)
		{
			OnPropertyChanged(nameof(IsCanceled));
		}
		else if (task.IsFaulted)
		{
			OnPropertyChanged(nameof(IsFaulted));
			OnPropertyChanged(nameof(Exception));
			OnPropertyChanged(nameof(InnerException));
			OnPropertyChanged(nameof(ErrorMessage));
		}
		else
		{
			OnPropertyChanged(nameof(IsSuccessfullyCompleted));
			OnPropertyChanged(nameof(Result));
		}
	}

	public Task<TResult> Task { get; private set; }
	public Task TaskCompletion { get; private set; }
	public TResult? Result
	{
		get => Task.Status == TaskStatus.RanToCompletion ? Task.Result : default;
	}
	public TaskStatus Status => Task.Status;
	public bool IsCompleted => Task.IsCompleted;
	public bool IsNotCompleted => !Task.IsCompleted;
	public bool IsSuccessfullyCompleted
	{
		get => Task.Status == TaskStatus.RanToCompletion;
	}
	public bool IsCanceled => Task.IsCanceled;
	public bool IsFaulted => Task.IsFaulted;
	public AggregateException? Exception => Task.Exception;
	public Exception? InnerException
	{
		get => Exception == null ? null : Exception.InnerException;
	}
	public string? ErrorMessage
	{
		get => InnerException == null ? null : InnerException.Message;
	}
}
