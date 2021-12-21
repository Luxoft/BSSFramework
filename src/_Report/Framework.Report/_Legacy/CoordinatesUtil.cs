using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Framework.Report
{

    public static class CoordinatesUtil
    {
        public static Rectangle GetContainingRectangle(IList<Point> points)
        {
            int minX = int.MaxValue;
            int maxX = 0;
            int minY = int.MaxValue;
            int maxY = 0;
            if (points.Count == 0) return new Rectangle();
            foreach (Point point in points)
            {
                if (minX > point.X)
                    minX = point.X;
                if (maxX < point.X)
                    maxX = point.X;
                if (minY > point.Y)
                    minY = point.Y;
                if (maxY < point.Y)
                    maxY = point.Y;
            }
            return new Rectangle(minX, minY, maxX - minX, maxY - minY);
        }
    }
}
