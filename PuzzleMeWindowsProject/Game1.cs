using ArarGameLibrary.Effect;
using ArarGameLibrary.Extension;
using ArarGameLibrary.Manager;
using ArarGameLibrary.Model;
using ArarGameLibrary.ScreenManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PuzzleMeWindowsProject.Manager;
using PuzzleMeWindowsProject.Model;
using PuzzleMeWindowsProject.Screens;
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

        Image image;

        Board board;

        Nest nest;

        Button button;

        Frame graph;

        Texture2D sample;

        Container container;

        
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
            Global.Theme = new Theme(ThemeMode.White);

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
            image = new Image("WP_20180819_005");
            image.LoadContent();
            image.SetPosition(new Vector2(100,100));
            image.SetRowAndColumnCount(2, 2);
            image.SetPieceSize(new Vector2(100, 100));
            image.SetPiecePosition();

            board = new Board(8,8,new Vector2(Global.ViewportWidth-100,Global.ViewportHeight));
            board.LoadContent();
            board.SpreadImagePiecesOnTheBoard(image);

            nest = new Nest(board,image);
            nest.Pieces.ForEach(p => p.SetDrawMethodType(1));

            animation = new Animation(TextureManager.CreateTexture2D("Textures/runningcat"),Vector2.Zero,new Vector2(300,250),4,2,8);
            animation.LoadContent();

            InputManager.IsActive = true;

            button = new Button("Helloww!!",new Vector2(100,100));
            button.LoadContent();
            graph = Frame.Create(button.DestinationRectangle,Color.Red);
            graph.LoadContent();

            sample = TextureManager.CreateTexture2DByRandomColor(1,1);

            container = new Container();
            container.SetSize(new Vector2(200,150));
            //container.SetColor(Color.MonoGameOrange);
            container.SetMargin(new Vector2(10,10));

            container.AddChild(new Button("",Vector2.Zero));

            var row = new Row();
            row.MakeFrameVisible(true);
            //row.SetTexture(TextureManager.CreateTexture2DByRandomColor());
            row.SetFrame(Color.Black);

            var column1 = new Column();
            column1.SetTexture(TextureManager.CreateTexture2DBySingleColor(Color.Red,1,1));

            var column2 = new Column();
            column2.SetTexture(TextureManager.CreateTexture2DBySingleColor(Color.Green, 1, 1));

            var column3 = new Column();
            column3.SetTexture(TextureManager.CreateTexture2DBySingleColor(Color.Blue, 1, 1));

            row.AddColumn(column1,30);
            row.AddColumn(column2, 30);
            row.AddColumn(column3, 30);

            container.AddRow(row, 1, 80);

            var row2 = new Row();
            row2.MakeFrameVisible(true);
            row2.SetTexture(TextureManager.CreateTexture2DBySingleColor(Color.Black, 1, 1));
            //row2.SetFrame(Color.White);

            container.AddRow(row2, 2, 20);

            container.PrepareRows();

            foreach (var cr in container.Rows)
            {
                foreach (var rc in cr.Columns)
                {
                    rc.AddImage(TextureManager.CreateTexture2DByRandomColor(1,1));
                }
            }
            //Card screen yqap/Level selecting screen


         //   container.ShowSimpleShadow(true);
          //  container.GetEffect<SimpleShadowEffect>().SetOffset(new Vector2(-5,-5));
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

            //button.Update();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Global.OnExit)
                Exit();
           // container.Update();
            //gameTime.IsRunningSlowly

            // board.Update();
            //     image.Update();
            //   nest.Update();

            //graph.Update();

            //animation.Update();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {

            GraphicsDevice.Clear(Global.Theme.GetColor());

            Global.SpriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);

            //container.Draw();

            ScreenManager.Draw();
            //button.Draw();
            //graph.Draw();
            //foreach (var piece in image.Pieces)
            //{
            //    piece.Draw();
            //}
            //   board.Draw();
            //  nest.Draw();
            //  image.Draw();
            //  animation.Draw();

            //var emptyPiece = board.Pieces.SingleOrDefault(p => p.IsEmpty);

            //emptyPiece.Draw();

            // Global.SpriteBatch.Draw(emptyPiece.Texture,new Rectangle(0,0,50,50),new Rectangle(0,0,emptyPiece.Texture.Width,emptyPiece.Texture.Height),Color.White,0f,Vector2.Zero,SpriteEffects.None,1f);

            Global.SpriteBatch.End();

            base.Draw(gameTime);
        }

        protected override void OnExiting(object sender, EventArgs args)
        {
            base.OnExiting(sender, args);
        }
    }
}
