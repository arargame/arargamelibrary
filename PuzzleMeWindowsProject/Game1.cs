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

        Triangle lastTriangle;


        Vector2 point1;
        Vector2 point2;
        Vector2 point3;

        Line lineAmongPoint1Point2;
        Line lineAmongPoint1Point3;
        

        List<Vector2> pointListAmongPoint2Point3 = new List<Vector2>();
        List<Line> lineListAmongPoint1Edge23 = new List<Line>();

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

            lastTriangle = Triangle.PlayButton(50,20,400,400,Color.Red);

            //lastTriangle = new Triangle(new Vector2(100,100), new Vector2(450,150), new Vector2(50,300), Color.Red);

            //lastTriangle.LoadContent();
            lastTriangle.SetFilled(Color.Red);
            

            //rt2D = lastTriangle.Texture as RenderTarget2D; //Fonk(()=>lastTriangle.Draw(), 400, 400);

            //Stream stream = File.Create("rt2D.png");
            //rt2D.SaveAsPng(stream, rt2D.Width, rt2D.Height);
            //stream.Dispose();


            point1 = new Vector2(100,100);
            point2 = new Vector2(450,150);
            point3 = new Vector2(50,300);

            lineAmongPoint1Point2 = new Line(Color.Blue,point1,point2);
            lineAmongPoint1Point2.LoadContent();

            lineAmongPoint1Point3 = new Line(Color.Blue,point1,point3);
            lineAmongPoint1Point3.LoadContent();

            var deltaY = point3.Y - point2.Y;

            var deltaX = point3.X - point2.X;

            var slope = 0f;

            if (deltaX != 0f)
                slope = deltaY / deltaX;

            for (int x = (int)point3.X; x < point2.X; x++)
            {
                var y = point3.Y + (x - point3.X) * slope;

                if (pointListAmongPoint2Point3.Any(p => p.X == x && p.Y == y))
                    continue;

                pointListAmongPoint2Point3.Add(new Vector2(x, y));
            }

            foreach (var point in pointListAmongPoint2Point3)
            {
                var line = new Line(Color.Red, point1, point);
                line.LoadContent();

                lineListAmongPoint1Edge23.Add(line);
            }

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
            
            //graph.Update();


            lastTriangle.Update();

            //lineAmongPoint1Point2.Update();
            //lineAmongPoint1Point3.Update();

            //foreach (var item in lineListAmongPoint1Edge23)
            //{
            //    item.Update();
            //}

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

            //lineAmongPoint1Point2.Draw();
            //lineAmongPoint1Point3.Draw();
            //foreach (var item in lineListAmongPoint1Edge23)
            //{
            //    item.Draw();
            //}

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

            lastTriangle.Draw();

            Global.SpriteBatch.End();



            base.Draw(gameTime);
        }

        protected override void OnExiting(object sender, EventArgs args)
        {
            base.OnExiting(sender, args);
        }
    }
}
