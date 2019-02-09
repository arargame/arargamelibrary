using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleMeWindowsProject.Manager
{
    public class Global
    {
        public static ContentManager Content { get; set; }

        public static GraphicsDeviceManager Graphics { get; set; }

        public static SpriteBatch SpriteBatch { get; set; }

        public static GameTime GameTime { get; set; }

        public static GameWindow GameWindow { get; set; }

        public static GraphicsDevice GraphicsDevice { get; set; }
    }
}
