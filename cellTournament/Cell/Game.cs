using System;
using System.Collections.Generic;
using System.Linq;
using CellTournament.Bots;

namespace CellTournament.Cell
{
	public class Game
	{
		public int Turn;
		private readonly Board m_board;
		private readonly List<IBot> m_bots;
		private readonly Dictionary<IBot, Player> m_botPlayerMapping;

		public Game(Map map, List<IBot> bots)
		{
			m_bots = bots;

			if (m_bots.Count < map.Players.Count)
			{
				throw new Exception("The selected map doesn't have enough player starting positions.");
			}

			m_botPlayerMapping = new Dictionary<IBot, Player>();
			for (int i = 0; i < m_bots.Count; i++) //assign each bot a player
			{
				m_bots[i].SetPlayer(map.Players[i]);
				m_botPlayerMapping.Add(m_bots[i], map.Players[i]);
			}

			m_board = new Board();

			foreach (var fort in map.Forts) //add forts to the board
			{
				m_board.Forts.Add(fort.ID, fort);
			}

			Turn = 1;
		}

		//Returns the winning player, otherwise null.
		public Player RunGameTurn()
		{
			Turn++;
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
					m_board.DoMove(move, m_botPlayerMapping[bot]);
				}
			}

			return m_board.GetTheWinner();
		}

		public IBot GetBotAssignedToPlayer(Player player)
		{ //this assumes that every player is only assigned to one bot
			return m_botPlayerMapping.FirstOrDefault(m => m.Value == player).Key;
		}

		public override string ToString()
		{
			return $"Turn: {Turn}{Environment.NewLine}" + m_board;
		}
	}
}
