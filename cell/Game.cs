using System;
using System.Collections.Generic;
using Cell.bots;

namespace Cell
{
	public class Game
	{
		private Map m_map;
		private List<Player> m_players;

		public Game(Map map)
		{
			m_map = map;
			m_players = map.Players;
		}

		public void RunGame()
		{
			var bots = new List<IBot> {new BotOne(), new BotOne()};

			if (bots.Count < m_map.Players.Count)
			{
				Console.WriteLine("The selected map doesn't have enough player starting positions.");
				Environment.Exit(0);
			}

			for (int i = 0; i < bots.Count; i++)
			{
				bots[i].SetPlayer(m_players[i]);
			}

			var board = new Board
			{
				Forts = m_map.Forts
			};

			var winner = board.GetTheWinner();

			while (winner == null)
			{
				board.CreateGuys();
				board.MoveGuyGroups();

				foreach (var bot in bots)
				{
					bot.Do(board);
				}

				Console.WriteLine($"{Environment.NewLine}Turn:{board.Turn++}");
				PrintGameState(board);
				winner = board.GetTheWinner();
			}
			Console.WriteLine("The Winner is " + winner.Name);

			Console.ReadLine();
		}

		public static void PrintGameState(Board board)
		{
			foreach (var fort in board.Forts)
			{
				Console.WriteLine(fort.GetDescription());
			}

			foreach (var gg in board.TravelingGGs)
			{
				Console.WriteLine(gg.GetDescription());
			}
		}
	}
}
