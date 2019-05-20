using ArarGameLibrary.Model;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArarGameLibrary.Manager
{
    public class FontManager : IXna
    {
        public SpriteFont Font { get; set; }

        public string Text { get; set; }

        public Color Color { get; set; }

        public Vector2 Position { get; set; }

        public Vector2 Scale { get; set; }

        public Vector2 Origin { get; set; }

        public float Rotation { get; set; }

        public float LayerDepth { get; set; }

        public SpriteEffects Effects { get; set; }

        public Vector2 TextMeasure { get; set; }

        public Vector2 Padding { get; set; }

        public Func<string> ChangeTextEvent { get; set; }

        public FontManager(string fontFile,
            string text,
            Vector2 position,
            Color color,
            float rotation,
            Vector2 origin,
            Vector2 scale,
            SpriteEffects effects,
            float layerDepth,
            Vector2 padding,
            Func<string> changeTextEvent)
        {
            Font = Global.Content.Load<SpriteFont>(fontFile ?? "Fonts/DefaultFont");

            SetText(text);

            SetPosition(position);

            SetColor(color);

            Rotation = rotation;

            Origin = origin;

            Scale = scale;

            Effects = effects;

            LayerDepth = layerDepth;

            Padding = padding;

            ChangeTextEvent = changeTextEvent;
        }

        public void Update(GameTime gameTime = null)
        {
            if (ChangeTextEvent != null)
                SetText(ChangeTextEvent.Invoke());
        }

        public void Draw(SpriteBatch spriteBatch = null)
        {
            Global.SpriteBatch.DrawString(Font, Text, new Vector2(Position.X + Padding.X, Position.Y + Padding.Y), Color, Rotation, Origin, Scale, Effects, LayerDepth);
        }

        public static FontManager Create(string text,Vector2 position,Color color)
        {
            return new FontManager("Fonts/MenuFont", text, position, color, 0f, Vector2.Zero, new Vector2(1, 1), SpriteEffects.None, 0f, Vector2.Zero,null); 
        }

        public FontManager SetText(string text)
        {
            Text = text;

            TextMeasure = Font.MeasureString(text);

            return this;
        }

        public FontManager SetColor(Color color)
        {
            Color = color;

            return this;
        }

        public FontManager SetPosition(Vector2 position)
        {
            Position = position;

            return this;
        }

        public void CalculateCenterVector2(Rectangle rect)
        {
            var x = rect.Center.X - TextMeasure.X / 2;
            var y = rect.Center.Y - TextMeasure.Y / 2;

            SetPosition(new Vector2(x, y));
        }

        public void Initialize()
        {
            throw new NotImplementedException();
        }

        public void LoadContent(Texture2D texture = null)
        {
            throw new NotImplementedException();
        }

        public void UnloadContent()
        {
            throw new NotImplementedException();
        }
    }
}
