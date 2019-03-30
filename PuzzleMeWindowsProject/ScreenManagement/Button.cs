using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PuzzleMeWindowsProject.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleMeWindowsProject.ScreenManagement
{
    public class Button : Component
    {
        Texture2D InnerTexture { get; set; }

        Vector2 InnerTextureSize;

        FontManager FontManager { get; set; }

        public Button(string text, Vector2 position, Color color)
        {
            var padding = new Vector2(25, 10);

            FontManager = new FontManager("Fonts/MenuFont", text, position, color, 0f, Vector2.Zero, new Vector2(1, 1), SpriteEffects.None, 0f, padding, null);

            LoadContent();

            SetPosition(position);

            SetSize(new Vector2(FontManager.TextMeasure.X + 2 * padding.X, FontManager.TextMeasure.Y + 2 * padding.Y));
        }

        public static List<Button> Sort(Dictionary<string, Action> collection, Vector2? center = null, Color? color = null, Vector2? margin = null, float topHeight = 100f)
        {
            LinkedList<Button> buttons = new LinkedList<Button>();

            var startingPosition = Vector2.Zero;

            if (center == null)
                center = Global.ViewportCenter;

            if (color == null)
                color = Color.White;

            if (margin == null)
                margin = new Vector2(-Global.ViewportCenter.X * 0.7f, 10);

            foreach (var item in collection)
            {
                var button = new Button(item.Key, Vector2.Zero, color.Value);

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

        public override void LoadContent()
        {
            Texture = Global.Content.Load<Texture2D>("Controls/button");

            InnerTexture = TextureManager.CreateTexture2DBySingleColor(Color.White,1,1);
        }

        public override void Update()
        {
            base.Update();

            FontManager.SetPosition(Position);

            if (IsHovering)
            {
                FontManager.SetColor(Color.Black);

                InnerTextureSize += new Vector2(10,0);
            }
            else
            {
                InnerTextureSize += new Vector2(-10, 0);

                FontManager.SetColor(Color.White);
            }

            if (InnerTextureSize.X < 0)
                InnerTextureSize.X = 0;

            if (InnerTextureSize.X > Size.X)
                InnerTextureSize.X = Size.X;

            InnerTextureSize.Y = Size.Y;

            FontManager.Update();
        }

        public override void Draw()
        {
            base.Draw();

           // if(IsHovering)
                Global.SpriteBatch.Draw(InnerTexture, new Rectangle((int)Position.X, (int)Position.Y, (int)InnerTextureSize.X, (int)InnerTextureSize.Y), Color.White);

            FontManager.Draw();
        }
    }
}
