﻿using QuizGenerator.Model.Entities;
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

			var selectNavigationService = new NavigationService<SelectViewModel>(
				navigationStore, 
				navigationJournal,
				() => new SelectViewModel(unitOfWork));
			var quizNavigationService = new NavigationService<QuizViewModel>(
				navigationStore, 
				navigationJournal,
				() => new QuizViewModel());
			var startNavigationService = new NavigationService<StartViewModel>(
				navigationStore, 
				navigationJournal,
				() => new StartViewModel(unitOfWork, quizNavigationService, selectNavigationService));
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

			var exampleQuiz1 = new Quiz("Last quiz");
			var exampleQuiz2 = new Quiz("Another quiz");

			var quizRepository = new RepositoryFake<Quiz>(new HashSet<Quiz>([exampleQuiz1, exampleQuiz2]), delay);
			var questionRepository = new RepositoryFake<Question>(delay);
			var questionDetailRepository = new RepositoryFake<QuestionDetail>(delay);
			var answerDetailRepositiry = new RepositoryFake<AnswerDetail>(delay);

			return new UnitOfWorkFake(
				saveDelay, 
				quizRepository, 
				questionRepository, 
				questionDetailRepository, 
				answerDetailRepositiry);
		}
	}

}
