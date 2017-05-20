using System;
using System.Threading;
using Nancy.Diagnostics;

namespace Cell
{
	class Program
	{
		static void Main(string[] args)
		{
			var options = new Options();
			CommandLine.Parser.Default.ParseArguments(args, options);
	        CheckOptions(options);

			MapCatalog.LoadMaps(options.MapLibrary);
			var map = MapCatalog.GetMap(options.MapName);
			var game = new Game(map);
			game.RunGame();
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

	}
}
