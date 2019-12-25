using ArarGameLibrary.Manager;
using ArarGameLibrary.Model;
using ArarGameLibrary.ScreenManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Linq;

namespace TestProject
{
    public class Game1 : Game
    {
        //52 133 111 255
        Column column;

        ScrollBar scrollBar;
        
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
            var cnt = new Container();
            cnt.SetTexture();
            cnt.SetSizeDifferenceRatioWithParent(new Vector2(100,90));
            


            column = new Column();
            column.SetSize(new Vector2(250, 250));
            column.SetTexture();
            column.SetPadding(new Padding(10,0,0,0,OffsetValueType.Ratio));
            //column.SetActive(false);
            column.AddChild(cnt);
            //Sorun var burada total bir rechange e ihtiyaç var
            var columns = new Column[15];

            for (int i = 0; i < columns.Length; i++)
            {
                columns[i] = new Column();
                columns[i].SetTexture();
            }

            var container = new Container();

            container.SetSizeDifferenceRatioWithParent(new Vector2(100,100));
            //container.SetPosition(new Vector2(columns[13].Position.X + columns[13].Size.X * 90 /100, columns[13].Position.Y));

         //   container.SetMargin(new Vector2(10));
            container.SetTexture();

            columns[13].SetPadding(new Padding(10,0,10,0,OffsetValueType.Ratio));
            columns[13].SetName("column13");
            columns[13].AddChild(container);
          //  columns[13].Prepare();

            scrollBar = new ScrollBar(columns:columns);

            scrollBar.Prepare();
            scrollBar.SetActive(false);
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

            scrollBar.Update();

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

            scrollBar.Draw();

            Global.SpriteBatch.End();

            base.Draw(gameTime);

        }
    }
}
