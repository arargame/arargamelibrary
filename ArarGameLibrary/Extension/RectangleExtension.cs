using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArarGameLibrary.Extension
{
    public static class RectangleExtension
    {
        public static Vector2 TopRightEdge(this Rectangle rectangle)
        {
            return new Vector2(rectangle.Right,rectangle.Top);
        }

        public static Vector2 TopLeftEdge(this Rectangle rectangle)
        {
            return new Vector2(rectangle.Left, rectangle.Top);
        }

        public static Vector2 BottomRightEdge(this Rectangle rectangle)
        {
            return new Vector2(rectangle.Right, rectangle.Bottom);
        }

        public static Vector2 BottomLeftEdge(this Rectangle rectangle)
        {
            return new Vector2(rectangle.Left, rectangle.Bottom);
        }

        public static Vector2 Position(this Rectangle rectangle)
        {
            return new Vector2(rectangle.X, rectangle.Y);
        }

        public static Vector2 Size(this Rectangle rectangle)
        {
            return new Vector2(rectangle.Width, rectangle.Height);
        }
    }
}
