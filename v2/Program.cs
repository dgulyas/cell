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
		private const string P1 = "zerg";
		private const string P2 = "stealer";
		private const string P3 = "patient";
		private const string P4 = "nothing";

		static void Main(string[] args)
		{
			string _filePath = Directory.GetParent(Directory.GetParent(System.IO.Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory)).FullName).FullName;

			var bot1 = new ZergBot();
			bot1.SetPlayer(P1);

			var bot2 = new FortStealerBot();
			bot2.SetPlayer(P2);

			var bot3 = new PatientBot();
			bot3.SetPlayer(P3);

			var bot4 = new DoNothingBot();
			bot4.SetPlayer(P4);

			var forts = new List<Fort>
			{
				new Fort { BirthSpeed = 1, BirthingType=GuyType.AVERAGE, ID = 1, FortOwner = P1, DefendingGuys = bot1.SetStartingArmy(), Location = new Point { X = 1, Y = 1 } },
				new Fort { BirthSpeed = 1, BirthingType=GuyType.AVERAGE, ID = 2, FortOwner = P1, DefendingGuys = bot1.SetStartingArmy(), Location = new Point { X = 8, Y = 8 } },
				new Fort { BirthSpeed = 1, BirthingType=GuyType.AVERAGE, ID = 3, FortOwner = P3, DefendingGuys = bot3.SetStartingArmy(), Location = new Point { X = 8, Y = 1 } },
				new Fort { BirthSpeed = 1, BirthingType=GuyType.AVERAGE, ID = 4, FortOwner = P3, DefendingGuys = bot3.SetStartingArmy(), Location = new Point { X = 1, Y = 8 } },
				new Fort { BirthSpeed = 1, BirthingType=GuyType.BEEFY, ID = 5, FortOwner = null, DefendingGuys = new List<Guy>(), Location = new Point { X = 4, Y = 4 } }
			};

			var jsonForts = JsonConvert.SerializeObject(forts);

			var players = new Dictionary<string, IBot>
			{
				{ P1, bot1 },
				{ P2, bot2 },
				{ P3, bot3 },
				{ P4, bot4 }
			};

			var gameState = new StringBuilder();
			var cellGame = new Cell();
			var winner = cellGame.PlayGame(jsonForts, players, gameState);

			//Console.WriteLine(FormatGameRecord(gameState));
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

			Console.WriteLine($"Turns: {turns.Length - 1}");
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
