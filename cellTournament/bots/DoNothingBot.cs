using System.Collections.Generic;
using CellTournament.Cell;

namespace CellTournament.Bots
{
	//This bot does nothing.
	public class DoNothingBot : IBot
	{
		private Player m_player;

		public List<Move> Do(Board board)
		{
			return new List<Move>();
		}

		public void SetPlayer(Player player)
		{
			m_player = player;
		}

	}
}
