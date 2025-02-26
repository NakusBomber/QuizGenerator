﻿using QuizGenerator.Model.Entities;
using QuizGenerator.ViewModel.ViewModels.Bases;
using System.Collections.ObjectModel;

namespace QuizGenerator.ViewModel.ViewModels.Models;

public class QuestionViewModel : ViewModelBase
{
	private Quiz? _quiz;
	private Guid? _quizId;

	private Guid _id;

	public Guid Id => _id;

	private int _evaluationPrice;

	public int EvaluationPrice
	{
		get => _evaluationPrice;
		set
		{
			_evaluationPrice = value;
			OnPropertyChanged();
		}
	}

	private int _listNumber;

	public int ListNumber
	{
		get => _listNumber;
		set
		{
			_listNumber = value;
			OnPropertyChanged();
		}
	}

	private QuestionType _questionType;

	public QuestionType QuestionType
	{
		get => _questionType;
		set
		{
			_questionType = value;
			OnPropertyChanged();
		}
	}

	private IEnumerable<QuestionDetail> _questionDetails;

	public IEnumerable<QuestionDetail> QuestionDetails
	{
		get => _questionDetails;
		set
		{
			_questionDetails = value;
			OnPropertyChanged();
		}
	}

	public QuestionViewModel()
		: this(new Question())
	{
	}

	public QuestionViewModel(Question question)
	{
		_id = question.Id;
		_quizId = question.QuizId;
		_quiz = question.Quiz;
		_evaluationPrice = question.EvaluationPrice;
		_listNumber = question.ListNumber;
		_questionType = question.QuestionType;
		_questionDetails = new ObservableCollection<QuestionDetail>(question.QuestionDetails);
	}

	public Question ToQuestion()
	{
		return new Question(
			_id,
			_quiz,
			_evaluationPrice,
			_questionType,
			_listNumber,
			_questionDetails); 
	}
}
