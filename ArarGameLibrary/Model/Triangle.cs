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
                startX = point1.X ;
                finishX = point2.X;
            }
            else
            {
                startX = point2.X ;
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

                var rect = new Rectangle((int)Points.Min(p => p.X),
                    (int)Points.Min(p => p.Y),
                    (int)Points.Max(p => p.X) + 1,
                    (int)Points.Max(p => p.Y) + 1);

                SetPosition(new Vector2(0,0));
                SetSize(new Vector2(rect.Width, rect.Height));

                SetTexture(TextureManager.Shot(() => Draw(), rect.Width, rect.Height));
                SetTexture(TextureManager.Crop(Texture, new Rectangle(rect.X, rect.Y, rect.Width - rect.X, rect.Height - rect.Y)));

                TestInfo.Show(true);
                TestInfo.AddParameters("DestinationRectangle");

                if (Texture != null)
                {
                    FillingLines.Clear();
                }
            }

            return this;
        }

        /// <summary>
        /// .(leftX,topLeftY)Point1
        /// |\
        /// | \
        /// |  \
        /// |   .(rightX,(topLeftY+bottomLeftY)/2)Point2
        /// |  /
        /// | /
        /// |/
        /// .(leftX,bottomLeftY)Point3
        /// </summary>
        /// <param name="leftX"></param>
        /// <param name="topLeftY"></param>
        /// <param name="bottomLeftY"></param>
        /// <param name="rightX"></param>
        /// <param name="lineColor"></param>
        /// <param name="thickness"></param>
        /// <returns></returns>
        public static Triangle PlayButton(Color lineColor, float leftX = 50, float topLeftY = 0, float bottomLeftY = 350, float rightX = 400, float thickness = 1f)
        {
            var point1 = new Vector2(leftX, topLeftY);

            var point2 = new Vector2(leftX, bottomLeftY);

            var point3 = new Vector2(rightX, (topLeftY + bottomLeftY) / 2);

            var triangle = new Triangle(point1, point2, point3, lineColor, thickness);

            triangle.LoadContent();

            triangle.SetFilled(lineColor);

            return triangle;
        }

    }
}
