using ArarGameLibrary.Manager;
using ArarGameLibrary.Model;
using ArarGameLibrary.ScreenManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;
using System.Linq;
using TestProject.Screens;

namespace TestProject
{
    public class Game1 : Game
    {
        //52 133 111 255
        //Column column;

        ScrollBar scrollBar;

        Container cnt;
   
        public Game1()
        {
            Content.RootDirectory = "Content";

            Global.Graphics = new GraphicsDeviceManager(this);
        }

        protected override void Initialize()
        {
            Global.ContentManager = Content;
            Global.GameWindow = Window;
            Global.GraphicsDevice = GraphicsDevice;
            Global.Random = new Random();
            Global.SpriteBatch = new SpriteBatch(GraphicsDevice);
            Global.Theme = new Theme(ThemeMode.Dark);

            InputManager.IsMouseVisible = IsMouseVisible = true;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            //column = new Column();
            //column.SetTexture();
            //column.SetSize(new Vector2(100, 100));
            ////column.SetSizeDifferenceRatioWithParent(new Vector2(80,80));

            //var container = new Container();
            //container.SetTexture();
            //container.SetSize(new Vector2(25, 25));
            //column.AddChild(container);

            //column.AlignChildAsCenter();

            //column.SetDragable();

            //scrollBar = SetScrollBar();
            //scrollBar.SetActive(false);

            var menuScreen = new MainMenu();

            ScreenManager.Add(menuScreen);
            ScreenManager.SetActive(false);

            cnt = new Container();
            cnt.SetTexture();
            cnt.SetFrame(Color.Wheat);
            cnt.SetPosition(new Vector2(250,250));
            cnt.SetSize(new Vector2(300,50));

            var cnt2 = new Container();
            cnt2.SetTexture();
            cnt2.SetTexture(Color.LightPink);
            cnt2.SetSizeRatioToParent(new Vector2(25,100));

            cnt.AddChild(cnt2);
            //cnt2.FloatTo("right");
            cnt.AlignChildAsCenter(new Vector2(20,20));
        }

        public ScrollBar SetScrollBar()
        {
            //column = new Column();

            Column[] columns = new Column[125];


            for (int i = 0; i < columns.Length; i++)
            {
                var container = new Container();

                var row = new Row();
                row.SetTexture();
                container.AddRow(row, 70);

                var row2 = new Row();
                row2.SetTexture();
                container.AddRow(row2, 30);

                var row2Column1 = new Column();
                row2Column1.SetTexture(Color.Black);
                row2.AddColumn(row2Column1, 30);
                row2Column1.AddImage(Triangle.PlayButton(Color.Yellow).Texture);

                var row2Column2 = new Column();
                row2Column2.SetTexture();
                row2Column2.SetFont("Level : " + i);
                row2.AddColumn(row2Column2, 70);

                container.PrepareRows();
                row2Column1.SetPadding(Offset.CreatePadding(OffsetValueType.Ratio, 10, 10, 10, 10));

                var column1 = new Column();
                column1.AddChild(container);

                columns[i] = column1;

                column1.SetFrame(Color.Yellow);
            }


            var scrollBar = new ScrollBar(rowsCountToShow: 3, rowPadding: Offset.CreatePadding(OffsetValueType.Ratio, 5, 5, 5, 5), columns: columns);

            scrollBar.SetActive(true);

            return scrollBar;
        }


        protected override void UnloadContent()
        {


        }


        protected override void Update(GameTime gameTime)
        {
            Global.GameTime = gameTime;
            InputManager.Update();
            InputManager.IsActive = true;

            //if (InputManager.IsLeftClicked)
            //    column.SetPosition(InputManager.CursorPosition);

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            //column.Update();

            //scrollBar.Update();

            ScreenManager.Update();
            cnt.Update();

            //if (InputManager.IsKeyDown(Keys.Up))
            //    column.SetSize(new Vector2(column.Size.X, column.Size.Y - 20));

            //if (InputManager.IsKeyDown(Keys.Down))
            //    column.SetSize(new Vector2(column.Size.X, column.Size.Y + 20));


            //if (InputManager.IsKeyDown(Keys.Left))
            //    column.SetSize(new Vector2(column.Size.X - 20, column.Size.Y));

            //if (InputManager.IsKeyDown(Keys.Right))
            //    column.SetSize(new Vector2(column.Size.X + 20, column.Size.Y));

            //if (InputManager.IsKeyDown(Keys.W))
            //    column.SetPosition(new Vector2(column.Position.X, column.Position.Y - 20));
            //if (InputManager.IsKeyDown(Keys.S))
            //    column.SetPosition(new Vector2(column.Position.X, column.Position.Y + 20));

            //if (InputManager.IsKeyDown(Keys.A))
            //    column.SetPosition(new Vector2(column.Position.X - 20, column.Position.Y));
            //if (InputManager.IsKeyDown(Keys.D))
            //    column.SetPosition(new Vector2(column.Position.X + 20, column.Position.Y));



            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Global.Theme.GetColor());

            Global.SpriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);

            //column.Draw();

            //scrollBar.Draw();

            ScreenManager.Draw();
            cnt.Draw();

            Global.SpriteBatch.End();

            base.Draw(gameTime);

        }
    }
}
