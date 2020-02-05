using ArarGameLibrary.Effect;
using ArarGameLibrary.Manager;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Text;

namespace ArarGameLibrary.Model
{
    //    font = new Font(text: "HELllow", color: Color.Coral, scale: 4f, position: new Vector2(400, 10), isPulsating: true);
    //font.SetDragable(true);
    //font.SetChangeTextEvent(() => 
    //{
    //    return font.Scale.ToString("0.0");
    //});

    public class Font : Sprite
    {
        public static Font Default { get; set; }

        public SpriteFont SpriteFont { get; set; }

        public string Text { get; set; }

        public Vector2 TextMeasure { get; set; }

        public Func<string> ChangeTextEvent { get; set; }

        public Font(string fontFile = null,
            string rootDirectory = null,
            string text = null,
            Vector2? position = null,
            Color? fontColor = null,
            float rotation = 0f,
            Vector2? origin = null,
            float scale = 1f,
            SpriteEffects effects = SpriteEffects.None,
            float layerDepth = 0.5f,
            Offset? margin = null,
            Func<string> changeTextEvent = null,
            bool isPulsating = false)
        {
            fontFile = fontFile ?? "Fonts/DefaultFont";

            rootDirectory = rootDirectory ?? "LibraryContent";

            SpriteFont = Global.Content(rootDirectory).Load<SpriteFont>(fontFile);
            
            SetText(text);

            SetPosition(position ?? Vector2.Zero);

            SetColor(fontColor ?? Color.White);

            Rotation = rotation;

            SetOrigin(origin ?? Vector2.Zero);

            SetScale(scale);

            if (isPulsating)
            {
                var pulsateEffect = GetEvent<PulsateEffect>();

                pulsateEffect.SetOriginalScale(scale);

                pulsateEffect.SetWhenToInvoke(() =>
                {
                    return IsHovering;
                });
            }

            SetSpriteEffects(effects);

            SetLayerDepth(layerDepth);

            SetMargin(margin ?? Offset.CreateMargin(OffsetValueType.Piksel, 0, 0, 0, 0));

            SetChangeTextEvent(changeTextEvent);
        }


        public Font SetText(string text)
        {
            Text = text;

            TextMeasure = SpriteFont.MeasureString(text);

            SetSize(TextMeasure);

            return this;
        }


        public void CalculateNewPosition(Rectangle rectangle, Offset? offset = null, bool isCentered = false)
        {
            var newPosition = Vector2.Zero;

            var x = 0f;
            var y = 0f;

            offset = offset ?? Offset.Zero();

            if (offset.Value.OffsetType == OffsetType.Margin)
                isCentered = true;

            if (isCentered)
            {
                x = rectangle.Center.X - TextMeasure.X / 2;
                y = rectangle.Center.Y - TextMeasure.Y / 2;
            }
            else
            {
                switch (offset.Value.OffsetValueType)
                {
                    case OffsetValueType.Piksel:

                        x = rectangle.Left + offset.Value.Left;

                        if (x > rectangle.Right - offset.Value.Right - TextMeasure.X)
                            x = rectangle.Right - offset.Value.Right - TextMeasure.X;

                        y = rectangle.Top + offset.Value.Top;

                        if (y > rectangle.Bottom - offset.Value.Bottom - TextMeasure.Y)
                            y = rectangle.Bottom - offset.Value.Bottom - TextMeasure.Y;

                        break;

                    case OffsetValueType.Ratio:

                        x = rectangle.Left + (rectangle.Width * offset.Value.Left / 100);

                        if (x > rectangle.Right - (rectangle.Width * offset.Value.Right / 100) - TextMeasure.X)
                            x = rectangle.Right - (rectangle.Width * offset.Value.Right / 100) - TextMeasure.X;

                        y = rectangle.Top + (rectangle.Height * offset.Value.Top / 100);

                        if (y > rectangle.Bottom - (rectangle.Height * offset.Value.Bottom / 100) - TextMeasure.Y)
                            y = rectangle.Bottom - (rectangle.Height * offset.Value.Bottom / 100) - TextMeasure.Y;

                        break;
                }
            }

            newPosition = new Vector2(x, y);

            SetPosition(newPosition);
        }

        public override void Initialize()
        {
            base.Initialize();

            SetDrawMethodType(3);
        }

        public override void Update(GameTime gameTime = null)
        {
            base.Update(gameTime);

            if (ChangeTextEvent != null)
                SetText(ChangeTextEvent());
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

        public Font SetChangeTextEvent(Func<string> changeTextEvent)
        {
            ChangeTextEvent = changeTextEvent;

            return this;
        }
        

        public static void Draw(string text, Vector2 position, Color color,Func<string> changeTextEvent = null)
        {
            if (Default == null)
                Default = new Font(text: text, position: position, fontColor: color, changeTextEvent: changeTextEvent);

            Default.Update();

            Default.Draw();
        }
    }
}
