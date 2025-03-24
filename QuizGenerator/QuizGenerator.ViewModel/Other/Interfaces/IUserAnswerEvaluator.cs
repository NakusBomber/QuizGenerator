using QuizGenerator.Model.Entities;
using QuizGenerator.ViewModel.ViewModels.Models;

namespace QuizGenerator.ViewModel.Other.Interfaces;

public interface IUserAnswerEvaluator
{
	public bool Validate(QuestionViewModel questionViewModel);
	public float CalculatePrice(QuestionViewModel questionViewModel);
	public bool IsCorrect(QuestionType questionType, AnswerDetailViewModel answerDetailViewModel);
}
