using GongSolutions.Wpf.DragDrop;
using QuizGenerator.Model.Entities;
using QuizGenerator.Model.Interfaces;
using QuizGenerator.ViewModel.Commands.Bases;
using QuizGenerator.ViewModel.Commands.Interfaces;
using QuizGenerator.ViewModel.ViewModels.Bases;
using QuizGenerator.ViewModel.ViewModels.Models;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace QuizGenerator.ViewModel.ViewModels;

public class QuizPageViewModel : ViewModelBase, IDropTarget
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IDropTarget _dropHandler;

	private Guid? _quizId;
	private Quiz? _quiz;

	private QuizViewModel? _quizViewModel;

	public QuizViewModel? Quiz
	{
		get => _quizViewModel;
		set
		{
			_quizViewModel = value;
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


	public ICommand OpenDropDownQuestionTypesCommand { get; }
	public ICommand AddNewQuestionCommand { get; }

	public IAsyncCommand<object?> LoadQuizCommand { get; }
	public IAsyncCommand<object?> SaveQuizCommand { get; }

	public QuizPageViewModel(Guid? id, IUnitOfWork unitOfWork)
	{
		_unitOfWork = unitOfWork;
		_dropHandler = new DefaultDropHandler();
		_quizId = id;
		_isNowSaving = false;

		LoadQuizCommand = AsyncDelegateCommand.Create(LoadQuizAsync);
		SaveQuizCommand = AsyncDelegateCommand.Create(SaveQuizAsync, (o) => Quiz != null);
		OpenDropDownQuestionTypesCommand = new DelegateCommand(OpenDropDownQuestionTypes, CanAddNewQuestion);
		AddNewQuestionCommand = new DelegateCommand(AddNewQuestion, CanAddNewQuestion);
	}

	public void Drop(IDropInfo dropInfo)
	{
		_dropHandler.Drop(dropInfo);
		ChangeAllQuestionNumbers();
	}

	public void DragOver(IDropInfo dropInfo) => _dropHandler.DragOver(dropInfo);

	private void ChangeAllQuestionNumbers()
	{
		if (Quiz == null)
		{
			return;
		}

		for (int i = 0; i < Quiz.Questions.Count; i++)
		{
			Quiz.Questions[i].ListNumber = i + 1;
		}
	}

	private async Task LoadQuizAsync(CancellationToken token)
	{
		if (_quizId is Guid id)
		{
			_quiz = await _unitOfWork.QuizRepository.GetByIdAsync(id, token);
		}
		else
		{
			_quiz = new Quiz();
			await _unitOfWork.QuizRepository.CreateAsync(_quiz, token);
			await _unitOfWork.SaveAsync(token);
		}

		_quizId = _quiz.Id;
		Quiz = new QuizViewModel(_quiz);
		Quiz.Questions = new ObservableCollection<QuestionViewModel>(Quiz.Questions.OrderBy(qVM => qVM.ListNumber));
	}

	private async Task SaveQuizAsync(CancellationToken token)
	{
		if (Quiz != null)
		{
			IsNowSaving = true;

			_quiz = Quiz.ToQuiz();
			await _unitOfWork.QuizRepository.UpdateAsync(_quiz, token);
			foreach (var question in _quiz.Questions)
			{
				try
				{
					await _unitOfWork.QuestionRepository.CreateAsync(question, token);
				}
				catch (Exception)
				{
					await _unitOfWork.QuestionRepository.UpdateAsync(question, token);
				}
			}
			await _unitOfWork.SaveAsync(token);

			IsNowSaving = false;
		}
	}

	private void AddNewQuestion(object? obj)
	{
		var minimalNumber = 1;

		if (obj is QuestionType questionType && _quiz != null && Quiz != null)
		{
			var listNumber = (Quiz.Questions.LastOrDefault()?.ListNumber + 1) ?? minimalNumber;
			var question = new Question(_quiz, 1, questionType, listNumber);
			Quiz.Questions.Add(new QuestionViewModel(question));
		}
	}

	private void OpenDropDownQuestionTypes(object? obj)
	{
		// ContextMenu located not in visual tree, thus "Binding" won't work
		if (obj is Button btn)
		{
			btn.ContextMenu.IsOpen = true;
		}
	}

	private bool CanAddNewQuestion(object? obj) => Quiz != null;
}
