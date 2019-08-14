using System;
using System.Collections.Generic;

namespace Cell.Bots
{
	class HumanBot : IBot
	{
		private Player m_player;

		public List<Move> Do(Board board)
		{
			Console.WriteLine($"{m_player.Name}'s Turn");
			var moves = new List<Move>();
			while (true)
			{
				Console.Write("Exit(e) or Move(m): ");
				var modeChoice = Console.ReadLine();
				modeChoice = modeChoice?.ToLower();
				switch (modeChoice)
				{
					case "e":
						return moves;
					case "m":
						GetAndProcessMove(board, moves);
						break;
					default:
						continue;
				}

			}
		}

		private void GetAndProcessMove(Board board, List<Move> moves)
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

			//only move positive int number guys
			if (numGuysToMove < 1) return;

			//this player must own the fort in order to move guys from it.
			if (sourceFort.FortOwner.Name != m_player.Name) return;

			var move = new Move(sourceFort.ID, destFort.ID, numGuysToMove);
			board.DoMove(move, m_player);
			moves.Add(move);
		}

		public void SetPlayer(Player player)
		{
			m_player = player;
		}

	}
}
