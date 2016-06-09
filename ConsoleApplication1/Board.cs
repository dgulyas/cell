using System.Collections.Generic;
using System.Linq;

namespace ConsoleApplication1
{
	public class Board
	{
		public int Turn;
		public List<Fort> Forts = new List<Fort>();
		public List<Player> Players = new List<Player>();
		public List<GuyGroup> TravelingGGs = new List<GuyGroup>();

		public Board()
		{
			Turn = 1;
		}

		public void AddFort(Fort fort)
		{
			Forts.Add(fort);
		}

		public void AddPlayer(Player player)
		{
			Players.Add(player);
		}


		//Returns the winning player, otherwise null.
		public Player GetTheWinner()
		{
			Player tmpPlayer = null;
			foreach (var fort in Forts)
			{
				if (fort.FortOwner == null) continue;
				if (tmpPlayer == null)
				{
					tmpPlayer = fort.FortOwner;
				}
				else
				{
					if (fort.FortOwner != tmpPlayer)
					{
						return null;
					}
				}
			}
			return tmpPlayer;
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

	}
}
