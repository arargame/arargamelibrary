using ArarGameLibrary.Manager;
using ArarGameLibrary.Model;
using ArarGameLibrary.ScreenManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace TestProject
{
    public class Game1 : Game
    {
        Container cnt1;
        Column clmn1;

        ScrollBar scrollBar;

        //52 133 111 255
        Container container;

        Column column;

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

            base.Initialize();
        }

        protected override void LoadContent()
        {
            container = new Container();
            container.SetSize(new Vector2(150, 150));
            container.SetPosition(new Vector2(250, 250));
            container.SetTexture(TextureManager.CreateTexture2DBySingleColor(Color.Pink));
            container.SetDragable();
            //container.TestInfo.Show();
            container.SetName("Cnt");
            //container.SetFrame();

            var row = new Row();
            row.SetTexture(TextureManager.CreateTexture2DByRandomColor());
            row.SetName("row1");
            row.TestInfo.Show("row");
            row.SetPosition(new Vector2(25, 25));
            container.AddRow(row, 70);

            var row2 = new Row();
            row2.SetTexture(TextureManager.CreateTexture2DBySingleColor(Color.BurlyWood));
            row2.SetName("row2");
            row2.TestInfo.Show("row2");
            container.AddRow(row2, 30);

            //var r2c1 = new Column();
            //r2c1.SetTexture(Triangle.PlayButton(Color.Moccasin).Texture);
            ////r2c1.SetFont("Left", Color.Khaki);
            ////r2c1.SetFrame(Color.Green, 2f);

            //var r2c2 = new Column();
            //r2c2.SetFont("Right", Color.Khaki);
            ////r2c2.SetFrame(Color.Brown, 2f);

            //row2.AddColumn(r2c1, 40);
            //row2.AddColumn(r2c2, 60);

            ////row2.SetFrame(Color.Yellow,2f);

            container.PrepareRows();




            column = new Column();
            column.SetFrame(Color.SteelBlue);
            column.SetSize(new Vector2(150, 150));
            column.SetPosition(new Vector2(200, 200));
            //column.SetTexture(TextureManager.CreateTexture2DBySingleColor(Color.Cornsilk));
            //column.SetDragable();
            //column.TestInfo.Show();
            //column.SetName("Column1001");
            column.SetPadding(new Vector2(10));

            column.AddChild(container);

            //column.SetPadding(new Vector2(10));

            


            ////columns: new Column[] { column }
            var clm = new Column();
            clm.SetSize(new Vector2(150, 150));
            //clm.SetFrame(Color.Red);
            clm.SetTexture();

            var clm2 = new Column();
            clm2.SetSize(new Vector2(50, 50));
            //clm2.SetFrame(Color.DarkBlue);
            clm2.SetTexture();

            var clm3 = new Column();
            clm3.SetSize(new Vector2(50, 50));
            clm3.SetTexture();

            var clm4 = new Column();
            clm4.SetSize(new Vector2(50, 50));
            clm4.SetTexture();

            //scrollBar = new ScrollBar(2,2,columns:new Column[] { clm,clm2,clm3,clm4,column });



            //var ccc = scrollBar.GetChildAs<Column>(c=>c.Name=="Column1001");


            //cnt1 = new Container();
            //cnt1.TestInfo.Show();
            //cnt1.SetFrame();
            //cnt1.SetPosition(new Vector2(100,100));
            //cnt1.SetSize(new Vector2(150,150));
            //cnt1.SetDragable(true);

            //clmn1 = new Column();
            //clmn1.TestInfo.Show("C1");
            //clmn1.SetFrame();
            //clmn1.SetPosition(new Vector2(350,350));
            //clmn1.SetSize(new Vector2(25,25));
            ////clmn1.FixToParentSize(false);

            //var row1 = new Row();
            //row1.SetTexture();
            //row1.SetSize(new Vector2(100, 10));
            //row1.AddColumn(clmn1, 50);

            ////var row2 = new Row();
            ////row2.SetSize(new Vector2(100, 30));
            ////row2.SetTexture();

            //cnt1.AddRow(row1,100);
            ////cnt1.AddRow(row2,30);
            //cnt1.PrepareRows();


        }

        protected override void UnloadContent()
        {

        }


        protected override void Update(GameTime gameTime)
        {
            Global.GameTime = gameTime;
            InputManager.Update();
            InputManager.IsActive = true;

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            //container.Update();

            column.Update();
            //container2.Update();

            //scrollBar.Update();
            //cnt1.Update();
            //clmn1.Update();

            if (InputManager.IsKeyDown(Keys.Up))
                column.SetSize(new Vector2(column.Size.X, column.Size.Y - 20));

            if (InputManager.IsKeyDown(Keys.Down))
                column.SetSize(new Vector2(column.Size.X, column.Size.Y + 20));


            if (InputManager.IsKeyDown(Keys.Left))
                column.SetSize(new Vector2(column.Size.X - 20, column.Size.Y));

            if (InputManager.IsKeyDown(Keys.Right))
                column.SetSize(new Vector2(column.Size.X + 20, column.Size.Y));

            if (InputManager.IsKeyDown(Keys.W))
                column.SetPosition(new Vector2(column.Position.X, column.Position.Y-20));
            if (InputManager.IsKeyDown(Keys.S))
                column.SetPosition(new Vector2(column.Position.X, column.Position.Y + 20));

            if (InputManager.IsKeyDown(Keys.A))
                column.SetPosition(new Vector2(column.Position.X-20, column.Position.Y));
            if (InputManager.IsKeyDown(Keys.D))
                column.SetPosition(new Vector2(column.Position.X+20, column.Position.Y));

            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            Global.SpriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);

            //container.Draw();

            column.Draw();
            //container2.Draw();

            //scrollBar.Draw();
            //cnt1.Draw();
            //clmn1.Draw();

            Global.SpriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
