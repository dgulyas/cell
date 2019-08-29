namespace CellTournament.Cell
{
	public class Move
	{
		public Move(int source, int dest, int numGuys)
		{
			SourceFortId = source;
			DestFortId = dest;
			NumGuys = numGuys;
		}

		public int SourceFortId;
		public int DestFortId;
		public int NumGuys;
	}
}
