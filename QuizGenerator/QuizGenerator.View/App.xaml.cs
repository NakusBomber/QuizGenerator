using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using QuizGenerator.DAL;
using QuizGenerator.Model.Entities;
using QuizGenerator.Model.Interfaces;
using QuizGenerator.View.Services;
using QuizGenerator.View.Views.Windows;
using QuizGenerator.ViewModel.Other;
using QuizGenerator.ViewModel.Other.Interfaces;
using QuizGenerator.ViewModel.ViewModels;
using QuizGenerator.ViewModel.ViewModels.Pages;
using QuizGenerator.ViewModel.ViewModels.Windows;
using System.IO;
using System.Windows;

namespace QuizGenerator.View;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
	private readonly IHost _host;
	private bool _debug = false;

	public App()
	{
		var hostBuilder = Host.CreateDefaultBuilder();
		hostBuilder.ConfigureServices(ConfigureServices);
		_host = hostBuilder.Build();

		StartNavigation();
	}

	protected override void OnStartup(StartupEventArgs e)
	{
		if (!_debug)
		{
			using var scope = _host.Services.CreateScope();
			var context = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
			context.Database.Migrate();
		}

		var startWindow = _host.Services.GetRequiredService<StartWindow>();
		startWindow.DataContext = _host.Services.GetRequiredService<NavigationViewModel>();
		startWindow.Show();

		base.OnStartup(e);
	}

	private void ConfigureServices(IServiceCollection services)
	{
		services.AddSingleton<StartWindow>();

		ConfigureApplicationContext(services);

		ConfigureRepositories(services);

		ConfigureUnitOfWork(services);

		ConfigureNavigation(services);
	}

	private void ConfigureApplicationContext(IServiceCollection services)
	{
		if (_debug)
		{
			services.AddDbContext<ApplicationContext>(
				optionsBuilder => optionsBuilder
					.UseInMemoryDatabase("databaseInMemory")
					.UseLazyLoadingProxies(),
				ServiceLifetime.Singleton);
		}
		else
		{
			var path = Path.Combine(Environment.CurrentDirectory, "database.db");
			services.AddDbContext<ApplicationContext>(
				optionsBuilder => optionsBuilder
					.UseSqlite($"Filename={path}")
					.UseLazyLoadingProxies(),
				ServiceLifetime.Singleton);
		}
	}

	private void ConfigureRepositories(IServiceCollection services)
	{
		services.AddSingleton<IRepository<Quiz>>(
			sp => new GeneralRepository<Quiz>(sp.GetRequiredService<ApplicationContext>()));
		services.AddSingleton<IRepository<Question>>(
			sp => new GeneralRepository<Question>(sp.GetRequiredService<ApplicationContext>()));
		services.AddSingleton<IRepository<QuestionDetail>>(
			sp => new GeneralRepository<QuestionDetail>(sp.GetRequiredService<ApplicationContext>()));
		services.AddSingleton<IRepository<AnswerDetail>>(
			sp => new GeneralRepository<AnswerDetail>(sp.GetRequiredService<ApplicationContext>()));
	}

	private void ConfigureUnitOfWork(IServiceCollection services)
	{
		services.AddSingleton<IUnitOfWork>(
			sp => new UnitOfWork(
				sp.GetRequiredService<ApplicationContext>(),
				sp.GetRequiredService<IRepository<Quiz>>(),
				sp.GetRequiredService<IRepository<Question>>(),
				sp.GetRequiredService<IRepository<QuestionDetail>>(),
				sp.GetRequiredService<IRepository<AnswerDetail>>()));
	}

	private void ConfigureNavigation(IServiceCollection services)
	{
		services.AddSingleton<NavigationStore>();
		services.AddSingleton<INavigationJournal, NavigationJournal>();

		services.AddSingleton<IBackNavigationService, BackNavigationService>();

		RegisterDialogNavigationServices(services);

		RegisterAnswerDetailNavigationService(services);
		RegisterQuestionDetailNavigationService(services);
		RegisterQuestionPageNavigationService(services);
		RegisterTrainingNavigationService(services);
		RegisterQuizPageNavigationService(services);
		RegisterSelectNavigationService(services);
		RegisterStartNavigationService(services);

		services.AddSingleton<NavigationViewModel>();
	}

	private void RegisterDialogNavigationServices(IServiceCollection services)
	{
		services.AddSingleton<IWindowNavigationService<ConfirmationWindowViewModel, bool>>(
			new ConfirmationWindowNavigationService());
	}

	private void RegisterQuestionPageNavigationService(IServiceCollection services)
	{
		services.AddSingleton<IParameterNavigationService<Guid?, QuestionPageViewModel>>(sp =>
			new ParameterNavigationService<Guid?, QuestionPageViewModel>(
				sp.GetRequiredService<NavigationStore>(),
				sp.GetRequiredService<INavigationJournal>(),
				p => new QuestionPageViewModel(p, 
					sp.GetRequiredService<IUnitOfWork>(),
					sp.GetRequiredService<IParameterNavigationService<Guid?, QuestionDetailPageViewModel>>(),
					sp.GetRequiredService<IBackNavigationService>())
			));
	}

	private void RegisterTrainingNavigationService(IServiceCollection services)
	{
		services.AddSingleton<IParameterNavigationService<Guid?, TrainingViewModel>>(sp =>
			new ParameterNavigationService<Guid?, TrainingViewModel>(
				sp.GetRequiredService<NavigationStore>(),
				sp.GetRequiredService<INavigationJournal>(),
				p => new TrainingViewModel(p)));
	}

	private void RegisterQuizPageNavigationService(IServiceCollection services)
	{
		services.AddSingleton<IParameterNavigationService<Guid?, QuizPageViewModel>>(sp =>
			new ParameterNavigationService<Guid?, QuizPageViewModel>(
				sp.GetRequiredService<NavigationStore>(),
				sp.GetRequiredService<INavigationJournal>(),
				p => new QuizPageViewModel(
					p,
					sp.GetRequiredService<IUnitOfWork>(),
					sp.GetRequiredService<IParameterNavigationService<Guid?, TrainingViewModel>>(),
					sp.GetRequiredService<IParameterNavigationService<Guid?, QuestionPageViewModel>>(),
					sp.GetRequiredService<IBackNavigationService>())
			));
	}

	private void RegisterSelectNavigationService(IServiceCollection services)
	{
		services.AddSingleton<INavigationService<SelectViewModel>>(sp =>
			new NavigationService<SelectViewModel>(
				sp.GetRequiredService<NavigationStore>(),
				sp.GetRequiredService<INavigationJournal>(),
				() => new SelectViewModel(sp.GetRequiredService<IUnitOfWork>(),
										 sp.GetRequiredService<IParameterNavigationService<Guid?, QuizPageViewModel>>())
			));
	}

	private void RegisterStartNavigationService(IServiceCollection services)
	{
		services.AddSingleton<INavigationService<StartViewModel>>(sp =>
			new NavigationService<StartViewModel>(
				sp.GetRequiredService<NavigationStore>(),
				sp.GetRequiredService<INavigationJournal>(),
				() => new StartViewModel(
					sp.GetRequiredService<IUnitOfWork>(),
					sp.GetRequiredService<IParameterNavigationService<Guid?, QuizPageViewModel>>(),
					sp.GetRequiredService<INavigationService<SelectViewModel>>())
			));
	}

	private void RegisterQuestionDetailNavigationService(IServiceCollection services)
	{
		services.AddSingleton<IParameterNavigationService<Guid?, QuestionDetailPageViewModel>>(sp =>
			new ParameterNavigationService<Guid?, QuestionDetailPageViewModel>(
				sp.GetRequiredService<NavigationStore>(),
				sp.GetRequiredService<INavigationJournal>(),
				p => new QuestionDetailPageViewModel(
					p,
					sp.GetRequiredService<IUnitOfWork>(),
					sp.GetRequiredService<IParameterNavigationService<Guid?, AnswerDetailPageViewModel>>(),
					sp.GetRequiredService<IBackNavigationService>())
			));
	}

	private void RegisterAnswerDetailNavigationService(IServiceCollection services)
	{
		services.AddSingleton<IParameterNavigationService<Guid?, AnswerDetailPageViewModel>>(sp =>
			new ParameterNavigationService<Guid?, AnswerDetailPageViewModel>(
				sp.GetRequiredService<NavigationStore>(),
				sp.GetRequiredService<INavigationJournal>(),
				p => new AnswerDetailPageViewModel(
					p,
					sp.GetRequiredService<IUnitOfWork>(),
					sp.GetRequiredService<IBackNavigationService>())
			));
	}


	private void StartNavigation()
	{
		var startNavigationService = _host.Services.GetService<INavigationService<StartViewModel>>();
		var navigationJournal = _host.Services.GetService<INavigationJournal>();

		if (startNavigationService == null || navigationJournal == null)
		{
			throw new InvalidOperationException("Invalid started navigation");
		}

		startNavigationService.Navigate();
		navigationJournal.Clear();
	}
}