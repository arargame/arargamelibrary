using ArarGameLibrary.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArarGameLibrary.ScreenManagement.Screens
{
    public class ChangeResolutionMenu : Menu
    {
        public override void Initialize()
        {
            Global.ChangeGameWindowTitle("Change Resolution");
        }

        public override bool Load()
        {
            var collection = new Dictionary<string, Action>();

            collection.Add(string.Format("800x480 : {0}", Global.ViewportWidth == 800 && Global.ViewportHeight == 480 ? "Enabled" : "Disabled"), () =>
            {
                ScreenManager.ChangeScreenResolution(800,400);
            });

            collection.Add("Back", () =>
            {
                DisableThenAddNew(new GraphicMenu());
            });

            Components.AddRange(Button.Sort(collection));

            return true && base.Load();
        }
    }
}
