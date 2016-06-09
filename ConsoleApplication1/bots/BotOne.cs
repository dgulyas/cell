using System.Collections.Generic;

namespace ConsoleApplication1.bots
{
	//Send all available guys to the first enemy fort, every tick.
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

		List<Move> IBot.GetMoves(Board board)
		{
			var moves = new List<Move>();

			var friendlyForts = AiHelper.GetFriendlyForts(m_player, board);
			var enemyForts = AiHelper.GetEnemyForts(m_player, board);

			if (enemyForts.Count > 0)
			{
				foreach (var fort in friendlyForts)
				{
					if (fort.NumDefendingGuys > 0)
					{
						moves.Add(new Move(fort, enemyForts[0], fort.NumDefendingGuys));
					}
				}
			}

			return moves;
		}

	}
}
