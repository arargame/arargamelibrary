using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArarGameLibrary.Manager
{
    public class InputManager
    {
        public static bool IsActive { get; set; }

        public static KeyboardState CurrentKeyboardState { get; set; }

        public static KeyboardState PreviousKeyboardState { get; set; }

        public static MouseState CurrentMouseState { get; set; }

        public static MouseState PreviousMouseState { get; set; }

        public static Vector2 CursorPosition { get; set; }

        //public static Texture2D CursorTexture { get; set; }

        public static bool IsMouseVisible { get; set; }

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

        public static bool IsLeftClicked()
        {
            return CurrentMouseState.LeftButton == ButtonState.Pressed && PreviousMouseState.LeftButton == ButtonState.Released;
        }

        public static bool Selected(Rectangle selectedRectangle)
        {
            return IsHovering(selectedRectangle) && IsLeftClicked();
        }

        public static bool IsHovering(Rectangle target)
        {
            return CursorRectangle.Intersects(target);
        }
    }
}
