using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArarGameLibrary.Manager;
using ArarGameLibrary.ScreenManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ArarGameLibrary.ScreenManagement
{
    public abstract class Menu : Screen
    {
        public List<Component> Components = new List<Component>();

        private LightDrop[] Drops = new LightDrop[15];

        public override bool Load()
        {
            for (int i = 0; i < Drops.Length; i++)
            {
                Drops[i] = new LightDrop();

                Drops[i].LoadContent();

                Drops[i].SetColor(Global.Theme.Mode==ThemeMode.White ? Theme.GetDefaultColorByMode(ThemeMode.Dark) : Theme.GetDefaultColorByMode(ThemeMode.White));
            }

            return true;
        }

        public override void Update(GameTime gameTime = null)
        {
            base.Update();

            if (ScreenState == ScreenState.Active)
            {
                foreach (var component in Components)
                {
                    component.Update();
                }

                foreach (var drop in Drops)
                {
                    drop.Update();
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch = null)
        {
            if (ScreenState == ScreenState.Active)
            {
                foreach (var component in Components)
                {
                    component.Draw();
                }

                foreach (var drop in Drops)
                {
                    drop.Draw();
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            //if (disposing)
            //{
            //    // free managed resources
            //    Components.Clear();
            //}

            base.Dispose(disposing);
        }

        public static List<Button> SortButtons(Dictionary<string, Action> collection,
            Vector2? center = null,
            Vector2? margin = null,
            Color? textColor = null,
            bool isFrameVisible = true,
            float topGap = 100f)
        {
            LinkedList<Button> buttons = new LinkedList<Button>();

            var startingPosition = Vector2.Zero;

            if (center == null)
                center = Global.ViewportCenter;

            if (margin == null)
                margin = new Vector2(-Global.ViewportCenter.X * 0.7f, 10);

            //if (textColor == null)
            //    textColor = Global.Theme.GetColor();

            foreach (var item in collection)
            {
                var button = new MenuButton(text: item.Key, textColor: textColor);

                button.LoadContent();

                button.SetClickAction(item.Value);

                button.SetSize(new Vector2(button.Font.Size.X + 50, button.Font.Size.Y + 20));

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
                    var x = center.Value.X - (float)(button.Size.X / 2);

                    startingPosition.X = x + margin.Value.X;

                    startingPosition.Y = startingPosition.Y + topGap;

                    button.SetPosition(startingPosition);
                }
            }

            return buttons.ToList();
        }
    }
}
