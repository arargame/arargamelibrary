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

        Color ThemeColor { get; set; }

        Color OppositeColor { get; set; }

        public MenuButton(string text,Color? textColor = null)
        {
            ThemeColor = textColor ?? Global.Theme.GetColor();
            OppositeColor = Global.Theme.Mode == ThemeMode.White ? Theme.GetDefaultColorByMode(ThemeMode.Dark) : Theme.GetDefaultColorByMode(ThemeMode.White);
        }

        public override void Initialize()
        {
            base.Initialize();

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

            if (IsHovering)
            {
                InnerTextureSize += new Vector2(10, 0);

                //FontManager.SetColor(ThemeColor);
            }
            else
            {
                InnerTextureSize += new Vector2(-10, 0);

                //FontManager.SetColor(OppositeColor);
            }


            InnerTextureSize.X = MathHelper.Clamp(InnerTextureSize.X, 0, Size.X);

            InnerTextureSize.Y = Size.Y;
        }

        public override void Draw(SpriteBatch spriteBatch = null)
        {
            base.Draw(spriteBatch);

            Global.SpriteBatch.Draw(InnerTexture, new Rectangle((int)Position.X, (int)Position.Y, (int)InnerTextureSize.X, (int)InnerTextureSize.Y), Color.White);
        }

        void MenuButton_OnChangeRectangle()
        {
            Frame = Frame.Create(DestinationRectangle, OppositeColor);

            Frame.LoadContent();
        }
    }


    public class Button : Component
    {
        FontManager FontManager { get; set; }

        public Button(string text, Vector2 position, Color? textColor = null,Vector2? textPadding = null )
        {
            var padding = textPadding ?? Vector2.Zero;

            FontManager = new FontManager("Fonts/MenuFont", text, position, Color.White, 0f, Vector2.Zero, new Vector2(1, 1), SpriteEffects.None, 0.5f, padding, null);



            SetPosition(position);

            SetSize(new Vector2(FontManager.TextMeasure.X + 2 * padding.X, FontManager.TextMeasure.Y + 2 * padding.Y));

            //Button_OnChangeRectangle();        
        }

        public Button SetFontManager(string text,Color textColor)
        {
            FontManager = new FontManager(text: text, color: textColor);

            return this;
        }

        public Button()
        {

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

            OnChangeRectangle += Button_OnChangeRectangle;    
        }

        public override void LoadContent(Texture2D texture = null)
        {
            Texture = Global.Content.Load<Texture2D>("Controls/button");
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

            FontManager.CalculateCenterVector2(DestinationRectangle);
            //FontManager.SetPosition(Position);



            FontManager.Update();
        }

        public override void Draw(SpriteBatch spriteBatch = null)
        {
            base.Draw();

             FontManager.Draw();
        }

        private void Button_OnChangeRectangle()
        {

            FontManager.SetPosition(new Vector2(Position.X + FontManager.Padding.X, Position.Y + FontManager.Padding.Y));
        }
    }
}
