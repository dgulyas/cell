using System.Collections.Generic;

namespace Cell.Bots
{
	//This bot does nothing.
	public class DoNothingBot : IBot
	{
		private Player m_player;

		public void Do(Board board)
		{

		}

		public void SetPlayer(Player player)
		{
			m_player = player;
		}

		public Player GetPlayer()
		{
			return m_player;
		}
	}
}
