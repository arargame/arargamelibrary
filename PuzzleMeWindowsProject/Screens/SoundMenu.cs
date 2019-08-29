using ArarGameLibrary.Manager;
using ArarGameLibrary.ScreenManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleMeWindowsProject.Screens
{
    public class SoundMenu : Menu
    {
        public override void Initialize()
        {
            Global.ChangeGameWindowTitle("Sound");
        }

        public override bool Load()
        {
            var collection = new Dictionary<string, Action>();

            collection.Add("Effect", (() =>
            {
                
            }));

            collection.Add("Music", () =>
            {
                
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
