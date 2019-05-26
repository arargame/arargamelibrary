using ArarGameLibrary.Manager;
using ArarGameLibrary.Model;
using ArarGameLibrary.ScreenManagement.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PuzzleMeWindowsProject.Manager;
using PuzzleMeWindowsProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace PuzzleMeWindowsProject
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        Animation animation;

        Graph graph;

        Texture2D texture1;
        Texture2D texture2;

        TextureManager textureManager;

        Vector2 position = new Vector2(0,0);

        Image image;

        Color bgColor = Color.Black;

        List<Piece> Pieces;

        SpriteFont sFont;

        private FrameManager frameManager = new FrameManager();

        Piece samplePiece;
        
        public Game1()
        {
            var f = (float)100 / 3;

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

            image = new Image("WP_20180819_005");
            image.LoadContent();
            image.SetPosition(new Vector2(100,100));
            image.SetRowAndColumnCount(4,4);
            image.SetPieceSize(new Vector2(100,100));
            image.SetPiecePosition();


            texture1 = Content.Load<Texture2D>("Textures/shutterstock_360399314");
            texture2 = TextureManager.Crop(texture1,new Rectangle(0,0,50,50));


            var list = new List<Vector2>();

            for (int i = 0; i < 100; i++)
            {
                //Thread.Sleep(10);

                Random r = new Random();

                list.Add(new Vector2(r.Next(400),r.Next(400)));
            }

            //new Vector2(10, 10), new Vector2(100, 10), new Vector2(100, 100), new Vector2(10, 100)
            graph = new Graph(true).PopulatePoints(list.ToArray())
                                .PopulateLines(Color.Blue);

            graph.LoadContent();

            animation = new Animation(TextureManager.CreateTexture2D("Textures/runningcat"),Vector2.Zero,new Vector2(300,250),4,2,8);
            animation.LoadContent();

            //animation.SetPieceSize(new Vector2(100,100));

            sFont = Content.Load<SpriteFont>("Fonts/MenuFont");

            samplePiece = new Piece()
                .SetPosition(new Vector2(0, 0))
                .SetSize(new Vector2(100, 100));

            samplePiece.LoadContent();
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

            image.Update();

            animation.Update();

            var amount = 5;

            if (Keyboard.GetState().IsKeyDown(Keys.Up))
                position.Y -= amount;
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
                position.Y += amount;
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
                position.X -= amount;
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
                position.X += amount;


            samplePiece.Update();
            if (InputManager.Selected(samplePiece.DestinationRectangle))
            {
                samplePiece.Pulsate(!samplePiece.IsPulsating);
            }

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


            //Global.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);


            //Global.SpriteBatch.Draw(texture2, new Rectangle(0, 0, texture1.Width, texture1.Height), Color.White);
            
            //Global.SpriteBatch.End();

            Global.SpriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);


            var scale = General.Pulsate(6);

            ////float frameRate = 1 / (float)gameTime.ElapsedGameTime.TotalSeconds;
            ////var fm = FontManager.Create(string.Format("FPS : {0} = 1 / {1} , IsRunningSlowly : {2}",frameRate,(float)gameTime.ElapsedGameTime.TotalSeconds,gameTime.IsRunningSlowly),new Vector2(10,10),Color.Bisque);
            ////fm.Draw();

            ////var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            ////frameManager.Update(deltaTime);

            ////Global.SpriteBatch.DrawString(fm.Font,"fps : "+frameManager.AverageFramesPerSecond,new Vector2(50,50),Color.Blue);

             ScreenManager.Draw();
            //graph.Draw();
            // image.Draw();


            //foreach (var piece in image.Pieces)
            //{
            //    piece.Draw();
            //}

            //var pi = image.Pieces[10];
            //Global.SpriteBatch.Draw(pi.Texture,pi.Position,new Rectangle(0,0,pi.Texture.Width/2,pi.Texture.Height/2),pi.Color);
            //Global.SpriteBatch.Draw(image.Texture, pi.Position,new Rectangle(250,250,100,100), pi.Color);
            //Global.SpriteBatch.Draw(image.Texture, new Vector2(pi.Position.X+100,pi.Position.Y), new Rectangle(350, 250, 100, 100), pi.Color);

            //Global.SpriteBatch.Draw(pi.Texture, new Rectangle(pi.DestinationRectangle.X, pi.DestinationRectangle.Y, (int)(pi.DestinationRectangle.Width * scale), (int)(pi.DestinationRectangle.Height * scale)), Color.White);

            //Global.SpriteBatch.Draw(image.Texture, pi.Position,pi.SourceRectangle, pi.Color, pi.Rotation, pi.Origin, new Vector2(pi.Scale), pi.SpriteEffects, pi.LayerDepth);


            //  animation.Draw();
            Global.SpriteBatch.End();

            base.Draw(gameTime);
        }

        protected override void OnExiting(object sender, EventArgs args)
        {
            base.OnExiting(sender, args);
        }
    }
}
