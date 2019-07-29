using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArarGameLibrary.Manager;
using ArarGameLibrary.Model;
using Microsoft.Xna.Framework;

namespace ArarGameLibrary.Effect
{
    public class DraggingEffect : EffectManager
    {
        Vector2 StartPointOfClicking { get; set; }
        //float LastLayerDepth { get; set; }

        public DraggingEffect(Sprite sprite) : base(sprite)
        {
            SetTask(()=>
            {
                if (StartPointOfClicking == Vector2.Zero)
                    StartPointOfClicking = InputManager.CursorPosition - Sprite.Position;

                Sprite.SetPosition(InputManager.CursorPosition - StartPointOfClicking);

                //if (LastLayerDepth != 0f)
                //    LastLayerDepth = Sprite.LayerDepth;

                //Sprite.SetLayerDepth(0f);
            });

            SetEndTask(()=>
            {
                StartPointOfClicking = Vector2.Zero;

                //Sprite.SetLayerDepth(LastLayerDepth);
            });
        }
    }
}
