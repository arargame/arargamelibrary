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
    public class Button : Component
    {
        Texture2D InnerTexture { get; set; }

        Vector2 InnerTextureSize;

        FontManager FontManager { get; set; }

        Color ThemeColor { get; set; }

        Color OppositeColor { get; set; }

        public Button(string text, Vector2 position, Color? textColor = null)
        {
            var padding = new Vector2(25, 10);

            FontManager = new FontManager("Fonts/MenuFont", text, position, Color.White, 0f, Vector2.Zero, new Vector2(1, 1), SpriteEffects.None, 0.5f, padding, null);

            ThemeColor = textColor ?? Global.Theme.GetColor();
            OppositeColor = Global.Theme.Mode == ThemeMode.White ? Theme.GetDefaultColorByMode(ThemeMode.Dark) : Theme.GetDefaultColorByMode(ThemeMode.White);

            LoadContent();

            SetPosition(position);

            SetSize(new Vector2(FontManager.TextMeasure.X + 2 * padding.X, FontManager.TextMeasure.Y + 2 * padding.Y));

            Button_OnChangeRectangle();

            OnChangeRectangle += Button_OnChangeRectangle;            
        }

        public override void Initialize()
        {
            base.Initialize();

            //var hoveringEvent = new SingularInvoker(this,
            //                whenToInvoke: () =>
            //                {
            //                    return IsHovering;
            //                },
            //                success: () =>
            //                {
            //                    InnerTextureSize += new Vector2(10, 0);

            //                    FontManager.SetColor(ThemeColor);
            //                },
            //                fail: () =>
            //                {
            //                    InnerTextureSize += new Vector2(-10, 0);

            //                    FontManager.SetColor(OppositeColor);
            //                });

            //hoveringEvent.SetContinuous(true);

           // Events.Add(hoveringEvent);
        }

        public override void LoadContent(Texture2D texture = null)
        {
            Texture = Global.Content.Load<Texture2D>("Controls/button");

            InnerTexture = TextureManager.CreateTexture2DBySingleColor(OppositeColor, 1, 1);
        }

        public override void Update(GameTime gameTime = null)
        {
            base.Update();

            if (IsHovering)
            {
                InnerTextureSize += new Vector2(10, 0);

                FontManager.SetColor(ThemeColor);
            }
            else
            {
                InnerTextureSize += new Vector2(-10, 0);

                FontManager.SetColor(OppositeColor);
            }

            FontManager.CalculateCenterVector2(DestinationRectangle);
            //FontManager.SetPosition(Position);

            InnerTextureSize.X = MathHelper.Clamp(InnerTextureSize.X, 0, Size.X);

            InnerTextureSize.Y = Size.Y;

            FontManager.Update();
        }

        public override void Draw(SpriteBatch spriteBatch = null)
        {
            base.Draw();

            Global.SpriteBatch.Draw(InnerTexture, new Rectangle((int)Position.X, (int)Position.Y, (int)InnerTextureSize.X, (int)InnerTextureSize.Y), Color.White);

            FontManager.Draw();
        }

        private void Button_OnChangeRectangle()
        {
            Frame = Frame.Create(DestinationRectangle, OppositeColor);

            Frame.LoadContent();
        }
    }
}
