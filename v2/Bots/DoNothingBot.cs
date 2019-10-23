using System;
using System.Collections.Generic;

namespace Cell.Bots
{
	class DoNothingBot : IBot
	{
		public List<Tuple<int, int, int>> Do(string board)
		{
			return new List<Tuple<int, int, int>>();
		}

		public void SetPlayer(string player)
		{
		}

	}
}
