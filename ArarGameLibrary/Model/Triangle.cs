using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArarGameLibrary.Model
{
    public class Triangle : Graph
    {
        public List<Vector2> PointListAmongPoint2Point3 { get; set; }

        public Triangle(Vector2 point1, Vector2 point2, Vector2 point3, Color lineColor, float thickness = 1f)
            : base(true)
        {
        //    var points = SortPointsByXAscending(point1, point2, point3);

        //    point1 = points[0];
        //    point2 = points[1];
        //    point3 = points[2];

            PointListAmongPoint2Point3 = new List<Vector2>();

            PointListAmongPoint2Point3.Add(point1);

            PointListAmongPoint2Point3.Add(point2);

            var deltaY = point3.Y-point2.Y;

            var deltaX = point3.X-point2.X;

            var slope = 0f;

            if (deltaX != 0f)
                slope = deltaY / deltaX;

            var angle = Math.Atan(slope);

            var radians = angle * 180 / Math.PI;

            //var newPointDeltaY = point2.Y + ((point2.X + 1)-point2.X) * slope;

            //PointListAmongPoint2Point3.Add(new Vector2(point2.X + 1,newPointDeltaY));

            for (int x = (int)point2.X; x < point3.X; x++)
            {
                var y = point2.Y + (x - point2.X) * slope;

                if (PointListAmongPoint2Point3.Any(p => p.X == x && p.Y == y))
                    continue;

                PointListAmongPoint2Point3.Add(new Vector2(x, y));
            }

            var startY = 0f;
            var finishY = 0f;

            if (point2.Y >= point3.Y)
            {
                startY = point3.Y;
                finishY = point2.Y;
            }
            else
            {
                startY = point2.Y;
                finishY = point3.Y;
            }

            for (int y = (int)startY; y < finishY; y++)
            {
                var x = point2.X + (y - point2.Y) / slope;

                if (PointListAmongPoint2Point3.Any(p => p.X == x && p.Y == y))
                    continue;

                PointListAmongPoint2Point3.Add(new Vector2(x, y));
            }



            PointListAmongPoint2Point3.Add(point3);

            PopulatePoints(PointListAmongPoint2Point3.ToArray());

            PopulateLines(lineColor, thickness);

            SetPosition(GetPointWithMinXY());
        }

        private List<Vector2> SortPointsByXAscending(params Vector2[] points)
        {
            return points.OrderBy(p => p.X).ToList();
        }

        private List<Vector2> SortPointsByYAscending(params Vector2[] points)
        {
            return points.OrderBy(p => p.Y).ToList();
        }
    }
}
