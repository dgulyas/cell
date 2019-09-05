using System.Collections.Generic;
using System.Linq;
namespace CellTournament.Cell
{
	public class Map
	{
		public List<Fort> Forts = new List<Fort>();

		public List<Player> GetPlayers()
		{
			return Forts.Where(f => f.FortOwner != null).Select(f => f.FortOwner).Distinct().ToList();
		}

		public Map Clone()
		{
			var map = new Map();

			foreach (var fort in Forts)
			{
				map.Forts.Add(fort.Clone());
			}

			return map;
		}
	}
}
