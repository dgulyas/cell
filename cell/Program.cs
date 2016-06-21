using System;
using System.Collections.Generic;
using Cell.bots;

namespace Cell
{
	class Program
	{
		static void Main()
		{
			var map = MapCatalog.MapOne2P();
			var game = new Game(map);
			game.RunGame();
		}
	}
}
