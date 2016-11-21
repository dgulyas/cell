//I want to store these as json files instead of in functions, but this will work for now.
//I just need something that will return a Map instance.

using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using NUnit.Framework.Internal.Commands;

namespace Cell
{
	public static class MapCatalog
	{
		public static Dictionary<string, Map> Maps;

		public static void LoadMaps(string mapJsonFile)
		{
			string fileContents = "";

			try
			{
				fileContents = System.IO.File.ReadAllText(mapJsonFile);
			}
			catch (Exception e)
			{
				Console.WriteLine("There was a problem reading the Map Library:");
				Console.WriteLine(e.Message);
				Environment.Exit(0);
			}
			Maps = JsonConvert.DeserializeObject<Dictionary<string, Map>>(fileContents);
		}

		public static Map GetMap(string mapName)
		{
			if (!Maps.ContainsKey(mapName))
			{
				Console.WriteLine($"The map {mapName} does not exist.");
				Environment.Exit(0);
			}
			return Maps[mapName];
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
			var map = new Map();
			map.Forts = forts;
			return map;
		}
	}
}
 