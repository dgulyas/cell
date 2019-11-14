using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;

namespace Cell
{
	public class Tournament
	{
		private readonly List<Tuple<string, ConstructorInfo>> m_botConstructors = new List<Tuple<string, ConstructorInfo>>();

		//string:BotName tuple:(win, ties, loses)
		private readonly Dictionary<string, BotRecord> m_tounamentResults = new Dictionary<string, BotRecord>();

		public void RunTournament()
		{
			PopulateBotConstructors();

			foreach (var botConstructor in m_botConstructors)
			{
				m_tounamentResults.Add(botConstructor.Item1, new BotRecord());
			}

			for (int i = 0; i < m_botConstructors.Count - 1; i++)
			{
				for (int j = i + 1; j < m_botConstructors.Count; j++)
				{
					var botCon1 = m_botConstructors[i];
					var botCon2 = m_botConstructors[j];

					for (var mapNum = 1; mapNum <= MapLibrary.NumMaps; mapNum++)
					{
						var players = new Dictionary<string, IBot>();

						var bot1 = (IBot) (botCon1.Item2.Invoke(new object[] { }));
						bot1.SetPlayer("P1");
						players.Add("P1", bot1);

						var bot2 = (IBot) (botCon2.Item2.Invoke(new object[] { }));
						bot2.SetPlayer("P2");
						players.Add("P2", bot2);

						var forts = MapLibrary.GetMap(mapNum);

						var sb = new StringBuilder();

						var game = new Cell();

						var winner = game.PlayGame(JsonConvert.SerializeObject(forts), players, sb);

						if (winner == null)
						{
							m_tounamentResults[botCon1.Item1].Ties++;
							m_tounamentResults[botCon2.Item1].Ties++;
						}
						else if (winner == "P1")
						{
							m_tounamentResults[botCon1.Item1].Wins++;
							m_tounamentResults[botCon2.Item1].Losses++;
						}
						else if (winner == "P2")
						{
							m_tounamentResults[botCon1.Item1].Losses++;
							m_tounamentResults[botCon2.Item1].Wins++;
						}

					}

				}
			}

		}


		public void PopulateBotConstructors()
		{
			var ibotType = typeof(IBot);
			var botTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes())
				.Where(t => ibotType.IsAssignableFrom(t) && !t.IsInterface && t.Name != "BaseBot");

			foreach (var type in botTypes)
			{
				m_botConstructors.Add(new Tuple<string, ConstructorInfo>(type.Name, type.GetConstructor(new Type[] { })));
			}
		}

	}

	class BotRecord
	{
		public int Wins;
		public int Ties;
		public int Losses;
	}
}
