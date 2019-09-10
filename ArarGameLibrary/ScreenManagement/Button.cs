using ArarGameLibrary.Effect;
using ArarGameLibrary.Event;
using ArarGameLibrary.Extension;
using ArarGameLibrary.Manager;
using ArarGameLibrary.Model;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArarGameLibrary.ScreenManagement
{
    public class MenuButton : Button
    {
        Texture2D InnerTexture { get; set; }

        Vector2 InnerTextureSize;

        float InnerTextureLayerDepth { get; set; }

        Color ThemeColor { get; set; }

        Color OppositeColor { get; set; }

        public MenuButton(string text, Color? textColor = null, Vector2? textPadding = null, bool isPulsating = false)
            : base(isPulsating)
        {
            ThemeColor = textColor ?? Global.Theme.GetColor();

            OppositeColor = Global.Theme.Mode == ThemeMode.White ? Theme.GetDefaultColorByMode(ThemeMode.Dark) : Theme.GetDefaultColorByMode(ThemeMode.White);

            textPadding = textPadding ?? new Vector2(10);

            SetFont(text, OppositeColor, textPadding);

            InnerTextureLayerDepth = LayerDepth;
        }

        public override void Initialize()
        {
            base.Initialize();

            var hoveringEvent = new SingularInvoker(this,
                            whenToInvoke: () =>
                            {
                                return IsHovering;
                            },
                            success: () =>
                            {
                                InnerTextureSize += new Vector2(10, 0);

                                Font.SetColor(ThemeColor);
                            },
                            fail: () =>
                            {
                                InnerTextureSize += new Vector2(-10, 0);

                                Font.SetColor(OppositeColor);
                            });

            hoveringEvent.SetContinuous(true);

            Events.Add(hoveringEvent);

            OnChangeRectangle += MenuButton_OnChangeRectangle;
        }

        public override void LoadContent(Texture2D texture = null)
        {
            base.LoadContent(texture);

            InnerTexture = TextureManager.CreateTexture2DBySingleColor(OppositeColor, 1, 1);
        }

        public override void Update(GameTime gameTime = null)
        {
            base.Update(gameTime);

            //if (IsHovering)
            //{
            //    InnerTextureSize += new Vector2(10, 0);

            //    Font.SetColor(ThemeColor);
            //}
            //else
            //{
            //    InnerTextureSize += new Vector2(-10, 0);

            //    Font.SetColor(OppositeColor);
            //}

            InnerTextureLayerDepth = LayerDepth - 0.01f;

            InnerTextureSize.X = MathHelper.Clamp(InnerTextureSize.X, 0, DestinationRectangle.Size.X);

            InnerTextureSize.Y = DestinationRectangle.Size.Y;
        }

        public override void Draw(SpriteBatch spriteBatch = null)
        {
            base.Draw(spriteBatch);

            Global.SpriteBatch.Draw(InnerTexture, new Rectangle((int)Position.X, (int)Position.Y, (int)InnerTextureSize.X, (int)InnerTextureSize.Y), null, Color.White, 0f, Vector2.Zero, SpriteEffects, 0.2f);
        }

        void MenuButton_OnChangeRectangle()
        {
            //Frame = Frame.Create(DestinationRectangle, OppositeColor);

            //Frame.LoadContent();
        }
    }


    public class Button : Component
    {
        public Font Font { get; private set; }

        public Button(bool isPulsating = false)
        {
            if (isPulsating)
            {
                var pulsateEffect = GetEvent<PulsateEffect>();

                pulsateEffect.SetOriginalScale(Scale);

                if (pulsateEffect != null)
                    pulsateEffect.SetWhenToInvoke(() =>
                    {
                        return IsHovering;
                    });
            }
        }

        public override void Initialize()
        {
            base.Initialize();

            OnChangeRectangle += Button_OnChangeRectangle;
        }

        public override void LoadContent(Texture2D texture = null)
        {
           // Texture = Global.Content.Load<Texture2D>("Controls/button");
        }

        public override void Update(GameTime gameTime = null)
        {
            base.Update();

            if (IsHovering)
            {
                //InnerTextureSize += new Vector2(10, 0);

                //FontManager.SetColor(ThemeColor);
            }
            else
            {
                //InnerTextureSize += new Vector2(-10, 0);

                //FontManager.SetColor(OppositeColor);
            }
            //FontManager.SetPosition(Position);

            if (Font != null)
                Font.Update();
        }

        public override void Draw(SpriteBatch spriteBatch = null)
        {
            base.Draw();

            if (Font != null)
                Font.Draw();
        }

        public Button SetFont(string text, Color? textColor, Vector2? textPadding = null)
        {
            textColor = textColor ?? Color.White;

            textPadding = textPadding ?? Vector2.Zero;

            Font = new Font(text: text, color: textColor);

            Font.IncreaseLayerDepth();

            SetPadding(textPadding ?? Vector2.Zero);

            var newSize = new Vector2(Font.TextMeasure.X + 2 * Padding.X, Font.TextMeasure.Y + 2 * Padding.Y);

            newSize = new Vector2(MathHelper.Clamp(newSize.X, Size.X, newSize.X), MathHelper.Clamp(newSize.Y, Size.Y, newSize.Y));

            SetSize(newSize);

            return this;
        }

        private void Button_OnChangeRectangle()
        {
            if (Font != null)
            {
                if (Padding == Vector2.Zero)
                    Font.CalculateCenterVector2(DestinationRectangle);
                else
                    Font.SetPosition(new Vector2(Position.X + Padding.X, Position.Y + Padding.Y));

                Font.SetScale(Scale);
            }
        }

    }
}
