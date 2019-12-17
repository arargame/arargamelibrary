﻿using ArarGameLibrary.Manager;
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
            container.SetTexture(TextureManager.CreateTexture2DBySingleColor(new Color(52,133,111,255)));
            container.SetFrame(Color.HotPink);
            container.SetSize(new Vector2(400,400));
            container.SetName("cnt");
            //container.SetMargin(new Vector2(10));
            //container.SetPadding(new Vector2(10));

            var row1 = new Row();
            row1.SetName("row1");
            //row1.SetVisible(false);
            row1.SetTexture();

            var row2 = new Row();
            row2.SetName("row2");
            row2.SetTexture();

            var column1 = new Column();
            //column1.SetTexture();
            column1.SetName("col1");
            
            
            //column1.TestInfo.Show();
            row2.AddColumn(column1,30);



            var column2 = new Column();
            column2.SetTexture();
            column2.SetName("col2");
            column2.TestInfo.Show();
            row2.AddColumn(column2, 70);

            container.AddRow(row1,70);
            container.AddRow(row2,30);
            container.PrepareRows();

            column1.SetPadding(new Vector2(10));
            column1.AddImage(Triangle.PlayButton(Color.Moccasin).Texture);

            column = new Column();
            column.SetPosition(new Vector2(0, 0));
            column.SetSize(new Vector2(400, 400));
            column.SetFrame();
            column.SetDragable();

            column.AddChild(container);

            //column1.GetChildAs<Column>().FirstOrDefault().SetMargin(new Vector2(10));

            column.SetMargin(new Vector2(50));

            column.Prepare();
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
