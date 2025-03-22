using QuizGenerator.ViewModel.ViewModels.Models;

namespace QuizGenerator.ViewModel.Other.Interfaces;

public interface IUserAnswerValidator
{
	public bool Validate(QuestionViewModel questionViewModel);
	public bool IsCorrect(AnswerDetailViewModel answerDetailViewModel);
}
