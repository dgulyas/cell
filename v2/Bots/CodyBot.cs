using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cell.Bots
{
	class CodyBot : BaseBot
	{
		public override List<Move> Do(string boardString)
		{
			var board = JsonConvert.DeserializeObject<BoardState>(boardString);

			var friendlyForts = GetFriendlyForts(board);
			var enemyForts = GetEnemyForts(board);
			var neutralForts = GetNeutralForts(board);

			var moves = new List<Move>();

			Fort runnersFort = null;

			Fort weakestEnemy = enemyForts[0];

			// find closest weak enemy and stockpile until can beat
			foreach (var fort in enemyForts)
			{
				if (fort.DefendingGuys.Count < weakestEnemy.DefendingGuys.Count)
				{
					weakestEnemy = fort;
				}
			}

			foreach (var fort in friendlyForts)
			{
				if (FilterRunner(fort.DefendingGuys).Count != 0)
				{
					runnersFort = fort;
				}
				if (fort.DefendingGuys.Sum(x => x.Strength) > weakestEnemy.DefendingGuys.Sum(x => x.Health))
				{
					moves.AddRange(MoveAll(fort, weakestEnemy));
				}
			}

			// find closest neutral fort and send the runners
			if(runnersFort != null)
			{
				if (runnersFort.DefendingGuys.Count != 0)
				{
					var closestNeutral = GetClosestFort(neutralForts, runnersFort);
					if(closestNeutral != null)
					{
						moves.Add(Move(runnersFort, closestNeutral, FilterRunner(runnersFort.DefendingGuys).Count, GuyType.RUNNER));
					}
					
				}
			}

			return moves;
		}

		public override List<Guy> SetStartingArmy()
		{
			var army = new List<Guy>();
			for (var i = 0; i < 8; i++)
			{
				army.Add(GuyFactory.CreateGuy(GuyType.ARMORED));
			}
			for (var i = 0; i < 2; i++)
			{
				army.Add(GuyFactory.CreateGuy(GuyType.RUNNER));
			}
			return army;
		}
	
	}
}
