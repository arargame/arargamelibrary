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
        ScrollBar scrollBar;
        //Column testColumn;
        //Column testColumn2;
        string message = "";

        Container container;

        Container cnt;
        Piece piece;

        Graph graph;
        RenderTarget2D rt2D;
        TextureManager tm;

        List<Line> Lines;

        Triangle myTriangle;
        Triangle myTriangle2;
        Triangle myTriangle3;

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


            cnt = new Container();
            cnt.SetTexture(TextureManager.CreateTexture2DBySingleColor(new Color(143, 166,225)));
            cnt.SetSize(new Vector2(250,200));
            cnt.SetFrame(Color.Black);


            var ppp = new Vector2(0,0);
            ppp=Vector2.Lerp(new Vector2(0,0),new Vector2(5,5),Vector2.Distance(new Vector2(0,0),new Vector2(10,10)));
            cnt.SetPosition(ppp);

            var cr1 = new Row();
            cr1.SetTexture(TextureManager.CreateTexture2D("Textures/coral"));

            var cr2 = new Row();
            cr2.SetTexture(TextureManager.CreateTexture2DByRandomColor());

            cnt.AddRow(cr1,80);
            cnt.AddRow(cr2,20);
            cnt.PrepareRows();

            cnt.SetDragable(true);
            

            var cr2c1 = new Column();
            cr2c1.SetTexture(TextureManager.CreateTexture2DByRandomColor());

            cr2.AddColumn(cr2c1, 20);
            cr2.PrepareColumns(floatTo:"left");

            var cr2c2 = new Column();
            cr2c2.SetTexture(TextureManager.CreateTexture2DByRandomColor());

            cr2.AddColumn(cr2c2,80);
            cr2.PrepareColumns(isCentralized:true,floatTo:"left");



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


            piece = new Piece(100,100);
            piece.SetTexture(TextureManager.CreateTexture2DByRandomColor());
            piece.SetPosition(new Vector2(250,250));
            piece.SetSize(new Vector2(100,100));
           // piece.Select();
            piece.SetClickable(true);
            piece.SetDragable(true);



            graph = new Graph(false);

            graph.PopulatePoints(new Vector2(0, 0), new Vector2(100, 0), new Vector2(100, 100));

            graph.PopulateLines(Color.Red,1f);

            graph.LoadContent();

            rt2D = new RenderTarget2D(Global.GraphicsDevice, GraphicsDevice.PresentationParameters.BackBufferWidth, GraphicsDevice.PresentationParameters.BackBufferHeight, false, SurfaceFormat.Color, DepthFormat.None);


            lastTriangle = new Triangle(new Vector2(50, 400), new Vector2(0, 0), new Vector2(400, 50), Color.Yellow, 1f);
            lastTriangle.LoadContent();
            lastTriangle.SetFilled(Color.Yellow);

            rt2D = lastTriangle.Texture as RenderTarget2D; //Fonk(()=>lastTriangle.Draw(), 400, 400);

            Stream stream = File.Create("rt2D.png");
            rt2D.SaveAsPng(stream, rt2D.Width, rt2D.Height);
            stream.Dispose();



            tm = new TextureManager();
            tm.Load("Textures/coral");


            myTriangle = new Triangle(new Vector2(0, 0), new Vector2(400, 50), new Vector2(50, 400), Color.Black, 1f);
            myTriangle.LoadContent();

            myTriangle2 = new Triangle(new Vector2(400, 50),new Vector2(0, 0), new Vector2(50, 400), Color.Black, 1f);
            myTriangle2.LoadContent();

            myTriangle3 = new Triangle(new Vector2(50, 400),new Vector2(0, 0), new Vector2(400, 50), Color.Black, 1f);
            myTriangle3.LoadContent();

            var f1 = new Vector2(0, 0);
            var s2 = new Vector2(400, 50);
            var t3 = new Vector2(50, 400);

            Lines = new List<Line>();
            //for (int i = 0; i < myTriangle.PointListAmongPoint2Point3.Count; i++)
            //{
            //    Lines.Add(new Line(Color.Yellow,f1, myTriangle.PointListAmongPoint2Point3[i]));
            //    Lines[i].LoadContent();
            //}

            //for (int i = 0; i < myTriangle2.PointListAmongPoint2Point3.Count; i++)
            //{
            //    Lines.Add(new Line(Color.Yellow, s2, myTriangle2.PointListAmongPoint2Point3[i]));
            //    Lines[i].LoadContent();
            //}

            //for (int i = 0; i < myTriangle3.PointListAmongPoint2Point3.Count; i++)
            //{
            //    Lines.Add(new Line(Color.Yellow, t3, myTriangle3.PointListAmongPoint2Point3[i]));
            //    Lines[i].LoadContent();
            //}


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
            //cnt.Update();

            //scrollBar.Update();

            //testColumn.Update();
            //testColumn2.Update();
            //container.Update();
            //piece.Update();
            graph.Update();


            foreach (var line in Lines)
            {
                line.Update();
            }


            myTriangle.Update();
            myTriangle2.Update();
            myTriangle3.Update();

            lastTriangle.Update();

            message = "" + InputManager.IsMouseScrolling;
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


            //GraphicsDevice.SetRenderTarget(rt2D);
            //GraphicsDevice.Clear(Color.Transparent);

            Global.SpriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);


            //scrollBar.Draw();
            //cnt.Draw();
            //piece.Draw();
            //foreach (var item in scrollBar.GetChildAs<Component>())
            //{
            //    item.Draw();
            //}
            //container.Draw();

            //ScreenManager.Draw();

            //List<Texture2D> textures = new List<Texture2D>();

            
            //graph.Draw();
            //Global.SpriteBatch.Draw(tm.Texture,new Rectangle(0,0,150,150),Color.LightGoldenrodYellow);
            
            //textures.Add(rt2D);

            //myTriangle.Draw();
            //myTriangle2.Draw();
            //myTriangle3.Draw();
            lastTriangle.Draw();
            //Global.SpriteBatch.Draw(lastTriangle.Texture,new Rectangle(400,400,50,50),Color.White);



            //Global.SpriteBatch.Draw(textures.FirstOrDefault(),new Rectangle(250,0,300,300),Color.White);

           // Global.SpriteBatch.Draw(rt2D, new Rectangle(400, 0, 300, 300), Color.White);


            //foreach (var line in lastTriangle.FillingLines)
            //{
            //    line.Draw();
            //}

            Global.SpriteBatch.End();
            //GraphicsDevice.SetRenderTarget(null);

            //GraphicsDevice.Clear(Global.Theme.GetColor());
            //Global.SpriteBatch.Begin();
            //Global.SpriteBatch.Draw(rt2D,new Rectangle(350,350,100,100),Color.White);
            //Global.SpriteBatch.End();



            base.Draw(gameTime);
        }

        public void DrawingAction()
        {
            //var counter = 0;
            //foreach (var line in Lines)
            //{
            //    counter++;


            //    //if (counter % 5 != 0)
            //    //    continue;

            //    if (line.Texture == null)
            //        line.LoadContent();

            //    line.SetVisible(true);
            //    line.Draw();
            //}

            //Global.SpriteBatch.Draw(TextureManager.CreateTexture2DBySingleColor(Color.Tan), new Rectangle(0, 0, 400, 400), Color.White);

            foreach (var line in lastTriangle.FillingLines)
            {
                line.SetVisible(true);
                line.Draw();
            }
        }

        public static RenderTarget2D Fonk(Action drawingAction,int width,int height)
        {
            var rt2D = new RenderTarget2D(Global.GraphicsDevice, width, height);

            Global.GraphicsDevice.SetRenderTarget(rt2D);
            Global.GraphicsDevice.Clear(Color.Transparent);

            Global.SpriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);
            drawingAction();
            Global.SpriteBatch.End();
            
            Global.GraphicsDevice.SetRenderTarget(null);

            return rt2D;
        }

        protected override void OnExiting(object sender, EventArgs args)
        {
            base.OnExiting(sender, args);
        }
    }
}
