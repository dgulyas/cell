using System.Collections.Generic;

namespace Cell.Bots
{
	public interface IBot
	{
		List<Move> Do(Board board);
		void SetPlayer(Player player);
	}
}
