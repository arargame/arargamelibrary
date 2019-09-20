using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArarGameLibrary.Extension;

namespace ArarGameLibrary.Model
{
    public class Frame : Graph
    {
        public Frame(Vector2 point1, Vector2 point2, Vector2 point3, Vector2 point4 , Color lineColor , float thickness = 1f) : base(true)
        {
            PopulatePoints(point1,point2,point3,point4);

            PopulateLines(lineColor,thickness);

            SetPosition(GetPointWithMinXY());

            SetSize(new Vector2(point2.X - point1.X, point3.Y - point2.Y));
        }

        public static Frame Create(Rectangle rect,Color lineColor, float thickness = 1f)
        {
            return new Frame(rect.TopLeftEdge(), rect.TopRightEdge(), rect.BottomRightEdge(), rect.BottomLeftEdge(), lineColor,thickness);
        }
    }
}
