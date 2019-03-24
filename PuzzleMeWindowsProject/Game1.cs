using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PuzzleMeWindowsProject.Manager;
using PuzzleMeWindowsProject.Model;
using PuzzleMeWindowsProject.ScreenManagement;
using PuzzleMeWindowsProject.ScreenManagement.Screens;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PuzzleMeWindowsProject
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        TextureManager textureManager;

        Vector2 position = new Vector2(0,0);

        Image image;

        Color bgColor = Color.Black;

        private FrameManager frameManager = new FrameManager();
        
        public Game1()
        {
            Global.Graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";

            IsFixedTimeStep = false;
            //TargetElapsedTime = TimeSpan.FromMilliseconds(30); // 20 milliseconds, or 50 FPS.
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            Global.Content = Content;
            Global.GameWindow = Window;
            Global.GraphicsDevice = GraphicsDevice;
            Global.Random = new Random();
            Global.SpriteBatch = new SpriteBatch(GraphicsDevice);

            InputManager.IsMouseVisible = IsMouseVisible = true;
            ScreenManager.SetFullScreen(false);

            ScreenManager.Add(new MainMenu());

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            textureManager = new TextureManager().Load(Content.Load<Texture2D>("WP_20180819_005"));

            image = new Image();
            image.SetName("WP_20180819_005");
            image.LoadContent();
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
            Global.GameTime = gameTime;

            InputManager.Update();
            ScreenManager.Update();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Global.OnExit)
                Exit();

            //gameTime.IsRunningSlowly


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
            GraphicsDevice.Clear(bgColor);

            //image.Draw();


            Global.SpriteBatch.Begin();

            //float frameRate = 1 / (float)gameTime.ElapsedGameTime.TotalSeconds;
            //var fm = FontManager.Create(string.Format("FPS : {0} = 1 / {1} , IsRunningSlowly : {2}",frameRate,(float)gameTime.ElapsedGameTime.TotalSeconds,gameTime.IsRunningSlowly),new Vector2(10,10),Color.Bisque);
            //fm.Draw();

            //var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            //frameManager.Update(deltaTime);

            //Global.SpriteBatch.DrawString(fm.Font,"fps : "+frameManager.AverageFramesPerSecond,new Vector2(50,50),Color.Blue);

            ScreenManager.Draw();


            Global.SpriteBatch.End();

            base.Draw(gameTime);
        }

        protected override void OnExiting(object sender, EventArgs args)
        {
            base.OnExiting(sender, args);
        }
    }
}
