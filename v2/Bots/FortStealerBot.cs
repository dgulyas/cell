using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Cell.Bots
{
	// Waits in their fort until a fort is empty then steals it as quickly as it can
	class FortStealerBot : BaseBot
	{
		public override List<Guy> SetStartingArmy()
		{
			var army = new List<Guy>();
			for (var i = 0; i < 10; i++)
			{
				army.Add(GuyFactory.CreateGuy(GuyType.RUNNER));
			}
			return army;
		}

		public override List<Move> Do(string boardString)
		{
			var board = JsonConvert.DeserializeObject<BoardState>(boardString);

			var friendlyForts = GetFriendlyForts(m_player, board);
			var enemyForts = GetEnemyForts(m_player, board);
			var neutralForts = GetNeutralForts(board);

			var moves = new List<Move>();

			if (friendlyForts.Count > 0)
			{
				var distances = new Dictionary<Fort, Fort>();

				// find the closest friendly fort to the closest empty forts
				// empty means no owner or an owner but no guys and no birthspeeds
				var forts = neutralForts.Concat(enemyForts.Where(f => f.DefendingGuys.Count() < 1 && f.BirthSpeed < 1).ToList());
				foreach (var targetFort in forts)
				{
					double shortestDistance = double.MaxValue;
					foreach (var fort in friendlyForts)
					{
						var distance = GetDistanceBetween(targetFort.Location, fort.Location);
						if (distance < shortestDistance)
						{
							shortestDistance = distance;
							distances[targetFort] = fort;
						}
					}
				}

				foreach (KeyValuePair<Fort, Fort> entry in distances)
				{
					var destFort = entry.Key;
					var srcFort = entry.Value;

					moves.Concat(MoveAll(srcFort, destFort));
				}
			}

			return moves;
		}
	}
}
