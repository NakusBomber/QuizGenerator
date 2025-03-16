using QuizGenerator.View.Views.Dialogs;
using QuizGenerator.ViewModel.Other;
using QuizGenerator.ViewModel.ViewModels.Windows;

namespace QuizGenerator.View.Services;

public class ConfirmationWindowNavigationService :
	WindowNavigationServiceBase<ConfirmationWindowViewModel, bool>
{
	protected override Type WindowType => typeof(ConfirmationDialog);

	public override bool Navigate(ConfirmationWindowViewModel viewModel) =>
		ShowWindow(viewModel).IsSuccess;
}