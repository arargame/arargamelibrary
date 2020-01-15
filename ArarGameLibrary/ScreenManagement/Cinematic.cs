using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArarGameLibrary.ScreenManagement
{
    public abstract class Cinematic : Screen
    {
        public override bool Load()
        {
            throw new NotImplementedException();
        }

        public override void Update(GameTime gameTime = null)
        {
            base.Update();

            if (ScreenState == ScreenState.Active)
            {

            }
        }

        public override void Draw(SpriteBatch spriteBatch = null)
        {
            if (ScreenState == ScreenState.Active)
            {

            }
        }
    }
}
