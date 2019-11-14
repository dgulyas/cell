using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Cell.Bots
{
	/// <summary>
	/// Waits until there are at least 15 guys defending before moving them as a group
	/// </summary>
	class PatientBot : BaseBot
	{
		public override List<Guy> SetStartingArmy()
		{
			var army = new List<Guy>();
			for (var i = 0; i < 10; i++)
			{
				army.Add(GuyFactory.CreateGuy(GuyType.ARMORED));
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

			foreach(var fort in friendlyForts)
			{
				if (fort.DefendingGuys.Count >= 15)
				{
					var closestFort = GetClosestFort(enemyForts.Concat(neutralForts).ToList(), fort);
					moves.AddRange(MoveAll(fort, closestFort));
				}
			}

			return moves;
		}
	}
}
