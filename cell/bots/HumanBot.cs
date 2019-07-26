using System;
using System.Collections.Generic;
using System.Linq;

namespace Cell.Bots
{
	class HumanBot : IBot
	{
		private Player m_player;

		public void Do(Board board)
		{
			throw new NotImplementedException();
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
