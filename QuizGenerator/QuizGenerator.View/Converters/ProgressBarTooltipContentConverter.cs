using System.Globalization;
using System.Windows.Data;

namespace QuizGenerator.View.Converters;

public class ProgressBarTooltipContentConverter : IMultiValueConverter
{
	public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
	{
		if (values.Length == 2 &&
			double.TryParse(values[0]?.ToString(), out double value) &&
			double.TryParse(values[1]?.ToString(), out double maximum))
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
