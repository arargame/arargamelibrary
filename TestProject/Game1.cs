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
        ScrollBar scrollBar;

        //52 133 111 255
        Container container;

        Column column;

        Container container2;

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
            container.SetSize(new Vector2(100,100));
            container.SetPosition(new Vector2(250,250));
            container.SetTexture(TextureManager.CreateTexture2DBySingleColor(Color.Pink));
            container.SetDragable();
            //container.TestInfo.Show();
            container.SetFrame();

            var row = new Row();
            row.SetTexture(TextureManager.CreateTexture2DByRandomColor());
            row.SetName("row1");
            //row.TestInfo.Show("row");
            container.AddRow(row, 70);

            var row2 = new Row();
            row2.SetTexture(TextureManager.CreateTexture2DBySingleColor(Color.BurlyWood));
            row2.SetName("row2");
            //row2.TestInfo.Show("row2");
            container.AddRow(row2, 30);

            var r2c1 = new Column();
            r2c1.SetTexture(Triangle.PlayButton(Color.Moccasin).Texture);
            //r2c1.SetFont("Left", Color.Khaki);
            r2c1.SetFrame(Color.Green, 2f);

            var r2c2 = new Column();
            r2c2.SetFont("Right", Color.Khaki);
            r2c2.SetFrame(Color.Brown, 2f);

            row2.AddColumn(r2c1, 40);
            row2.AddColumn(r2c2, 60);

            //row2.SetFrame(Color.Yellow,2f);

            container.PrepareRows();



            column = new Column();
            column.SetFrame(Color.SteelBlue);
            column.SetSize(new Vector2(200, 200));
            column.SetPosition(new Vector2(0, 0));
            column.SetTexture(TextureManager.CreateTexture2DBySingleColor(Color.Cornsilk));
            column.SetDragable();
            column.SetFrame(Color.Red);
            //column.TestInfo.Show();
            //column.SetName("Column1001");

            column.AddChild(container);
            column.SetDragable();


            container2 = new Container();
            container2.SetSize(new Vector2(100,100));
            container2.SetPosition(new Vector2(300,300));
            container2.TestInfo.Show();
            container2.SetFrame();

            //container2.AddChild(column);
            container2.SetDragable();
            //columns: new Column[] { column }
            var clm = new Column();
            clm.SetSize(new Vector2(50,50));
            clm.SetFrame(Color.Red);
            clm.SetTexture();

            var clm2 = new Column();
            clm2.SetSize(new Vector2(50, 50));
            clm2.SetFrame(Color.DarkBlue);
            clm2.SetTexture();

            var clm3 = new Column();
            clm3.SetSize(new Vector2(50, 50));
            clm3.SetTexture();

            var clm4 = new Column();
            clm4.SetSize(new Vector2(50, 50));
            clm4.SetTexture();

            scrollBar = new ScrollBar(2,2,columns:new Column[] { clm,clm2,clm3,clm4,column });

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

            //column.Update();
            //container2.Update();

            scrollBar.Update();


            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            Global.SpriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);

            //container.Draw();

            //column.Draw();
            //container2.Draw();

            scrollBar.Draw();

            Global.SpriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
