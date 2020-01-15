using ArarGameLibrary.Manager;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArarGameLibrary.ScreenManagement.Screens
{
    //Bir sistem tasarla zaman alsın ve örneğin şu kadar saniye sonra şu işi yapsın şeklinde
    //Gelişmiş : ne kadarda bir çalışması gerektiği belirtilen zmana güdümlü bir sistem saniyede 1 dakikada 1 falan ve bu devamlı veya bir kereliğine olabilir
    public class TradeMarkScreen101 : Cinematic
    {
        private Texture2D TradeMarkLogo { get; set; }

        private Rectangle LogoRectangle { get; set; }

        byte logoOpacity;

        public override void Initialize()
        {
            Global.ChangeGameWindowTitle("Arar Game");

            logoOpacity = 0;
        }

        public override bool Load()
        {
            TradeMarkLogo = TextureManager.CreateTexture2D("Textures/SmilemanLogo","LibraryContent");

            var x = Global.ViewportRect.Center.X - (450 / 2);

            var y = Global.ViewportRect.Center.Y - (450 / 2) - 20;

            var w = 450;

            var h = 450;

            LogoRectangle = new Rectangle((int)x, (int)y, (int)w, (int)h);

            return true;
        }

        public override void Update(GameTime gameTime = null)
        {
            base.Update();

            if (ScreenState == ScreenState.Active)
            {
                if (logoOpacity < 255)
                {
                    logoOpacity++;
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch = null)
        {
            if (ScreenState == ScreenState.Active)
            {
                Global.GraphicsDevice.Clear(Color.Black);

                Global.SpriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.NonPremultiplied);
                Global.SpriteBatch.Draw(TradeMarkLogo, LogoRectangle, Color.White * logoOpacity);
                Global.SpriteBatch.End();
            }
        }
    }
}
