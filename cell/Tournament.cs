using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Cell.Bots;

namespace Cell
{
	class Tournament
	{
		//The names of the maps that every bot pairing will play on
		private readonly List<string> m_maps;

		//The names of the bot classes that should be in the tournament
		private readonly List<string> m_bots;

		//The collection of maps. Must contain a map matching every map name in m_maps.
		private readonly MapCatalog m_mCatalog;

		//The constructors for the bots. Used to create a new instance of a bot for each game.
		private readonly Dictionary<string, ConstructorInfo> m_botConstructors = new Dictionary<string, ConstructorInfo>();

		//Round robin pairings
		//string, string, string -> botName, botName, mapName. Those bots will fight on that map.
		private readonly List<Tuple<string, string, string>> m_pairings = new List<Tuple<string, string, string>>();

		//The value is the winning bot of the game with the tuple's bots/map
		private readonly Dictionary<Tuple<string,string,string>, string> m_results = new Dictionary<Tuple<string, string, string>, string>();

		public Tournament(MapCatalog mCatalog, List<string> maps, List<string> bots)
		{
			if (maps == null || maps.Count < 1)
			{
				Console.WriteLine("A tournament needs at least one map.");
				Environment.Exit(0);
			}

			if (bots == null || bots.Count < 2)
			{
				Console.WriteLine("A tournament needs at least two bots.");
				Environment.Exit(0);
			}

			foreach (var map in maps)
			{
				if (!mCatalog.MapExists(map))
				{
					Console.WriteLine($"Can't find map '{map}' in map catalog.");
					Environment.Exit(0);
				}
			}

			m_maps = maps;
			m_bots = bots;
			m_mCatalog = mCatalog;
		}

		public void Run()
		{
			PopulateBotConstructors();
			if (!BotsExist() | !MapsExist()) // use | to avoid short circuiting. Want to always do both functions.
			{
				return;
			}

			GenerateRoundRobinPairings();

			ExecutePairings();

			PrintResults(); //TODO: This class shouldn't be printing to the console. Make function to return results as string.
		}

		public string FindWinner() //TODO: Write this function
		{
			return "a";
		}

		public void PrintResults()
		{
			foreach (var pairing in m_results.Keys)
			{
				Console.WriteLine($"{pairing.Item1} vs {pairing.Item2} on {pairing.Item3}: Winner {m_results[pairing]}");
			}
		}

		public void ExecutePairings()
		{ //TODO: Save the board state each turn and store it somewhere. Then dump it somewhere permanent when the game is done.
			foreach (var pairing in m_pairings)
			{
				var bot1 = m_botConstructors[pairing.Item1].Invoke(new object[] { });
				var bot2 = m_botConstructors[pairing.Item2].Invoke(new object[] { });
				var botList = new List<IBot>{(IBot)bot1, (IBot)bot2};
				var map = m_mCatalog.GetMap(pairing.Item3);

				var game = new Game(map, botList);

				var gameWasATie = false;
				Player winningPlayer = null;
				do
				{  //TODO: The game updates the state then gets the moves from the bots. This doesn't let the board be printed inbetween, so console has old info for the humanBot to use.
					if (game.Turn > 4000) //Game has gone on too long. Declare a tie.
					{
						gameWasATie = true;
					}
					else
					{
						winningPlayer = game.RunGameTurn();
					}
				}
				while (!gameWasATie && winningPlayer == null);

				if (gameWasATie)
				{
					m_results[pairing] = "Game Was A Tie";
				}
				else
				{
					m_results[pairing] = game.GetBotAssignedToPlayer(winningPlayer).GetType().Name;
				}
			}
		}

		public void GenerateRoundRobinPairings()
		{
			m_pairings.Clear();
			for (int i = 0; i < m_bots.Count-1; i++) //iterate though all bots, excluding the last one
			{
				for (int j = i + 1; j < m_bots.Count; j++) //iterate from the bot after bot i, to the last one
				{
					foreach (var map in m_maps)
					{
						m_pairings.Add(new Tuple<string, string, string>(m_bots[i], m_bots[j], map));
					}
				}
			}
		}

		//Check that the maps we want to use are in the map catalog
		//A map will be in the catalog if it was deserialized from the file passed in as a command line argument
		public bool MapsExist()
		{
			var allMapsExist = true;
			foreach (var map in m_maps)
			{
				if (!m_mCatalog.MapExists(map))
				{
					allMapsExist = false;
					Console.WriteLine($"Could not find map:{map}");
				}
			}
			return allMapsExist;
		}

		//Check that the bots we want to be in the tournament exist
		//A bot exists if there's a class with it's name that implements IBot
		public bool BotsExist()
		{
			var allBotsFound = true;
			foreach (var bot in m_bots)
			{
				if (!m_botConstructors.Keys.Contains(bot))
				{
					allBotsFound = false;
					Console.WriteLine($"ERROR: Could not find bot:{bot}.");
				}
			}
			return allBotsFound;
		}

		public void PopulateBotConstructors()
		{
			var ibotType = typeof(IBot);
			var botTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes())
				.Where(t => ibotType.IsAssignableFrom(t) && !t.IsInterface);

			foreach (var type in botTypes)
			{
				m_botConstructors.Add(type.Name, type.GetConstructor(new Type[] { }));
			}
		}
	}
}
