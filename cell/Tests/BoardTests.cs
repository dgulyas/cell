using System.Collections.Generic;
using NUnit.Framework;

namespace Cell.Tests
{
	[TestFixture]
	public class BoardTests
	{
		[Test, TestCaseSource(nameof(GetWinnerTestCases))]
		//make this take a test source of a tuple where the first element is the forts and the second is the winner
		public void TestGetTheWinnerWithWinner(List<Fort> forts, Player winner)
		{
			var board = new Board();
			board.Forts.AddRange(forts);
			var winnner = board.GetTheWinner();
			Assert.AreEqual(winner, winnner);
		}

		public static object[] GetWinnerTestCases()
		{
			Player p1 = new Player { Name = "player1" };
			Player p2 = new Player { Name = "player2" };

			Fort fort1 = new Fort();
			Fort fort2 = new Fort();

			Fort p1Fort1 = new Fort { FortOwner = p1 };
			Fort p1Fort2 = new Fort { FortOwner = p1 };
			Fort p1Fort3 = new Fort { FortOwner = p1 };

			Fort p2Fort1 = new Fort { FortOwner = p2 };
			Fort p2Fort2 = new Fort { FortOwner = p2 };

			return new object[]
			{
				new object[] {new List<Fort> {p1Fort1, p1Fort2, p1Fort3, fort1, fort2}, p1},
				new object[] {new List<Fort> {p1Fort1, p1Fort2, p1Fort3}, p1},
				new object[] {new List<Fort> {p2Fort1, fort1, fort2}, p2},
				new object[] {new List<Fort> {fort1, fort2}, null},
				new object[] {new List<Fort> {p1Fort1, p1Fort2, p2Fort1, fort1, fort2}, null},
				new object[] {new List<Fort> {p1Fort1, p1Fort2, p2Fort1, p2Fort2, fort1, fort2}, null},
				new object[] {new List<Fort>(), null}
			};
		}
		
	}
}
