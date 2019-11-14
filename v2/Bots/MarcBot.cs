using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Cell.Bots
{
	/// <summary>
	/// Waits until there are at least 15 guys defending before moving them as a group
	/// </summary>
	class MarcBot : BaseBot
	{
		public override List<Guy> SetStartingArmy()
		{
			var army = new List<Guy>();
			for (var i = 0; i < 4; i++)
			{
				army.Add(GuyFactory.CreateGuy(GuyType.ARMORED));
			}
			for (var i = 0; i < 5; i++)
			{
				army.Add(GuyFactory.CreateGuy(GuyType.BEEFY));
			}
			for (var i = 0; i < 1; i++)
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
			var otherForts = enemyForts.Concat(neutralForts).ToList();

			var moves = new List<Move>();

			// special opening move
			if (board.TurnNumber == 1)
			{
				var myFort = friendlyForts[0];

				// try to steal a neutral fort with my runners
				var closestFort = GetClosestFort(neutralForts, myFort);
				moves.Add(MoveRunner(myFort, closestFort, FilterRunner(myFort.DefendingGuys).Count));

				return moves;
			}

			if (board.TurnNumber == 5)
			{
				var myFort = friendlyForts[0];

				// try to get a quick kill
				var closestFort = GetClosestFort(enemyForts, myFort);
				moves.Add(MoveAverage(myFort, closestFort, FilterAverage(myFort.DefendingGuys).Count));
				moves.Add(MoveBeefy(myFort, closestFort, FilterBeefy(myFort.DefendingGuys).Count));

				return moves;
			}

			foreach (var fort in friendlyForts)
			{
				if (enemyForts.Count < 1)
					return moves;

				var orderedForts = GetOrderedFortsByDistance(enemyForts, fort);

				// keep at least 10 armored guys in my base, send in groups of 5 after that
				var numArmored = FilterArmored(fort.DefendingGuys).Count;
				if (numArmored > 10 && numArmored % 5 == 0)
				{
					var targetFort = orderedForts[0];
					moves.Add(MoveArmored(fort, targetFort, 5));
				}

				// send beefy guys in groups of 2
				var numBeefy = FilterBeefy(fort.DefendingGuys).Count;
				if (numBeefy % 2 == 0)
				{
					var targetFort = orderedForts.Count > 1 ? orderedForts[1] : orderedForts[0];
					moves.Add(MoveBeefy(fort, targetFort, 2));
				}

				// harass with average in groups of 4
				var numAverage = FilterAverage(fort.DefendingGuys).Count;
				if (numAverage % 4 == 0)
				{
					var targetFort = orderedForts.Count > 2 ? orderedForts[2] : orderedForts[0];
					moves.Add(MoveAverage(fort, targetFort, 4));
				}
			}

			return moves;
		}
	}
}
