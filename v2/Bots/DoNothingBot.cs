using System;
using System.Collections.Generic;

namespace Cell.Bots
{
	class DoNothingBot : BaseBot
	{
		public override List<Move> Do(string boardString)
		{
			return new List<Move>();
		}


		public override List<Guy> SetStartingArmy()
		{
			return new List<Guy>();
		}
	}
}
