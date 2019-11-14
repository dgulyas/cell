using System;
using System.Collections.Generic;

namespace Cell
{
	public class MapLibrary
	{
		public static int NumMaps = 5;

		public static List<Fort> GetMap(int i)
		{
			switch (i)
			{
				case 1:
					return Map1();
				case 2:
					return Map2();
				case 3:
					return Map3();
				case 4:
					return Map4();
				case 5:
					return Map5();
				default:
					throw new Exception("Bad map number");
			}
		}

		//Diamond
		public static List<Fort> Map1()
		{
			var forts = new List<Fort>();
			forts.Add(new Fort { ID = 1, Location = new Point { X = 0, Y = 5 }, BirthSpeed = 3, BirthingType = GuyType.AVERAGE, DefendingGuys = new List<Guy>(), FortOwner = "P1", DefensiveBonus = 0 });
			forts.Add(new Fort { ID = 2, Location = new Point { X = 10, Y = 0 }, BirthSpeed = 3, BirthingType = GuyType.AVERAGE, DefendingGuys = new List<Guy>(), FortOwner = null, DefensiveBonus = 0 });
			forts.Add(new Fort { ID = 3, Location = new Point { X = 10, Y = 5 }, BirthSpeed = 3, BirthingType = GuyType.AVERAGE, DefendingGuys = new List<Guy>(), FortOwner = null, DefensiveBonus = 0 });
			forts.Add(new Fort { ID = 4, Location = new Point { X = 10, Y = 10 }, BirthSpeed = 3, BirthingType = GuyType.AVERAGE, DefendingGuys = new List<Guy>(), FortOwner = null, DefensiveBonus = 0 });
			forts.Add(new Fort { ID = 5, Location = new Point { X = 20, Y = 5 }, BirthSpeed = 3, BirthingType = GuyType.AVERAGE, DefendingGuys = new List<Guy>(), FortOwner = "P2", DefensiveBonus = 0 });
			return forts;
		}

		//Line
		public static List<Fort> Map2()
		{
			var forts = new List<Fort>();
			forts.Add(new Fort { ID = 1, Location = new Point { X = 0, Y = 0 }, BirthSpeed = 3, BirthingType = GuyType.AVERAGE, DefendingGuys = new List<Guy>(), FortOwner = "P1", DefensiveBonus = 0 });
			forts.Add(new Fort { ID = 2, Location = new Point { X = 5, Y = 0 }, BirthSpeed = 3, BirthingType = GuyType.AVERAGE, DefendingGuys = new List<Guy>(), FortOwner = null, DefensiveBonus = 0 });
			forts.Add(new Fort { ID = 3, Location = new Point { X = 10, Y = 0 }, BirthSpeed = 3, BirthingType = GuyType.AVERAGE, DefendingGuys = new List<Guy>(), FortOwner = null, DefensiveBonus = 0 });
			forts.Add(new Fort { ID = 4, Location = new Point { X = 15, Y = 0 }, BirthSpeed = 3, BirthingType = GuyType.AVERAGE, DefendingGuys = new List<Guy>(), FortOwner = null, DefensiveBonus = 0 });
			forts.Add(new Fort { ID = 5, Location = new Point { X = 20, Y = 0 }, BirthSpeed = 3, BirthingType = GuyType.AVERAGE, DefendingGuys = new List<Guy>(), FortOwner = "P2", DefensiveBonus = 0 });
			return forts;
		}

		//Big Center
		public static List<Fort> Map3()
		{
			var forts = new List<Fort>
			{
				MakeFort(0, 10, "P1", 1),
				MakeFort(5, 5, null, 2),
				MakeFort(10, 0, "P1", 3),
				MakeFort(5, 15, null, 4),
				MakeFort(15, 5, null, 6),
				MakeFort(10, 20, "P2", 7),
				MakeFort(15, 15, null, 8),
				MakeFort(20, 10, "P2", 9)
			};

			var bigCenterFort = MakeFort(10, 10, null, 5);
			bigCenterFort.BirthSpeed = 15;
			forts.Add(bigCenterFort);

			return forts;
		}

		//Neighbours
		public static List<Fort> Map4()
		{
			var forts = new List<Fort>
			{
				MakeFort(0, 0, "P1", 1),
				MakeFort(2, 0, "P2", 2),
				MakeFort(0, 10, null, 3),
				MakeFort(2, 10, null, 4)
			};

			return forts;
		}

		//Many small forts
		public static List<Fort> Map5()
		{
			var forts = new List<Fort>();

			for (int i = 1; i < 6; i++)
			{
				for (int j = 1; j < 6; j++)
				{
					forts.Add(MakeFort(i*3, j*3, null, (i-1)*5+j));
				}
			}

			var p1Fort = MakeFort(0, 9, "P1", 26);
			p1Fort.BirthSpeed = 15;
			forts.Add(p1Fort);

			var p2Fort = MakeFort(0, 9, "P2", 27);
			p1Fort.BirthSpeed = 15;
			forts.Add(p2Fort);

			return forts;
		}


		public static Fort MakeFort(int x, int y, string owner, int id)
		{
			return new Fort
			{
				ID = id,
				Location = new Point {X = x, Y = y},
				BirthSpeed = 3,
				BirthingType = GuyType.AVERAGE,
				DefendingGuys = new List<Guy>(),
				FortOwner = owner,
				DefensiveBonus = 0
			};
		}


	}
}
