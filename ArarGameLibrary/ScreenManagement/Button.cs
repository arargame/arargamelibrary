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

        public override void LoadContent(Texture2D texture = null)
        {
            Texture = Global.Content.Load<Texture2D>("Controls/button");

            InnerTexture = TextureManager.CreateTexture2DBySingleColor(OppositeColor, 1, 1);
        }

        public override void Update(GameTime gameTime = null)
        {
            base.Update();

            FontManager.SetPosition(Position);

            if (IsHovering)
            {
                InnerTextureSize += new Vector2(10,0);

                FontManager.SetColor(ThemeColor);
            }
            else
            {
                InnerTextureSize += new Vector2(-10, 0);

                FontManager.SetColor(OppositeColor);
            }
            
            if (InnerTextureSize.X < 0)
                InnerTextureSize.X = 0;

            if (InnerTextureSize.X > Size.X)
                InnerTextureSize.X = Size.X;

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

        public static List<Button> Sort(Dictionary<string, Action> collection, Vector2? center = null,Vector2? margin = null, Color? textColor = null,bool isFrameVisible = true, float topHeight = 100f)
        {
            LinkedList<Button> buttons = new LinkedList<Button>();

            var startingPosition = Vector2.Zero;

            if (center == null)
                center = Global.ViewportCenter;

            if (margin == null)
                margin = new Vector2(-Global.ViewportCenter.X * 0.7f, 10);

            if(textColor == null)
                textColor = Global.Theme.GetColor();

            foreach (var item in collection)
            {
                var button = new Button(item.Key, Vector2.Zero, textColor);

                button.MakeFrameVisible(true);

                button.OnClick(item.Value);

                buttons.AddLast(button);
            }

            foreach (var button in buttons)
            {
                var node = buttons.Find(button);

                if (node.Previous != null)
                {
                    var previousButton = node.Previous.Value;

                    button.SetPosition(new Vector2(previousButton.Position.X, previousButton.Position.Y + previousButton.Size.Y + margin.Value.Y));
                }
                else
                {
                    var x = center.Value.X - (float)(button.Texture.Width / 2);

                    startingPosition.X = x + margin.Value.X;

                    startingPosition.Y = startingPosition.Y + topHeight;

                    button.SetPosition(startingPosition);
                }
            }

            return buttons.ToList();
        }
    }
}
