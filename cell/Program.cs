using System;
using System.Collections.Generic;

namespace Cell
{
	class Program
	{
		static void Main(string[] args)
		{

			var bots = new List<string>{"DoNothingBot", "BotOne"};
			//var bots = new List<string>{"HumanBot","DoNothingBot"};
			var maps = new List<string>{"fourFortsSquare", "threeFortsLine"};


			var options = new Options();
			CommandLine.Parser.Default.ParseArguments(args, options);
			var mapCat = new MapCatalog();
			mapCat.LoadMaps(options.MapCatalog);

			var tournament = new Tournament(mapCat, maps, bots);

			tournament.Run();

			//CheckOptions(options);

			//var bots = new List<IBot>();
			////bots = new List<IBot> { new BotOne(), new BotOne() };
			////bots = new List<IBot> { new DoNothingBot(), new HumanBot() };

			//var game = new Game(GetMapFromCatalog(options.MapCatalog, options.MapName), bots);
			//Console.WriteLine(game.ToString());

			//Player winner;
			//do
			//{  //TODO: The game updates the state then gets the moves from the bots. This doesn't let the board be printed inbetween, so console has old info for the humanBot to use.
			//	winner = game.RunGameTurn();
			//	Console.WriteLine(game.ToString());
			//}
			//while (winner == null);

			//Console.WriteLine($"The winner is {winner.Name}");
			Console.ReadLine();
		}

		public static void CheckOptions(Options options) //TODO: Move this to the options class?
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

	}
}
