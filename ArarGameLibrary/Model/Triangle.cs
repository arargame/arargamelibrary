using ArarGameLibrary.Manager;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArarGameLibrary.Model
{
    public class Triangle : Graph
    {
        public Vector2 FirstPoint { get; set; }

        public Vector2 SecondPoint { get; set; }

        public Vector2 ThirdPoint { get; set; }



        public Triangle(Vector2 point1, Vector2 point2, Vector2 point3, Color lineColor, float thickness = 1f)
            : base(true)
        {
            var points = SortPointsByXAscending(point1, point2, point3);

            FirstPoint = points[0];
            SecondPoint = points[1];
            ThirdPoint = points[2];

            PopulatePoints(FirstPoint,SecondPoint,ThirdPoint);
            PopulateLines(lineColor, thickness);

            SetPosition(new Vector2(GetPointWithMinX().X,GetPointWithMinY().Y));
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public List<Vector2> PointListAmong2Points(Vector2 point1,Vector2 point2)
        {
            var list = new List<Vector2>();

            var slopeAmongPoint1Point2 = Line.Slope(point1, point2);

            var startX = 0f;
            var finishX = 0f;

            if(point2.X > point1.X)
            {
                startX = point1.X + 1;
                finishX = point2.X;
            }
            else
            {
                startX = point2.X + 1;
                finishX = point1.X;
            }

            for (int x = (int)startX; x < finishX; x++)
            {
                var y = point1.Y + (x - point1.X) * slopeAmongPoint1Point2;

                if (list.Any(p => p.X == x && p.Y == y))
                    continue;

                list.Add(new Vector2(x, y));
            }

            return list;
        }

        public Triangle SetFilled(Color lineColor,bool enable = true)
        {
            if (IsFilled = enable)
            {
                var PointListAmongPoint1Point2 = new List<Vector2>();
                var PointListAmongPoint1Point3 = new List<Vector2>();
                var PointListAmongPoint2Point3 = new List<Vector2>();

                PointListAmongPoint1Point2 = PointListAmong2Points(FirstPoint, SecondPoint);
                PointListAmongPoint1Point3 = PointListAmong2Points(FirstPoint, ThirdPoint);
                PointListAmongPoint2Point3 = PointListAmong2Points(SecondPoint, ThirdPoint);

                for (int i = 0; i < PointListAmongPoint1Point2.Count; i++)
                {
                    FillingLines.Add(new Line(lineColor, ThirdPoint, PointListAmongPoint1Point2[i]));                    
                }

                for (int i = 0; i < PointListAmongPoint1Point3.Count; i++)
                {
                    FillingLines.Add(new Line(lineColor, SecondPoint, PointListAmongPoint1Point3[i]));
                }

                for (int i = 0; i < PointListAmongPoint2Point3.Count; i++)
                {
                    FillingLines.Add(new Line(lineColor, FirstPoint, PointListAmongPoint2Point3[i]));
                }

                foreach (var line in FillingLines)
                {
                    line.LoadContent();
                }

                var w = (int)Math.Abs(Points.Max(p => p.X) - Points.Min(p => p.X));
                var h = (int)Math.Abs(Points.Max(p => p.Y) - Points.Min(p => p.Y));

                SetPosition(new Vector2(0,0));
                SetSize(new Vector2(w, h));

                SetTexture(TextureManager.Shot(() => Draw(), w, h));

                if (Texture != null)
                {
                    FillingLines.Clear();
                }
            }

            return this;
        }

    }
}
