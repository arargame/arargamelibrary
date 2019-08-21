using ArarGameLibrary.Manager;
using ArarGameLibrary.Model;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArarGameLibrary.Event
{
    public class DraggingEvent : EventManager
    {
        public Vector2 StartPointOfClicking { get; set; }

        public DraggingEvent(Sprite sprite)
            : base(sprite, isContinuous: true)
        {
            SetTask(() =>
            {
                if (Sprite.IsDragging)
                {
                    if (StartPointOfClicking == Vector2.Zero)
                        StartPointOfClicking = InputManager.CursorPosition - Sprite.Position;

                    Sprite.SetPosition(InputManager.CursorPosition - StartPointOfClicking);
                }
                else
                {
                    StartPointOfClicking = Vector2.Zero;
                }
            });
        }

        public override void Update()
        {
            if (Sprite.IsClickable && Sprite.IsDragable)
            {
                Sprite.IsDragging = InputManager.IsDragging(Sprite);
            }

            base.Update();
        }
    }
}
