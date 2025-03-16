using QuizGenerator.Model.Entities;
using QuizGenerator.ViewModel.ViewModels.Models;
using System.Collections.ObjectModel;

namespace QuizGenerator.ViewModel.Other;

public static class ModelToViewModelMapperExtensions
{
	public static QuizViewModel ToVM(this Quiz quiz)
	{
		return new QuizViewModel(quiz)
		{
			Questions = new ObservableCollection<QuestionViewModel>(
				quiz.Questions.Select(q => q.ToVM()))
		};
	}

	public static QuestionViewModel ToVM(this Question question)
	{
		return new QuestionViewModel(question)
		{
			QuestionDetails = new ObservableCollection<QuestionDetailViewModel>(
				question.QuestionDetails.Select(qd => qd.ToVM()))
		};
	}

	public static QuestionDetailViewModel ToVM(this QuestionDetail questionDetail)
	{
		return new QuestionDetailViewModel(questionDetail)
		{
			AnswerDetails = new ObservableCollection<AnswerDetailViewModel>(
				questionDetail.AnswerDetails.Select(a => new AnswerDetailViewModel(a)))
		};
	}

	public static ObservableCollection<QuestionViewModel> ToVMs(
		this IEnumerable<Question> questions)
	{
		return new ObservableCollection<QuestionViewModel>(
			questions.Select(q => q.ToVM()));
	}

	public static ObservableCollection<QuestionDetailViewModel> ToVMs(
		this IEnumerable<QuestionDetail> questionDetails)
	{
		return new ObservableCollection<QuestionDetailViewModel>(
			questionDetails.Select(qd => qd.ToVM()));
	}

	public static ObservableCollection<AnswerDetailViewModel> ToVMs(
		this IEnumerable<AnswerDetail> answers)
	{
		return new ObservableCollection<AnswerDetailViewModel>(
			answers.Select(a => new AnswerDetailViewModel(a)));
	}
}
