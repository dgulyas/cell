using System;
using Nancy;


//Ideally this will start a web service that remote bots can use
//to interact with the game.
namespace Cell.bots
{
	public class WebBot : IBot
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
