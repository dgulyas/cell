using System.Collections.Generic;
using CellTournament.Cell;
using Newtonsoft.Json;

namespace CellTournament
{
	public class MapCatalog
	{
		public Dictionary<string, Map> Maps;

		public static MapCatalog Load(string mapJsonFile)
		{
			var fileContents = System.IO.File.ReadAllText(mapJsonFile);
			return new MapCatalog {Maps = JsonConvert.DeserializeObject<Dictionary<string, Map>>(fileContents)};
		}

		public Map GetMap(string mapName)
		{
			if (!Maps.ContainsKey(mapName))
			{
				return null;
			}
			return Maps[mapName].Clone();
		}

		public bool MapExists(string mapName)
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