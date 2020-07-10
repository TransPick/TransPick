using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransPick.Utilities
{
    internal static class PropertyCalculator
    {
		internal static int GetWidth(int a, int b)
		{
			if (a >= b)
			{
				return Math.Abs(a - b);
			}
			else
			{
				return Math.Abs(b - a);
			}
		}

		internal static int GetHeight(int a, int b)
		{
			if (a >= b)
			{
				return Math.Abs(a - b);
			}
			else
			{
				return Math.Abs(b - a);
			}
		}

		internal static Point GetLeftUpperPoint(Point a, Point b)
		{
			if (a.X <= b.X)
			{
				return a;
			}
			else
			{
				return b;
			}
		}
	}
}
