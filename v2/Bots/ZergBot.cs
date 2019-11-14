using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Cell.Bots
{
	/// <summary>
	/// As soon as a friendly base has a guy it is sent off to the closest enemy fort.
	/// </summary>
	class ZergBot : BaseBot
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

			var friendlyForts = GetFriendlyForts(board);
			var enemyForts = GetEnemyForts(board);
			var neutralForts = GetNeutralForts(board);

			var moves = new List<Move>();

			foreach (var fort in friendlyForts)
			{
				// move to closest enemy fort
				if (fort.DefendingGuys.Count > 0)
				{
					var targetFort = GetClosestFort(enemyForts, fort);
					moves.AddRange(MoveAll(fort, targetFort));
				}
			}

			return moves;
		}
	}
}
