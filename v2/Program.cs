using System;
using System.Collections.Generic;
using System.Text;
using Cell.Bots;
using Newtonsoft.Json;

namespace Cell
{
	class Program
	{
		static void Main(string[] args)
		{
			var forts = new List<Fort>
			{
				new Fort { BirthSpeed = 10, ID = 1, FortOwner = "p1", NumDefendingGuys = 10, Location = new Point { X = 1, Y = 1 } },
				new Fort { BirthSpeed = 0, ID = 2, FortOwner = "p2", NumDefendingGuys = 0, Location = new Point { X = 8, Y = 8 } }
			};

			var jsonForts = JsonConvert.SerializeObject(forts);

			var players = new Dictionary<string, IBot>();
			var bot1 = new Bot1();
			bot1.SetPlayer("p1");

			var bot2 = new Bot1();
			bot2.SetPlayer("p2");

			players.Add("p1", bot1);
			players.Add("p2", bot2);

			var gameState = new StringBuilder();
			var cellGame = new Cell();
			var winner = cellGame.PlayGame(jsonForts, players, gameState);

			Console.WriteLine(FormatGameRecord(gameState));
			Console.WriteLine($"Winner: {winner}");
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
