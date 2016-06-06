using System.Collections.Generic;

namespace ConsoleApplication1
{
	class Fort
	{
		public Point Location;
		public Player FortOwner;
		public int UnitSpeed;
		public int BirthSpeed;

		public int NumDefendingGuys;

		public void CreateGuys()
		{
			NumDefendingGuys += BirthSpeed;
		}

		public GuyGroup SendGuyGroup(Fort destination, int numGuys)
		{
			if (numGuys > NumDefendingGuys)
			{
				return null;
			}
			NumDefendingGuys -= numGuys;

			return new GuyGroup(destination, numGuys, Location, FortOwner);
		}

		public void ReceiveGuys(int attGuys, Player attackingPlayer)
		{
			if (attackingPlayer == FortOwner)
			{
				DefendingGuys.AddRange(attGuys);
				return;
			}
			
			if(attGuys.Count > DefendingGuys.Count){
				//attackers win
				for(int i = 0; i < DefendingGuys.Count; i++){
					attGuys.RemoveAt(0);
				}
				
				DefendingGuys = attGuys;
				FortOwner = attackingPlayer;
			}else{
				//defenders win
				for(int i = 0; i < attGuys.Count; i++){
					DefendingGuys.RemoveAt(0);
				}
			}
			

		}


	}
}
