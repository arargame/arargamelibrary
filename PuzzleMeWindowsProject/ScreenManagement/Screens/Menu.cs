using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleMeWindowsProject.ScreenManagement.Screens
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
            }

            return true;
        }

        public override void Update()
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

        public override void Draw()
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
