using ArarGameLibrary.Model;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArarGameLibrary.Manager
{
    public class Global
    {
        public static ContentManager Content { get; set; }

        public static GameTime GameTime { get; set; }

        public static GameWindow GameWindow { get; set; }

        public static GraphicsDeviceManager Graphics { get; set; }

        public static GraphicsDevice GraphicsDevice { get; set; }

        public static Random Random { get; set; }

        public static SpriteBatch SpriteBatch { get; set; }

        public static bool OnExit { get; set; }

        public static void ChangeGameWindowTitle(string title)
        {
            GameWindow.Title = title;
        }

        public static Rectangle ViewportRect
        {
            get
            {
                return new Rectangle(0, 0, ViewportWidth, ViewportHeight);
            }
        }

        public static int ViewportWidth
        {
            get
            {
                return GraphicsDevice.Viewport.Width;
            }
        }

        public static int ViewportHeight
        {
            get
            {
                return GraphicsDevice.Viewport.Height;
            }
        }

        public static Vector2 ViewportCenter
        {
            get
            {
                return new Vector2((float)(ViewportWidth / 2), (float)(ViewportHeight / 2));
            }
        }

        public static bool IsInScreen(Rectangle rect)
        {
            return ViewportRect.Intersects(rect);
        }

        public static bool IsInFirstRow(Rectangle rect)
        {
            return rect.Bottom < 0;
        }

        public static bool IsInSecondRow(Rectangle rect)
        {
            return 0 <= rect.Bottom && rect.Y <= ViewportHeight;
        }

        public static bool IsInThirdRow(Rectangle rect)
        {
            return rect.Y > ViewportHeight;
        }

        public static bool IsInFirstColumn(Rectangle rect)
        {
            return rect.Right < 0;
        }

        public static bool IsInSecondColumn(Rectangle rect)
        {
            return 0 <= rect.Right && rect.X <= ViewportWidth;
        }

        public static bool IsInThirdColumn(Rectangle rect)
        {
            return rect.X > ViewportWidth;
        }
    }
}
