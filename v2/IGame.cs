using System.Collections.Generic;
using System.Text;

namespace Cell
{
	public interface IGame
	{
		//void SetMap(string jsonMap);
		//void SetPlayers(Dictionary<string, IBot> players);
		string PlayGame(string jsonMap, Dictionary<string, IBot> players, StringBuilder gameState);
		//string GetWinner();


	}
}
