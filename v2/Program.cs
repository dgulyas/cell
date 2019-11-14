using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Cell.Bots;
using Newtonsoft.Json;

namespace Cell
{
	class Program
	{
		static void Main(string[] args)
		{
			string _filePath = Directory.GetParent(Directory.GetParent(System.IO.Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory)).FullName).FullName;

			var bot1 = new Bot1();
			bot1.SetPlayer("p1");

			var bot2 = new Bot1();
			bot2.SetPlayer("p2");

			var forts = new List<Fort>
			{
				new Fort { BirthSpeed = 1, ID = 1, FortOwner = "p1", DefendingGuys = bot1.SetStartingArmy(), Location = new Point { X = 1, Y = 1 } },
				new Fort { BirthSpeed = 1, ID = 2, FortOwner = "p2", DefendingGuys = bot2.SetStartingArmy(), Location = new Point { X = 8, Y = 8 } }
			};

			var jsonForts = JsonConvert.SerializeObject(forts);

			var players = new Dictionary<string, IBot>
			{
				{ "p1", bot1 },
				{ "p2", bot2 }
			};

			var gameState = new StringBuilder();
			var cellGame = new Cell();
			var winner = cellGame.PlayGame(jsonForts, players, gameState);

			Console.WriteLine(FormatGameRecord(gameState));

			string resultsPath = _filePath + Path.Combine(@"\Results\", DateTime.UtcNow.ToFileTime().ToString()) + ".txt";

			var turns = JsonConvert.DeserializeObject<BoardState[]>(gameState.ToString());
			using(var sw = new StreamWriter(resultsPath))
			{
				foreach(var board in turns)
				{
					sw.WriteLine((JsonConvert.SerializeObject(board)));
				}
			}

			Console.WriteLine($"Winner: {winner ?? "tie"}");

			var test = _filePath + Path.Combine(@"\Results\", DateTime.UtcNow.ToFileTime().ToString()) + ".txt";
			System.IO.File.WriteAllText(test, gameState.ToString());

			Console.ReadLine();
		}

		static string FormatGameRecord(StringBuilder gameState)
		{
			var turns = JsonConvert.DeserializeObject<BoardState[]>(gameState.ToString());

			var sb = new StringBuilder();
			foreach (var board in turns)
			{
				sb.AppendLine(board.ToString());
			}

			return sb.ToString();
		}
	}
}
