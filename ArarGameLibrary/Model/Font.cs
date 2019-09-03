using ArarGameLibrary.Effect;
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
    public class Font : Sprite
    {
        public SpriteFont SpriteFont { get; set; }

        public string Text { get; set; }

        public Vector2 TextMeasure { get; set; }

        public Func<string> ChangeTextEvent { get; set; }


        public Font(string fontFile = null,
            string text = null,
            Vector2? position = null,
            Color? color = null,
            float rotation = 0f,
            Vector2? origin = null,
            float scale = 1f,
            SpriteEffects effects = SpriteEffects.None,
            float layerDepth = 0.5f,
            Vector2? padding = null,
            Func<string> changeTextEvent = null,
            bool isPulsating = false)
        {
            //config.json veya appconfig tarzı bişey yap default font vs için Fonts/DefaultFont
            SpriteFont = Global.Content.Load<SpriteFont>(fontFile ?? "Fonts/MenuFont");

            SetText(text);

            SetPosition(position ?? Vector2.Zero);

            SetColor(color ?? Color.White);

            Rotation = rotation;

            SetOrigin(origin ?? Vector2.Zero);

            SetScale(scale);

            if (isPulsating)
            {
                var pulsateEffect = GetEvent<PulsateEffect>();

                pulsateEffect.SetOriginalScale(scale);

                if (pulsateEffect != null)
                    pulsateEffect.SetWhenToInvoke(() =>
                    {
                        return IsHovering;
                    });
            }

            SetSpriteEffects(effects);

            SetLayerDepth(layerDepth);

            SetPadding(padding ?? Vector2.Zero);

            ChangeTextEvent = changeTextEvent;
        }


        public Font SetText(string text)
        {
            Text = text;

            TextMeasure = SpriteFont.MeasureString(text);

            SetSize(TextMeasure);

            return this;
        }

        public void CalculateCenterVector2(Rectangle rect)
        {
            var x = rect.Center.X - TextMeasure.X;
            var y = rect.Center.Y - TextMeasure.Y;

            SetPosition(new Vector2(x, y));
        }

        public override void Initialize()
        {
            base.Initialize();

            SetDrawMethodType(3);
        }

        public override void Draw(SpriteBatch spriteBatch = null)
        {
            SetSpriteBatch(spriteBatch);

            if (IsActive && IsVisible)
            {
                switch (DrawMethodType)
                {
                    case 1:

                        SpriteBatch.DrawString(SpriteFont, Text, Position, Color);

                        break;

                    case 2:

                        SpriteBatch.DrawString(SpriteFont, new StringBuilder(Text), Position, Color);

                        break;

                    case 3:

                        SpriteBatch.DrawString(SpriteFont, Text, Position, Color, Rotation, Origin, Scale, SpriteEffects, LayerDepth);

                        break;

                    case 4:

                        SpriteBatch.DrawString(SpriteFont, Text, Position, Color, Rotation, Origin, new Vector2(Scale), SpriteEffects, LayerDepth);

                        break;

                    case 5:

                        SpriteBatch.DrawString(SpriteFont, new StringBuilder(Text), Position, Color, Rotation, Origin, Scale, SpriteEffects, LayerDepth);

                        break;

                    case 6:

                        SpriteBatch.DrawString(SpriteFont, new StringBuilder(Text), Position, Color, Rotation, Origin, new Vector2(Scale), SpriteEffects, LayerDepth);

                        break;
                }
            }
        }
    }
}
