using System.Collections.Generic;
using System.Linq;
using CellTournament.Cell;

namespace CellTournament
{
	public static class AiHelper
	{
		public static List<Fort> GetFriendlyForts(Player player, Board board)
		{
			return board.Forts.Values.Where(f => f.FortOwner?.Name == player.Name).ToList();
		}

		public static List<Fort> GetUnclaimedForts(Player player, Board board)
		{
			return board.Forts.Values.Where(f => f.FortOwner == null).ToList();
		}

		public static List<Fort> GetEnemyForts(Player player, Board board)
		{
			return board.Forts.Values.Where(f => f.FortOwner != null && f.FortOwner.Name != player.Name).ToList();
		}

		public static List<GuyGroup> GetFriendlyGuyGroups(Player player, Board board)
		{
			return board.TravelingGGs.Where(tgg => tgg.GroupOwner.Name == player.Name).ToList();
		}

		public static List<GuyGroup> GetEnemyGuyGroups(Player player, Board board)
		{
			return board.TravelingGGs.Where(tgg => tgg.GroupOwner.Name != player.Name).ToList();
		}

	}
}
