using ArarGameLibrary.Manager;
using ArarGameLibrary.ScreenManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject.Screens
{
    public class MainMenu : Menu
    {
        public override void Initialize()
        {
            Global.ChangeGameWindowTitle("Main Menu");
        }

        public override bool Load()
        {
            var collection = new Dictionary<string, Action>();

            collection.Add("Play", () =>
            {
                //DisableThenAddNew(new EpisodesMenu());
            });

            collection.Add("Settings", () =>
            {
                //DisableThenAddNew(new SettingsMenu());
            });

            collection.Add("Exit", () =>
            {
                Global.OnExit = true;
            });


            Components.AddRange(SortButtons(collection));

            return true && base.Load();
        }
    }
}
