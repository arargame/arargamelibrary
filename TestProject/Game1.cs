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
            var container = new Container();
            container.SetTexture(Color.Yellow);
            //container.SetSize(new Vector2(70,100));
            container.SetSizeDifferenceRatioWithParent(new Vector2(70,100));
            container.SetFrame();

            var row = new Row();
            row.SetTexture();
            container.AddRow(row,80);

            var row2 = new Row();
            row2.SetTexture();
            //container.AddRow(row2, 20);
            

            container.PrepareRows();

            row.SetMargin(Offset.CreatePadding(OffsetValueType.Ratio,10,10,10,10));

            var container2 = new Container();
            container2.SetTexture(Color.Red);
            container2.SetPosition(new Vector2(70,0));
            container2.SetSize(new Vector2(30, 100));
            container2.SetFrame();

            column = new Column();
            column.SetSize(new Vector2(100, 100));
            column.SetTexture();
            column.SetName("clmn");
            column.SetActive(true);

            column.AddChild(container);
            column.AddChild(container2);

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

            column.Draw();

            Global.SpriteBatch.End();

            base.Draw(gameTime);

        }
    }
}
