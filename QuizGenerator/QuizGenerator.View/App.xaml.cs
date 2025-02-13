using QuizGenerator.Model.Entities;
using QuizGenerator.Model.Interfaces;
using QuizGenerator.Model.Models.Fakes;
using QuizGenerator.View.Views.Windows;
using QuizGenerator.ViewModel.Commands;
using QuizGenerator.ViewModel.Other;
using QuizGenerator.ViewModel.ViewModels;
using System.Configuration;
using System.Data;
using System.Windows;

namespace QuizGenerator.View
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		private void Application_Startup(object sender, StartupEventArgs e)
		{
			var unitOfWork = GetUnitOfWork();

			var navigationStore = new NavigationStore();
			var navigationJournal = new NavigationJournal();

			var quizParameterNavigationService = new ParameterNavigationService<Guid?, QuizPageViewModel>(
				navigationStore,
				navigationJournal,
				(p) => new QuizPageViewModel(p, unitOfWork));
			var selectNavigationService = new NavigationService<SelectViewModel>(
				navigationStore, 
				navigationJournal,
				() => new SelectViewModel(unitOfWork, quizParameterNavigationService));
			var startNavigationService = new NavigationService<StartViewModel>(
				navigationStore, 
				navigationJournal,
				() => new StartViewModel(unitOfWork, quizParameterNavigationService, selectNavigationService));
			var backNavigationService = new BackNavigationService(navigationStore, navigationJournal);

			MainWindow = new StartWindow();
			var navigationVM = new NavigationViewModel(navigationStore, navigationJournal, backNavigationService);
			MainWindow.DataContext = navigationVM;
			startNavigationService.Navigate();
			navigationJournal.Clear();

			MainWindow.Show();
		}

		private IUnitOfWork GetUnitOfWork()
		{
			TimeSpan delay = TimeSpan.FromSeconds(1);
			TimeSpan saveDelay = TimeSpan.FromSeconds(2);

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
			var questionRepository = new RepositoryFake<Question>(delay);
			var questionDetailRepository = new RepositoryFake<QuestionDetail>(delay);
			var answerDetailRepository = new RepositoryFake<AnswerDetail>(delay);

			return new UnitOfWorkFake(
				saveDelay, 
				quizRepository, 
				questionRepository, 
				questionDetailRepository, 
				answerDetailRepository);
		}
	}

}
