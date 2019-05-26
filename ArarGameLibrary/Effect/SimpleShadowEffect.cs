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

        public Vector2 OffSet { get; set; }

        public SimpleShadowEffect(Sprite sprite,Vector2? offset) : base(sprite)
        {
            Texture = TextureManager.CreateTexture2DBySingleColor(Color.Black, 1, 1);
            
            OffSet = offset ?? new Vector2(0, 0);

            SetTask(() => 
            {
                Rectangle = new Rectangle((int)(Sprite.DestinationRectangle.X + OffSet.X), (int)(Sprite.DestinationRectangle.Y + OffSet.Y), Sprite.DestinationRectangle.Width, Sprite.DestinationRectangle.Height);
            });

            SetEndTask(() => 
            {
            
            });

            SetDrawingTask(() => 
            {
                Global.SpriteBatch.Draw(Texture, new Vector2(Rectangle.X,Rectangle.Y), Rectangle, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            });
        }
    }
}
