using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApplication1
{
	public static class AiHelper
	{
		public static List<Fort> GetFriendlyForts(Player player, Board board)
		{
			return board.Forts.Where(f => f.FortOwner == player).ToList();
		}

		public static List<Fort> GetUnclaimedForts(Player player, Board board)
		{
			return board.Forts.Where(f => f.FortOwner == null).ToList();
		}

		public static List<Fort> GetEnemyForts(Player player, Board board)
		{
			return board.Forts.Where(f => f.FortOwner != player && f.FortOwner != null).ToList();
		}

		public static List<GuyGroup> GetFriendlyGuyGroups(Player player, Board board)
		{
			return board.TravelingGGs.Where(tgg => tgg.GroupOwner == player).ToList();
		}

		public static List<GuyGroup> GetEnemyGuyGroups(Player player, Board board)
		{
			return board.TravelingGGs.Where(tgg => tgg.GroupOwner != player).ToList();
		}

	}
}
