using System;
using System.Collections.Generic;
using CellTournament.Cell;
using Newtonsoft.Json;

//Ideally this will start a web service that remote bots can use
//to interact with the game.
namespace CellTournament.Bots
{
	public class WebBot : IBot
	{
		private Player m_player;

		private string jsonBoard;

		public List<Move> Do(Board board)
		{
			jsonBoard = JsonConvert.SerializeObject(board);
			Console.WriteLine();
			return new List<Move>();
		}

		public void SetPlayer(Player player)
		{
			m_player = player;
		}

	}
}
