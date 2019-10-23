using Newtonsoft.Json;

namespace CellTournament.Cell
{
	public class Fort
	{
		public Point Location;
		public int BirthSpeed;
		public int NumDefendingGuys;
		public Player FortOwner;

		private static int m_instanceCounter;
		[JsonIgnore]
		public int ID;

		public Fort()
		{
			ID = m_instanceCounter;
			m_instanceCounter++;
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

		public string GetOwnerName()
		{
			return FortOwner?.Name;
		}

		public override string ToString()
		{
			return $"Fort -> X:{Location.X} Y:{Location.Y} NumGuys:{NumDefendingGuys} BirthSpeed:{BirthSpeed} Owner:{FortOwner?.Name ?? "Unowned"}";
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
