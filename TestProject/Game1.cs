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
            container.SetFrame(Color.Yellow,2f);
            container.SetSize(new Vector2(100,100));
            container.SetPosition(new Vector2(250,250));
            container.SetTexture(TextureManager.CreateTexture2DByRandomColor());
            container.SetDragable();

            var row = new Row();
            row.SetTexture(TextureManager.CreateTexture2DByRandomColor());
            container.AddRow(row,70);


            var row2 = new Row();
            row2.SetTexture();
            container.AddRow(row2, 30);

            var r2c1 = new Column();
            r2c1.SetTexture(Triangle.PlayButton(Color.Moccasin).Texture);
            


            var r2c2 = new Column();
            r2c2.SetFont("Right", Color.Khaki);

            row2.AddColumn(r2c1,40);
            row2.AddColumn(r2c2,60);
            

            container.PrepareRows();

            

            column = new Column();
            column.SetFrame(Color.SteelBlue);
            column.SetSize(new Vector2(200,200));
            column.SetPosition(new Vector2(0,0));
            column.SetTexture();
            column.SetDragable();

            column.AddChild(container);

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

            container.Update();

            column.Update();


            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            Global.SpriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);

            container.Draw();

            column.Draw();

            Global.SpriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
