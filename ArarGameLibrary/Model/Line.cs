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
    public class Line : BaseObject, IXna
    {
        public Color Color { get; set; }

        public Texture2D Texture { get; set; }

        private static Texture2D defaultTexture;
        public static Texture2D DefaultTexture
        {
            get
            {
                return defaultTexture ?? (defaultTexture = TextureManager.CreateTexture2DBySingleColor(Global.RandomColor(), 1, 1));
            }
        }

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

        public void LoadContent(Texture2D texture = null)
        {
            //Texture = new Texture2D(Global.SpriteBatch.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            //Texture.SetData(new[] { Color.White });

            if (IsInPerformanceMode)
                Texture = DefaultTexture;
            else
                Texture = TextureManager.CreateTexture2DBySingleColor(Color, 1, 1);
        }

        public void UnloadContent()
        {
            throw new NotImplementedException();
        }

        public void Update(GameTime gameTime = null)
        {
        }

        public void Draw(SpriteBatch spriteBatch = null)
        {
            var origin = new Vector2(0f, 0.0f);

            var scale = new Vector2(Lenght, Thickness);

            Global.SpriteBatch.Draw(Texture, From, null, Color.White, Angle, origin, scale, SpriteEffects.None, 1);
        }

        public Line ChangeColor(Color color)
        {
            Color = color;

            LoadContent();

            return this;
        }
    }
}
