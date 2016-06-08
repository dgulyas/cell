using System;
using System.Collections.Generic;

namespace ConsoleApplication1.bots
{
	class BotOne : IBot
	{
		private Player m_player;


		public void SetPlayer(Player player)
		{
			m_player = player;
		}

		public Player GetPlayer()
		{
			return m_player;
		}

		List<Tuple<Fort, Fort, int>> IBot.GetMoves(Board board)
		{
			var moves = new List<Tuple<Fort, Fort, int>>();

			var friendlyForts = board.GetFriendlyForts(m_player);
			var enemyForts = board.GetEnemyForts(m_player);
			foreach (var fort in friendlyForts)
			{
				if (fort.NumDefendingGuys > 0)
				{
					moves.Add(new Tuple<Fort, Fort, int>(fort, enemyForts[0], fort.NumDefendingGuys));
				}
			}

			return moves;
		}

	}
}
