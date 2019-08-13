using System;
using System.Collections.Generic;

//Ideally this will start a web service that remote bots can use
//to interact with the game.
namespace Cell.Bots
{
	public class WebBot : IBot
	{
		private Player m_player;


		public List<Move> Do(Board board)
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
