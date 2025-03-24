using QuizGenerator.Model.Entities;
using QuizGenerator.ViewModel.Other.Interfaces;
using QuizGenerator.ViewModel.Other.QuestionEvaluators;
using QuizGenerator.ViewModel.ViewModels.Models;

namespace QuizGenerator.ViewModel.Other;

public class UserAnswerEvaluator : IUserAnswerEvaluator
{
	private readonly IEnumerable<IQuestionEvaluator> _evaluators;

	public UserAnswerEvaluator(IEnumerable<IQuestionEvaluator> evaluators)
	{
		_evaluators = evaluators;
	}


	public bool IsCorrect(QuestionType questionType, AnswerDetailViewModel answerDetailViewModel)
	{
		if (answerDetailViewModel.UserAnswer == null)
		{
			return false;
		}

		var evaluator = GetEvaluator(questionType);
		return evaluator == null || evaluator.IsCorrect(answerDetailViewModel);
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

			var evaluator = GetEvaluator(questionViewModel.QuestionType);
			if (evaluator == null)
			{
				result += score;
				continue;
			}

			result += evaluator.CalculatePrice(score, questionDetail);
		}

		return result;
	}

	public bool Validate(QuestionViewModel questionViewModel)
	{
		var evaluator = GetEvaluator(questionViewModel.QuestionType);

		return evaluator == null || evaluator.Validate(questionViewModel);
	}

	private IQuestionEvaluator? GetEvaluator(QuestionType questionType) =>
		_evaluators.FirstOrDefault(e => e.QuestionType == questionType);

}
