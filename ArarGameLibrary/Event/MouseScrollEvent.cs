using ArarGameLibrary.Manager;
using ArarGameLibrary.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArarGameLibrary.Event
{
    public class MouseScrollEvent : EventManager
    {
        MouseScrollState ScrollState { get; set; }

        public MouseScrollEvent(Sprite sprite, Action whenScrollStateIsUp, Action whenScrollStateIsDown, Action whenScrollStateIsIdle = null)
            : base(sprite, isContinuous: true)
        {
            SetTask(() =>
            {
                switch (ScrollState)
                {
                    case MouseScrollState.Up:

                            whenScrollStateIsUp.Invoke();

                        break;

                    case MouseScrollState.Down:

                            whenScrollStateIsDown.Invoke();

                        break;

                    case MouseScrollState.Idle:

                        if (whenScrollStateIsIdle != null)
                            whenScrollStateIsIdle.Invoke();

                        break;

                    default:
                        break;
                }
            });
        }

        public override void Update()
        {
            if (InputManager.IsMouseScrollUp)
                ScrollState = MouseScrollState.Up;
            else if (InputManager.IsMouseScrollDown)
                ScrollState = MouseScrollState.Down;
            else if (InputManager.IsMouseScrollIdle)
                ScrollState = MouseScrollState.Idle;

            base.Update();
        }
    }
}
