﻿using System.Windows.Input;

namespace QuizGenerator.ViewModel.Commands.Bases;

public sealed class CancelAsyncCommand : ICommand
{
	private CancellationTokenSource _cts = new CancellationTokenSource();
	private bool _commandExecuting;

	public CancellationToken Token => _cts.Token;

	public void NotifyCommandStarting()
	{
		_commandExecuting = true;
		if (!_cts.IsCancellationRequested)
		{
			return;
		}

		_cts = new CancellationTokenSource();
		RaiseCanExecuteChanged();
	}

	public void NotifyCommandFinished()
	{
		_commandExecuting = false;
		RaiseCanExecuteChanged();
	}

	public bool CanExecute(object? parameter)
	{
		return _commandExecuting && !_cts.IsCancellationRequested;
	}

	void ICommand.Execute(object? parameter)
	{
		_cts.Cancel();
		RaiseCanExecuteChanged();
	}

	public event EventHandler? CanExecuteChanged
	{
		add { CommandManager.RequerySuggested += value; }
		remove { CommandManager.RequerySuggested -= value; }
	}

	private void RaiseCanExecuteChanged()
	{
		CommandManager.InvalidateRequerySuggested();
	}
}
