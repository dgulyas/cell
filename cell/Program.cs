using System;
using System.Collections.Generic;
using Cell.Bots;

namespace Cell
{
	class Program
	{
		static void Main(string[] args)
		{
			var tournament = new Tournament();
			tournament.Bots = new List<string>{"a","b", "c", "d", "e" };
			tournament.maps = new List<string>{"map1", "map2"};
			tournament.GenerateRoundRobinPairings();

			tournament.Run();

			var options = new Options();
			CommandLine.Parser.Default.ParseArguments(args, options);
			CheckOptions(options);

			var bots = new List<IBot>();
			//bots = new List<IBot> { new BotOne(), new BotOne() };
			//bots = new List<IBot> { new DoNothingBot(), new HumanBot() };

			var game = new Game(GetMapFromCatalog(options.MapCatalog, options.MapName), bots);
			Console.WriteLine(game.ToString());

			Player winner;
			do
			{  //TODO: The game updates the state then gets the moves from the bots. This doesn't let the board be printed inbetween, so console has old info for the humanBot to use.
				winner = game.RunGameTurn();
				Console.WriteLine(game.ToString());
			}
			while (winner == null);

			Console.WriteLine($"The winner is {winner.Name}");
			Console.ReadLine();
		}

		public static void CheckOptions(Options options)
		{
			if (options.Help)
			{
				Console.Write(options.GetUsage());
				Environment.Exit(0);
			}

			if (options.MapCatalog == null)
			{
				Console.WriteLine("The -c option must be defined.");
				Environment.Exit(0);
			}

			if (options.MapName == null)
			{
				Console.WriteLine("The -n option must be defined.");
				Environment.Exit(0);
			}
		}

		private static Map GetMapFromCatalog(string catalogFilePath, string mapName)
		{
			try
			{
				MapCatalog.LoadMaps(catalogFilePath);
			}
			catch (Exception e)
			{
				Console.WriteLine("There was a problem loading the map catalog");
				Console.WriteLine(e.Message);
			}

			var map = MapCatalog.GetMap(mapName);
			if (map == null)
			{
				Console.WriteLine("Could not load map.");
				Environment.Exit(0);
			}
			return map;
		}

	}
}
