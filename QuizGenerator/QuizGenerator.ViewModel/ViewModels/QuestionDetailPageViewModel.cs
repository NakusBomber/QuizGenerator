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

public class QuestionDetailPageViewModel : SavingStateViewModel
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IParameterNavigationService<Guid?, AnswerDetailPageViewModel> _answerDetailNavigationService;
	private readonly IBackNavigationService _backNavigationService;

	private readonly PropertyChangedEventHandler _propertyChangedHandler;
	private readonly ICollection<AnswerDetail> _newAnswerDetails;
	private QuestionDetail? _questionDetail;
	private Guid? _questionDetailId;

	private QuestionDetailViewModel? _questionDetailViewModel;

	public QuestionDetailViewModel? QuestionDetail
	{
		get => _questionDetailViewModel;
		set
		{
			_questionDetailViewModel = value;
			OnPropertyChanged();
		}
	}


	public IAsyncCommand<object?> LoadCommand { get; }
	public IAsyncCommand<object?> SaveCommand { get; }
	public IAsyncCommand<object?> DeleteQuestionDetailCommand { get; }
	public IAsyncCommand<object?> DeleteAnswerDetailCommand { get; }
	public IAsyncCommand<object?> EditAnswerDetailCommand { get; }
	public ICommand AddAnswerDetailCommand { get; }
	public QuestionDetailPageViewModel(
		Guid? id,
		IUnitOfWork unitOfWork,
		IParameterNavigationService<Guid?, AnswerDetailPageViewModel> answerDetailNavigationService,
		IBackNavigationService backNavigationService)
	{
		_unitOfWork = unitOfWork;
		_answerDetailNavigationService = answerDetailNavigationService;
		_backNavigationService = backNavigationService;

		_newAnswerDetails = new List<AnswerDetail>();
		_propertyChangedHandler = (s, e) => IsNeedSaving = true;
		_questionDetailId = id;

		LoadCommand = AsyncDelegateCommand.Create(LoadQuestionDetailAsync);
		SaveCommand = AsyncDelegateCommand.Create(SaveQuestionDetailAsync, o => IsNeedSaving);
		DeleteQuestionDetailCommand 
			= AsyncDelegateCommand.Create(DeleteQuestionDetailAsync, o => QuestionDetail != null);
		DeleteAnswerDetailCommand 
			= AsyncDelegateCommand.Create(DeleteAnswerDetailAsync, o => QuestionDetail != null);
		EditAnswerDetailCommand = AsyncDelegateCommand.Create(OpenEditAnswerDetailAsync);
		AddAnswerDetailCommand = new DelegateCommand(AddAnswerDetail, o => QuestionDetail != null);
	}

	~QuestionDetailPageViewModel()
	{
		if (QuestionDetail != null)
		{
			QuestionDetail.PropertyChanged -= _propertyChangedHandler;
		}
	}

	private async Task LoadQuestionDetailAsync(CancellationToken token)
	{
		await Task.Run(async () =>
		{
			if (_questionDetailId is Guid id)
			{
				_questionDetail = await _unitOfWork.QuestionDetailRepository.GetByIdAsync(id, token);
			}
			else
			{
				_questionDetail = new QuestionDetail();
			}

			_questionDetailId = _questionDetail.Id;
			var answerDetailViewModels = _questionDetail.AnswerDetails
				.Select(a => new AnswerDetailViewModel(a));

			Application.Current.Dispatcher.Invoke(() =>
			{
				QuestionDetail = new QuestionDetailViewModel(_questionDetail);
				QuestionDetail.AnswerDetails = 
					new ObservableCollection<AnswerDetailViewModel>(answerDetailViewModels);

				QuestionDetail.PropertyChanged += _propertyChangedHandler;
			});

		}, token);
	}

	private async Task SaveQuestionDetailAsync(CancellationToken token)
	{
		if (QuestionDetail == null || _questionDetail == null)
		{
			return;
		}

		IsNowSaving = true;

		await Task.Run(async () =>
		{
			QuestionDetail.CopyToQuestionDetail(_questionDetail);

			await _unitOfWork.QuestionDetailRepository.UpdateOrCreateAsync(_questionDetail, token);

			foreach (var answerDetailVM in QuestionDetail.AnswerDetails)
			{
				if (_newAnswerDetails.FirstOrDefault(a => a.Id == answerDetailVM.Id) 
						is AnswerDetail newAnswer)
				{
					answerDetailVM.CopyToAnswerDetail(newAnswer);
					await _unitOfWork.AnswerDetailRepository.CreateAsync(newAnswer, token);
				}
				else
				{
					var answer = await _unitOfWork.AnswerDetailRepository
											.GetByIdAsync(answerDetailVM.Id, token);
					answerDetailVM.CopyToAnswerDetail(answer);
					await _unitOfWork.AnswerDetailRepository.UpdateAsync(answer, token);
				}
			}

			_newAnswerDetails.Clear();
			await _unitOfWork.SaveAsync(token);
		}, token);

		IsNeedSaving = false;
		IsNowSaving = false;
	}

	private async Task DeleteQuestionDetailAsync(CancellationToken token)
	{
		if (QuestionDetail == null || _questionDetail == null)
		{
			return;
		}

		try
		{
			var questionDetail = await _unitOfWork.QuestionDetailRepository
											.GetByIdAsync(QuestionDetail.Id, token);
			await _unitOfWork.QuestionDetailRepository.DeleteAsync(questionDetail, token);

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

	private async Task DeleteAnswerDetailAsync(object? parameter, CancellationToken token)
	{
		if (parameter is AnswerDetailViewModel answerDetailVM && QuestionDetail != null)
		{
			if (_newAnswerDetails.FirstOrDefault(a => a.Id == answerDetailVM.Id)
					is AnswerDetail newAnswer)
			{
				_newAnswerDetails.Remove(newAnswer);
			}
			else
			{
				var answerDetail = await _unitOfWork.AnswerDetailRepository
											.GetByIdAsync(answerDetailVM.Id, token);
				await _unitOfWork.AnswerDetailRepository.DeleteAsync(answerDetail, token);
			}
			QuestionDetail.AnswerDetails.Remove(answerDetailVM);

			IsNeedSaving = true;
		}
	}

	private async Task OpenEditAnswerDetailAsync(object? parameter, CancellationToken token)
	{
		if (parameter is AnswerDetailViewModel answerDetailViewModel)
		{
			if (SaveCommand.CanExecute(null))
			{
				await SaveCommand.ExecuteAsync(null);
			}

			_answerDetailNavigationService.Navigate(answerDetailViewModel.Id);
		}
	}

	private void AddAnswerDetail(object? parameter)
	{
		if (QuestionDetail == null || _questionDetail == null)
		{
			return;
		}

		var answerDetail = new AnswerDetail(QuestionDetail.Id, "Answer");
		_newAnswerDetails.Add(answerDetail);

		var answerDetailVM = new AnswerDetailViewModel(answerDetail);
		QuestionDetail.AnswerDetails.Add(answerDetailVM);

		IsNeedSaving = true;
	}
}
