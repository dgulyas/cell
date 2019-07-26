using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cell
{
	public class Board
	{
		public int Turn;
		public List<Fort> Forts = new List<Fort>();
		public List<GuyGroup> TravelingGGs = new List<GuyGroup>();

		public Board()
		{
			Turn = 1;
		}

		//Returns the winning player, otherwise null.
		public Player GetTheWinner()
		{
			var remainingPlayers = Forts.Select(f => f.FortOwner).Where(fo => fo != null).Distinct().ToList();
			if (remainingPlayers.Count == 1)
			{
				return remainingPlayers.First();
			}
			return null;
		}

		public void CreateGuys()
		{
			Forts.ForEach(f => f.CreateGuys());
		}

		public void MoveGuyGroups()
		{
			var ggsToRemove = new List<GuyGroup>();
			foreach (var travelingGuyGroup in TravelingGGs)
			{
				if(travelingGuyGroup.TicksTillFinished < 1){
					travelingGuyGroup.EnterFort();
					ggsToRemove.Add(travelingGuyGroup);
				}else{
					travelingGuyGroup.TicksTillFinished--;
				}
			}

			foreach (var guyGroup in ggsToRemove)
			{
				TravelingGGs.Remove(guyGroup);
			}
		}

		public void DoMove(Move move, Player player)
		{
			if (move.Source.FortOwner == player && move.NumGuys > 0 && move.Source.NumDefendingGuys >= move.NumGuys)
			{
				TravelingGGs.Add(move.Source.SendGuyGroup(move.Dest, move.NumGuys));
			}
		}

		public override string ToString()
		{
			var sb = new StringBuilder();

			sb.AppendLine($"{Environment.NewLine}Turn:{Turn}");
			foreach (var fort in Forts)
			{
				sb.AppendLine(fort.ToString());
			}

			foreach (var gg in TravelingGGs)
			{
				sb.AppendLine(gg.ToString());
			}

			return sb.ToString();
		}

	}
}
