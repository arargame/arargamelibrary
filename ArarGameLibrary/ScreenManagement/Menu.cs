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
        public List<IComponent> Components = new List<IComponent>();

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
    }
}
