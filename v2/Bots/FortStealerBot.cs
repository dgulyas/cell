using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Cell.Bots
{
	/// <summary>
	/// Waits in their fort until a fort is empty then steals it as quickly as it can
	/// </summary>
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

			var friendlyForts = GetFriendlyForts(board);
			var enemyForts = GetEnemyForts(board);
			var neutralForts = GetNeutralForts(board);

			var moves = new List<Move>();

			// find the closest friendly fort to the closest empty forts
			// empty means no owner or an owner but no guys and no birthspeeds
			var emptyForts = neutralForts.Concat(enemyForts.Where(f => f.DefendingGuys.Count() < 1 && f.BirthSpeed < 1).ToList()).ToList();
			if (emptyForts.Count > 0)
			{
				foreach (var fort in friendlyForts)
				{
					var destFort = GetClosestFort(emptyForts, fort);
					moves.AddRange(MoveAll(fort, destFort));
				}
			}

			return moves;
		}
	}
}
