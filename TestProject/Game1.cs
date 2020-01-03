using ArarGameLibrary.Manager;
using ArarGameLibrary.Model;
using ArarGameLibrary.ScreenManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;
using System.Linq;

namespace TestProject
{
    public class Game1 : Game
    {
        //52 133 111 255
        Column column;

        //ScrollBar scrollBar;

        
        public Game1()
        {
            Content.RootDirectory = "Content";

            Global.Graphics = new GraphicsDeviceManager(this);

        }

        protected override void Initialize()
        {
            Global.Content = Content;
            Global.GameWindow = Window;
            Global.GraphicsDevice = GraphicsDevice;
            Global.Random = new Random();
            Global.SpriteBatch = new SpriteBatch(GraphicsDevice);

            InputManager.IsMouseVisible = IsMouseVisible = true;
            Mouse.SetCursor(MouseCursor.Wait);
            

            base.Initialize();
        }

        protected override void LoadContent()
        {
            column = new Column();
            column.SetTexture();
            column.SetPosition(new Vector2(100,100));
            column.SetSize(new Vector2(200,200));
            column.SetDragable();

            var cnt1 = new Container();
            cnt1.SetTexture();
            cnt1.SetPosition(column.Position);
            cnt1.SetSizeDifferenceRatioWithParent(new Vector2(100,100));


            var r1 = new Row();
            r1.SetTexture();
            r1.TestInfo.Show("r1");
            cnt1.AddRow(r1,70);

            var r2 = new Row();
            r2.SetTexture();
            r2.TestInfo.Show("r2");
            cnt1.AddRow(r2,30);

            cnt1.PrepareRows();

            column.AddChild(cnt1);


            //var container = new Container();
            //container.SetTexture(Color.Yellow);
            ////container.SetSize(new Vector2(70,100));
            //container.SetSizeDifferenceRatioWithParent(new Vector2(70, 100));
            ////container.SetFrame(Color.Red);

            //var row = new Row();
            //row.SetTexture();
            //container.AddRow(row, 80);
            ////row.SetVisible(false);

            //var row2 = new Row();
            //row2.SetTexture();
            //container.AddRow(row2, 20);

            //var c = new Column();
            //c.SetTexture();
            //row2.AddColumn(c, 20);
            //c.AddImage(Triangle.PlayButton(Color.Yellow).Texture);

            //var c2 = new Column();
            //c2.SetTexture(Color.SandyBrown);
            //c2.SetName("c2");
            ////c2.SetFrame();
            //row2.AddColumn(c2, 80);
            ////c2.SetFont("Hellow",textPadding: Offset.CreatePadding(OffsetValueType.Ratio,50,10,0,0));
            //c2.SetFont("Hellow");

            //container.PrepareRows();
            
            //var imageColumn = c.Child.FirstOrDefault();

            //imageColumn.SetMargin(Offset.CreateMargin(OffsetValueType.Ratio,10,10,10,10));
            ////c.SetPadding(Offset.CreatePadding(OffsetValueType.Ratio, 10, 10, 10, 10));

            //var container2 = new Container();
            //container2.SetTexture(Color.Red);
            //container2.SetPosition(new Vector2(210, 0));
            ////container2.SetSize(new Vector2(30, 100));
            //container2.SetSizeDifferenceRatioWithParent(new Vector2(30, 100));
            ////container2.SetFrame(Color.Yellow);

            //column = new Column();
            //column.SetSize(new Vector2(300, 300));
            //column.SetTexture();
            //column.SetName("clmn");
            //column.SetActive(true);

            //column.AddChild(container);
            //column.AddChild(container2);

            Column[] columns = new Column[125];


            //for (int i = 0; i < columns.Length; i++)
            //{
            //    var container = new Container();

            //    var row = new Row();
            //    row.SetTexture();
            //    container.AddRow(row, 70);

            //    var row2 = new Row();
            //    row2.SetTexture();
            //    container.AddRow(row2, 30);

            //    var row2Column1 = new Column();
            //    row2Column1.SetTexture(Color.Black);
            //    row2.AddColumn(row2Column1, 30);
            //    row2Column1.AddImage(Triangle.PlayButton(Color.Yellow).Texture);

            //    var row2Column2 = new Column();
            //    row2Column2.SetTexture();
            //    row2Column2.SetFont("Level : " + i);
            //    row2.AddColumn(row2Column2, 70);

            //    container.PrepareRows();
            //    row2Column1.SetPadding(Offset.CreatePadding(OffsetValueType.Ratio, 10, 10, 10, 10));

            //    var column1 = new Column();
            //    column1.AddChild(container);

            //    columns[i] = column1;
            //}


            //scrollBar = new ScrollBar(rowsCountToShow:3,columns: columns);

            //scrollBar.SetActive(false);
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

            column.Update();

            //scrollBar.Update();


            if (InputManager.IsKeyDown(Keys.Up))
                column.SetSize(new Vector2(column.Size.X, column.Size.Y - 20));

            if (InputManager.IsKeyDown(Keys.Down))
                column.SetSize(new Vector2(column.Size.X, column.Size.Y + 20));


            if (InputManager.IsKeyDown(Keys.Left))
                column.SetSize(new Vector2(column.Size.X - 20, column.Size.Y));

            if (InputManager.IsKeyDown(Keys.Right))
                column.SetSize(new Vector2(column.Size.X + 20, column.Size.Y));

            if (InputManager.IsKeyDown(Keys.W))
                column.SetPosition(new Vector2(column.Position.X, column.Position.Y - 20));
            if (InputManager.IsKeyDown(Keys.S))
                column.SetPosition(new Vector2(column.Position.X, column.Position.Y + 20));

            if (InputManager.IsKeyDown(Keys.A))
                column.SetPosition(new Vector2(column.Position.X - 20, column.Position.Y));
            if (InputManager.IsKeyDown(Keys.D))
                column.SetPosition(new Vector2(column.Position.X + 20, column.Position.Y));



            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            Global.SpriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);

            column.Draw();

            //scrollBar.Draw();

            Global.SpriteBatch.End();

            base.Draw(gameTime);

        }
    }
}
