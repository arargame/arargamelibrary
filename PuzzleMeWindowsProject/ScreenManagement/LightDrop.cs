using Microsoft.Xna.Framework;
using PuzzleMeWindowsProject.Manager;
using PuzzleMeWindowsProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleMeWindowsProject.ScreenManagement
{
    public class LightDrop : Sprite
    {
        public override void SetStartingPosition()
        {
            SetPosition(new Vector2(Global.Random.Next(0, Global.ViewportWidth), Global.Random.Next(-100, 0)));
        }

        public override void SetStartingSpeed()
        {
            SetSpeed(new Vector2(Global.Random.Next(-3, 3), Global.Random.Next(1, 3)));
        }

        public override void SetStartingSize()
        {
            SetSize(new Vector2(50, 50));
        }

        public override void LoadContent()
        {
            SetTexture("Textures/lightDrop");
        }

        public override void Update()
        {
            Position = new Vector2(Position.X + Speed.X, Position.Y + Speed.Y);

            if (!Global.IsInFirstRow(DestinationRectangle) && !Global.IsInScreen(DestinationRectangle))
            {
                SetStartingPosition();
                SetStartingSpeed();
            }

            base.Update();
        }
    }
}
