using QuizGenerator.Model.Entities;
using QuizGenerator.ViewModel.ViewModels.Models;

namespace QuizGenerator.ViewModel.Other.Interfaces;

public interface IQuestionEvaluator
{
	public abstract QuestionType QuestionType { get; }
	public bool Validate(QuestionViewModel questionViewModel);
	public float CalculatePrice(float maxPrice, QuestionDetailViewModel questionDetailViewModel);
	public bool IsCorrect(AnswerDetailViewModel answerDetailViewModel);
}
