using ArarGameLibrary.Manager;
using ArarGameLibrary.ScreenManagement;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleMeWindowsProject.Screens
{
    public class SettingsMenu : Menu
    {
        public override void Initialize()
        {
            Global.ChangeGameWindowTitle("Settings");
        }

        public override bool Load()
        {
            var collection = new Dictionary<string, Action>();

            collection.Add("Graphic", (() =>
            {
                DisableThenAddNew(new GraphicMenu());
            }));

            collection.Add("Sound", () =>
            {
                DisableThenAddNew(new SoundMenu());
            });

            collection.Add("Back", () =>
            {
                DisableThenAddNew(new MainMenu());
            });

            Components.AddRange(SortButtons(collection));

            return true && base.Load();
        }
    }
}
