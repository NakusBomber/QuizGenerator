using System.Globalization;
using System.Windows.Data;

namespace QuizGenerator.View.Converters;

public class ScoreConverter : IMultiValueConverter
{
	public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
	{
		if (values.Length == 2 &&
			values[0] is float current &&
			values[1] is int max)
		{
			var firstValue = (current == 0f || current == max) 
								? string.Format("{0:F0}", current)
								: string.Format("{0:F2}", current).Replace(",00", "").Replace(".00", "");
			return $"{firstValue.Replace(',', '.')}/{max}";
		}
		return string.Empty;
	}

	public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
