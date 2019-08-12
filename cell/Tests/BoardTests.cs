using System.Collections.Generic;
using NUnit.Framework;

namespace Cell.Tests
{
	[TestFixture]
	public class BoardTests
	{
		[Test, TestCaseSource(nameof(GetWinnerTestCases))]
		public void TestGetTheWinnerWithWinner(List<Fort> forts, Player expectedWinner)
		{
			var board = new Board();
			foreach (var fort in forts)
			{
				board.Forts.Add(fort.ID, fort);
			}
			var winner = board.GetTheWinner();
			Assert.AreEqual(expectedWinner, winner);
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
