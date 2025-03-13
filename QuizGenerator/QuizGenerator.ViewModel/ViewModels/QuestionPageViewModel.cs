using QuizGenerator.Model.Entities;
using QuizGenerator.Model.Interfaces;
using QuizGenerator.ViewModel.Commands.Bases;
using QuizGenerator.ViewModel.Commands.Interfaces;
using QuizGenerator.ViewModel.Other.Interfaces;
using QuizGenerator.ViewModel.ViewModels.Bases;
using QuizGenerator.ViewModel.ViewModels.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace QuizGenerator.ViewModel.ViewModels;

public class QuestionPageViewModel : SavingStateViewModel
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IParameterNavigationService<Guid?, QuestionDetailPageViewModel> _questionDetailPageNavigationService;
	private readonly IBackNavigationService _backNavigationService;

	private readonly PropertyChangedEventHandler _propertyChangedHandler;
	private readonly ICollection<QuestionDetail> _newQuestionDetails;
	private Question? _question;
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

	public IAsyncCommand<object?> LoadQuestionCommand { get; }
	public IAsyncCommand<object?> SaveQuestionCommand { get; }
	public IAsyncCommand<object?> DeleteQuestionCommand { get; }
	public IAsyncCommand<object?> DeleteQuestionDetailCommand { get; }
	public IAsyncCommand<object?> EditQuestionDetailCommand { get; }
	public ICommand AddQuestionDetailCommand { get; }

	public QuestionPageViewModel(
		Guid? id,
		IUnitOfWork unitOfWork,
		IParameterNavigationService<Guid?, QuestionDetailPageViewModel> questionDetailPageNavigationService,
		IBackNavigationService backNavigationService)
	{
		_questionId = id;
		_unitOfWork = unitOfWork;
		_questionDetailPageNavigationService = questionDetailPageNavigationService;
		_backNavigationService = backNavigationService;

		_newQuestionDetails = new List<QuestionDetail>();
		_propertyChangedHandler = (s, e) => IsNeedSaving = true;

		LoadQuestionCommand = AsyncDelegateCommand.Create(LoadQuestionAsync);
		SaveQuestionCommand = AsyncDelegateCommand.Create(SaveQuestionAsync, o => IsNeedSaving);
		DeleteQuestionCommand = AsyncDelegateCommand.Create(DeleteQuestionAsync, o => Question != null);
		DeleteQuestionDetailCommand = 
			AsyncDelegateCommand.Create(DeleteQuestionDetailAsync, o => Question != null);
		EditQuestionDetailCommand = AsyncDelegateCommand.Create(OpenEditQuestionDetailPageAsync);
		AddQuestionDetailCommand = new DelegateCommand(AddQuestionDetail, o => Question != null);
	}

	~QuestionPageViewModel()
	{
		if (Question != null)
		{
			Question.PropertyChanged -= _propertyChangedHandler;
		}
	}

	private async Task LoadQuestionAsync(CancellationToken token)
	{
		await Task.Run(async () =>
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

			var questionDetailViewModels = _question.QuestionDetails
				.Select(qd => new QuestionDetailViewModel(qd));

			Application.Current.Dispatcher.Invoke(() =>
			{
				Question = new QuestionViewModel(_question);
				Question.QuestionDetails = new ObservableCollection<QuestionDetailViewModel>(questionDetailViewModels);

				Question.PropertyChanged += _propertyChangedHandler;
			});

		}, token);
	}

	private async Task SaveQuestionAsync(CancellationToken token)
	{
		if (Question == null || _question == null)
		{
			return;
		}

		IsNowSaving = true;

		await Task.Run(async () =>
		{
			Question.CopyToQuestion(_question);

			await _unitOfWork.QuestionRepository.UpdateOrCreateAsync(_question, token);

			foreach (var questionDetailVM in Question.QuestionDetails)
			{
				if (_newQuestionDetails.FirstOrDefault(qd => qd.Id == questionDetailVM.Id) 
						is QuestionDetail newQuestionDetail)
				{
					questionDetailVM.CopyToQuestionDetail(newQuestionDetail);
					await _unitOfWork.QuestionDetailRepository.CreateAsync(newQuestionDetail, token);
				}
				else
				{
					var questionDetail = await _unitOfWork.QuestionDetailRepository
													.GetByIdAsync(questionDetailVM.Id, token);
					questionDetailVM.CopyToQuestionDetail(questionDetail);
					await _unitOfWork.QuestionDetailRepository.UpdateAsync(questionDetail, token);
				}
			}

			_newQuestionDetails.Clear();
			await _unitOfWork.SaveAsync(token);
		}, token);

		IsNowSaving = false;
		IsNeedSaving = false;
	}

	private async Task DeleteQuestionAsync(CancellationToken token)
	{
		if (Question == null || _question == null)
		{
			return;
		}

		try
		{
			var question = await _unitOfWork.QuestionRepository.GetByIdAsync(Question.Id, token);
			await _unitOfWork.QuestionRepository.DeleteAsync(question, token);

			await _unitOfWork.SaveAsync(token);
		}
		catch (InvalidOperationException)
		{
		}
		finally
		{
			_backNavigationService.Navigate();
		}
	}

	private async Task OpenEditQuestionDetailPageAsync(object? parameter, CancellationToken token)
	{
		if (parameter is QuestionDetailViewModel qdViewModel)
		{
			if (SaveQuestionCommand.CanExecute(null))
			{
				await SaveQuestionCommand.ExecuteAsync(null);
			}

			_questionDetailPageNavigationService.Navigate(qdViewModel.Id);
		}
	}

	private async Task DeleteQuestionDetailAsync(object? parameter, CancellationToken token)
	{
		if (parameter is QuestionDetailViewModel qdViewModel && Question != null)
		{
			if (_newQuestionDetails.FirstOrDefault(qd => qd.Id == qdViewModel.Id) is QuestionDetail newQd)
			{
				_newQuestionDetails.Remove(newQd);
			}
			else
			{
				var questionDetail = await _unitOfWork.QuestionDetailRepository
												.GetByIdAsync(qdViewModel.Id, token);
				await _unitOfWork.QuestionDetailRepository.DeleteAsync(questionDetail, token);
			}
			Question.QuestionDetails.Remove(qdViewModel);

			IsNeedSaving = true;
		}
	}

	private void AddQuestionDetail(object? parameter)
	{
		if (Question == null)
		{
			return;
		}

		var questionDetail = new QuestionDetail(Question.Id, "No text");
		_newQuestionDetails.Add(questionDetail);

		var questionDetailViewModel = new QuestionDetailViewModel(questionDetail);
		Question.QuestionDetails.Add(questionDetailViewModel);

		IsNeedSaving = true;
	}
}
