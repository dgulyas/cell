using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Cell.Bots
{
	class Bot1 : IBot
	{
		private string m_player;

		public void SetPlayer(string player)
		{
			m_player = player;
		}

		public List<Tuple<int, int, int>> Do(string boardString)
		{
			var board = JsonConvert.DeserializeObject<BoardState>(boardString);


			var friendlyForts = GetFriendlyForts(m_player, board);
			var enemyForts = GetEnemyForts(m_player, board);

			var moves = new List<Tuple<int, int, int>>();

			if (enemyForts.Count > 0)
			{
				foreach (var fort in friendlyForts)
				{
					if (fort.NumDefendingGuys > 0)
					{
						var move = new Tuple<int, int, int>(fort.ID, enemyForts[0].ID, fort.NumDefendingGuys);
						moves.Add(move);
					}
				}
			}

			return moves;
		}

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
