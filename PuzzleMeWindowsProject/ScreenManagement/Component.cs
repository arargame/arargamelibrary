using PuzzleMeWindowsProject.Manager;
using PuzzleMeWindowsProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleMeWindowsProject.ScreenManagement
{
    public abstract class Component : Sprite
    {
        public bool IsHovering { get; set; }

        public Action ClickAction;

        public void OnClick(Action action)
        {
            ClickAction = action;
        }

        public override void Update()
        {
            base.Update();

            IsHovering = InputManager.CursorRectangle.Intersects(DestinationRectangle);

            if (IsHovering && InputManager.IsLeftClicked())
            {
                if (ClickAction != null)
                    ClickAction.Invoke();
            }
        }
    }
}
