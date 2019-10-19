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
using System.IO;
using System.Linq;
using System.Threading;

namespace PuzzleMeWindowsProject
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        Column columnForImageButton;


        ScrollBar scrollBar;
        //Column testColumn;
        //Column testColumn2;

        Container container;

        Container cnt;

        RenderTarget2D rt2D;

        Triangle lastTriangle;

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


            //ScreenManager.Add(new MainMenu());
            

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {

            InputManager.IsActive = true;

            var scrollBarColumns = new Column[16];
            for (int i = 0; i < scrollBarColumns.Length; i++)
            {
                scrollBarColumns[i] = new Column();
            

                //scrollBarColumns[i].SetFrame(Color.Black);

                //scrollBarColumns[i].SetTexture(TextureManager.CreateTexture2DByRandomColor());
            }

            scrollBar = new ScrollBar(3, 3, 2.5f, scrollBarColumns);
            scrollBar.LoadContent(TextureManager.CreateTexture2DBySingleColor(Color.Tan));
            //scrollBar.SetFrame(Color.Yellow,2f);
            //scrollBar.PrepareRows(true);



            //scrollBar.SetListContainer(columns: scrollBarColumns);
            scrollBar.RefreshRectangle();

            var firstScrollBarRow = scrollBar.Rows.FirstOrDefault();
            var firstScrollBarRowColumn = firstScrollBarRow.Columns.FirstOrDefault();

            var c = new Column();
            c.SetTexture(TextureManager.CreateTexture2DByRandomColor());


            firstScrollBarRowColumn.AddChild(c);
            c.SetMargin(new Vector2(10, 10));
            c.SetSize(new Vector2(50,50));
            c.IncreaseLayerDepth();
            


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

            container = new Container();
            container.SetTexture(TextureManager.CreateTexture2DBySingleColor(Color.Beige));
            container.SetSize(new Vector2(300,200));
            container.SetMargin(new Vector2(10,10));

            var row = new Row();
            row.SetTexture(TextureManager.CreateTexture2DBySingleColor(new Color(36,220,151)));
            container.AddRow(row,100);
            var r1c1 = new Column();
            r1c1.SetTexture(TextureManager.CreateTexture2DByRandomColor());
            row.AddColumn(r1c1,30);

            var r1c2 = new Column();
            r1c2.SetTexture(TextureManager.CreateTexture2DByRandomColor());
            row.AddColumn(r1c2, 30);

            var r1c3 = new Column();
            r1c3.SetTexture(TextureManager.CreateTexture2DByRandomColor());
            row.AddColumn(r1c3, 30);



            var row2 = new Row();
            row2.SetTexture(TextureManager.CreateTexture2DByRandomColor());
            container.AddRow(row2, 100);

            var row3 = new Row();
            row3.SetTexture(TextureManager.CreateTexture2DByRandomColor());
            container.AddRow(row3, 100);

            var row4 = new Row();
            row4.SetTexture(TextureManager.CreateTexture2DByRandomColor());
            container.AddRow(row4, 100);
                        
            container.PrepareRows();

            row.PrepareColumns(true,"right");

            ///////Container
            cnt = new Container();
            cnt.SetTexture(TextureManager.CreateTexture2DBySingleColor(new Color(143, 166,225)));
            cnt.SetSize(new Vector2(250,200));
            cnt.SetFrame(Color.Black);


            var firstRow = new Row();
            firstRow.SetTexture(TextureManager.CreateTexture2D("Textures/coral"));

            var secondRow = new Row();
            secondRow.SetTexture(TextureManager.CreateTexture2DByRandomColor());

            cnt.AddRow(firstRow, 80);
            cnt.AddRow(secondRow, 20);
            cnt.PrepareRows();
            cnt.SetDragable(true);
            
            var column1 = new Column();
            column1.SetTexture(TextureManager.CreateTexture2DByRandomColor());

            var column2 = new Column();
            column2.SetTexture(TextureManager.CreateTexture2DByRandomColor());

            //secondRow.PrepareColumns(floatTo:"left");

            //cr2c1.SetPadding(new Vector2(10));

            secondRow.AddColumn(column1, 20);
            secondRow.AddColumn(column2, 80);
            secondRow.PrepareColumns(isCentralized:true,floatTo:"left");

            lastTriangle = Triangle.PlayButton(Color.Red);
            column1.AddImage(lastTriangle.Texture);
            column1.SetPadding(new Vector2(10));
            
            var pulsateEvent = (column1.Child.FirstOrDefault() as Column).GetEvent<PulsateEffect>();
            pulsateEvent.SetWhenToInvoke(() => { return (column1.Child.FirstOrDefault() as Column).IsHovering; });

            var x = 100;


            ////////------------

            //Button b = new Button("Play with me",new Vector2(400,150));
            //cr2c1.AddChild(b);
            //b.SetPosition(new Vector2(300, 150));
            //b.SetSize(cr2c1.Size - new Vector2(-20 - 20));


            //cnt.ShowSimpleShadow(true);

            //var pulsateEvent = cnt.GetEvent<PulsateEffect>();
            //pulsateEvent.SetWhenToInvoke(() => { return true; });

            //foreach (var children in cnt.Child)
            //{
            //    var pulsateEvent2 = (children as Sprite).GetEvent<PulsateEffect>();
            //    pulsateEvent2.SetWhenToInvoke(() => { return true; });    
            //}



            rt2D = new RenderTarget2D(Global.GraphicsDevice, GraphicsDevice.PresentationParameters.BackBufferWidth, GraphicsDevice.PresentationParameters.BackBufferHeight, false, SurfaceFormat.Color, DepthFormat.None);
            

            //rt2D = lastTriangle.Texture as RenderTarget2D; //Fonk(()=>lastTriangle.Draw(), 400, 400);

            //Stream stream = File.Create("rt2D.png");
            //rt2D.SaveAsPng(stream, rt2D.Width, rt2D.Height);
            //stream.Dispose();

            columnForImageButton = new Column();

            //columnForImageButton.SetPosition(new Vector2(10, 0));
            //columnForImageButton.SetSize(new Vector2(240, 250));

            columnForImageButton.SetPosition(new Vector2(0, 0));
            columnForImageButton.SetSize(new Vector2(200, 200));

            columnForImageButton.AddImage(lastTriangle.Texture);


            
            columnForImageButton.TestInfo.Show();
            columnForImageButton.TestInfo.AddParameters("DestinationRectangle");

            

            columnForImageButton.SetPadding(new Vector2(20));
            var columnImage = columnForImageButton.Child.FirstOrDefault() as Component;
            
            columnForImageButton.ClickAction = () => 
            {
                columnForImageButton.SetPosition(new Vector2(columnForImageButton.Position.X + 10, columnForImageButton.Position.Y));
            };
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
            cnt.Update();

            //scrollBar.Update();

            //testColumn.Update();
            //testColumn2.Update();
            //container.Update();
            

            //lastTriangle.Update();
            //columnForImageButton.Update();


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


            //GraphicsDevice.SetRenderTarget(rt2D);
            //GraphicsDevice.Clear(Color.Transparent);

            Global.SpriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);
            //columnForImageButton.Draw();

            //scrollBar.Draw();
            cnt.Draw();

            //foreach (var item in scrollBar.GetChildAs<Component>())
            //{
            //    item.Draw();
            //}
            //container.Draw();

            //ScreenManager.Draw();

            //List<Texture2D> textures = new List<Texture2D>();

            //lastTriangle.Draw();

            Global.SpriteBatch.End();



            base.Draw(gameTime);
        }

        protected override void OnExiting(object sender, EventArgs args)
        {
            base.OnExiting(sender, args);
        }
    }
}
