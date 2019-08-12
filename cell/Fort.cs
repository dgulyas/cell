using Newtonsoft.Json;

namespace Cell
{
	public class Fort
	{
		public Point Location;
		public Player FortOwner;
		public int BirthSpeed;

		public int NumDefendingGuys;

		private static int InstanceCounter;
		[JsonIgnoreAttribute]
		public int ID;

		public Fort()
		{
			ID = InstanceCounter;
			InstanceCounter++;
		}

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

		public override string ToString()
		{
			return $"Fort -> X:{Location.X} Y:{Location.Y} NumGuys:{NumDefendingGuys} Owner:{FortOwner?.Name ?? "Unowned"}";
		}

		public Fort Clone()
		{
			return new Fort
			{
				BirthSpeed = BirthSpeed,
				NumDefendingGuys = NumDefendingGuys,
				FortOwner = FortOwner?.Clone(),
				Location = Location.Clone(),
				ID = ID
			};
		}
	}
}
