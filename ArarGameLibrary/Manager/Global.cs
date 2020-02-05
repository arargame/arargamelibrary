using ArarGameLibrary.ScreenManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace ArarGameLibrary.Manager
{
    public static class Global
    {
        public static ContentManager ContentManager { get; set; }

        public static GameTime GameTime { get; set; }

        public static GameWindow GameWindow { get; set; }

        public static GraphicsDeviceManager GraphicsDeviceManager { get; set; }

        public static GraphicsDevice GraphicsDevice { get; set; }

        public static Random Random { get; set; }

        public static SpriteBatch SpriteBatch { get; set; }

        public static bool OnExit { get; set; }

        public static Theme Theme = new Theme(ThemeMode.Dark);

        public static ContentManager Content(string rootDirectory = "Content")
        {
            ContentManager.RootDirectory = rootDirectory;

            return ContentManager;
        }

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

        public static readonly int DefaultViewportWidth = 800;

        public static int ViewportWidth
        {
            get
            {
                return GraphicsDevice.Viewport.Width;
            }
        }

        public static readonly int DefaultViewportHeight = 480;

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

        public static Vector2 Scale
        {
            get
            {
                return new Vector2(ViewportWidth * 1f / DefaultViewportWidth, ViewportHeight * 1f / DefaultViewportHeight);
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

        public static Color RandomColor(bool isOpaque = true)
        {
            return new Color(RandomNext(0,256), RandomNext(0, 256), RandomNext(0, 256), isOpaque ? 255 : RandomNext(0, 256));
        }

        public static int RandomNext(int minValue,int maxValue)
        {
            return Random.Next(minValue, maxValue);
        }

        public static float DeltaTime(this GameTime gameTime)
        {
            return gameTime.ElapsedGameTime.Milliseconds / 1000f;
        }

        public static void Game(Game game)
        {
            game.Content.RootDirectory = "Content";

            GraphicsDeviceManager = new GraphicsDeviceManager(game);
        }

        public static void Initialize(Game game)
        {
            InputManager.IsMouseVisible = game.IsMouseVisible = true;

            InputManager.IsActive = true;

            ProjectManager.Load();

            Random = new Random();

            ContentManager = game.Content;

            GameWindow = game.Window;

            GraphicsDevice = game.GraphicsDevice;
            
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            Theme = new Theme(ThemeMode.Dark);
        }

        public static void LoadContent()
        {

        }

        public static void Update(GameTime gameTime)
        {
            GameTime = gameTime;

            InputManager.Update();

            ScreenManager.Update();
        }

        public static void Draw()
        {

        }
    }
}
