﻿using ArarGameLibrary.Event;
using ArarGameLibrary.Manager;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArarGameLibrary.ScreenManagement
{
    //    menuButton = new MenuButton("hellow jupiter",isPulsating:true);
    //menuButton.LoadContent();
    //menuButton.SetPosition(new Vector2(370,250));

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
}