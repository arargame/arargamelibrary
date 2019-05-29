using ArarGameLibrary.Manager;
using ArarGameLibrary.ScreenManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleMeWindowsProject.Screens
{
    public class GraphicMenu : Menu
    {
        public override void Initialize()
        {
            Global.ChangeGameWindowTitle("Graphic");
        }

        public override bool Load()
        {
            var collection = new Dictionary<string, Action>();

            collection.Add("Change Resolution", (() =>
            {
                DisableThenAddNew(new ChangeResolutionMenu());
            }));

            collection.Add(string.Format("Full Screen : {0}", Global.Graphics.IsFullScreen ? "Enabled" : "Disabled"), () =>
            {
                ScreenManager.SetFullScreen(!Global.Graphics.IsFullScreen);
            });

            collection.Add("Back", () =>
            {
                DisableThenAddNew(new SettingsMenu());
            });

            Components.AddRange(Button.Sort(collection));

            return true && base.Load();
        }
    }
}
