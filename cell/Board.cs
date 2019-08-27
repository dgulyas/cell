using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cell
{
	public class Board
	{
		public Dictionary<int, Fort> Forts = new Dictionary<int, Fort>();
		public List<GuyGroup> TravelingGGs = new List<GuyGroup>();

		//Returns the winning player, otherwise null.
		public Player GetTheWinner()
		{
			var remainingPlayers = Forts.Values.Select(f => f.FortOwner).Where(fo => fo != null).Distinct().ToList();
			if (remainingPlayers.Count == 1)
			{
				return remainingPlayers.First();
			}
			return null;
		}

		public void CreateGuys()
		{
			foreach (var fort in Forts.Values)
			{
				fort.CreateGuys();
			}
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

		//returns true if move is accepted, false otherwise.
		public bool DoMove(Move move, Player player)
		{
			var sourceFort = Forts[move.SourceFortId];
			var destFort = Forts[move.DestFortId];

			if (sourceFort.FortOwner.Name == player.Name && move.NumGuys > 0 && sourceFort.NumDefendingGuys >= move.NumGuys)
			{
				TravelingGGs.Add(sourceFort.SendGuyGroup(destFort, move.NumGuys));
				return true;
			}

			return false;
		}

		public override string ToString()
		{
			var sb = new StringBuilder();

			//sb.AppendLine($"{Environment.NewLine}Turn:{Turn}");

			for (int i = 0; i < Forts.Count; i++)
			{
				sb.Append(Forts[i]);
				sb.AppendLine($" Index:{i}");
			}

			foreach (var gg in TravelingGGs)
			{
				sb.AppendLine(gg.ToString());
			}

			return sb.ToString();
		}

		public Board Clone()
		{
			var b = new Board();
			foreach (var fortKey in Forts.Keys)
			{
				b.Forts.Add(fortKey, Forts[fortKey].Clone());
			}

			foreach (var travelingGG in TravelingGGs)
			{
				b.TravelingGGs.Add(travelingGG.Clone());
			}

			return b;
		}

	}
}
