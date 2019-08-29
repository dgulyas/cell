using System;
using CommandLine;
using CommandLine.Text;

namespace Cell
{
	class Options
	{
		[Option('c', "mapCatalog", Required = false, HelpText = "Specify the file path to the json file holding the maps.")]
		public string MapCatalogFilePath { get; set; }

		[Option('o', "outputFile", Required = false, HelpText = "If specified, the program will also print output to this file.")]
		public string OutputFile { get; set; }

		[Option('t', "tournamentConfig", Required = false, HelpText = "If specified, a tournament will be run with the provided config.")]
		public string TournamentConfigFilePath { get; set; }

		[Option('n', "mapName", Required = false, HelpText = "Specify which map to use.")]
		public string MapName { get; set; }

		[Option('h', "help", Required = false, HelpText = "Display this help screen.")]
		public bool Help { get; set; }

		[ParserState]
		public IParserState LastParserState { get; set; }

		public string GetUsage()
		{
			return HelpText.AutoBuild(this,
				current => HelpText.DefaultParsingErrorsHandler(this, current));
		}

		public void CheckValues()
		{
			if (Help)
			{
				Console.Write(GetUsage());
				Environment.Exit(0);
			}

			if (MapCatalogFilePath == null)
			{
				Console.WriteLine("The -c option must be defined.");
				Environment.Exit(0);
			}
		}

	}
}
