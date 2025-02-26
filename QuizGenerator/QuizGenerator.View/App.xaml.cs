using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using QuizGenerator.Model.Entities;
using QuizGenerator.Model.Interfaces;
using QuizGenerator.Model.Models.Fakes;
using QuizGenerator.View.Helpers;
using QuizGenerator.View.Views.Windows;
using QuizGenerator.ViewModel.Commands;
using QuizGenerator.ViewModel.Other;
using QuizGenerator.ViewModel.Other.Interfaces;
using QuizGenerator.ViewModel.ViewModels;
using QuizGenerator.ViewModel.ViewModels.Bases;
using System.Configuration;
using System.Data;
using System.Windows;

namespace QuizGenerator.View;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
	private readonly IHost _host;

	public App()
	{
		var hostBuilder = Host.CreateDefaultBuilder();
		hostBuilder.ConfigureServices(ConfigureServices);
		_host = hostBuilder.Build();

		StartNavigation();
	}

	protected override void OnStartup(StartupEventArgs e)
	{
		var startWindow = _host.Services.GetRequiredService<StartWindow>();
		startWindow.DataContext = _host.Services.GetRequiredService<NavigationViewModel>();
		startWindow.Show();

		base.OnStartup(e);
	}

	private void ConfigureServices(IServiceCollection services)
	{
		services.AddSingleton<StartWindow>();

		ConfigureUnitOfWork(services);

		ConfigureNavigation(services);
	}

	private void ConfigureUnitOfWork(IServiceCollection services)
	{
		services.AddSingleton<IUnitOfWork>(TestData.GetUnitOfWork(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(2)));
	}

	private void ConfigureNavigation(IServiceCollection services)
	{
		services.AddSingleton<NavigationStore>();
		services.AddSingleton<INavigationJournal, NavigationJournal>();

		services.AddSingleton<IBackNavigationService, BackNavigationService>();

		RegisterQuestionPageNavigationService(services);
		RegisterTrainingNavigationService(services);
		RegisterQuizPageNavigationService(services);
		RegisterSelectNavigationService(services);
		RegisterStartNavigationService(services);

		services.AddSingleton<NavigationViewModel>();
	}

	private void RegisterQuestionPageNavigationService(IServiceCollection services)
	{
		services.AddSingleton<IParameterNavigationService<Guid?, QuestionPageViewModel>>(sp =>
			new ParameterNavigationService<Guid?, QuestionPageViewModel>(
				sp.GetRequiredService<NavigationStore>(),
				sp.GetRequiredService<INavigationJournal>(),
				p => new QuestionPageViewModel(p, sp.GetRequiredService<IUnitOfWork>())
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
					sp.GetRequiredService<IParameterNavigationService<Guid?, QuestionPageViewModel>>())
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