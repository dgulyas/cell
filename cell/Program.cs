using System;

namespace Cell
{
	class Program
	{
		static void Main(string[] args)
		{
			var options = new Options();
			CommandLine.Parser.Default.ParseArguments(args, options);
			CheckOptions(options);

			var game = new Game(GetMapFromLibrary(options.MapLibrary, options.MapName));

			Player winner;
			do
			{
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

			if (options.MapLibrary == null)
			{
				Console.WriteLine("The -l option must be defined.");
				Environment.Exit(0);
			}

			if (options.MapName == null)
			{
				Console.WriteLine("The -m option must be defined.");
				Environment.Exit(0);
			}
		}

		private static Map GetMapFromLibrary(string libraryFilePath, string mapName)
		{
			try
			{
				MapCatalog.LoadMaps(libraryFilePath);
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
