using QuizGenerator.Model.Entities;
using QuizGenerator.ViewModel.Other.Interfaces;
using QuizGenerator.ViewModel.ViewModels.Models;

namespace QuizGenerator.ViewModel.Other.QuestionEvaluators;

public class QuestionManyEvaluator : QuestionOneEvaluator
{
	public override QuestionType QuestionType => QuestionType.Many;

	public override float CalculatePrice(
		float maxPrice, 
		QuestionDetailViewModel questionDetailViewModel)
	{
		var countAnswers = questionDetailViewModel.AnswerDetails.Count;
		if (countAnswers == 0)
		{
			return maxPrice;
		}

		var priceAnswer = maxPrice / countAnswers;
		var countIncorrect = questionDetailViewModel.AnswerDetails.Count(a => !IsCorrect(a));

		return maxPrice - (countIncorrect * priceAnswer);
	}
}
