using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using CellTournament.Bots;
using CellTournament.Cell;

namespace CellTournament
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

		//The value is the winning bot of the game with the tuple's bots/map. Null value indicates a tie.
		private readonly Dictionary<Tuple<string,string,string>, string> m_matchResults = new Dictionary<Tuple<string, string, string>, string>();

		//Key is a bot name. Value is an array holding the bot's #wins, #draws, #losses
		private readonly Dictionary<string, int[]> m_botRecords = new Dictionary<string, int[]>();

		public Tournament(MapCatalog mCatalog, List<string> maps, List<string> bots)
		{
			var errorSb = new StringBuilder();
			PopulateBotConstructors();
			CheckBots(bots, errorSb);
			CheckMaps(maps, mCatalog, errorSb);
			if (errorSb.Length > 0)
			{
				throw  new Exception(errorSb.ToString());
			}

			m_maps = maps;
			m_bots = bots;
			m_mCatalog = mCatalog;
		}

		public string Run()
		{
			GenerateRoundRobinPairings();
			InitializeBotRecords();
			ExecutePairings();
			return PrintResults();
		}

		private void InitializeBotRecords()
		{
			foreach (var bot in m_bots)
			{
				m_botRecords.Add(bot, new[]{0,0,0}); //TODO: Can't have the same bot play itself.
			}
		}

		public string PrintResults()
		{
			var sb = new StringBuilder();
			foreach (var pairing in m_matchResults.Keys)
			{
				var winner = m_matchResults[pairing] ?? "Tie";
				sb.AppendLine($"{pairing.Item1} vs {pairing.Item2} on {pairing.Item3}: Winner {winner}");
			}

			sb.AppendLine("Bot/wins/ties/losses");
			foreach (var botRecord in m_botRecords)
			{
				sb.AppendLine($"{botRecord.Key}/{botRecord.Value[0]}/{botRecord.Value[1]}/{botRecord.Value[2]}");
			}

			return sb.ToString();
		}

		private string FindWinner()
		{
			return "";
		}

		public void ExecutePairings()
		{ //TODO: Save the board state each turn and store it somewhere. Then dump it somewhere permanent when the game is done. Use options argument
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
					m_matchResults[pairing] = null;
					m_botRecords[pairing.Item1][1]++;
					m_botRecords[pairing.Item2][1]++;
				}
				else
				{
					var winningBotName = game.GetBotAssignedToPlayer(winningPlayer).GetType().Name;
					var losingBotName = winningBotName == pairing.Item1 ? pairing.Item2 : pairing.Item1; //get the bot that isn't the winner

					m_matchResults[pairing] = winningBotName;
					m_botRecords[winningBotName][0]++;
					m_botRecords[losingBotName][2]++;
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
		public void CheckMaps(List<string> maps, MapCatalog mCatalog, StringBuilder errorSb)
		{
			if (maps == null || maps.Count < 1)
			{
				errorSb.AppendLine("ERROR: A tournament needs at least one map.");
			}
			else
			{
				foreach (var map in maps)
				{
					if (!mCatalog.MapExists(map))
					{
						errorSb.AppendLine($"ERROR: Can't find map '{map}' in map catalog.");
					}
				}
			}
		}

		//Check that the bots we want to be in the tournament exist
		//A bot exists if there's a class with it's name that implements IBot
		public void CheckBots(List<string> bots, StringBuilder errorSb)
		{
			if (bots == null || bots.Count < 2)
			{
				errorSb.AppendLine("ERROR: A tournament needs at least two bots.");
			}
			else
			{
				foreach (var bot in bots)
				{
					if (!m_botConstructors.Keys.Contains(bot))
					{
						errorSb.AppendLine($"ERROR: Could not find bot:{bot}.");
					}
				}
			}
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
