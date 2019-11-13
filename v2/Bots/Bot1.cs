using System.Collections.Generic;
using Newtonsoft.Json;

namespace Cell.Bots
{
	class Bot1 : BaseBot
	{
		public override string SetStartingArmy()
		{
			var army = new List<Guy>();
			for (var i = 0; i < 10; i++)
			{
				army.Add(GuyFactory.CreateGuy(GuyType.AVERAGE));
			}
			return JsonConvert.SerializeObject<List<Guy>>(army);
		}

		public override List<Move> Do(string boardString)
		{
			var board = JsonConvert.DeserializeObject<BoardState>(boardString);

			var friendlyForts = GetFriendlyForts(m_player, board);
			var enemyForts = GetEnemyForts(m_player, board);

			var moves = new List<Move>();

			if (enemyForts.Count > 0)
			{
				foreach (var fort in friendlyForts)
				{
					if (fort.DefendingGuys.Count > 0)
					{
						var move = new Move{SourceFortID=fort.ID, DestinationFortID=enemyForts[0].ID, NumGuys=fort.DefendingGuys.Count, GuyType=GuyType.AVERAGE};
						moves.Add(move);
					}
				}
			}

			return moves;
		}
	}
}
