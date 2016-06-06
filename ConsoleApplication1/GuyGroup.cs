using System.Collections.Generic;
using System.Linq;
using System;

namespace ConsoleApplication1
{
	class GuyGroup
	{
		public Player GroupOwner;
		public int NumGuys;
		
		//The number of turns/ticks until the group reaches its destination fort.
		public int TicksTillFinished;

		//where this group is heading;
		public Fort DestinationFort;

		public GuyGroup(Fort destinationFort, int numGuys, Point currentLocation, Player owner)
		{
			DestinationFort = destinationFort;
			NumGuys = numGuys;
			TicksTillFinished = (int)currentLocation.GetDistanceTo(destinationFort.Location);
			GroupOwner = owner;
		}
		
		public void EnterFort(){
			DestinationFort.ReceiveGuys(NumGuys, GroupOwner);
		}
		
	}
}
