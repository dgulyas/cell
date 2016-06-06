using System;

namespace ConsoleApplication1
{
	class Program
	{
		static void Main()
		{
			var player1 = new Player { name = "p1" };
			var player2 = new Player { name = "p2" };

			var board = new Board();
			board.AddPlayer(player1);
			board.AddPlayer(player2);

			var fortUpperRight = new Fort { Location = new Point { X = 80, Y = 80 }, BirthSpeed = 5, UnitAttack = 10, UnitHealth = 10, UnitSpeed = 10, FortOwner = player2 };
			var fortUpperLeft = new Fort { Location = new Point { X = 20, Y = 80 }, BirthSpeed = 5, UnitAttack = 10, UnitHealth = 10, UnitSpeed = 10 };
			var fortLowerRight = new Fort { Location = new Point { X = 80, Y = 20 }, BirthSpeed = 5, UnitAttack = 10, UnitHealth = 10, UnitSpeed = 10 };
			var fortLowerLeft = new Fort { Location = new Point { X = 20, Y = 20 }, BirthSpeed = 5, UnitAttack = 10, UnitHealth = 10, UnitSpeed = 10, FortOwner = player1 };
			board.AddFort(fortUpperRight);
			board.AddFort(fortUpperLeft);
			board.AddFort(fortLowerRight);
			board.AddFort(fortLowerLeft);

			var winner = board.GetTheWinner();

			while (winner == null)
			{
				board.CreateGuys();
				//board.ResolveCombats();
				board.MoveGuyGroups();


				winner = board.GetTheWinner();
			}
			Console.WriteLine("The Winner is " + winner);
		}
	}
}
