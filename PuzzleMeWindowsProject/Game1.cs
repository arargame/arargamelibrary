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
        ScrollBar scrollBar;
        FontManager font;
        //Column testColumn;
        //Column testColumn2;
        string message = "";
        Vector2 startPointOfDragging;
        Vector2 startPointOfDragging2;



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
            font = FontManager.Create("IsLeftpressing : " + InputManager.IsPressing, new Vector2(350, 70), Color.Black);
            //font.SetChangeTextEvent(()=>
            //{
            //    return "IsLeftpressing : " + InputManager.IsPressing + " " + " Position:" + testColumn.Position.ToString()+" MouseCursor:"+InputManager.CursorPosition.ToString();
            //});

            InputManager.IsActive = true;


            scrollBar = new ScrollBar(3, 3, Vector2.Zero, new Vector2(Global.ViewportWidth, Global.ViewportHeight));
            scrollBar.LoadContent(TextureManager.CreateTexture2DBySingleColor(Color.Tan));
            //scrollBar.SetFrame(Color.Yellow,2f);
            //scrollBar.PrepareRows(true);
            scrollBar.RefreshRectangle();

            var scrollBarColumns = new Column[4];
            for (int i = 0; i < scrollBarColumns.Length; i++)
            {
                scrollBarColumns[i] = new Column();

                scrollBarColumns[i].SetFrame(Color.Black);

                scrollBarColumns[i].SetTexture(TextureManager.CreateTexture2DByRandomColor());
            }
            scrollBar.PrepareList(columns:scrollBarColumns);

            //testColumn = new Column();

            //testColumn.SetTexture(TextureManager.CreateTexture2DByRandomColor());
            //testColumn.SetSize(new Vector2(100,100));
            //testColumn.SetPosition(new Vector2(200,200));
            //testColumn.SetDragable(true);
            //testColumn.SetClickable(true);

            //testColumn2 = new Column();

            //testColumn2.SetTexture(TextureManager.CreateTexture2DByRandomColor());
            //testColumn2.SetSize(new Vector2(75, 100));
            //testColumn2.SetPosition(new Vector2(75, 150));
            //testColumn2.SetDragable(true);
            //testColumn2.SetClickable(true);
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
            //ScreenManager.Update();

            scrollBar.Update();
            font.Update();

            //testColumn.Update();
            //testColumn2.Update();


            message = "";
            //message += "\n IsHovering:"+InputManager.IsHovering(testColumn.DestinationRectangle);
            //message += "\nIsDragging:" + testColumn.IsDragging;
            //message += "\nISDragging2" + testColumn2.IsDragging;
            //message += "\ntestColumn.GetEffect<DraggingEffect>().IsActive" + testColumn.GetEffect<DraggingEffect>().IsActive;
            //message += "\ntestColumn2.GetEffect<DraggingEffect>().IsActive" + testColumn2.GetEffect<DraggingEffect>().IsActive;

            //message += "\nDraggingObject.ID:" + (InputManager.DraggingObject != null ? InputManager.DraggingObject.Id : (Guid?)null);
            //message+= "\nselecting:"+ testColumn.IsSelecting;

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Global.OnExit)
                Exit();

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

            //testColumn.Draw();
            //testColumn2.Draw();
            Global.SpriteBatch.DrawString(font.Font,message,new Vector2(350,250),Color.DarkSeaGreen);
            scrollBar.Draw();


            //foreach (var item in scrollBar.GetChildAs<Component>())
            //{
            //    item.Draw();
            //}


            font.Draw();
            //ScreenManager.Draw();

            Global.SpriteBatch.End();

            base.Draw(gameTime);
        }

        protected override void OnExiting(object sender, EventArgs args)
        {
            base.OnExiting(sender, args);
        }
    }
}
