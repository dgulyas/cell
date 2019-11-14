using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
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
		//private List<Color> m_playerColors = new List<Color>{Colors.OrangeRed, Colors.DarkBlue, Colors.DarkOliveGreen};
		//private Dictionary<string, Color> m_playerColorMapping = new Dictionary<string, Color>();

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
			int size = 25;

			var circle = new Ellipse();
			circle.SetValue(Canvas.TopProperty, (double)fort.Location.Y * 10);
			circle.SetValue(Canvas.LeftProperty, (double)fort.Location.X * 10);
			circle.Height = size;
			circle.Width = circle.Height;
			circle.StrokeThickness = 2;
			circle.Stroke = Brushes.Black;
			//circle.Stroke = m_playerColorMapping[fort.FortOwner.Name];
			Canvas.Children.Add(circle);

			DrawText(fort.Location.X + size/4 - 4, fort.Location.Y + size/4 -1, fort.BirthSpeed.ToString(), Colors.Black);
			DrawText(fort.Location.X -3, fort.Location.Y - size / 2 - 2, fort.ID + ":" + fort.FortOwner, Colors.Black);
			DrawText(fort.Location.X + size / 4 - 4, fort.Location.Y + size, fort.NumDefendingGuys.ToString(), Colors.Black);
		}

		public void DrawGuyGroup(GuyGroup gg)
		{

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

		public delegate void BoardChangedEventHandler();

		public event BoardChangedEventHandler BoardChangedEvent;
		public void LoadTestState()
		{
			lock (currentBoard)
			{
				currentBoard.Forts = new List<Fort>();
				currentBoard.GuyGroups = new List<GuyGroup>();
				currentBoard.Forts.Add(new Fort { Location = new Cell.Point { X = 40, Y = 100 }, NumDefendingGuys = 67, FortOwner = "P1", BirthSpeed = 5 });
				currentBoard.Forts.Add(new Fort { Location = new Cell.Point { X = 100, Y = 40 }, NumDefendingGuys = 77, FortOwner = "P2", BirthSpeed = 10 });
				currentBoard.Forts.Add(new Fort { Location = new Cell.Point { X = 40, Y = 40 }, NumDefendingGuys = 87, FortOwner = null, BirthSpeed = 5 });
				currentBoard.Forts.Add(new Fort { Location = new Cell.Point { X = 100, Y = 100 }, NumDefendingGuys = 97, FortOwner = null , BirthSpeed = 25 });
			}
		}

	}
}
