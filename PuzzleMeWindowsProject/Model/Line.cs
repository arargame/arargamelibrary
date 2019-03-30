using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PuzzleMeWindowsProject.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleMeWindowsProject.Model
{
    public class Line : BaseObject,IXna
    {
        public Color Color { get; set; }

        public Texture2D Texture { get; set; }

        public Vector2 From { get; set; }

        public Vector2 To { get; set; }

        public float Thickness { get; set; }

        public float Lenght { get; set; }

        public float Angle { get; set; }

        public Line(Color color, Vector2 from, Vector2 to, float thickness = 1f)
        {
            Color = color;

            From = from;

            To = to;

            Thickness = thickness;

            Initialize();
        }

        public void Initialize()
        {
            Lenght = Vector2.Distance(From, To);

            Angle = (float)Math.Atan2(To.Y - From.Y, To.X - From.X);
        }

        public void LoadContent()
        {
            //Texture = new Texture2D(Global.SpriteBatch.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            //Texture.SetData(new[] { Color.White });

            Texture = TextureManager.CreateTexture2DBySingleColor(Color,1,1);
        }

        public void UnloadContent()
        {
            throw new NotImplementedException();
        }

        public void Update()
        {
            throw new NotImplementedException();
        }

        public void Draw()
        {
            var origin = new Vector2(0f, 0.0f);

            var scale = new Vector2(Lenght, Thickness);

            Global.SpriteBatch.Draw(Texture, From, null, Color, Angle, origin, scale, SpriteEffects.None, 0);
        }

        public Line ChangeColor(Color color)
        {
            Color = color;

            LoadContent();

            return this;
        }
    }
}
