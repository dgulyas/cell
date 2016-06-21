//I want to store these as json files instead of in functions, but this will work for now.
//I just need something that will return a Map instance.

using System.Collections.Generic;

namespace Cell
{
	public static class MapCatalog
	{
		public static Dictionary<string, Map> Maps;

		//the first 2 player map
		public static Map MapOne2P()
		{
			var player1 = new Player {Name = "p1"};
			var player2 = new Player {Name = "p2"};
			var forts = new List<Fort>
			{
				new Fort {Location = new Point {X = 8, Y = 8}, BirthSpeed = 0, FortOwner = player2},
				new Fort {Location = new Point {X = 2, Y = 8}, BirthSpeed = 0},
				new Fort {Location = new Point {X = 8, Y = 2}, BirthSpeed = 0},
				new Fort {Location = new Point {X = 2, Y = 2}, BirthSpeed = 5, FortOwner = player1}
			};
			var map = new Map();
			map.Forts = forts;
			return map;
		}
	}
}
