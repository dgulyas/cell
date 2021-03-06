﻿using System;

namespace CellTournament.Cell
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

		public string GetDiscription()
		{
			return $"X:{X} Y:{Y}";
		}

		public Point Clone()
		{
			return  new Point{X = X, Y = Y};
		}
	}
}
