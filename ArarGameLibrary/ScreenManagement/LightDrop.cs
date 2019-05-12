using ArarGameLibrary.Manager;
using ArarGameLibrary.Model;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArarGameLibrary.ScreenManagement
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

        public override void LoadContent(Texture2D texture = null)
        {
            SetTexture("Textures/lightDrop");
        }

        public override void Update(GameTime gameTime = null)
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
