﻿using ArarGameLibrary.Manager;
using ArarGameLibrary.ScreenManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PuzzleMeWindowsProject.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleMeWindowsProject.Screens
{
    public class GameScreen : Screen
    {
        public GameLevel Level { get; set; }

        public override void Initialize()
        {
            Global.ChangeGameWindowTitle("Game Screen");
        }

        public override bool Load()
        {
            Level = new GameLevel()
            {
                Name = "Level X"
            };

            Level.LoadContent();

            return true; 
        }

        public override void Update(GameTime gameTime = null)
        {
            base.Update();

            if (ScreenState == ScreenState.Active)
            {
                if (InputManager.IsNewKeyPress(Microsoft.Xna.Framework.Input.Keys.P))
                    Freeze(new PauseMenu());

                Level.Update();
            }
        }

        public override void Draw(SpriteBatch spriteBatch = null)
        {
            if (ScreenState == ScreenState.Active)
            {
                Level.Draw();
            }
        }
    }
}
