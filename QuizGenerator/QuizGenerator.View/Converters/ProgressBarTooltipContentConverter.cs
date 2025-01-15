using System.Globalization;
using System.Windows.Data;

namespace QuizGenerator.View.Converters;

public class ProgressBarTooltipContentConverter : IMultiValueConverter
{
	public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
	{
		if (values.Length == 2 &&
			values[0] is double value &&
			values[1] is double maximum)
		{
			return $"{value} / {maximum}";
		}

		return string.Empty;
	}

	public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
