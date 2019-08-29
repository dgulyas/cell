using System.Collections.Generic;
using CellTournament.Cell;

namespace CellTournament.Bots
{
	//Send all available guys to the first enemy fort, every tick.
	public class BotOne : IBot
	{
		private Player m_player;

		public void SetPlayer(Player player)
		{
			m_player = player;
		}

		public List<Move> Do(Board board)
		{
			var friendlyForts = AiHelper.GetFriendlyForts(m_player, board);
			var enemyForts = AiHelper.GetEnemyForts(m_player, board);

			var moves = new List<Move>();

			if (enemyForts.Count > 0)
			{
				foreach (var fort in friendlyForts)
				{
					if (fort.NumDefendingGuys > 0)
					{
						var move = new Move(fort.ID, enemyForts[0].ID, fort.NumDefendingGuys);
						if(board.DoMove(move, m_player))
						{
							moves.Add(move);
						}
					}
				}
			}

			return moves;
		}

	}
}
