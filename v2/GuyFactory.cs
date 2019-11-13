using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cell
{
	public enum GuyType
	{
		AVERAGE,
		ARMORED,
		BEEFY,
		RUNNER
	}

	public class Guy
	{
		public int Strength;
		public int Health;
		public int Speed;
		public GuyType Type;
	}

	public static class GuyFactory
	{
		public static Guy CreateGuy(GuyType guyType)
		{
			switch(guyType){
				default:
				case GuyType.AVERAGE:
					return new Guy
					{
						Strength = 1,
						Health = 2,
						Speed = 2,
						Type = guyType
					};
				case GuyType.BEEFY:
					return new Guy
					{
						Strength = 2,
						Health = 1,
						Speed = 1,
						Type = guyType
					};
				case GuyType.ARMORED:
					return new Guy
					{
						Strength = 1,
						Health = 2,
						Speed = 1,
						Type = guyType
					};
				case GuyType.RUNNER:
					return new Guy
					{
						Strength = 1,
						Health = 1,
						Speed = 3,
						Type = guyType
					};
			}
		}
	}
}
