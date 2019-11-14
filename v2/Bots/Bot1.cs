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
					if (fort.DefendingGuys.Count > 0)
					{
						var numBeefy = fort.DefendingGuys.Where(g => g.Type == GuyType.BEEFY).Count();
						var move = new Move { SourceFortID = fort.ID, DestinationFortID = enemyForts[0].ID, NumGuys = numBeefy, GuyType = GuyType.BEEFY };
						moves.Add(move);

						var numArmored = fort.DefendingGuys.Where(g => g.Type == GuyType.ARMORED).Count();
						move = new Move { SourceFortID = fort.ID, DestinationFortID = enemyForts[0].ID, NumGuys = numArmored, GuyType = GuyType.ARMORED };
						moves.Add(move);

						var numAverage = fort.DefendingGuys.Where(g => g.Type == GuyType.AVERAGE).Count();
						move = new Move { SourceFortID = fort.ID, DestinationFortID = enemyForts[0].ID, NumGuys = numAverage, GuyType = GuyType.AVERAGE };
						moves.Add(move);

						var numRunner = fort.DefendingGuys.Where(g => g.Type == GuyType.RUNNER).Count();
						move = new Move { SourceFortID = fort.ID, DestinationFortID = enemyForts[0].ID, NumGuys = numRunner, GuyType = GuyType.RUNNER };
						moves.Add(move);
					}
				}
			}

			return moves;
		}
	}
}
