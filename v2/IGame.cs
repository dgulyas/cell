using System.Collections.Generic;
using System.Text;

namespace Cell
{
	public interface IGame
	{
		string PlayGame(string jsonMap, Dictionary<string, IBot> players, StringBuilder gameState);
	}
}
