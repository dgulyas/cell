using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cell.Bots
{
	abstract class BaseBot : IBot
	{
		protected string m_player;

		public void SetPlayer(string player)
		{
			m_player = player;
		}

		public List<Guy> CreateAndValidateArmy()
		{
			var jsonArmy = SetStartingArmy();
			var army = JsonConvert.DeserializeObject<List<Guy>>(jsonArmy);

			// TODO: add cost and max number guys rules here

			return army;
		}

		public abstract string SetStartingArmy();

		public abstract List<Move> Do(string boardString);

		public static List<Fort> GetFriendlyForts(string player, BoardState board)
		{
			return board.Forts.Where(f => f.FortOwner == player).ToList();
		}
		public static List<Fort> GetEnemyForts(string player, BoardState board)
		{
			return board.Forts.Where(f => f.FortOwner != null && f.FortOwner != player).ToList();
		}
	}
}
