using System.Windows.Media;

namespace QuizGenerator.View.Models;

public class PieChartData
{
	public Brush Brush { get; set; }
	public string Name { get; set; }
	public int Weight { get; set; }

	public PieChartData(string name, int weight, Brush? brush = null)
	{
		Name = name;
		Weight = weight;

		if (brush == null)
		{
			var rand = new Random();
			
			var r = (byte)rand.Next(0, 256);
			var g = (byte)rand.Next(0, 256);
			var b = (byte)rand.Next(0, 256);
			this.Brush = new SolidColorBrush(Color.FromRgb(r, g, b));
		}
		else
		{
			this.Brush = brush;
		}
	}

	public PieChartData() :
		this(string.Empty, 0)
	{
	
	}

}
