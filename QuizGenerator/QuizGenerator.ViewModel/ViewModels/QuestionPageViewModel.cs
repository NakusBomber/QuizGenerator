using QuizGenerator.Model.Entities;
using QuizGenerator.Model.Interfaces;
using QuizGenerator.ViewModel.Commands.Bases;
using QuizGenerator.ViewModel.Commands.Interfaces;
using QuizGenerator.ViewModel.ViewModels.Bases;
using QuizGenerator.ViewModel.ViewModels.Models;
using System.Collections.ObjectModel;

namespace QuizGenerator.ViewModel.ViewModels;

public class QuestionPageViewModel : ViewModelBase
{
	private readonly IUnitOfWork _unitOfWork;
	private Question? _question = null;
	private Guid? _questionId;

	private QuestionViewModel? _questionVM;

	public QuestionViewModel? Question
	{
		get => _questionVM;
		set
		{
			_questionVM = value;
			OnPropertyChanged();
		}
	}

	private bool _isNowSaving;

	public bool IsNowSaving
	{
		get => _isNowSaving;
		set
		{
			_isNowSaving = value;
			OnPropertyChanged();
		}
	}


	public IAsyncCommand<object?> LoadQuestionCommand { get; }
	public IAsyncCommand<object?> SaveQuestionCommand { get; }

	public QuestionPageViewModel(
		Guid? id,
		IUnitOfWork unitOfWork)
	{
		_questionId = id;
		_unitOfWork = unitOfWork;

		_isNowSaving = false;

		LoadQuestionCommand = AsyncDelegateCommand.Create(LoadQuestionAsync);
		SaveQuestionCommand = AsyncDelegateCommand.Create(SaveQuestionAsync);
	}

	private async Task LoadQuestionAsync(CancellationToken token)
	{
		if (_questionId is Guid id)
		{
			_question = await _unitOfWork.QuestionRepository.GetByIdAsync(id, token);
		}
		else
		{
			_question = new Question();
		}

		_questionId = _question.Id;
		Question = new QuestionViewModel(_question);
	}

	private async Task SaveQuestionAsync(CancellationToken token)
	{
		if (Question != null)
		{
			IsNowSaving = true;

			_question = Question.ToQuestion();

			try
			{
				await _unitOfWork.QuestionRepository.UpdateAsync(_question, token);
			}
			catch (Exception)
			{
				await _unitOfWork.QuestionRepository.CreateAsync(_question, token);
			}

			foreach (var questionDetail in _question.QuestionDetails)
			{
				try
				{
					await _unitOfWork.QuestionDetailRepository.CreateAsync(questionDetail, token);
				}
				catch (Exception)
				{
					await _unitOfWork.QuestionDetailRepository.UpdateAsync(questionDetail, token);
				}
			}

			await _unitOfWork.SaveAsync(token);

			IsNowSaving = false;
		}

	}
}
