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
			var navigationStore = new NavigationStore();
			var navigationJournal = new NavigationJournal();

			var selectNavigationService = new NavigationService<SelectViewModel>(
				navigationStore, 
				navigationJournal,
				() => new SelectViewModel());
			var quizNavigationService = new NavigationService<QuizViewModel>(
				navigationStore, 
				navigationJournal,
				() => new QuizViewModel());
			var startNavigationService = new NavigationService<StartViewModel>(
				navigationStore, 
				navigationJournal,
				() => new StartViewModel(quizNavigationService, selectNavigationService));
			var backNavigationService = new BackNavigationService(navigationStore, navigationJournal);

			MainWindow = new StartWindow();
			var navigationVM = new NavigationViewModel(navigationStore, navigationJournal, backNavigationService);
			MainWindow.DataContext = navigationVM;
			startNavigationService.Navigate();
			navigationJournal.Clear();

			MainWindow.Show();
		}
	}

}
