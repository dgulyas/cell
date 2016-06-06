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
			if (FortOwner != null)
			{
				NumDefendingGuys += BirthSpeed;
			}
		}

		public GuyGroup SendGuyGroup(Fort destination, int numGuys)
		{
			if (destination == this || numGuys > NumDefendingGuys)
			{
				return null;
			}

			NumDefendingGuys -= numGuys;

			return new GuyGroup(destination, numGuys, Location, FortOwner);
		}

		public void ReceiveGuys(int numAttGuys, Player attackingPlayer)
		{
			if (attackingPlayer == FortOwner)
			{
				NumDefendingGuys += numAttGuys;
				return;
			}

			NumDefendingGuys -= numAttGuys;

			if (NumDefendingGuys < 0)
			{
				FortOwner = attackingPlayer;
				NumDefendingGuys *= -1;
			}
		}


	}
}
