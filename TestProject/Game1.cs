using ArarGameLibrary.Manager;
using ArarGameLibrary.Model;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace TestProject
{
    public class Game1 : Game
    {
        DrawableObject sprite;

        Piece piece;

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
            sprite = new DrawableObject(Content.Load<Texture2D>("58669757_1118224611712565_8241604631101177856_o"),
                                            new Vector2(20, 20),
                                            new Vector2(120, 120));

            piece = new Piece(50,50)
                .SetPosition(new Vector2(250,250))
                .SetSize(new Vector2(100,100))
                .SetBackgroundTextureByRandomColor();


            piece.LoadContent();
        }

        protected override void UnloadContent()
        {

        }


        protected override void Update(GameTime gameTime)
        {
            Global.GameTime = gameTime;
            InputManager.Update();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();


            sprite.Update();

            piece.Update();

            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            Global.SpriteBatch.Begin();

            sprite.Draw();

            piece.Draw();

            Global.SpriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
