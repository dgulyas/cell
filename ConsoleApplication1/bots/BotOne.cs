using System;
using System.Collections.Generic;

namespace ConsoleApplication1.bots
{
	class BotOne : IBot
	{
		public void SetPlayer(Player player)
		{
			throw new NotImplementedException();
		}

		List<Tuple<Fort, Fort, int>> IBot.GetMoves(Board board)
		{
			throw new NotImplementedException();
		}
	}
}
