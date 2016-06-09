namespace ConsoleApplication1
{
	public class Move
	{
		public Move(Fort source, Fort dest, int numGuys)
		{
			Source = source;
			Dest = dest;
			NumGuys = numGuys;
		}

		public Fort Source;
		public Fort Dest;
		public int NumGuys;
	}
}
