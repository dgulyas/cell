using System;
using System.Collections.Generic;
using ConsoleApplication1.bots;

namespace ConsoleApplication1
{
	class Program
	{
		static void Main()
		{
			var player1 = new Player { Name = "p1" };
			var player2 = new Player { Name = "p2" };

			var bots = new List<IBot>();
			var bot1 = new BotOne();
			bot1.SetPlayer(player1);
			var bot2 = new BotOne();
			bot2.SetPlayer(player2);
			bots.Add(bot1);
			bots.Add(bot2);

			var board = new Board();
			board.AddPlayer(player1);
			board.AddPlayer(player2);

			var fortUpperRight = new Fort { Location = new Point { X = 80, Y = 80 }, BirthSpeed = 5, UnitSpeed = 10, FortOwner = player2 };
			var fortUpperLeft = new Fort { Location = new Point { X = 20, Y = 80 }, BirthSpeed = 5, UnitSpeed = 10 };
			var fortLowerRight = new Fort { Location = new Point { X = 80, Y = 20 }, BirthSpeed = 5, UnitSpeed = 10 };
			var fortLowerLeft = new Fort { Location = new Point { X = 20, Y = 20 }, BirthSpeed = 5, UnitSpeed = 10, FortOwner = player1 };
			board.AddFort(fortUpperRight);
			board.AddFort(fortUpperLeft);
			board.AddFort(fortLowerRight);
			board.AddFort(fortLowerLeft);

			var winner = board.GetTheWinner();



			while (winner == null)
			{
				board.CreateGuys();
				board.MoveGuyGroups();


				winner = board.GetTheWinner();

				foreach (var bot in bots)
				{
					var botMoves = bot.GetMoves(board);
					foreach (var botMove in botMoves)
					{
						//need to verify that the move is valid







					}
				}
			}
			Console.WriteLine("The Winner is " + winner);
		}
	}
}
