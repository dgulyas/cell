using System;
using System.Collections.Generic;

namespace ConsoleApplication1.bots
{
	public interface IBot
	{
		//From Fort, To Fort, Num Guys
		List<Tuple<Fort, Fort, int>> GetMoves(Board board);
		void SetPlayer(Player player);
		Player GetPlayer();
	}
}
