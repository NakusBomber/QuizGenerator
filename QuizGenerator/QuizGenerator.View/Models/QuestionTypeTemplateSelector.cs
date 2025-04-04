using QuizGenerator.Model.Entities;
using QuizGenerator.ViewModel.ViewModels.Models;
using System.Windows;
using System.Windows.Controls;

namespace QuizGenerator.View.Models;

public class QuestionTypeTemplateSelector : DataTemplateSelector
{
	public DataTemplate? OneTemplate { get; set; }
	public DataTemplate? OpenTemplate { get; set; }
	public DataTemplate? ManyTemplate { get; set; }

	public override DataTemplate? SelectTemplate(object item, DependencyObject container)
	{
		if (item is QuestionViewModel question)
		{
			return question.QuestionType switch
			{
				QuestionType.One => OneTemplate,
				QuestionType.Open => OpenTemplate,
				QuestionType.Many => ManyTemplate,
				_ => base.SelectTemplate(item, container)
			};
		}

		return base.SelectTemplate(item, container);
	}
}
