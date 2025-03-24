using QuizGenerator.Model.Entities;
using QuizGenerator.ViewModel.Other.Interfaces;
using QuizGenerator.ViewModel.ViewModels.Models;

namespace QuizGenerator.ViewModel.Other;

public class UserAnswerEvaluator : IUserAnswerEvaluator
{
	public bool IsCorrect(QuestionType questionType, AnswerDetailViewModel answerDetailViewModel)
	{
		if (questionType == QuestionType.Open)
		{
			if (answerDetailViewModel.UserAnswer == null ||
				string.IsNullOrEmpty(answerDetailViewModel.UserAnswer.Text))
			{
				return false;
			}

			var answerText = FormatedAnswer(answerDetailViewModel.Text);
			var userText = FormatedAnswer(answerDetailViewModel.UserAnswer.Text);
			return answerDetailViewModel.IsCorrect &&
					answerText == userText;
		}

		if (answerDetailViewModel.UserAnswer == null)
		{
			return false;
		}

		return answerDetailViewModel.IsCorrect == answerDetailViewModel.UserAnswer.IsSelected;
	}

	public float CalculatePrice(QuestionViewModel questionViewModel)
	{
		if (questionViewModel.QuestionDetails.Count == 0)
		{
			return questionViewModel.EvaluationPrice;
		}
		float result = 0.0f;

		float score = questionViewModel.EvaluationPrice / (float)questionViewModel.QuestionDetails.Count;
		foreach (var questionDetail in questionViewModel.QuestionDetails)
		{
			if (questionDetail.AnswerDetails.Count == 0 ||
				questionDetail.AnswerDetails.All(a => !a.IsCorrect))
			{
				result += score;
				continue;
			}

			result += CalculatePriceQuestionDetail(
				questionViewModel.QuestionType,
				score,
				questionDetail);
		}

		return result;
	}

	public bool Validate(QuestionViewModel questionViewModel)
	{
		return questionViewModel.QuestionType switch
		{
			QuestionType.One => ValidateOneOrMany(questionViewModel),
			QuestionType.Open => ValidateOpen(questionViewModel),
			QuestionType.Many => ValidateOneOrMany(questionViewModel),
			_ => true
		};
	}

	private bool ValidateOneOrMany(QuestionViewModel questionViewModel) =>
		questionViewModel.QuestionDetails.All(
			qd => qd.AnswerDetails.Any(a => a.UserAnswer.IsSelected));

	private bool ValidateOpen(QuestionViewModel questionViewModel) =>
		questionViewModel.QuestionDetails.All(
			qd => qd.AnswerDetails.Any(a => !string.IsNullOrEmpty(a.UserAnswer.Text)));

	private string FormatedAnswer(string text) =>
		text.Trim().ToLower();

	private float CalculatePriceQuestionDetail(
		QuestionType questionType,
		float maxPrice,
		QuestionDetailViewModel questionDetailViewModel)
	{
		return questionType switch
		{
			QuestionType.One => CalculatePriceOne(maxPrice, questionDetailViewModel),
			QuestionType.Open => CalculatePriceOpen(maxPrice, questionDetailViewModel),
			QuestionType.Many => CalculatePriceMany(maxPrice, questionDetailViewModel),
			_ => maxPrice
		};
	}

	private float CalculatePriceOpen(
		float maxPrice, 
		QuestionDetailViewModel questionDetailViewModel)
	{
		if (questionDetailViewModel.AnswerDetails
				.Any(a => IsCorrect(QuestionType.Open, a)))
		{
			return maxPrice;
		}

		return 0.0f;
	}

	private float CalculatePriceOne(
		float maxPrice,
		QuestionDetailViewModel questionDetailViewModel)
	{
		if (questionDetailViewModel.AnswerDetails
				.All(a => IsCorrect(QuestionType.One, a)))
		{
			return maxPrice;
		}

		return 0.0f;
	}

	private float CalculatePriceMany(
		float maxPrice,
		QuestionDetailViewModel questionDetailViewModel)
	{
		var countAnswers = questionDetailViewModel.AnswerDetails.Count;
		if (countAnswers == 0)
		{
			return maxPrice;
		}

		var priceAnswer = maxPrice / countAnswers;
		var countIncorrect = questionDetailViewModel.AnswerDetails
								.Count(a => !IsCorrect(QuestionType.Many, a));
		return maxPrice - (countIncorrect * priceAnswer);
	}
}
