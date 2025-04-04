﻿using System.Globalization;
using System.Windows.Data;

namespace QuizGenerator.View.Converters;

public class BooleanInvertedConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value is bool b)
		{
			return !b;
		}
		return value;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		return Convert(value, targetType, parameter, culture);
	}
}
