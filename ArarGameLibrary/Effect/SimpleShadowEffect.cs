using ArarGameLibrary.Event;
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
    //Color.Lerp(Color.Black, Color.Transparent, 0.9f)
    public class SimpleShadowEffect : EventManager
    {
        private Texture2D Texture { get; set; }

        private Rectangle Rectangle { get; set; }

        private Vector2 OffSet { get; set; }

        public SimpleShadowEffect(Sprite sprite, Vector2? offset,Func<bool> whenToInvoke = null)
            : base(sprite,true)
        {
            Texture = TextureManager.CreateTexture2DBySingleColor(Color.Black, 1, 1);

            SetOffset(offset ?? new Vector2(0, 0));

            SetWhenToInvoke(whenToInvoke);

            SetTask(() => 
            {
                if (WhenToInvoke == null)
                    return;

                if (WhenToInvoke())
                {
                    Rectangle = new Rectangle((int)(Sprite.DestinationRectangle.X + OffSet.X), (int)(Sprite.DestinationRectangle.Y + OffSet.Y), Sprite.DestinationRectangle.Width, Sprite.DestinationRectangle.Height);

                    SetDrawable(true);
                }
                else
                {
                    Rectangle = Sprite.DestinationRectangle;

                    SetDrawable(false);
                }
            });

            SetDrawingTask(() =>
            {
                Global.SpriteBatch.Draw(Texture, new Vector2(Rectangle.X, Rectangle.Y), Rectangle, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, Sprite.LayerDepth - 0.5f);
            });
        }

        public SimpleShadowEffect SetOffset(Vector2 offset)
        {
            OffSet = offset;

            return this;
        }
    }
}
