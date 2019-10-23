using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using CellTournament.Cell;
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

		private Board currentBoard = new Board();
		private Thread server;

		public MainWindow()
		{
			InitializeComponent();

			//var myLine = new Line();
			//myLine.Stroke = Brushes.LightSteelBlue;
			//myLine.X1 = 1;
			//myLine.X2 = 50;
			//myLine.Y1 = 1;
			//myLine.Y2 = 50;
			//myLine.StrokeThickness = 2;
			//Canvas.Children.Add(myLine);


			LoadTestState();

			DrawBoardState();

			BoardChangedEvent += DrawBoardState;
			server = new Thread(RunServer);
			//server.Start();

			//Closing += StopServer;
		}

		public void DrawBoardState()
		{
			lock (currentBoard)
			{

				Canvas.Children.Clear();

				foreach (var fort in currentBoard.Forts.Values)
				{
					DrawFort(fort);
				}

				foreach (var gg in currentBoard.TravelingGGs)
				{
					DrawGuyGroup(gg);
				}
			}

		}

		public void DrawFort(Fort fort)
		{
			int size = 25;

			var circle = new Ellipse();
			circle.SetValue(Canvas.TopProperty, (double)fort.Location.Y);
			circle.SetValue(Canvas.LeftProperty, (double)fort.Location.X);
			circle.Height = size;
			circle.Width = circle.Height;
			circle.StrokeThickness = 2;
			circle.Stroke = Brushes.Black;
			//circle.Stroke = m_playerColorMapping[fort.FortOwner.Name];
			Canvas.Children.Add(circle);

			DrawText(fort.Location.X + size/4 - 4, fort.Location.Y + size/4 -1, fort.BirthSpeed.ToString(), Colors.Black);
			DrawText(fort.Location.X -3, fort.Location.Y - size / 2 - 2, fort.ID + ":" + fort.GetOwnerName(), Colors.Black);
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

		private void RunServer()
		{
			TcpListener server = new TcpListener(IPAddress.Any, 6432);

			server.Start();

			while (true)
			{
				TcpClient client = server.AcceptTcpClient();

				NetworkStream ns = client.GetStream();

				while (client.Connected)
				{
					string message;
					using (var sr = new StreamReader(ns))
					{
						message = sr.ReadToEnd();
					}

					var board = JsonConvert.DeserializeObject<Board>(message);
					lock (currentBoard)
					{
						currentBoard = board;
					}

					BoardChangedEvent();
				}
			}
		}

		public void StopServer(object sender, CancelEventArgs e)
		{
			server.Abort();
		}

		public void LoadTestState()
		{
			lock (currentBoard)
			{
				currentBoard.Forts.Add(0, new Fort { Location = new CellTournament.Cell.Point { X = 40, Y = 100 }, NumDefendingGuys = 87, FortOwner = new Player { Name = "P1" }, BirthSpeed = 5 });
				currentBoard.Forts.Add(1, new Fort { Location = new CellTournament.Cell.Point { X = 100, Y = 40 }, NumDefendingGuys = 87, FortOwner = new Player { Name = "P2" }, BirthSpeed = 10 });
				currentBoard.Forts.Add(2, new Fort { Location = new CellTournament.Cell.Point { X = 40, Y = 40 }, NumDefendingGuys = 87, FortOwner = new Player { Name = null }, BirthSpeed = 5 });
				currentBoard.Forts.Add(3, new Fort { Location = new CellTournament.Cell.Point { X = 100, Y = 100 }, NumDefendingGuys = 87, FortOwner = new Player { Name = null }, BirthSpeed = 25 });
			}
		}

	}
}
