using System;
using System.Collections.Generic;

namespace ConsoleApplication1.bots
{
	public class BotOne : IBot
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

			var friendlyForts = AiHelper.GetFriendlyForts(m_player, board);
			var enemyForts = AiHelper.GetEnemyForts(m_player, board);

			if (enemyForts.Count > 0)
			{
				foreach (var fort in friendlyForts)
				{
					if (fort.NumDefendingGuys > 0)
					{
						moves.Add(new Tuple<Fort, Fort, int>(fort, enemyForts[0], fort.NumDefendingGuys));
					}
				}
			}

			return moves;
		}

	}
}
