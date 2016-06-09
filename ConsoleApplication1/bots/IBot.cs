using System;
using System.Collections.Generic;

namespace ConsoleApplication1.bots
{
	public interface IBot
	{
		//From Fort, To Fort, Num Guys
		List<Move> GetMoves(Board board);
		void SetPlayer(Player player);
		Player GetPlayer();
	}
}
