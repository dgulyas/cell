using System;
using System.Collections.Generic;

namespace Cell.Bots
{
	class DoNothingBot : IBot
	{
		public List<Move> Do(string board)
		{
			return new List<Move>();
		}

		public void SetPlayer(string player)
		{
		}

	}
}
