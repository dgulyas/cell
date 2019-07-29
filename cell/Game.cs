using System;
using System.Collections.Generic;
using Cell.Bots;

namespace Cell
{
	public class Game
	{
		private readonly Board m_board;
		private readonly List<IBot> m_bots;

		public Game(Map map)
		{
			//m_bots = new List<IBot> { new BotOne(), new BotOne() };
			m_bots = new List<IBot> { new DoNothingBot(), new HumanBot() };

			if (m_bots.Count < map.Players.Count)
			{
				throw new Exception("The selected map doesn't have enough player starting positions.");
			}

			for (int i = 0; i < m_bots.Count; i++)
			{
				m_bots[i].SetPlayer(map.Players[i]);
			}

			m_board = new Board
			{
				Forts = map.Forts
			};
		}

		//Returns the winning player, otherwise null.
		public Player RunGameTurn()
		{
			m_board.Turn++;
			m_board.CreateGuys();
			m_board.MoveGuyGroups();

			foreach (var bot in m_bots)
			{
				bot.Do(m_board);
			}

			return m_board.GetTheWinner();
		}

		public override string ToString()
		{
			return m_board.ToString();
		}
	}
}
