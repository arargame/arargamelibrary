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
            Lines = new List<Line>();

            Points = new LinkedList<Vector2>();

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
            if (IsVisible)
            {
                foreach (var line in Lines)
                {
                    line.SetVisible(IsVisible);
                    line.Update();
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch = null)
        {
            if (IsVisible)
            {
                foreach (var line in Lines)
                {
                    line.Draw();
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


        public Vector2 GetPointWithMinXY()
        {
            var minX = Points.Min(p => p.X);
            var minY = Points.Min(p=>p.Y);

            return Points.Where(p => p.X == minX && p.Y == minY).FirstOrDefault();
        }
    }
}
