using ArarGameLibrary.Model;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArarGameLibrary.Manager
{
    public enum MouseScrollState
    {
        Up,
        Down,
        Idle
    }

    public class InputManager
    {
        public static Sprite DraggingObject { get; set; }

        public static bool IsActive { get; set; }

        public static KeyboardState CurrentKeyboardState { get; set; }

        public static KeyboardState PreviousKeyboardState { get; set; }

        public static MouseState CurrentMouseState { get; set; }

        public static MouseState PreviousMouseState { get; set; }

        public static Vector2 CursorPosition { get; set; }

        //public static Texture2D CursorTexture { get; set; }

        public static bool IsMouseVisible { get; set; }

        public static int MouseWheelValue { get; set; }

        public static Rectangle? RectangleWhenPressingStart { get; set; }

        public static Rectangle? RectangleWhenPressingFinish { get; set; }

        public static Rectangle CursorRectangle
        {
            get
            {
                return new Rectangle((int)CursorPosition.X, (int)CursorPosition.Y, 1, 1);
            }
        }


        public static void Update()
        {
            PreviousKeyboardState = CurrentKeyboardState;

            PreviousMouseState = CurrentMouseState;

            if (IsActive)
            {
                CurrentKeyboardState = Keyboard.GetState();

                CurrentMouseState = Mouse.GetState();
            }
            else
            {
                CurrentKeyboardState = new KeyboardState();

                CurrentMouseState = new MouseState();
            }

            CursorPosition = CurrentMouseState.Position.ToVector2();

            MouseWheelValue = CurrentMouseState.ScrollWheelValue - PreviousMouseState.ScrollWheelValue;

            if (IsPressing)
            {
                RectangleWhenPressingStart = RectangleWhenPressingStart ?? CursorRectangle;

                RectangleWhenPressingFinish = null;
            }
            else
            {
                RectangleWhenPressingStart = null;

                RectangleWhenPressingFinish = CursorRectangle;
            }
        }

        public static bool IsNewKeyPress(Keys key)
        {
            return CurrentKeyboardState.IsKeyDown(key) && PreviousKeyboardState.IsKeyUp(key);
        }

        public static bool IsKeyDown(Keys key)
        {
            return CurrentKeyboardState.IsKeyDown(key);
        }

        public static bool IsKeyUp(Keys key)
        {
            return CurrentKeyboardState.IsKeyUp(key);
        }

        public static bool IsMouseScrollIdle
        {
            get
            {
                return MouseWheelValue == 0;
            }
        }

        public static bool IsMouseScrollUp
        {
            get
            {
                return MouseWheelValue > 0;
            }
        }

        public static bool IsMouseScrollDown
        {
            get
            {
                return MouseWheelValue < 0;
            }
        }

        public static bool IsMouseScrolling
        {
            get
            {
                return MouseWheelValue != 0;
            }
        }

        public static bool IsLeftClicked
        {
            get
            {
                return CurrentMouseState.LeftButton == ButtonState.Pressed && PreviousMouseState.LeftButton == ButtonState.Released;
            }
        }

        public static bool IsRightClicked
        {
            get
            {
                return CurrentMouseState.RightButton == ButtonState.Pressed && PreviousMouseState.RightButton == ButtonState.Released;
            }
        }

        public static bool IsPressing
        {
            get
            {
                return CurrentMouseState.LeftButton == ButtonState.Pressed && PreviousMouseState.LeftButton != ButtonState.Released;
            }
        }

        public static bool Selected(Rectangle selectedRectangle)
        {
            return IsHovering(selectedRectangle) && IsLeftClicked;
        }

        public static bool IsHovering(Rectangle target)
        {
            return CursorRectangle.Intersects(target);
        }

        public static bool IsDragging(Sprite sprite)
        {
            if (!IsPressing)
            {
                DraggingObject = null;

                return false;
            }

            if (IsHovering(sprite.DestinationRectangle) && DraggingObject == null && sprite.DestinationRectangle.Intersects(RectangleWhenPressingStart.Value))
                DraggingObject = sprite;

            //DraggingObject = IsHovering(sprite.DestinationRectangle) && DraggingObject == null ? DraggingObject = sprite : null ;


            return DraggingObject!= null && DraggingObject.Id == sprite.Id;

            //return IsPressing;//IsHovering(draggingRectangle) &&
        }
    }
}
