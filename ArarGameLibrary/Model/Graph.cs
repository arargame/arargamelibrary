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
    public class Graph : Sprite 
    {
        public List<Line> Lines { get; set; }

        public LinkedList<Vector2> Points { get; set; }

        public bool IsClosedType { get; set; }

        public bool IsFilled { get; set; }

        public List<Line> FillingLines { get; set; }

        public Color LinesColor 
        {
            get
            {
                return Lines.FirstOrDefault().Color;
            }
        }

        public float LinesThickness
        {
            get
            {
                return Lines.FirstOrDefault().Thickness;
            }
        }

        public Graph(bool isClosedType = false)
        {
            Points = new LinkedList<Vector2>();

            Lines = new List<Line>();

            FillingLines = new List<Line>();

            IsClosedType = isClosedType;

            SetVisible(true);
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void LoadContent(Texture2D texture = null)
        {
            foreach (var line in Lines)
            {
                if (IsInPerformanceMode)
                    line.IsInPerformanceMode = IsInPerformanceMode;

                line.LoadContent();
            }
        }

        public override void UnloadContent()
        {

        }

        public override void Update(GameTime gameTime = null)
        {
            if (IsActive)
            {
                base.Update(gameTime);

                if (Texture != null)
                {
                    foreach (var line in Lines)
                    {
                        line.SetVisible(IsVisible);
                        line.Update();
                    }

                    foreach (var line in FillingLines)
                    {
                        line.SetVisible(IsVisible);
                        line.Update();
                    }
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch = null)
        {
            if (IsVisible)
            {
                if (Texture != null)
                {
                    base.Draw(spriteBatch);
                }
                else
                {
                    foreach (var line in Lines)
                    {
                        line.Draw(spriteBatch);
                    }

                    foreach (var line in FillingLines)
                    {
                        line.Draw(spriteBatch);
                    }
                }
            }
        }

        public Graph PopulatePoints(params Vector2[] points)
        {
            foreach (var point in points)
            {
                Points.AddLast(point);
            }

            return this;
        }

        public Graph PopulateLines(Color color, float thickness = 1f)
        {
            var point = Points.First;

            for (int i = 0; i < Points.Count; i++)
            {
                if (point.Next != null)
                {
                    var line = new Line(color, point.Value, point.Next.Value, thickness);

                    Lines.Add(line);

                    point = point.Next;
                }
            }

            if (IsClosedType)
            {
                var last = Points.Last;

                var first = Points.First;

                Lines.Add(new Line(color, last.Value, first.Value, thickness));
            }


            return this;
        }

        public Graph PopulateLines(params Line[] lines)
        {
            Lines.AddRange(lines);

            return this;
        }

        public Graph ChangeAllLinesColor(Color color)
        {
            Lines.ForEach(l => l.ChangeColor(color));

            return this;
        }


        public Vector2 GetPointWithMinX()
        {
            var minX = Points.Min(p => p.X);

            return Points.Where(p => p.X == minX).FirstOrDefault();
        }

        public Vector2 GetPointWithMinY()
        {
            var minY = Points.Min(p => p.Y);

            return Points.Where(p => p.Y == minY).FirstOrDefault();
        }

        public Vector2 GetPointWithMinXY()
        {
            return Points.Where(p => p.X == GetPointWithMinX().X)
                        .OrderBy(p => p.Y)
                        .FirstOrDefault();
        }

        public static List<Vector2> SortPointsByXAscending(params Vector2[] points)
        {
            return points.OrderBy(p => p.X).ToList();
        }

        public static List<Vector2> SortPointsByYAscending(params Vector2[] points)
        {
            return points.OrderBy(p => p.Y).ToList();
        }

    }
}
