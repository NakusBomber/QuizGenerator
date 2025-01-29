using QuizGenerator.ViewModel.ViewModels.Bases;
using System.Windows.Media;

namespace QuizGenerator.View.Models;

public class PieChartData : ObservableObject
{
	private Brush _brush;
	public Brush Brush
	{
		get => _brush;
		set
		{
			_brush = value;
			OnPropertyChanged();
		}
	}

	private string _name;
	public string Name
	{
		get => _name;
		set
		{
			_name = value;
			OnPropertyChanged();
		}
	}

	private int _weight;
	public int Weight
	{
		get => _weight;
		set
		{
			_weight = value;
			OnPropertyChanged();
		}
	}


	public PieChartData(string name, int weight, Brush? brush = null)
	{
		_name = name;
		_weight = weight;

		if (brush == null)
		{
			var rand = new Random();
			
			var r = (byte)rand.Next(0, 256);
			var g = (byte)rand.Next(0, 256);
			var b = (byte)rand.Next(0, 256);
			this._brush = new SolidColorBrush(Color.FromRgb(r, g, b));
		}
		else
		{
			this._brush = brush;
		}
		this._brush.Freeze();
	}

	public PieChartData() :
		this(string.Empty, 0)
	{
	
	}

}
