namespace ConsoleApplication1
{
	public class GuyGroup
	{
		public Player GroupOwner;
		public int NumGuys;
		
		//The number of turns/ticks until the group reaches its destination fort.
		public int TicksTillFinished;

		//where this group is heading;
		public Fort DestinationFort;

		// ReSharper disable once InconsistentNaming
		private static int InstanceCounter;
		public int InstanceID;

		public GuyGroup(Fort destinationFort, int numGuys, Point currentLocation, Player owner)
		{
			DestinationFort = destinationFort;
			NumGuys = numGuys;
			TicksTillFinished = (int)currentLocation.GetDistanceTo(destinationFort.Location);
			GroupOwner = owner;
			InstanceID = InstanceCounter++;
		}
		
		public void EnterFort(){
			DestinationFort.ReceiveGuys(NumGuys, GroupOwner);
		}

		public string GetDescription()
		{
			return $"GuyGroup -> ID:{InstanceID} owner:{GroupOwner.Name} numGuys:{NumGuys} ticksLeft:{TicksTillFinished} fort:{DestinationFort.ID}";
		}
		
	}
}
