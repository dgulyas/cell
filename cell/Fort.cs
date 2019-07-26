using Newtonsoft.Json;

namespace Cell
{
	public class Fort
	{
		public Point Location;
		public Player FortOwner;
		public int BirthSpeed;

		public int NumDefendingGuys;

		[JsonIgnoreAttribute]
		public string ID => Location.GetDiscription();

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
	}
}
