using System;
using System.Collections.Generic;
using System.Linq;

namespace Cell.Bots
{
	class HumanBot : IBot
	{
		private Player m_player;

		public void Do(Board board)
		{
			Console.WriteLine($"{m_player.Name}'s Turn");
			while (true)
			{
				Console.Write("Exit(e) or Move(m): ");
				var modeChoice = Console.ReadLine();
				modeChoice = modeChoice?.ToLower();
				switch (modeChoice)
				{
					case "e":
						return;
					case "m":
						GetAndProcessMove(board);
						break;
					default:
						continue;
				}

			}
		}

		private void GetAndProcessMove(Board board)
		{
			Console.Write("Enter move in format <Source Fort Index> <Num Guys> <Destination Fort Index>: ");
			var moveInput = Console.ReadLine();
			var moveStringTokens = moveInput?.Split(' ');
			if (moveStringTokens == null) return;

			var moveIntTokens = new List<int>();

			foreach (var moveStringToken in moveStringTokens)
			{
				int parseResult;
				if (int.TryParse(moveStringToken, out parseResult))
				{
					moveIntTokens.Add(parseResult);
				}
			}

			if (moveIntTokens.Count != 3) return;

			//forts must exist
			if (moveIntTokens[0] >= board.Forts.Count || moveIntTokens[0] < 0) return;
			if (moveIntTokens[2] >= board.Forts.Count || moveIntTokens[2] < 0) return;

			var sourceFort = board.Forts[moveIntTokens[0]];
			var numGuysToMove = moveIntTokens[1];
			var destFort = board.Forts[moveIntTokens[2]];

			//number of guys must exist in fort in order to move them
			if (numGuysToMove > sourceFort.NumDefendingGuys ) return;

			//this player must own the fort in order to move guys from it.
			if (sourceFort.FortOwner != m_player) return;

			board.DoMove(new Move(sourceFort, destFort, numGuysToMove), m_player);
		}

		public void SetPlayer(Player player)
		{
			m_player = player;
		}

		public Player GetPlayer()
		{
			return m_player;
		}
	}
}
