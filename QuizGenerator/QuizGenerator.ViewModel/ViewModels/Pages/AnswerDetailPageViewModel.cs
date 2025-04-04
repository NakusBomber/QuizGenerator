using QuizGenerator.Model.Entities;
using QuizGenerator.Model.Interfaces;
using QuizGenerator.ViewModel.Commands.Bases;
using QuizGenerator.ViewModel.Commands.Interfaces;
using QuizGenerator.ViewModel.Other.Interfaces;
using QuizGenerator.ViewModel.ViewModels.Bases;
using QuizGenerator.ViewModel.ViewModels.Models;
using QuizGenerator.ViewModel.ViewModels.Windows;
using System.ComponentModel;
using System.Windows;

namespace QuizGenerator.ViewModel.ViewModels.Pages;

public class AnswerDetailPageViewModel : SavingStateViewModel
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IBackNavigationService _backNavigationService;
	private readonly IWindowNavigationService<ConfirmationWindowViewModel, bool> _confirmationNavigationService;

	private readonly PropertyChangedEventHandler _propertyChangedHandler;
	private Guid? _answerDetailId;
	private AnswerDetail? _answerDetail;

	private AnswerDetailViewModel? _answerDetailViewModel;

	public AnswerDetailViewModel? AnswerDetail
	{
		get => _answerDetailViewModel;
		set
		{
			_answerDetailViewModel = value;
			OnPropertyChanged();
		}
	}

	public IAsyncCommand<object?> LoadCommand { get; }
	public IAsyncCommand<object?> SaveCommand { get; }
	public IAsyncCommand<object?> DeleteAnswerCommand { get; }

	public AnswerDetailPageViewModel(
		Guid? id,
		IUnitOfWork unitOfWork,
		IBackNavigationService backNavigationService,
		IWindowNavigationService<ConfirmationWindowViewModel, bool> confirmationNavigationService)
	{
		_unitOfWork = unitOfWork;
		_backNavigationService = backNavigationService;
		_confirmationNavigationService = confirmationNavigationService;

		_answerDetailId = id;
		_propertyChangedHandler = (s, e) => IsNeedSaving = true;

		LoadCommand = AsyncDelegateCommand.Create(LoadAnswerDetailAsync);
		SaveCommand = AsyncDelegateCommand.Create(SaveAnswerDetailAsync, o => IsNeedSaving);
		DeleteAnswerCommand = 
			AsyncDelegateCommand.Create(DeleteAnswerDetailAsync, o => AnswerDetail != null);
	}

	~AnswerDetailPageViewModel()
	{
		if (AnswerDetail != null)
		{
			AnswerDetail.PropertyChanged -= _propertyChangedHandler;
		}
	}

	private async Task LoadAnswerDetailAsync(CancellationToken token)
	{
		await Task.Run(async () =>
		{
			if (_answerDetailId is Guid id)
			{
				_answerDetail = await _unitOfWork.AnswerDetailRepository.GetByIdAsync(id, token);
			}
			else
			{
				_answerDetail = new AnswerDetail();
			}

			_answerDetailId = _answerDetail.Id;

			Application.Current.Dispatcher.Invoke(() =>
			{
				AnswerDetail = new AnswerDetailViewModel(_answerDetail);
				AnswerDetail.PropertyChanged += _propertyChangedHandler;
			});
		}, token);
	}

	private async Task SaveAnswerDetailAsync(CancellationToken token)
	{
		if (AnswerDetail == null || _answerDetail == null)
		{
			return;
		}

		IsNowSaving = true;

		await Task.Run(async () =>
		{
			AnswerDetail.CopyToAnswerDetail(_answerDetail);
			await _unitOfWork.AnswerDetailRepository.UpdateOrCreateAsync(_answerDetail, token);

			await _unitOfWork.SaveAsync(token);
		}, token);

		IsNeedSaving = false;
		IsNowSaving = false;
	}

	private async Task DeleteAnswerDetailAsync(CancellationToken token)
	{
		if (AnswerDetail == null || _answerDetail == null)
		{
			return;
		}

		var result = _confirmationNavigationService.Navigate(
			ConfirmationWindowViewModel.DeletePrefab(AnswerDetail.Text));

		if (!result)
		{
			return;
		}

		try
		{
			var answerDetail = await _unitOfWork.AnswerDetailRepository
										.GetByIdAsync(AnswerDetail.Id, token);
			await _unitOfWork.AnswerDetailRepository.DeleteAsync(answerDetail, token);

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
}
