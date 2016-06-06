using System;

namespace ConsoleApplication1
{
	public class Point
	{
		public int X;
		public int Y;
		
		public double GetDistanceTo(Point dest){
			double dX = X - dest.X;
			double dY = Y - dest.Y;
			return Math.Sqrt(dX * dX + dY * dY);
		}
	}
}
