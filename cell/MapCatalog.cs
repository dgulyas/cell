using System.Collections.Generic;
using Newtonsoft.Json;

namespace Cell
{
	public static class MapCatalog //TODO: This class doesn't need to be static?
	{
		public static Dictionary<string, Map> Maps;

		public static void LoadMaps(string mapJsonFile)
		{
			string fileContents = "";
			fileContents = System.IO.File.ReadAllText(mapJsonFile);
			Maps = JsonConvert.DeserializeObject<Dictionary<string, Map>>(fileContents);
		}

		public static Map GetMap(string mapName)
		{
			if (!Maps.ContainsKey(mapName))
			{
				return null;
			}
			return Maps[mapName].Clone();
		}

		public static bool MapExists(string mapName)
		{
			return Maps.ContainsKey(mapName);
		}


		//This shouldn't be used, but is left here for testing.
		public static Map MapOne2P()
		{
			var player1 = new Player { Name = "p1" };
			var player2 = new Player { Name = "p2" };
			var forts = new List<Fort>
			{
				new Fort {Location = new Point {X = 8, Y = 8}, BirthSpeed = 0, FortOwner = player2},
				new Fort {Location = new Point {X = 2, Y = 8}, BirthSpeed = 0},
				new Fort {Location = new Point {X = 8, Y = 2}, BirthSpeed = 0},
				new Fort {Location = new Point {X = 2, Y = 2}, BirthSpeed = 5, FortOwner = player1}
			};
			var map = new Map {Forts = forts};
			return map;
		}
	}
}