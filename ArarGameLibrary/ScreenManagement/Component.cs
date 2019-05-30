using ArarGameLibrary.Manager;
using ArarGameLibrary.Model;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArarGameLibrary.ScreenManagement
{
    public interface IComponent : IXna
    {
        IScreen Screen { get; set; }

        bool IsHovering { get; set; }

        void OnClick(Action action);
    }

    public abstract class Component : Sprite , IComponent
    {
        public IScreen Screen { get; set; }

        public bool IsHovering { get; set; }

        public Action ClickAction;

        public void OnClick(Action action)
        {
            ClickAction = action;
        }

        public override void Update(GameTime gameTime = null)
        {
            base.Update();

            IsHovering = InputManager.CursorRectangle.Intersects(DestinationRectangle);

            if (IsHovering && InputManager.IsLeftClicked())
            {
                if (ClickAction != null)
                    ClickAction.Invoke();
            }
        }

        public Component SetScreen(IScreen screen)
        {
            Screen = screen;

            return this;
        }
    }
}
