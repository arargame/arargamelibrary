using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PuzzleMeWindowsProject.Manager;

namespace PuzzleMeWindowsProject
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        TextureManager textureManager;

        Vector2 position = new Vector2(0,0);
        
        public Game1()
        {
            Global.Graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            Global.GameWindow = Window;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            Global.SpriteBatch = new SpriteBatch(GraphicsDevice);
            Global.Content = Content;
            Global.GraphicsDevice = GraphicsDevice;

            textureManager = new TextureManager().Load(Content.Load<Texture2D>("WP_20180819_005"));

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
           
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            var amount = 5;

            if (Keyboard.GetState().IsKeyDown(Keys.Up))
                position.Y -= amount;
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
                position.Y += amount;
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
                position.X -= amount;
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
                position.X += amount;

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            Global.SpriteBatch.Begin();

            

            Global.SpriteBatch.Draw(TextureManager.Crop(textureManager.Texture,new Rectangle((int)position.X,(int)position.Y,100,600)),new Rectangle(0,0,200,200),Color.White);

            Global.SpriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
