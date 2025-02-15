using QuizGenerator.Model.Entities;
using QuizGenerator.Model.Interfaces;
using QuizGenerator.Model.Models.Fakes;

namespace QuizGenerator.View.Helpers;

public static class TestData
{
	public static UnitOfWorkFake GetUnitOfWork(TimeSpan delay, TimeSpan saveDelay)
	{
		var exampleQuiz1 = new Quiz("Quiz with questions");
		var question1 = new Question(exampleQuiz1, 1, QuestionType.OneMore, 0);
		var questionText1 = new QuestionDetail(question1, "QuestionText1");
		var answerText1 = new AnswerDetail(questionText1, "Answer1", true);
		var answerText2 = new AnswerDetail(questionText1, "Answer2");
		var answerText3 = new AnswerDetail(questionText1, "Answer3");
		var answerText4 = new AnswerDetail(questionText1, "Answer4");
		questionText1.AnswerDetails = new List<AnswerDetail> { answerText1, answerText2, answerText3, answerText4 };
		question1.QuestionDetails = new List<QuestionDetail> { questionText1 };
		var question2 = new Question(exampleQuiz1, 1, QuestionType.Open, 1);
		exampleQuiz1.Questions = new List<Question> { question1, question2 };

		var exampleQuiz2 = new Quiz("Another quiz");

		var quizRepository = new RepositoryFake<Quiz>(new HashSet<Quiz>([exampleQuiz1, exampleQuiz2]), delay);
		var questionRepository = new RepositoryFake<Question>(new HashSet<Question>([question1, question2]), delay);
		var questionDetailRepository = new RepositoryFake<QuestionDetail>(new HashSet<QuestionDetail>([questionText1]), delay);
		var answerDetailRepository = new RepositoryFake<AnswerDetail>(new HashSet<AnswerDetail>([answerText1, answerText2, answerText3, answerText4]), delay);

		return new UnitOfWorkFake(
			saveDelay,
			quizRepository,
			questionRepository,
			questionDetailRepository,
			answerDetailRepository);
	}
}
