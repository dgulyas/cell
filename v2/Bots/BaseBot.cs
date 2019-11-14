using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cell.Bots
{
	abstract class BaseBot : IBot
	{
		protected string m_player;

		public void SetPlayer(string player)
		{
			m_player = player;
		}

		public abstract List<Guy> SetStartingArmy();

		public abstract List<Move> Do(string boardString);

		public List<Fort> GetFriendlyForts(BoardState board)
		{
			return board.Forts.Where(f => f.FortOwner == m_player).ToList();
		}

		public List<Fort> GetEnemyForts(BoardState board)
		{
			return board.Forts.Where(f => f.FortOwner != null && f.FortOwner != m_player).ToList();
		}

		public static List<Fort> GetNeutralForts(BoardState board)
		{
			return board.Forts.Where(f => f.FortOwner == null || f.FortOwner == string.Empty).ToList();
		}

		public static List<Guy> FilterGuys(List<Guy> guys, GuyType type)
		{
			return guys.Where(g => g.Type == type).ToList();
		}

		public static List<Guy> FilterAverage(List<Guy> guys)
		{
			return guys.Where(g => g.Type == GuyType.AVERAGE).ToList();
		}

		public static List<Guy> FilterArmored(List<Guy> guys)
		{
			return guys.Where(g => g.Type == GuyType.ARMORED).ToList();
		}

		public static List<Guy> FilterBeefy(List<Guy> guys)
		{
			return guys.Where(g => g.Type == GuyType.BEEFY).ToList();
		}

		public static List<Guy> FilterRunner(List<Guy> guys)
		{
			return guys.Where(g => g.Type == GuyType.RUNNER).ToList();
		}

		public static double GetDistanceBetween(Point source, Point dest)
		{
			double dX = source.X - dest.X;
			double dY = source.Y - dest.Y;
			return Math.Sqrt(dX * dX + dY * dY);
		}

		public static Move Move(Fort srcFort, Fort destFort, int numGuys, GuyType type)
		{
			return new Move { SourceFortID = srcFort.ID, DestinationFortID = destFort.ID, NumGuys = numGuys, GuyType = type };
		}

		public static List<Move> MoveAll(Fort srcFort, Fort destFort)
		{
			return new List<Move>
			{
				MoveAverage(srcFort, destFort, FilterAverage(srcFort.DefendingGuys).Count),
				MoveArmored(srcFort, destFort, FilterArmored(srcFort.DefendingGuys).Count),
				MoveBeefy(srcFort, destFort, FilterBeefy(srcFort.DefendingGuys).Count),
				MoveRunner(srcFort, destFort, FilterRunner(srcFort.DefendingGuys).Count)
			};
		}

		public static Move MoveAverage(Fort srcFort, Fort destFort, int numGuys)
		{
			return new Move { SourceFortID = srcFort.ID, DestinationFortID = destFort.ID, NumGuys = numGuys, GuyType = GuyType.AVERAGE };
		}

		public static Move MoveArmored(Fort srcFort, Fort destFort, int numGuys)
		{
			return new Move { SourceFortID = srcFort.ID, DestinationFortID = destFort.ID, NumGuys = numGuys, GuyType = GuyType.ARMORED };
		}

		public static Move MoveBeefy(Fort srcFort, Fort destFort, int numGuys)
		{
			return new Move { SourceFortID = srcFort.ID, DestinationFortID = destFort.ID, NumGuys = numGuys, GuyType = GuyType.BEEFY };
		}

		public static Move MoveRunner(Fort srcFort, Fort destFort, int numGuys)
		{
			return new Move { SourceFortID = srcFort.ID, DestinationFortID = destFort.ID, NumGuys = numGuys, GuyType = GuyType.RUNNER };
		}

		public Fort GetClosestFort(List<Fort> forts, Fort srcFort)
		{
			Fort closestFort = null;
			double shortestDistance = double.MaxValue;
			foreach (var fort in forts)
			{
				var distance = GetDistanceBetween(srcFort.Location, fort.Location);
				if (distance < shortestDistance)
				{
					shortestDistance = distance;
					closestFort = fort;
				}
			}
			return closestFort;
		}

		public List<Fort> GetOrderedFortsByDistance(List<Fort> forts, Fort srcFort)
		{
			return forts.OrderBy(f => f,
				Comparer<Fort>.Create(
					(x, y) => GetDistanceBetween(srcFort.Location, x.Location).CompareTo(GetDistanceBetween(srcFort.Location, y.Location)))).ToList();
		}
	}
}
