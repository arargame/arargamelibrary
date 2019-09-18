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
    //    btn1001 = new Button();
    //btn1001.SetPosition(new Vector2(250,250));
    //btn1001.LoadContent(TextureManager.CreateTexture2DByRandomColor());
    //btn1001.SetFont("Hello Pluton",Color.Salmon,new Vector2(10));
    //btn1001.SetFrame(Color.Yellow, 2f);
    //btn1001.SetDragable(true);

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
            base.LoadContent(texture);
        }

        public override void Update(GameTime gameTime = null)
        {
            base.Update();

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
