using System.Collections.Generic;
using CellTournament.Cell;

namespace CellTournament.Bots
{
	public interface IBot
	{
		List<Move> Do(Board board);
		void SetPlayer(Player player);
	}
}
