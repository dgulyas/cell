using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Cell;
using Newtonsoft.Json;

namespace Display
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private List<Brush> colorOptions = new List<Brush> { Brushes.DarkRed, Brushes.Blue, Brushes.Green, Brushes.Orange, Brushes.Purple, Brushes.Brown };
		private Dictionary<string, Brush> m_playerColorMapping = new Dictionary<string, Brush>();
		private int SCALINGFACTOR = 100;
		public BoardState currentBoard = new BoardState();
		private string _filePath = Directory.GetParent(Directory.GetParent(System.IO.Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory)).FullName).FullName;
		public List<BoardState> boards = new List<BoardState>();
		public MainWindow()
		{
			_filePath = Directory.GetParent(Directory.GetParent(_filePath).FullName).FullName;
			DirectoryInfo di = new DirectoryInfo(_filePath + @"\v2\Results\");
			string[] text = System.IO.File.ReadAllLines(Directory.GetFiles(di.FullName)[0]);
			foreach (string t in text)
			{
				boards.Add(JsonConvert.DeserializeObject<BoardState>(t));
			}
			foreach(var player in boards[0].Forts.Select(x => x.FortOwner).Distinct().Where(x => x != null))
			{

				m_playerColorMapping.Add(player, PickBrush());
			}
			//var myLine = new Line();
			//myLine.Stroke = Brushes.LightSteelBlue;
			//myLine.X1 = 1;
			//myLine.X2 = 50;
			//myLine.Y1 = 1;
			//myLine.Y2 = 50;
			//myLine.StrokeThickness = 2;
			//Canvas.Children.Add(myLine);

			//LoadTestState();
			InitializeComponent();
			this.Height = (boards[0].Forts.Max(x => x.Location.Y)) * SCALINGFACTOR + 150;
			this.Width = (boards[0].Forts.Max(x => x.Location.X)) * SCALINGFACTOR + 150;


		}

		public void DrawBoardState()
		{

				Canvas.Children.Clear();

				foreach (var fort in currentBoard.Forts)
				{
					DrawFort(fort);
				}

				foreach (var gg in currentBoard.GuyGroups)
				{
					DrawGuyGroup(gg);
				}

			

		}

		public void DrawFort(Fort fort)
		{
			int size = 100;

			var circle = new Ellipse();
			circle.SetValue(Canvas.TopProperty, (double)(fort.Location.Y * SCALINGFACTOR));
			circle.SetValue(Canvas.LeftProperty, (double)(fort.Location.X * SCALINGFACTOR));
			circle.Height = size;
			circle.Width = circle.Height;
			circle.StrokeThickness = 2;
			if(fort.FortOwner == null)
			{
				circle.Stroke = Brushes.Black;
			}
			else
			{
				circle.Stroke = m_playerColorMapping[fort.FortOwner];
			}
			Canvas.Children.Add(circle);

			DrawText((fort.Location.X) * SCALINGFACTOR, (fort.Location.Y) * SCALINGFACTOR - 45, "BirthType: " + fort.BirthingType.ToString(), Colors.Black);
			DrawText((fort.Location.X) * SCALINGFACTOR, (fort.Location.Y) * SCALINGFACTOR - 15, "BirthRate: " + fort.BirthSpeed.ToString(), Colors.Black);
			DrawText((fort.Location.X) * SCALINGFACTOR , (fort.Location.Y) * SCALINGFACTOR - 30, "Owner: " + fort.FortOwner, Colors.Black);
			DrawText((fort.Location.X) * SCALINGFACTOR + size/4, (fort.Location.Y) * SCALINGFACTOR + size/4, "->: " + fort.DefendingGuys.Sum(g => g.Strength).ToString(), Colors.Black);
			DrawText((fort.Location.X) * SCALINGFACTOR + size/4, (fort.Location.Y) * SCALINGFACTOR + size/4 + 15, "+: " + fort.DefendingGuys.Sum(g => g.Health).ToString(), Colors.Black);
		}

		public void DrawGuyGroup(GuyGroup gg)
		{
			int size = Math.Min(10 * (int)(gg.Guys.Count), 40);
			var square = new Rectangle();
			int numTotalTicks = (int)distance(gg.SourceFort.Location, gg.DestinationFort.Location) / gg.Guys[0].Speed;
			double tickRatio = (numTotalTicks - gg.TicksTillFinished) / (double)numTotalTicks;
			double xDistanceMoved = (int)((gg.DestinationFort.Location.X - gg.SourceFort.Location.X) * tickRatio);
			double xLocation = gg.SourceFort.Location.X + xDistanceMoved;
			double yDistanceMoved = (int)((gg.DestinationFort.Location.Y - gg.SourceFort.Location.Y) * tickRatio);
			double yLocation = gg.SourceFort.Location.Y + yDistanceMoved;
			square.SetValue(Canvas.TopProperty, yLocation * SCALINGFACTOR);
			square.SetValue(Canvas.LeftProperty, xLocation * SCALINGFACTOR);
			square.Height = size;
			square.Width = square.Height;
			square.StrokeThickness = 3;
			square.Stroke = m_playerColorMapping[gg.GroupOwner];
			Canvas.Children.Add(square);

			DrawText(xLocation * SCALINGFACTOR, yLocation * SCALINGFACTOR -size/ 2 - 15, "->: " + gg.Guys.Sum(g => g.Strength).ToString(), Colors.Black);
			DrawText(xLocation * SCALINGFACTOR, yLocation * SCALINGFACTOR - size / 2, "+: " + gg.Guys.Sum(g => g.Health).ToString(), Colors.Black);
		}

		private double distance(Cell.Point l1, Cell.Point l2)
		{
			double dX = l1.X - l2.X;
			double dY = l1.Y - l2.Y;
			return System.Math.Sqrt(dX * dX + dY * dY);
		}

		private void DrawText(double x, double y, string text, Color color)
		{
			var textBlock = new TextBlock();
			textBlock.Text = text;
			textBlock.Foreground = new SolidColorBrush(color);
			Canvas.SetLeft(textBlock, x+5);
			Canvas.SetTop(textBlock, y);
			Canvas.Children.Add(textBlock);
		}

		private Brush PickBrush()
		{
			Brush result = Brushes.Transparent;
			
			Random rnd = new Random();


			int random = rnd.Next(colorOptions.Count);
			result = colorOptions[random];

			colorOptions.RemoveAt(random);

			return result;
		}
		public void LoadTestState()
		{
			lock (currentBoard)
			{
				currentBoard.Forts = new List<Fort>();
				currentBoard.GuyGroups = new List<GuyGroup>();
				currentBoard.Forts.Add(new Fort { Location = new Cell.Point { X = 40, Y = 100 }, DefendingGuys = new List<Guy>(), FortOwner = "P1", BirthSpeed = 5 });
				currentBoard.Forts.Add(new Fort { Location = new Cell.Point { X = 100, Y = 40 }, DefendingGuys = new List<Guy>(), FortOwner = "P2", BirthSpeed = 10 });
				currentBoard.Forts.Add(new Fort { Location = new Cell.Point { X = 40, Y = 40 }, DefendingGuys = new List<Guy>(), FortOwner = null, BirthSpeed = 5 });
				currentBoard.Forts.Add(new Fort { Location = new Cell.Point { X = 100, Y = 100 }, DefendingGuys = new List<Guy>(), FortOwner = null , BirthSpeed = 25 });
			}
		}

	}
}
