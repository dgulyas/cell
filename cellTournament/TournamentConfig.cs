using System.Collections.Generic;
using Newtonsoft.Json;

namespace CellTournament
{
	class TournamentConfig
	{
		public List<string> Bots;
		public List<string> Maps;

		public static TournamentConfig Load(string configJsonFile)
		{
			var fileContents = System.IO.File.ReadAllText(configJsonFile);
			return JsonConvert.DeserializeObject<TournamentConfig>(fileContents);
		}
	}
}
