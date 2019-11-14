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
		private const string P1 = "p1";
		private const string P2 = "p2";

		static void Main(string[] args)
		{
			string _filePath = Directory.GetParent(Directory.GetParent(System.IO.Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory)).FullName).FullName;

			var bot1 = new Bot1();
			bot1.SetPlayer(P1);

			var bot2 = new Bot2();
			bot2.SetPlayer(P2);

			var forts = new List<Fort>
			{
				new Fort { BirthSpeed = 1, BirthingType=GuyType.AVERAGE, ID = 1, FortOwner = P1, DefendingGuys = bot1.SetStartingArmy(), Location = new Point { X = 1, Y = 1 } },
				new Fort { BirthSpeed = 1, BirthingType=GuyType.AVERAGE, ID = 2, FortOwner = P2, DefendingGuys = bot2.SetStartingArmy(), Location = new Point { X = 8, Y = 8 } },
				new Fort { BirthSpeed = 1, BirthingType=GuyType.ARMORED, ID = 3, FortOwner = null, DefendingGuys = new List<Guy>(), Location = new Point { X = 8, Y = 1 } },
				new Fort { BirthSpeed = 1, BirthingType=GuyType.BEEFY, ID = 4, FortOwner = null, DefendingGuys = new List<Guy>(), Location = new Point { X = 1, Y = 8 } },
				new Fort { BirthSpeed = 1, BirthingType=GuyType.RUNNER, ID = 5, FortOwner = null, DefendingGuys = new List<Guy>(), Location = new Point { X = 4, Y = 4 } }
			};

			var jsonForts = JsonConvert.SerializeObject(forts);

			var players = new Dictionary<string, IBot>
			{
				{ P1, bot1 },
				{ P2, bot2 }
			};

			var gameState = new StringBuilder();
			var cellGame = new Cell();
			var winner = cellGame.PlayGame(jsonForts, players, gameState);

			Console.WriteLine(FormatGameRecord(gameState));
			Console.WriteLine($"Winner: {winner ?? "tie"}");

			string resultsPath = _filePath + Path.Combine(@"\Results\", DateTime.UtcNow.ToFileTime().ToString()) + ".txt";

			var turns = JsonConvert.DeserializeObject<BoardState[]>(gameState.ToString());
			using(var sw = new StreamWriter(resultsPath))
			{
				foreach(var board in turns)
				{
					sw.WriteLine((JsonConvert.SerializeObject(board)));
				}
			}
			
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
