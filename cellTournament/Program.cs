using System;

namespace CellTournament
{
	class Program
	{
		static void Main(string[] args)
		{
			var options = new Options();
			CommandLine.Parser.Default.ParseArguments(args, options);
			options.CheckValues();

			var mapCat = MapCatalog.Load(options.MapCatalogFilePath);
			var tc = TournamentConfig.Load(options.TournamentConfigFilePath);

			//tc.Bots = new List<string> { "DoNothingBot", "BotOne" };

			var tournament = new Tournament(mapCat, tc.Maps, tc.Bots);
			var results = tournament.Run();
			Console.WriteLine(results);

			Console.ReadLine();
		}
	}
}
