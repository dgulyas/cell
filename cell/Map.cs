using System.Collections.Generic;
using System.Linq;

namespace Cell
{
	public class Map
	{
		public List<Fort> Forts = new List<Fort>();
		public List<Player> Players => Forts.Where(f => f.FortOwner != null).Select(f => f.FortOwner).Distinct().ToList();
		public int NumPlayers => Players.Count;
	}
}
