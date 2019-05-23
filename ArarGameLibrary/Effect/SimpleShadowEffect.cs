using ArarGameLibrary.Manager;
using ArarGameLibrary.Model;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArarGameLibrary.Effect
{
    public class SimpleShadowEffect : EffectManager
    {
        private Texture2D Texture { get; set; }

        private Rectangle Rectangle { get; set; }

        public SimpleShadowEffect(Sprite sprite,float offset = 5f) : base(sprite)
        {
            Texture = TextureManager.CreateTexture2DBySingleColor(Color.Black,1,1);

            SetTask(() => 
            {
                Rectangle = new Rectangle((int)(Sprite.DestinationRectangle.X + offset), (int)(Sprite.DestinationRectangle.Y + offset), Sprite.DestinationRectangle.Width, Sprite.DestinationRectangle.Height);
            });

            SetEndTask(() => 
            {
            
            });

            SetDrawingTask(() => 
            {
                Global.SpriteBatch.Draw(Texture, Rectangle, Rectangle, Color.White, 0f, Vector2.Zero, SpriteEffects.None, Sprite.LayerDepth + 1);
            });
        }
    }
}
