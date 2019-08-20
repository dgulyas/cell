using System;
using System.Collections.Generic;
using System.Linq;
using Cell.Bots;

namespace Cell
{
	public class Game
	{
		private readonly Board m_board;
		private readonly List<IBot> m_bots;
		private readonly Dictionary<IBot, Player> botPlayerMapping;

		public Game(Map map, List<IBot> players)
		{
			//m_bots = new List<IBot> { new BotOne(), new BotOne() };
			//m_bots = new List<IBot> { new DoNothingBot(), new HumanBot() };
			m_bots = players;

			if (m_bots.Count < map.Players.Count)
			{
				throw new Exception("The selected map doesn't have enough player starting positions.");
			}

			botPlayerMapping = new Dictionary<IBot, Player>();
			for (int i = 0; i < m_bots.Count; i++) //assign each bot a player
			{
				m_bots[i].SetPlayer(map.Players[i]);
				botPlayerMapping.Add(m_bots[i], map.Players[i]);
			}

			m_board = new Board();

			foreach (var fort in map.Forts) //add forts to the board
			{
				m_board.Forts.Add(fort.ID, fort);
			}
		}

		//Returns the winning player, otherwise null.
		public Player RunGameTurn()
		{
			m_board.Turn++;
			m_board.CreateGuys();
			m_board.MoveGuyGroups();

			var moveDict = new Dictionary<IBot, List<Move>>();

			foreach (var bot in m_bots) //collect all the moves from the bots
			{
				var botMoves = bot.Do(m_board.Clone()) ?? new List<Move>();
				moveDict.Add(bot, botMoves);
			}

			foreach (var bot in moveDict.Keys) //apply all the moves to the board
			{
				foreach (var move in moveDict[bot])
				{
					m_board.DoMove(move, botPlayerMapping[bot]);
				}
			}

			return m_board.GetTheWinner();
		}

		public IBot GetBotAssignedToPlayer(Player player)
		{
			return botPlayerMapping.FirstOrDefault(m => m.Value == player).Key;
		}

		public override string ToString()
		{
			return m_board.ToString();
		}
	}
}
