﻿using ArarGameLibrary.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArarGameLibrary.ScreenManagement.Screens
{
    public class GameScreen : Screen
    {
        public Level Level { get; set; }

        public override void Initialize()
        {
            Global.ChangeGameWindowTitle("Game Screen");
        }

        public override bool Load()
        {
            Level = new Level()
            {
                Name = "Level X"
            };

            Level.LoadContent();

            return true; 
        }

        public override void Update()
        {
            base.Update();

            if (ScreenState == ScreenState.Active)
            {
                if (InputManager.IsNewKeyPress(Microsoft.Xna.Framework.Input.Keys.P))
                    Freeze(new PauseMenu());

                Level.Update();
            }
        }

        public override void Draw()
        {
            if (ScreenState == ScreenState.Active)
            {
                Level.Draw();
            }
        }
    }
}
