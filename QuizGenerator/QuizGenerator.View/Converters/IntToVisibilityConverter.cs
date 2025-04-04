using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace QuizGenerator.View.Converters;

public class IntToVisibilityConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value is int i)
		{
			var result = i > 0;
			if (parameter != null && parameter.ToString() == "Invert")
			{
				result = !result;
			}

			return result ? Visibility.Visible : Visibility.Collapsed;
		}

		return Visibility.Collapsed;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
