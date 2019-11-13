using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Cell.Bots
{
	class Bot1 : BaseBot
	{
		public override List<Guy> SetStartingArmy()
		{
			var army = new List<Guy>();
			for (var i = 0; i < 10; i++)
			{
				army.Add(GuyFactory.CreateGuy(GuyType.AVERAGE));
			}
			return army;
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
					if (fort.NumDefendingGuys > 0)
					{
						var move = new Move{Source=fort, Destination=enemyForts[0], NumGuys=fort.NumDefendingGuys };
						moves.Add(move);
					}
				}
			}

			return moves;
		}
	}
}
