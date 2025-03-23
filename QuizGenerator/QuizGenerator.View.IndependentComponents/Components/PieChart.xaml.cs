using QuizGenerator.View.IndependentComponents.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.DirectoryServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace QuizGenerator.View.IndependentComponents.Components
{
	/// <summary>
	/// Interaction logic for PieChart.xaml
	/// </summary>
	public partial class PieChart : UserControl
	{
		private class PieChartSegment
		{
			public ArcSegment ArcSegment { get; set; }
			public double StartAngle { get; set; }
			public double EndAngle { get; set; }

			public PieChartSegment(ArcSegment arcSegment, double startAngle, double endAngle)
			{
				ArcSegment = arcSegment;
				StartAngle = startAngle;
				EndAngle = endAngle;
			}
		}

		private readonly TimeSpan _oneAngleAnimationDuration = TimeSpan.FromMilliseconds(2.5d);
		private AnimationClock? _clock;
		private double _radius = 0;

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			
			CreateSectors();
		}

		public ObservableCollection<PieChartData> Data
		{
			get { return (ObservableCollection<PieChartData>)GetValue(DataProperty); }
			set { SetValue(DataProperty, value); }
		}

		public static readonly DependencyProperty DataProperty =
			DependencyProperty.Register("Data", typeof(ObservableCollection<PieChartData>), typeof(PieChart), new PropertyMetadata(null, OnDataChanged));


		public double StartAngle
		{
			get { return (double)GetValue(StartAngleProperty); }
			set { SetValue(StartAngleProperty, value); }

		}

		public static readonly DependencyProperty StartAngleProperty =
			DependencyProperty.Register("StartAngle", typeof(double), typeof(PieChart), new PropertyMetadata(0d));


		public PieChart()
		{
			InitializeComponent();
		}

		private static void OnDataChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			if (o is PieChart chart)
			{

				chart.OnDataCollectionChanged(
					e.OldValue as ObservableCollection<PieChartData>,
					e.NewValue as ObservableCollection<PieChartData>);
			}
		}

		private void OnDataCollectionChanged(
			ObservableCollection<PieChartData>? oldCollection,
			ObservableCollection<PieChartData>? newCollection)
		{
			if (oldCollection != null)
			{
				oldCollection.CollectionChanged -= Data_CollectionChanged;
				foreach (var item in oldCollection)
				{
					item.PropertyChanged -= Data_PropertyChanged;
				}
			}

			if (newCollection != null)
			{
				newCollection.CollectionChanged += Data_CollectionChanged;
				foreach (var item in newCollection)
				{
					item.PropertyChanged += Data_PropertyChanged;
				}
			}

			CreateSectors();
		}

		private void Data_PropertyChanged(object? sender, PropertyChangedEventArgs e)
		{
			CreateSectors();
		}

		private void Data_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			if (e.OldItems != null)
			{
				foreach (PieChartData oldItem in e.OldItems)
				{
					oldItem.PropertyChanged -= Data_PropertyChanged;
				}
			}

			if (e.NewItems != null)
			{
				foreach (PieChartData newItem in e.NewItems)
				{
					newItem.PropertyChanged += Data_PropertyChanged;
				}
			}
			

			CreateSectors();
		}

		private void CreateSectors()
		{
			if (Template.FindName("_canvas", this) is Canvas canvas &&
				Data != null)
			{
				canvas.Children.Clear();

				var startAngleDegree = StartAngle;
				var sumItems = (double)Data.Sum(d => d.Weight);

				_radius = Width / 2;

				var segmentsQueue = new Queue<PieChartSegment>();

				foreach (var item in Data)
				{
					var centerPoint = new Point(_radius, _radius);

					var startPointOfArc = CalculateArcPoint(startAngleDegree);

					var angleDegree = ((item.Weight / sumItems) * 360.0);
					var endAngleDegree = startAngleDegree + angleDegree;

					var arcSegment = CreateArcSegment(startPointOfArc, angleDegree > 180);

					var pathFigure = new PathFigure(
						centerPoint,
						[new LineSegment(startPointOfArc, true), arcSegment],
						true);

					var pathGeometry = new PathGeometry([pathFigure]);
					var path = new Path
					{
						Data = pathGeometry,
						Fill = item.Brush,
						ToolTip = item.Name
					};
					canvas.Children.Add(path);

					segmentsQueue.Enqueue(new PieChartSegment(arcSegment, startAngleDegree, endAngleDegree));

					startAngleDegree += angleDegree;
				}

				AnimateNextSegment(segmentsQueue);
			}
		}

		private void AnimateNextSegment(Queue<PieChartSegment> queue)
		{
			if (queue.Count == 0)
			{
				return;
			}

			var sector = queue.Dequeue();

			// If sector 100% of all circle, then need shrink angle to 359.9(9)
			// Otherwise start point and end point it is same point
			if (Math.Abs(sector.EndAngle - sector.StartAngle) >= 360)
			{
				sector.EndAngle -= 0.00001;
			}

			var durationAnimation = Math.Abs(sector.EndAngle - sector.StartAngle) * _oneAngleAnimationDuration;
			var animation = new DoubleAnimation(sector.StartAngle, sector.EndAngle, durationAnimation)
			{
				EasingFunction = new CircleEase { EasingMode = EasingMode.EaseOut }
			};

			_clock = animation.CreateClock();
			_clock.CurrentTimeInvalidated += (s, e) =>
			{
				if (s is AnimationClock animationClock)
				{
					var progress = animationClock.CurrentProgress ?? 0.0;

					// Interpolating
					var currentAngle = sector.StartAngle + (sector.EndAngle - sector.StartAngle) * progress;

					var endPoint = CalculateArcPoint(currentAngle);
					var isLargeArc = Math.Abs(currentAngle - sector.StartAngle) > 180;

					sector.ArcSegment.Size = new Size(_radius, _radius);
					sector.ArcSegment.IsLargeArc = isLargeArc;
					sector.ArcSegment.Point = endPoint;
				}
			};

			_clock.Completed += (s, e) => AnimateNextSegment(queue);

			_clock.Controller.Begin();
		}

		private Point CalculateArcPoint(double angleDegree)
		{
			var angleRadians = angleDegree * Math.PI / 180.0;
			return new Point(
				Math.Cos(angleRadians) * _radius + _radius,
				Math.Sin(angleRadians) * _radius + _radius);
		}

		private ArcSegment CreateArcSegment(Point startPointOfArc, bool isLargeArc)
		{
			return new ArcSegment(startPointOfArc, new Size(_radius, _radius),
								0, // After animation set up normal value
								isLargeArc, SweepDirection.Clockwise, true);
		}
	}
}

