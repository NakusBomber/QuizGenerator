using QuizGenerator.Model.Entities;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace QuizGenerator.View.Models.Selectors;

public class QuestionTemplateSelector : DataTemplateSelector
{
	private DataTemplate _defaultTemplate = new();
	public DataTemplate OneMoreTemplate { get; set; }
	public DataTemplate OpenTemplate { get; set; }

	public override DataTemplate SelectTemplate(object item, DependencyObject container)
	{
		if (item is Question question)
		{
			switch (question.QuestionType)
			{
				case QuestionType.OneMore:
					return OneMoreTemplate;
				case QuestionType.Open:
					return OpenTemplate;
				default:
					break;
			}
		}
		return _defaultTemplate;
	}
}
