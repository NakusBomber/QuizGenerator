using QuizGenerator.Model.Entities;
using QuizGenerator.ViewModel.Other.Interfaces;
using QuizGenerator.ViewModel.ViewModels.Models;

namespace QuizGenerator.ViewModel.Other;

public class UserAnswerValidator : IUserAnswerValidator
{
	public bool IsCorrect(AnswerDetailViewModel answerDetailViewModel)
	{
		// TODO : Make logic
		return false;
	}

	public bool Validate(QuestionViewModel questionViewModel)
	{
		return questionViewModel.QuestionType switch
		{
			QuestionType.One => OneOrMany(questionViewModel),
			QuestionType.Open => Open(questionViewModel),
			QuestionType.Many => OneOrMany(questionViewModel),
			_ => true
		};
	}

	private bool OneOrMany(QuestionViewModel questionViewModel) =>
		questionViewModel.QuestionDetails.All(
			qd => qd.AnswerDetails.Any(a => a.UserAnswer.IsSelected));

	private bool Open(QuestionViewModel questionViewModel) =>
		questionViewModel.QuestionDetails.All(
			qd => qd.AnswerDetails.Any(a => !string.IsNullOrEmpty(a.UserAnswer.Text)));
}
