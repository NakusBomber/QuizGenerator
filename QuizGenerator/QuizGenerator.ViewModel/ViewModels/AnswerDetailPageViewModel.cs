using QuizGenerator.Model.Interfaces;
using QuizGenerator.ViewModel.Other.Interfaces;
using QuizGenerator.ViewModel.ViewModels.Bases;

namespace QuizGenerator.ViewModel.ViewModels;

public class AnswerDetailPageViewModel : SavingStateViewModel
{

	public AnswerDetailPageViewModel(
		Guid? id,
		IUnitOfWork unitOfWork,
		IBackNavigationService backNavigationService)
	{

	}
}
