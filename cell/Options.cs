using CommandLine;
using CommandLine.Text;

namespace Cell
{
	class Options
	{
		[Option('l', "mapsLibrary", Required = false, HelpText = "Specify the file path to the json file holding the maps.")]
		public string MapLibrary { get; set; }

		[Option('o', "outputFile", Required = false, HelpText = "If specified, the program will also print output to this file.")]
		public string OutputFile { get; set; }

		[Option('m', "map", Required = false, HelpText = "Specify which map to use.")]
		public string MapName { get; set; }

		[Option('h', "help", HelpText = "Display this help screen.", Required = false)]
		public bool Help { get; set; }

		[ParserState]
		public IParserState LastParserState { get; set; }

		public string GetUsage()
		{
			return HelpText.AutoBuild(this,
				current => HelpText.DefaultParsingErrorsHandler(this, current));
		}
	}
}
