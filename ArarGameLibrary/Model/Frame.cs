using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArarGameLibrary.Model
{
    public class Frame : Graph
    {
        public Frame(Vector2 point1, Vector2 point2, Vector2 point3, Vector2 point4 , Color lineColor) : base(true)
        {
            PopulatePoints(point1,point2,point3,point4);
            PopulateLines(lineColor);
        }
    }
}
