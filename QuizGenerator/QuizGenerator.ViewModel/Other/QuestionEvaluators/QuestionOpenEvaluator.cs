using QuizGenerator.Model.Entities;
using QuizGenerator.ViewModel.Other.Interfaces;
using QuizGenerator.ViewModel.ViewModels.Models;

namespace QuizGenerator.ViewModel.Other.QuestionEvaluators;

public class QuestionOpenEvaluator : IQuestionEvaluator
{
	public QuestionType QuestionType => QuestionType.Open;

	public float CalculatePrice(float maxPrice, QuestionDetailViewModel questionDetailViewModel)
	{
		if (questionDetailViewModel.AnswerDetails.Any(IsCorrect))
		{
			return maxPrice;
		}

		return 0.0f;
	}

	public bool IsCorrect(AnswerDetailViewModel answerDetailViewModel)
	{
		if (answerDetailViewModel.UserAnswer == null ||
			string.IsNullOrEmpty(answerDetailViewModel.UserAnswer.Text))
		{
			return false;
		}

		var answerText = FormattedAnswer(answerDetailViewModel.Text);
		var userText = FormattedAnswer(answerDetailViewModel.UserAnswer.Text);

		return answerDetailViewModel.IsCorrect && answerText == userText;
	}

	public bool Validate(QuestionViewModel questionViewModel) =>
		questionViewModel.QuestionDetails
			.Where(q => q.AnswerDetails.Count > 0)
			.All(qd => qd.AnswerDetails.Any(a => !string.IsNullOrEmpty(a.UserAnswer.Text)));

	private string FormattedAnswer(string text) =>
		text.Trim().ToLower();
}
