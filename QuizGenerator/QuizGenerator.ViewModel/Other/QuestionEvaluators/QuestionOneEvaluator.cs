using QuizGenerator.Model.Entities;
using QuizGenerator.ViewModel.Other.Interfaces;
using QuizGenerator.ViewModel.ViewModels.Models;

namespace QuizGenerator.ViewModel.Other.QuestionEvaluators;

public class QuestionOneEvaluator : IQuestionEvaluator
{
	public virtual QuestionType QuestionType => QuestionType.One;

	public virtual float CalculatePrice(float maxPrice, QuestionDetailViewModel questionDetailViewModel)
	{
		if (questionDetailViewModel.AnswerDetails.All(IsCorrect))
		{
			return maxPrice;
		}

		return 0.0f;
	}

	public bool IsCorrect(AnswerDetailViewModel answerDetailViewModel) =>
		answerDetailViewModel.IsCorrect == answerDetailViewModel.UserAnswer.IsSelected;

	public bool Validate(QuestionViewModel questionViewModel) =>
		questionViewModel.QuestionDetails
			.Where(q => q.AnswerDetails.Count > 0)
			.All(qd => qd.AnswerDetails.Any(a => a.UserAnswer.IsSelected));
}
