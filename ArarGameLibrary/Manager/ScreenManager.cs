using ArarGameLibrary.ScreenManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArarGameLibrary.Manager
{
    public class ScreenManager
    {
        public static List<Screen> Screens = new List<Screen>();

        public ScreenManager() { }

        public static void Add(Screen screen)
        {
            if(!Screens.Any(s=>s.Id==screen.Id))
                Screens.Add(screen);
        }

        public static void Update()
        {
            for (int i = 0; i < Screens.Count; i++)
            {
                if (Screens[i].ScreenState == ScreenState.Inactive)
                    Screens[i].UnloadContent();
                else if (Screens[i].ScreenState == ScreenState.Active || Screens[i].ScreenState == ScreenState.Preparing)
                    Screens[i].Update();
            }

            Screens.RemoveAll(s => s.ScreenState == ScreenState.Inactive);
        }

        public static void Draw()
        {
            foreach (var screen in Screens)
            {
                if (screen.ScreenState != ScreenState.Inactive)
                    screen.Draw();
            }
        }


        public static void ChangeScreenResolution(int width,int height)
        {
            Global.Graphics.PreferredBackBufferWidth = width;

            Global.Graphics.PreferredBackBufferHeight = height;

            Global.Graphics.ApplyChanges();
        }

        public static void SetFullScreen(bool enable)
        {
            Global.Graphics.IsFullScreen = enable;

            Global.Graphics.ApplyChanges();
        }
    }



    
}
