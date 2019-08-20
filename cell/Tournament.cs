using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Cell.Bots;

namespace Cell
{
	class Tournament
	{
		public List<string> maps = new List<string>();
		public List<string> Bots = new List<string>(); //the names of the bot classes that should be in the tournament
		private Dictionary<string, ConstructorInfo> m_botConstructors = new Dictionary<string, ConstructorInfo>();
		//string, string, string -> botName, botName, mapName. Those bots will fight on that map. The value in the dict is the bot that won
		private Dictionary<Tuple<string,string,string>, string> pairings = new Dictionary<Tuple<string, string, string>, string>();

		public void Run()
		{
			PopulateBotConstructors();
			if (!BotsExist() | !MapsExist()) // use | to avoid short circuiting. Want to always do both functions.
			{
				return;
			}

			GenerateRoundRobinPairings();

			ExecuteParings();
		}

		public void ExecuteParings()
		{
			foreach (var pairing in pairings.Keys)
			{
				var bot1 = m_botConstructors[pairing.Item1].Invoke(new object[] { });
				var bot2 = m_botConstructors[pairing.Item2].Invoke(new object[] { });
				var botList = new List<IBot>{(IBot)bot1, (IBot)bot2};
				var map = MapCatalog.GetMap(pairing.Item3);

				var game = new Game(map, botList);

				Player winningPlayer;
				do
				{  //TODO: The game updates the state then gets the moves from the bots. This doesn't let the board be printed inbetween, so console has old info for the humanBot to use.
					winningPlayer = game.RunGameTurn();
				}
				while (winningPlayer == null);

				pairings[pairing] = game.GetBotAssignedToPlayer(winningPlayer).GetType().Name;
			}
		}

		public void GenerateRoundRobinPairings()
		{
			for (int i = 0; i < Bots.Count-1; i++) //iterate though all bots, excluding the last one
			{
				for (int j = i + 1; j < Bots.Count; j++) //iterate from the bot after bot i, to the last one
				{
					foreach (var map in maps)
					{
						pairings.Add(new Tuple<string, string, string>(Bots[i], Bots[j], map), null );
					}
				}
			}
		}

		//Check that the maps we want to use are in the map catalog
		//A map will be in the catalog if it was deserialized from the file passed in as a command line argument
		public bool MapsExist()
		{
			var allMapsExist = true;
			foreach (var map in maps)
			{
				if (!MapCatalog.MapExists(map))
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
			foreach (var bot in Bots)
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
