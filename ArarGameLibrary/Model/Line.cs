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
    public class Line : Sprite
    {
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

        public override void Initialize()
        {
            Lenght = Vector2.Distance(From, To);

            Angle = (float)Math.Atan2(To.Y - From.Y, To.X - From.X);

            IsActive = IsAlive = IsVisible = true;
        }

        public override void LoadContent(Texture2D texture = null)
        {
            //Texture = new Texture2D(Global.SpriteBatch.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            //Texture.SetData(new[] { Color.White });

            Texture = TextureManager.CreateTexture2DBySingleColor(Color, 1, 1);
        }

        public override void UnloadContent()
        {
            throw new NotImplementedException();
        }

        public override void Update(GameTime gameTime = null)
        {
        }

        public override void Draw(SpriteBatch spriteBatch = null)
        {
            if (IsVisible)
            {
                var origin = new Vector2(0f, 0.0f);

                var scale = new Vector2(Lenght, Thickness);

                Global.SpriteBatch.Draw(Texture, From, null, Color.White, Angle, origin, scale, SpriteEffects.None, 1);
            }
        }

        public Line ChangeColor(Color color)
        {
            Color = color;

            LoadContent();

            return this;
        }

        public static float Slope(Vector2 point1, Vector2 point2)
        {
            var deltaY = point2.Y - point1.Y;

            var deltaX = point2.X - point1.X;

            var slope = 0f;

            if (deltaX != 0f)
                slope = deltaY / deltaX;

            return slope;
        }
    }
}
