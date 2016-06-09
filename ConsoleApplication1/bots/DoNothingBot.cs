using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1.bots
{
	public class DoNothingBot : IBot
	{
		private Player m_player;

		public List<Tuple<Fort, Fort, int>> GetMoves(Board board)
		{
			return new List<Tuple<Fort, Fort, int>>();
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
